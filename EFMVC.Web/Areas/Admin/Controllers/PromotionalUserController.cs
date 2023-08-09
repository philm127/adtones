using ClosedXML.Excel;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Web.Areas.Admin.ViewModel;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("PromotionalUser")]
    public class PromotionalUserController : Controller
    {
        private class MSISDNModel
        {
            public string MSISDN { get; set; }
        }

        private const int ProvisionBatchSize = 1500;
        private const string DestinationTableName = "dbo.PromotionalUsers";

        private readonly IPromotionalUserRepository _promotionalUserRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IOperatorRepository _operatorRepository;
        private readonly ICommandBus _commandBus;

        public PromotionalUserController(ICommandBus commandBus, IPromotionalUserRepository promotionalUserRepository, ICountryRepository countryRepository, IOperatorRepository operatorRepository)
        {
            _commandBus = commandBus;
            _promotionalUserRepository = promotionalUserRepository;
            _countryRepository = countryRepository;
            _operatorRepository = operatorRepository;
        }

        // GET: Admin/PromotionalUserController
        [Route("Index")]
        public ActionResult Index()
        {
            FillCountry(0);
            return View();
        }

        [Route("SavePromotionalUser")]
        [HttpPost]
        public async Task<ActionResult> SavePromotionalUser(PromotionalUserFormModel model, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid && file != null)
                {
                    string operatorName = _operatorRepository.GetById(model.OperatorId).OperatorName;

                    var operatorConnectionString = ConnectionString.GetSingleConnectionStringByOperatorId(model.OperatorId);

                    if (!string.IsNullOrEmpty(operatorConnectionString))
                    {
                        using (EFMVCDataContex db = new EFMVCDataContex(operatorConnectionString))
                        {
                            var batchIDExist = await db.PromotionalUsers.AnyAsync(top => top.BatchID.Equals(model.BatchID));

                            if (batchIDExist)
                            {
                                FillCountry(model.CountryId);
                                TempData["Error"] = "Batch Id Exist to " + operatorName + " Operator.";
                                return View("Index");
                            }

                            List<MSISDNModel> import = ImportExcel(model.OperatorId, file);

                            HashSet<string> promoMsisdns = new HashSet<string>(import.Select(m => m.MSISDN));
                            HashSet<string> existingUsers = new HashSet<string>(await db.Userprofiles.Select( top => top.MSISDN).ToListAsync());
                            var existingPromoUsers = new HashSet<string>(await db.PromotionalUsers.Select(top => top.MSISDN).ToListAsync());

                            promoMsisdns.ExceptWith(existingUsers);
                            promoMsisdns.ExceptWith(existingPromoUsers);

                            if (model.OperatorId == (int)OperatorTableId.Expresso)
                            {
                                var promoMsisdnsList = promoMsisdns.ToList(); // to ensure Skip,Take would handle records in a same order.

                                int overallCount = promoMsisdnsList.Count;
                                int chunkCount = overallCount / ProvisionBatchSize;
                                int processedCount = 0;

                                for (int i = 0; i < chunkCount; i++)
                                {
                                    var nextChunk = promoMsisdnsList.Skip(processedCount).Take(ProvisionBatchSize).ToList();
                                    processedCount += nextChunk.Count;
                                    await DoExpressoProvisionAndSaveToDatabase(nextChunk, operatorConnectionString, DestinationTableName, model);
                                }

                                if (overallCount - processedCount > 0) // processing the rest of the batch.
                                    await DoExpressoProvisionAndSaveToDatabase(promoMsisdnsList.Skip(processedCount).ToList(), operatorConnectionString, DestinationTableName, model);

                                TempData["success"] = "User(s) added successfully for operator " + operatorName;
                            }
                            else if (model.OperatorId == (int)OperatorTableId.Safaricom)
                            {
                                await DoSaveToDatabase(promoMsisdns,
                                    (source, table) => source.Select(isdn => // this is a converter method that transforms MSISDNs to a List of DataRows.
                                        {
                                            var row = table.NewRow();
                                            row.BeginEdit();
                                            row["MSISDN"] = isdn;
                                            row["WeeklyPlay"] = 0;
                                            row["DailyPlay"] = 0;
                                            row["Status"] = (int)PromotionalUserStatus.Active;
                                            row["BatchID"] = model.BatchID;
                                            row.EndEdit();
                                            return row;
                                        }).ToList(),
                                    operatorConnectionString, DestinationTableName);

                                TempData["success"] = "User(s) added successfully for operator " + operatorName;
                            }
                            else
                            {
                                TempData["Error"] = operatorName + " operator implementation is under process.";
                            }
                        }
                    }
                    else
                    {
                        TempData["Error"] = operatorName + " operator implementation is under process.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
            }
            FillCountry(model.CountryId);
            return View("Index");
        }

        private async Task DoExpressoProvisionAndSaveToDatabase(List<string> nextChunk, string operatorConnectionString, string destinationTableName, PromotionalUserFormModel model)
        {
            var requests = nextChunk.Select(isdn => new ProvisionModelRequest { isdn = isdn, provision = true }).ToList();

            // calling Expresso HLR
            var provisionResults = await ExpressoOperator.ExpressoProvisionBatch(requests);

            // Saving records to Operator's DB.
            await DoSaveToDatabase(provisionResults,
                (source, table) => // this is a converter method that transforms all ProvisionRequests to a List of DataRows.
                                source
                                //.Where(ExpressoOperator.IsSuccess)   //TODO!!! uncomment to save ONLY NEWLY ONBOARDED (code==0001). Means only ACTIVE records will be saved to DB and Fail will be filtered out.
                                .Select(pr =>
                                {
                                    var row = table.NewRow();
                                    row.BeginEdit();
                                    row["MSISDN"] = pr.isdn;
                                    row["WeeklyPlay"] = 0;
                                    row["DailyPlay"] = 0;
                                    row["Status"] = ExpressoOperator.IsSuccess(pr)
                                        ? (int)PromotionalUserStatus.Active
                                        : (int)PromotionalUserStatus.Fail;
                                    row["BatchID"] = model.BatchID;
                                    row.EndEdit();
                                    return row;
                                }).ToList(),
                            operatorConnectionString, destinationTableName);
        }

        private async Task DoSaveToDatabase<T>(IEnumerable<T> source, Func<IEnumerable<T>, DataTable, List<DataRow>> rowConverter, string operatorConnectionString, string destinationTableName)
        {
            // creating inmemory table
            DataTable table = new DataTable("PromotionalUsers");
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("MSISDN", typeof(string));
            table.Columns.Add("BatchID", typeof(int));
            table.Columns.Add("WeeklyPlay", typeof(int));
            table.Columns.Add("DailyPlay", typeof(int));
            table.Columns.Add("Status", typeof(int));
            table.BeginInit();

            // converting source items to DataRow instances via rowConverter.
            List<DataRow> promotionalRecords = rowConverter(source, table);

            // adding all converted DataRows to inmemory table.
            promotionalRecords.ForEach(table.Rows.Add);

            table.EndInit();

            // bulk insert.
            using (SqlBulkCopy copy = new SqlBulkCopy(operatorConnectionString))
            {
                copy.BatchSize = 10000;
                copy.DestinationTableName = destinationTableName;
                await copy.WriteToServerAsync(table, DataRowState.Added);
            }
        }

        //Method to get data from csv,xls,xlsx file
        private List<MSISDNModel> ImportExcel(int OperatorId, HttpPostedFileBase file)
        {
            List<MSISDNModel> MSISDNData = new List<MSISDNModel>();

            //Save the uploaded Excel file.
            var fileName = DateTime.Now.Ticks + System.IO.Path.GetFileName(file.FileName);
            var filePath = System.IO.Path.Combine(Server.MapPath("~/PromotionalUserCSV/"), fileName);
            file.SaveAs(filePath);

            if (OperatorId == (int)OperatorTableId.Expresso)
            {
                //Open the Excel file using ClosedXML.
                using (XLWorkbook workBook = new XLWorkbook(filePath))
                {
                    //Read the first Sheet from Excel file.
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Create a new DataTable.
                    DataTable dt = new DataTable();

                    dt.Columns.Add("MSISDN");
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                        {
                            string MSISDN = "";
                            if (!String.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                string countryCode = cell.Value.ToString().Substring(0, 3);
                                if (countryCode == "221") MSISDN = cell.Value.ToString();
                                else MSISDN = string.Concat("221", cell.Value.ToString());
                                //dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                dt.Rows[dt.Rows.Count - 1][i] = MSISDN;
                                i++;
                            }
                        }
                    }
                    System.IO.File.Delete(filePath);
                    MSISDNData = CommonMethod.ConvertToList<MSISDNModel>(dt);
                    MSISDNData = MSISDNData.Where(top => !String.IsNullOrEmpty(top.MSISDN)).ToList();
                }
            }

            if (OperatorId == (int)OperatorTableId.Safaricom)
            {
                //Open the Excel file using ClosedXML.
                using (XLWorkbook workBook = new XLWorkbook(filePath))
                {
                    //Read the first Sheet from Excel file.
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Create a new DataTable.
                    DataTable dt = new DataTable();

                    dt.Columns.Add("MSISDN");
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                        {
                            string MSISDN = "";
                            if (!String.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                string countryCode = cell.Value.ToString().Substring(0, 3);
                                if (countryCode == "254") MSISDN = cell.Value.ToString();
                                else MSISDN = string.Concat("254", cell.Value.ToString());
                                //dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                dt.Rows[dt.Rows.Count - 1][i] = MSISDN;
                                i++;
                            }
                        }
                    }
                    System.IO.File.Delete(filePath);
                    MSISDNData = CommonMethod.ConvertToList<MSISDNModel>(dt);
                    MSISDNData = MSISDNData.Where(top => !String.IsNullOrEmpty(top.MSISDN)).ToList();
                }
            }
            return MSISDNData;
        }

        //Method to Get List From DataTable
        public static class CommonMethod
        {
            public static List<T> ConvertToList<T>(DataTable dt)
            {
                var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
                var properties = typeof(T).GetProperties();
                return dt.AsEnumerable().Select(row =>
                {
                    var objT = Activator.CreateInstance<T>();
                    foreach (var pro in properties)
                    {
                        if (columnNames.Contains(pro.Name.ToLower()))
                        {
                            try
                            {
                                pro.SetValue(objT, row[pro.Name]);
                            }
                            catch (Exception ex) { }
                        }
                    }
                    return objT;
                }).ToList();
            }
        }

        //Fill Country Drop Down List
        public void FillCountry(int countryId)
        {
            var countrydetails = _countryRepository.GetAll().Select(top => new
            {
                Name = top.Name,
                Id = top.Id
            }).ToList();
            ViewBag.countrydetails = new MultiSelectList(countrydetails, "Id", "Name");
            FillOperator(countryId);
        }

        //Fill Operator Drop Down List
        [HttpPost]
        [Route("FillOperator")]
        public ActionResult FillOperator(int countryId)
        {
            if (countryId == 0)
            {
                var operatordetails = _operatorRepository.GetMany(top => top.IsActive == true).Select(top => new
                {
                    Name = top.OperatorName,
                    Id = top.OperatorId
                }).ToList();
                ViewBag.operatordetails = new MultiSelectList(operatordetails, "Id", "Name");
            }
            else
            {
                var operatordetails = _operatorRepository.GetMany(top => top.CountryId == countryId && top.IsActive == true).Select(top => new
                {
                    Name = top.OperatorName,
                    Id = top.OperatorId
                }).ToList();
                ViewBag.operatordetails = new MultiSelectList(operatordetails, "Id", "Name");
            }
            return Json(ViewBag.operatordetails);
        }
    }
}