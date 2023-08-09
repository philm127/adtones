using EFMVC.Data;
using EFMVC.Model;
using EFMVC.Model.Entities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace EFMVC.Web.Controllers
{
    public class CSVImportController : Controller
    {
        EFMVCDataContex db = new EFMVCDataContex();
        
        public ActionResult Index()
        {
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath("~/Safaricom/"));
            var allFiles = directory.GetFiles().ToList();
            foreach (var item in allFiles)
            {

                var fileName = System.IO.Path.GetFileName(item.FullName);
                var path = System.IO.Path.Combine(Server.MapPath("~/Safaricom/"), fileName);

                StreamReader sr = new StreamReader(path);

                DataTable importUserCsvDt = new DataTable();
                importUserCsvDt.Columns.Add("PhoneNumber");
                importUserCsvDt.Columns.Add("OperationType");
                importUserCsvDt.Columns.Add("Email");
                importUserCsvDt.Columns.Add("DateCreated");
                importUserCsvDt.Columns.Add("AddedDate");
                importUserCsvDt.Columns.Add("Proceed");
                


                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    try
                    {

                        if (!string.IsNullOrEmpty(line))
                        {
                            string[] rowsArray = line.Split(';');
                            string FirstValue = rowsArray[0];
                            if (FirstValue.ToLower() != "header" && FirstValue.ToLower() != "trailer")
                            {

                                if (rowsArray[1].ToUpper() == "E")
                                {
                                    DataRow row = importUserCsvDt.NewRow();                                  

                                    var Date = rowsArray[4];
                                    var year = Date.Substring(0, 4);
                                    var month = Date.Substring(4, 2);
                                    var day = Date.Substring(6, 2);
                                    var hour = Date.Substring(8, 2);
                                    var min = Date.Substring(10, 2);
                                    var sec = Date.Substring(12, 2);

                                    var strDate = year + "-" + month + "-" + day + " " + hour + ":" + min + ":" + sec;

                                    DateTime dateFormat = Convert.ToDateTime(strDate);
                                   
                                    row["PhoneNumber"] = rowsArray[0];
                                    row["OperationType"] = rowsArray[1];
                                    row["Email"] = rowsArray[2];
                                    row["DateCreated"] = dateFormat;
                                    row["AddedDate"] = DateTime.Now;
                                    row["Proceed"] = false;

                                    importUserCsvDt.Rows.Add(row);
                                    
                                }
                                
                            }
                        }

                        ViewBag.Window = "True";
                    }
                   
                    catch (Exception msg)
                    {
                        Console.WriteLine(msg);
                    }

                }

                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.DestinationTableName = "dbo.ImportUserCSVs";

                        sqlBulkCopy.ColumnMappings.Add("PhoneNumber", "PhoneNumber");
                        sqlBulkCopy.ColumnMappings.Add("OperationType", "OperationType");
                        sqlBulkCopy.ColumnMappings.Add("Email", "Email");
                        sqlBulkCopy.ColumnMappings.Add("DateCreated", "DateCreated");
                        sqlBulkCopy.ColumnMappings.Add("AddedDate", "AddedDate");
                        sqlBulkCopy.ColumnMappings.Add("Proceed", "Proceed");
                        con.Open();
                        sqlBulkCopy.WriteToServer(importUserCsvDt);
                        con.Close();
                    }                   
                }

                sr.Close();
                System.IO.File.Delete(path);

            }

            var conn = System.Configuration.ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString;

            using (SqlConnection con = new SqlConnection(conn))
            {

                using (SqlCommand cmd = new SqlCommand("CSVImport", con))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ViewBag.ExecuteNonQuery = "Yes";
                    con.Close();
                }
            }

            ViewBag.Window = "True";
            return View();
        }

     
    }
}