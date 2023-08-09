using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Models;
using EFMVC.Web.SearchClass;
using EFMVC.Web.ViewModels;

namespace EFMVC.Web.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Advertiser")]
    public class HelpController : Controller
    {
        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileRepository;


        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IQuestionSubjectRepository _questionSubjectRepository;


        /// <summary>
        /// The _question repository
        /// </summary>
        private readonly IQuestionRepository _questionRepository;

        /// <summary>
        /// The _question repository
        /// </summary>
        private readonly IQuestionImagesRepository _questionImageRepository;

        /// <summary>
        /// The _questioncommentImage repository
        /// </summary>
        private readonly IQuestionCommentImagesRepository _questioncommentImageRepository;

        /// <summary>
        /// The _questioncomment repository
        /// </summary>
        private readonly IQuestionCommentRepository _questioncommentRepository;
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        public HelpController(ICommandBus commandBus, IClientRepository clientRepository, ICampaignProfileRepository profileRepository, IBillingRepository billingRepository, IPaymentMethodRepository paymentMethodRepository, IUsersCreditRepository userCreditRepository, IQuestionSubjectRepository questionSubjectRepository, IQuestionRepository questionRepository, IQuestionImagesRepository questionImageRepository, IQuestionCommentImagesRepository questioncommentImageRepository, IQuestionCommentRepository questioncommentRepository, IUserRepository userRepository)
        {
            _commandBus = commandBus;
            _clientRepository = clientRepository;
            _profileRepository = profileRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _questionSubjectRepository = questionSubjectRepository;
            _questionRepository = questionRepository;
            _questionImageRepository = questionImageRepository;
            _questioncommentImageRepository = questioncommentImageRepository;
            _questioncommentRepository = questioncommentRepository;
            _userRepository = userRepository;
        }
        public ActionResult Index()
        {
            //List<HelpResult> _result = FillHelpResult();
            List<HelpResult> _result = new List<HelpResult>();
            FillClientDropdown(0);
            FillQuestionSubjectDropdown(0);
            FillQuestionStatus();
            FillPaymentDropdown(0);
            HelpFilter _filterCritearea = new HelpFilter();
            return View(Tuple.Create(_result, _filterCritearea));
        }
        public List<HelpResult> FillHelpResult()
        {
            List<HelpResult> _helpResult = new List<HelpResult>();
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            //var result = _questionRepository.GetMany(top => top.UserId == efmvcUser.UserId).OrderBy(top => top.Status).ThenByDescending(top => top.LastResponseDateTime);
            var result = _questionRepository.GetMany(top => top.SubjectId != (int)QuestionSubjectStatus.AdvertError && top.SubjectId != (int)QuestionSubjectStatus.Adreview && top.SubjectId != (int)QuestionSubjectStatus.OperatorAdreview && top.UserId == efmvcUser.UserId).OrderByDescending(top => top.CreatedDate).ToList();
            foreach (var item in result)
            {
                string clientname = string.Empty;
                string campaignname = string.Empty;
                string subject = string.Empty;
                int? clientId = 0;
                int? campaingnId = 0;
                int? questionSubjectId = 0;
                int? paymentMethodId = 0;
                string status = string.Empty;
                if (item.Client != null)
                {
                    clientname = item.Client.Name;
                    clientId = item.ClientId;
                }
                if (item.CampaignProfile != null)
                {
                    campaignname = item.CampaignProfile.CampaignName;
                    campaingnId = item.CampaignProfileId;
                }

                if (item.QuestionSubject != null)
                {
                    subject = item.QuestionSubject.Name;
                    questionSubjectId = item.SubjectId;
                }
                if (item.PaymentMethod != null)
                {
                    paymentMethodId = item.PaymentMethodId;

                }
                QuestionStatus qStatus = (QuestionStatus)item.Status;
                status = qStatus.ToString();

                //CultureInfo provider = CultureInfo.InvariantCulture;
                //string date = item.CreatedDate.Value.ToString();
                //string format = "dd/MM/yyyy hh:mm";
                //provider = new CultureInfo("fr-FR");
                //DateTime setdate = DateTime.ParseExact(date, format, provider);

                //_helpResult.Add(new HelpResult { QANumber = item.QNumber, Id = item.Id, ClientId = clientId, ClientName = clientname, CampaignProfileId = campaingnId, CampaignName = campaignname, QuestionDateTime = item.CreatedDate, QuestionTitle = item.Title, QuestionSubject = subject, QuestionSubjectId = questionSubjectId, Status = item.Status, LastResponseDatetime = item.LastResponseDateTime, PaymentMethodId = paymentMethodId, fStatus = status });
                _helpResult.Add(new HelpResult { QANumber = item.QNumber, Id = item.Id, ClientId = clientId, ClientName = clientname, CampaignProfileId = campaingnId, CampaignName = campaignname, QuestionDateTime = item.CreatedDate == null ? null : item.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), QuestionDateTimeSort = item.CreatedDate == null ? null : item.CreatedDate, QuestionTitle = item.Title, QuestionSubject = subject, QuestionSubjectId = questionSubjectId, Status = item.Status, LastResponseDatetime = item.LastResponseDateTime == null ? null : item.LastResponseDateTime.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), LastResponseDatetimeSort = item.LastResponseDateTime == null ? null : item.LastResponseDateTime, PaymentMethodId = paymentMethodId, fStatus = status });
            }
            return _helpResult;
        }

        //Add 26-06-2019
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                List<HelpResult> _result = new List<HelpResult>();
                IEnumerable<Question> help = null;
                DateTimeFormat dateTimeConvert = new DateTimeFormat();
                string status = string.Empty;
                ViewBag.SearchResult = false;
                var cnt = 10;
                int userId = 0;

                bool searchValue = false;
                List<String> columnSearch = new List<string>();

                if (param.Columns != null)
                {
                    foreach (var col in param.Columns)
                    {
                        columnSearch.Add(col.Search.Value);
                        if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
                            searchValue = true;
                    }
                }

                if (searchValue == true)
                {
                    #region Search Functionality
                    string TicketNo = "";
                    int?[] ClientId = new int?[cnt];
                    int[] StatusId = new int[cnt];
                    int[] ModeofPaymentId = new int[cnt];
                    int[] SubjectId = new int[cnt];
                    DateTime CreatedDatefromdate = new DateTime();
                    DateTime CreatedDatetodate = new DateTime();
                    DateTime LastResponseDatefromdate = new DateTime();
                    DateTime LastResponseDatetodate = new DateTime();
                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null")
                        {
                            TicketNo = columnSearch[0].ToString();
                        }
                        else
                        {
                            columnSearch[0] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[1]))
                    {
                        if (columnSearch[1] != "null")
                        {
                            ClientId = columnSearch[1].Split(',').Select(a => (int?)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[1] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[2]))
                    {
                        if (columnSearch[2] != "null")
                        {
                            var data = columnSearch[2].Split(',').ToArray();
                            CreatedDatefromdate = Convert.ToDateTime(data[0]);
                            CreatedDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else
                        {
                            columnSearch[2] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[3]))
                    {
                        if (columnSearch[3] != "null")
                        {
                            var data = columnSearch[3].Split(',').ToArray();
                            LastResponseDatefromdate = Convert.ToDateTime(data[0]);
                            LastResponseDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else
                        {
                            columnSearch[3] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[4]))
                    {
                        if (columnSearch[4] != "null")
                        {
                            StatusId = columnSearch[4].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[4] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[5]))
                    {
                        if (columnSearch[5] != "null")
                        {
                            ModeofPaymentId = columnSearch[5].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[5] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[6]))
                    {
                        if (columnSearch[6] != "null")
                        {
                            SubjectId = columnSearch[6].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[6] = null;
                        }
                    }

                    help = _questionRepository.GetMany(top => top.SubjectId != (int)QuestionSubjectStatus.AdvertError && top.SubjectId != (int)QuestionSubjectStatus.Adreview && top.SubjectId != (int)QuestionSubjectStatus.OperatorAdreview && top.UserId == efmvcUser.UserId).OrderByDescending(top => top.CreatedDate).ToList();
                    if (columnSearch[0] != null)
                    {
                        help = help.Where(top => top.QNumber == TicketNo).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        help = help.Where(top => (ClientId.Contains(Convert.ToInt32(top.ClientId)))).ToList();
                    }
                    if (columnSearch[2] != null)
                    {
                        help = help.Where(top => (top.CreatedDate >= CreatedDatefromdate && top.CreatedDate <= CreatedDatetodate)).ToList();
                    }
                    if (columnSearch[3] != null)
                    {
                        help = help.Where(top => (top.LastResponseDateTime >= LastResponseDatefromdate && top.LastResponseDateTime <= LastResponseDatetodate)).ToList();
                    }
                    if (columnSearch[4] != null)
                    {
                        help = help.Where(top => (StatusId.Contains((int)top.Status))).ToList();
                    }
                    if (columnSearch[5] != null)
                    {
                        help = help.Where(top => (ModeofPaymentId.Contains((int)top.PaymentMethodId))).ToList();
                    }
                    if (columnSearch[6] != null)
                    {
                        help = help.Where(top => (SubjectId.Contains((int)top.QuestionSubject.SubjectId))).ToList();
                    }

                    cnt = help.Count();
                    help = help.Skip(param.Start).Take(param.Length);

                    #endregion
                }
                else
                {
                    help = _questionRepository.GetMany(top => top.SubjectId != (int)QuestionSubjectStatus.AdvertError && top.SubjectId != (int)QuestionSubjectStatus.Adreview && top.SubjectId != (int)QuestionSubjectStatus.OperatorAdreview && top.UserId == efmvcUser.UserId).OrderByDescending(top => top.CreatedDate).ToList();
                    cnt = help.Count();
                    help = help.Skip(param.Start).Take(param.Length);
                }

                if (help.Count() > 0)
                {
                    foreach (var item in help)
                    {
                        string clientname = string.Empty;
                        string campaignname = string.Empty;
                        string subject = string.Empty;
                        int? clientId = 0;
                        int? campaingnId = 0;
                        int? questionSubjectId = 0;
                        int? paymentMethodId = 0;
                        string fstatus = string.Empty;

                        if (item.Client != null)
                        {
                            clientname = item.Client.Name;
                            clientId = item.ClientId;
                        }
                        if (item.CampaignProfile != null)
                        {
                            campaignname = item.CampaignProfile.CampaignName;
                            campaingnId = item.CampaignProfileId;
                        }

                        if (item.QuestionSubject != null)
                        {
                            subject = item.QuestionSubject.Name;
                            questionSubjectId = item.SubjectId;
                        }
                        if (item.PaymentMethod != null)
                        {
                            paymentMethodId = item.PaymentMethodId;

                        }
                        QuestionStatus qStatus = (QuestionStatus)item.Status;
                        fstatus = qStatus.ToString();

                        _result.Add(new HelpResult { QANumber = item.QNumber, Id = item.Id, ClientId = clientId, ClientName = clientname, CampaignProfileId = campaingnId, CampaignName = campaignname, QuestionDateTime = item.CreatedDate == null ? null : item.CreatedDate.Value.ToString("dd/MM/yyyy  HH:mm:ss tt"), QuestionDateTimeSort = item.CreatedDate == null ? null : item.CreatedDate, QuestionTitle = item.Title, QuestionSubject = subject, QuestionSubjectId = questionSubjectId, Status = item.Status, LastResponseDatetime = item.LastResponseDateTime == null ? null : item.LastResponseDateTime.Value.ToString("dd/MM/yyyy  HH:mm:ss tt"), LastResponseDatetimeSort = item.LastResponseDateTime == null ? null : item.LastResponseDateTime, PaymentMethodId = paymentMethodId, fStatus = fstatus });
                    }
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<HelpResult> result = new DTResult<HelpResult>
                {
                    draw = param.Draw,
                    data = _result,
                    recordsFiltered = cnt,
                    recordsTotal = cnt
                };

                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        //Add 26-06-2019
        // Function For Filter/Sorting Help Data
        private static List<HelpResult> ApplySorting(DTParameters param, List<HelpResult> result)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.QANumber).ToList();
                    else
                        result = result.OrderByDescending(top => top.QANumber).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.ClientName).ToList();
                    else
                        result = result.OrderByDescending(top => top.ClientName).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CampaignName).ToList();
                    else
                        result = result.OrderByDescending(top => top.CampaignName).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.QuestionDateTimeSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.QuestionDateTimeSort).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.QuestionTitle).ToList();
                    else
                        result = result.OrderByDescending(top => top.QuestionTitle).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.QuestionSubject).ToList();
                    else
                        result = result.OrderByDescending(top => top.QuestionSubject).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.fStatus).ToList();
                    else
                        result = result.OrderByDescending(top => top.fStatus).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.LastResponseDatetimeSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.LastResponseDatetimeSort).ToList();
                }
            }
            return result;
        }

        public void FillQuestionStatus()
        {
            IEnumerable<Common.QuestionStatus> questionstatusTypes = Enum.GetValues(typeof(Common.QuestionStatus))
                                                     .Cast<Common.QuestionStatus>();
            //var questionstatus = (from action in questionstatusTypes
            //                      select new SelectListItem
            //                      {
            //                          Text = action.ToString(),
            //                          Value = ((int)action).ToString()
            //                      }).ToList();
            var questionstatus = (from action in questionstatusTypes
                                  select new
                                  {
                                      Text = action.ToString(),
                                      Value = ((int)action).ToString()
                                  }).ToList();
            ViewBag.questionstatus = new MultiSelectList(questionstatus, "Value", "Text");
            //Comment 30-05-2019
            //questionstatus.Insert(0, new SelectListItem { Text = "--Select Status--", Value = "0" });
            //ViewBag.questionstatus = questionstatus;
        }
        public void FillClientDropdown(int type)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var clientdetails = _clientRepository.GetAll().Where(top => top.UserId == efmvcUser.UserId).Select(top => new
            {
                Text = top.Name,
                Value = top.Id,
            }).ToList();
            var client = clientdetails.ToList();
            if (type == 0)
            {
                ViewBag.client = new MultiSelectList(client, "Value", "Text");
            }
            else
            {
                var clientdetails1 = _clientRepository.GetAll().Where(top => top.UserId == efmvcUser.UserId).Select(top => new SelectListItem
                {
                    Text = top.Name,
                    Value = top.Id.ToString(),
                });
                var client1 = clientdetails1.ToList();
                client1.Insert(0, new SelectListItem { Text = "--Select Client--", Value = "0" });
                ViewBag.client = client1;
            }
        }
        public void FillQuestionSubjectDropdown(int type)
        {
            //Commented 04-03-2019
            //var Qsubjectdetails = _questionSubjectRepository.GetAll().Select(top => new SelectListItem
            //{
            //    Text = top.Name,
            //    Value = top.SubjectId.ToString(),
            //});

            //Add 04-03-2019
            string[] questionList = { "Rewards & Credits", "Inappropriate Content", "Advert Error", "Ad review", "Operator Ad review" };
            var Qsubjectdetails = _questionSubjectRepository.GetMany(question => !questionList.Contains(question.Name)).Select(top => new
            {
                Text = top.Name,
                Value = top.SubjectId,
            }).ToList();

            var Qsubject = Qsubjectdetails.ToList();
            if (type == 0)
            {
                ViewBag.Qsubject = new MultiSelectList(Qsubjectdetails, "Value", "Text");
            }
            else
            {
                var Qsubjectdetails1 = _questionSubjectRepository.GetMany(question => !questionList.Contains(question.Name)).Select(top => new SelectListItem
                {
                    Text = top.Name,
                    Value = top.SubjectId.ToString(),
                });
                var Qsubject1 = Qsubjectdetails1.ToList();
                Qsubject1.Insert(0, new SelectListItem { Text = "--Select Subject--", Value = "0" });
                ViewBag.Qsubject = Qsubject1;
            }
        }
        public void FillPaymentDropdown(int type)
        {
            var paymentMethoddetails = _paymentMethodRepository.GetAll().Select(top => new
            {
                Text = top.Name,
                Value = top.Id,
            }).ToList();
            var paymentMethod = paymentMethoddetails.ToList();
            if (type == 0)
            {
                ViewBag.paymentMethod = new MultiSelectList(paymentMethod, "Value", "Text");
            }
            else
            {
                var paymentMethoddetails1 = _paymentMethodRepository.GetAll().Select(top => new SelectListItem
                {
                    Text = top.Description,
                    Value = top.Id.ToString(),
                });
                var paymentMethod1 = paymentMethoddetails1.ToList();
                paymentMethod1.Insert(0, new SelectListItem { Text = "--Select Method--", Value = "0" });
                ViewBag.paymentMethod = paymentMethod1;
            }
        }
        [HttpPost]
        public JsonResult GetCampaign(int ClientId)
        {
            TempData["ClientId"] = ClientId;
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var campaigndetails = _profileRepository.GetAll().Where(top => top.ClientId == ClientId && top.UserId == efmvcUser.UserId).Select(top => new SelectListItem
            {
                Text = top.CampaignName,
                Value = top.CampaignProfileId.ToString()
            });

            var campaigns = campaigndetails.ToList();
            campaigns.Insert(0, new SelectListItem { Text = "--Select Campaign--", Value = "0" });

            return Json(campaigns);
        }
        public ActionResult AddQuestion()
        {
            var campaign = new List<SelectListItem>();
            campaign.Add(new SelectListItem() { Text = "--Select campaign--", Value = " " });
            ViewBag.campaign = campaign;
            FillClientDropdown(1);
            FillQuestionSubjectDropdown(1);
            FillPaymentDropdown(1);
            return View();
        }
        [HttpPost]
        public ActionResult AddQuestion(QuestionFormModel _model, HttpPostedFileBase[] questionfile)
        {
            if (ModelState.IsValid)
            {
                if (_model.SubjectId == 2)
                {
                    bool status = ValidateBilling(_model);
                    if (status)
                    {
                        return RedirectToAction("AddQuestion", "Help");
                    }
                }
                string QNo = string.Empty;
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                if (_model.Id == 0)
                    _model.CreatedDate = DateTime.Now;
                _model.UpdatedDate = DateTime.Now;
                _model.LastResponseDateTime = null;
                _model.LastResponseDateTimeByUser = null;
                _model.Status = 1;
                _model.ClientId = _model.ClientId == 0 ? null : _model.ClientId;
                _model.CampaignProfileId = _model.CampaignProfileId == 0 ? null : _model.CampaignProfileId;
                _model.PaymentMethodId = _model.PaymentMethodId == 0 ? null : _model.PaymentMethodId;
                //generate QuestionNumber
                //var Quesno = _questionRepository.GetAll().OrderByDescending(p => p.Id).FirstOrDefault();
                //if (Quesno == null)
                //{
                //    QNo = "1";
                //}
                //else
                //{
                //    QNo = (Quesno.Id + 1).ToString();
                //}

                var email = _userRepository.GetById(efmvcUser.UserId).Email;
                string ticketCode = LiveAgent.CreateTicket(_model.Title, _model.Description, email);
                // _model.QNumber = "QA" + QNo;
                _model.QNumber = ticketCode;
                _model.UserId = efmvcUser.UserId;
                _model.Title = _model.Title.Trim();
                _model.Description = _model.Description;
                CreateOrUpdateQuestionCommand command =
                   Mapper.Map<QuestionFormModel, CreateOrUpdateQuestionCommand>(_model);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {

                    if (questionfile != null)
                    {
                        foreach (var item in questionfile)
                            UploadQuestionImages(efmvcUser, result, item);
                        //TempData["msgsuccess"] = "Record added successfully.";
                        TempData["msgsuccess"] = "Ticket ID " + _model.QNumber + " added successfully.";
                        return RedirectToAction("Index", "Help");
                    }
                    else
                    {
                        //TempData["msgsuccess"] = "Record added successfully.";
                        TempData["msgsuccess"] = "Ticket ID " + _model.QNumber + " added successfully.";
                        return RedirectToAction("Index", "Help");
                    }
                }

            }
            return RedirectToAction("Index", "Help");
        }

        private void UploadQuestionImages(EFMVCUser efmvcUser, ICommandResult result, HttpPostedFileBase item)
        {
            if (item != null)
            {
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(item.FileName);

                string directoryName = Server.MapPath("~/Question/");
                directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                string path = Path.Combine(directoryName, fileName + extension);
                item.SaveAs(path);

                string archiveDirectoryName = Server.MapPath("~/Question/Archive/");

                if (!Directory.Exists(archiveDirectoryName))
                    Directory.CreateDirectory(archiveDirectoryName);

                string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                item.SaveAs(archivePath);

                QuestionImagesFormModel _Qimages = new QuestionImagesFormModel();
                _Qimages.QuestionId = result.Id;
                _Qimages.UploadImage = string.Format("/Question/{0}/{1}", efmvcUser.UserId.ToString(),
                                                            fileName + extension);
                CreateOrUpdateQuestionImagesCommand qimagescommand =
                 Mapper.Map<QuestionImagesFormModel, CreateOrUpdateQuestionImagesCommand>(_Qimages);
                ICommandResult qimagesresult = _commandBus.Submit(qimagescommand);
                if (qimagesresult.Success)
                {
                }
            }
        }

        private bool ValidateBilling(QuestionFormModel _model)
        {
            var _error = false;
            if (_model.ClientId != null)
            {
                if (_model.ClientId == 0)
                {
                    TempData["error"] = "The ClientId field is required.";
                    _error = true;
                }
            }
            else
            {
                TempData["error"] = "The ClientId field is required.";
                _error = true;
            }

            if (_model.CampaignProfileId != null)
            {
                if (_model.CampaignProfileId == 0)
                {
                    TempData["error"] = "The CampaignProfileId field is required.";
                    _error = true;
                }
            }
            else
            {
                TempData["error"] = "The CampaignProfileId field is required.";
                _error = true;
            }

            if (_model.PaymentMethodId != null)
            {
                if (_model.PaymentMethodId == 0)
                {
                    TempData["error"] = "The PaymentMethodId field is required.";
                    _error = true;
                }
            }
            else
            {
                TempData["error"] = "The PaymentMethodId field is required.";
                _error = true;
            }
            return _error;
        }

        public ActionResult QuestionDetails(int id)
        {
            var _questionDetails = _questionRepository.GetMany(top => top.Id == id).OrderByDescending(top => top.Status == 2).ThenByDescending(top => top.Status == 1).ThenByDescending(top => top.LastResponseDateTime).FirstOrDefault();
            string closeMsg = "";
            if (_questionDetails.UpdatedBy != null && _questionDetails.Status == (int)QuestionStatus.Closed)
            {
                var userData = _userRepository.GetMany(s => s.UserId == _questionDetails.UpdatedBy).FirstOrDefault();
                if (userData != null)
                {
                    closeMsg = userData.FirstName + " " + userData.LastName + " closed the ticket";
                }
            }
            ViewBag.closeMsg = closeMsg;
            return View(_questionDetails);
        }
        public ActionResult SearchQuestion([Bind(Prefix = "Item2")]HelpFilter _filterCritearea, int[] ClientId, int[] SubjectId, int[] StatusId, int[] PaymentMethodId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<HelpResult> _result = new List<HelpResult>();
                var finalresult = new List<HelpResult>();
                if (_filterCritearea != null)
                {
                    _result = FillHelpResult();
                    finalresult = gethelpResult(_result, _filterCritearea, ClientId, SubjectId, StatusId, PaymentMethodId);
                }
                else
                {
                    _result = FillHelpResult();
                    finalresult = gethelpResult(_result, _filterCritearea, ClientId, SubjectId, StatusId, PaymentMethodId);
                }

                return PartialView("_HelpList", finalresult);
            }
            else
            {
                return PartialView("_HelpList", "notauthorise");
            }
        }
        public List<HelpResult> gethelpResult(List<HelpResult> helpresult, HelpFilter _filterCritearea, int[] ClientId, int[] SubjectId, int[] StatusId, int[] PaymentMethodId)
        {
            if (helpresult != null && _filterCritearea != null)
            {
                if (!String.IsNullOrEmpty(_filterCritearea.ID))
                {
                    // int id = Convert.ToInt32(_filterCritearea.ID);
                    helpresult = helpresult.Where(top => top.QANumber == _filterCritearea.ID).ToList();

                }

                //if (!String.IsNullOrEmpty(_filterCritearea.ClientId.ToString()))
                //{
                //    if (_filterCritearea.ClientId.ToString() != "0")
                //    {
                //        int clientid = _filterCritearea.ClientId;
                //        helpresult = helpresult.Where(top => top.ClientId == clientid).ToList();
                //    }
                //}
                if (ClientId != null)
                {
                    helpresult = helpresult.Where(top => ClientId.Contains((int)(top.ClientId == null ? 0 : top.ClientId))).ToList();
                }

                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    //Comment 06-06-2019
                    ////  helpresult = helpresult.Where(top => top.QuestionDateTime.Value.Date >= _filterCritearea.Fromdate.Value.Date && top.QuestionDateTime.Value.Date <= _filterCritearea.Todate.Value.Date).ToList();
                    //string strTodate = _filterCritearea.Todate;
                    //DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //string strFromdate = _filterCritearea.Fromdate;
                    //DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //helpresult = helpresult.Where(top => top.QuestionDateTime.Value.Date >= Fromdate && top.QuestionDateTime.Value.Date <= Todate).ToList();

                    //Add 06-06-2019
                    string strTodate = "";
                    string strFromdate = "";
                    DateTime? Todate = null;
                    DateTime? Fromdate = null;
                    //if (_filterCritearea.Todate.Substring(11, 8) == "12:00 PM")
                    //{
                    //    var date = _filterCritearea.Todate.Substring(0, 11);
                    //    var time = _filterCritearea.Todate.Substring(11, 8);
                    //    time = "23:59:00";
                    //    _filterCritearea.Todate = String.Concat(date, time);
                    //    strTodate = _filterCritearea.Todate;
                    //    //Todate = DateTime.ParseExact(strTodate.Substring(0, 15), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //    Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //}
                    //else
                    //{
                    //strTodate = _filterCritearea.Todate.Substring(0, 11) + DateTime.ParseExact(_filterCritearea.Todate.Substring(11, 8), "HH:mm", CultureInfo.InvariantCulture);
                    //Todate = DateTime.ParseExact(strTodate.Substring(0, 15), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    DateTime to = DateTime.Parse(_filterCritearea.Todate.Substring(11, 8));
                    string todatetime = _filterCritearea.Todate.Substring(0, 11) + Convert.ToString(to.ToString("HH:mm:ss"));
                    Todate = DateTime.ParseExact(todatetime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //}
                    //if (_filterCritearea.Fromdate.Substring(11, 8) == "12:00 PM")
                    //{
                    //    var date = _filterCritearea.Fromdate.Substring(0, 11);
                    //    var time = _filterCritearea.Fromdate.Substring(11, 8);
                    //    time = "23:59:00";
                    //    _filterCritearea.Fromdate = String.Concat(date, time);
                    //    strFromdate = _filterCritearea.Fromdate;
                    //    //Fromdate = DateTime.ParseExact(strFromdate.Substring(0, 15), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //    Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //}
                    //else
                    //{
                    //strFromdate = _filterCritearea.Fromdate.Substring(0, 11) + DateTime.ParseExact(_filterCritearea.Fromdate.Substring(11, 8), "HH:mm", CultureInfo.InvariantCulture);
                    //Fromdate = DateTime.ParseExact(strFromdate.Substring(0, 15), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    DateTime from = DateTime.Parse(_filterCritearea.Fromdate.Substring(11, 8));
                    string fromdatetime = _filterCritearea.Fromdate.Substring(0, 11) + Convert.ToString(from.ToString("HH:mm:ss"));
                    Fromdate = DateTime.ParseExact(fromdatetime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //}

                    //helpresult = helpresult.Where(top => DateTime.ParseExact(top.QuestionDateTime.Value.ToString("dd/MM/yyyy HH:mm").Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) >= Fromdate && DateTime.ParseExact(top.QuestionDateTime.Value.ToString("dd/MM/yyyy HH:mm").Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) <= Todate).ToList();
                    //helpresult = helpresult.Where(top => DateTime.ParseExact(top.QuestionDateTimeSort.Value.ToString("dd/MM/yyyy HH:mm").Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) >= Fromdate && DateTime.ParseExact(top.QuestionDateTimeSort.Value.ToString("dd/MM/yyyy HH:mm").Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) <= Todate).ToList();
                    helpresult = helpresult.Where(top => top.QuestionDateTimeSort.Value >= Fromdate.Value && top.QuestionDateTimeSort.Value <= Todate.Value).ToList();
                }
                if ((_filterCritearea.LastResponseFromdate != null && _filterCritearea.LastResponseTodate != null))
                {
                    //Comment 06-06-2019
                    //helpresult = helpresult.Where(top => top.LastResponseDatetime != null).ToList();
                    //// helpresult = helpresult.Where(top => top.LastResponseDatetime.Value.Date >= _filterCritearea.LastResponseFromdate.Value.Date && top.LastResponseDatetime.Value.Date <= _filterCritearea.LastResponseTodate.Value.Date).ToList();
                    //string strTodate = _filterCritearea.LastResponseTodate;
                    //DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //string strFromdate = _filterCritearea.LastResponseFromdate;
                    //DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //helpresult = helpresult.Where(top => top.LastResponseDatetime.Value.Date >= Fromdate && top.LastResponseDatetime.Value.Date <= Todate).ToList();

                    //Add 03-06-2019
                    string strTodate = "";
                    string strFromdate = "";
                    DateTime? Todate = null;
                    DateTime? Fromdate = null;
                    helpresult = helpresult.Where(top => top.LastResponseDatetime != null).ToList();
                    //if (_filterCritearea.LastResponseTodate.Substring(11, 8) == "12:00 PM")
                    //{
                    //    var date = _filterCritearea.LastResponseTodate.Substring(0, 11);
                    //    var time = _filterCritearea.LastResponseTodate.Substring(11, 8);
                    //    time = "23:59:00";
                    //    _filterCritearea.LastResponseTodate = String.Concat(date, time);
                    //    strTodate = _filterCritearea.LastResponseTodate;
                    //    //Todate = DateTime.ParseExact(strTodate.Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //    Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //}
                    //else
                    //{
                    //strTodate = _filterCritearea.LastResponseTodate.Substring(0, 11) + DateTime.Parse(_filterCritearea.LastResponseTodate.Substring(11, 8)).ToString().Substring(11, 8);
                    //Todate = DateTime.ParseExact(strTodate.Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    DateTime to = DateTime.Parse(_filterCritearea.LastResponseTodate.Substring(11, 8));
                    string todatetime = _filterCritearea.LastResponseTodate.Substring(0, 11) + Convert.ToString(to.ToString("HH:mm:ss"));
                    Todate = DateTime.ParseExact(todatetime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //}
                    //if (_filterCritearea.LastResponseFromdate.Substring(11, 8) == "12:00 PM")
                    //{
                    //    var date = _filterCritearea.LastResponseFromdate.Substring(0, 11);
                    //    var time = _filterCritearea.LastResponseFromdate.Substring(11, 8);
                    //    time = "23:59:00";
                    //    _filterCritearea.LastResponseFromdate = String.Concat(date, time);
                    //    strFromdate = _filterCritearea.LastResponseFromdate;
                    //    //Fromdate = DateTime.ParseExact(strFromdate.Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //    Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //}
                    //else
                    //{
                    //strFromdate = _filterCritearea.LastResponseFromdate.Substring(0, 11) + DateTime.Parse(_filterCritearea.LastResponseFromdate.Substring(11, 8)).ToString().Substring(11, 8);
                    //Fromdate = DateTime.ParseExact(strFromdate.Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    DateTime from = DateTime.Parse(_filterCritearea.LastResponseFromdate.Substring(11, 8));
                    string fromdatetime = _filterCritearea.LastResponseFromdate.Substring(0, 11) + Convert.ToString(from.ToString("HH:mm:ss"));
                    Fromdate = DateTime.ParseExact(fromdatetime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //}

                    //helpresult = helpresult.Where(top => DateTime.ParseExact(top.LastResponseDatetime.Value.ToString("dd/MM/yyyy HH:mm").Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) >= Fromdate && DateTime.ParseExact(top.LastResponseDatetime.Value.ToString("dd/MM/yyyy HH:mm").Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) <= Todate).ToList();
                    //helpresult = helpresult.Where(top => DateTime.ParseExact(top.LastResponseDatetimeSort.Value.ToString("dd/MM/yyyy HH:mm").Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) >= Fromdate && DateTime.ParseExact(top.LastResponseDatetimeSort.Value.ToString("dd/MM/yyyy HH:mm").Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) <= Todate).ToList();
                    helpresult = helpresult.Where(top => top.LastResponseDatetimeSort.Value >= Fromdate.Value && top.LastResponseDatetimeSort.Value <= Todate.Value).ToList();
                }

                //if (!String.IsNullOrEmpty(_filterCritearea.SubjectId.ToString()))
                //{
                //    if (_filterCritearea.SubjectId.ToString() != "0")
                //    {
                //        int SubjectId = _filterCritearea.SubjectId;
                //        helpresult = helpresult.Where(top => top.QuestionSubjectId == SubjectId).ToList();
                //    }
                //}
                if (SubjectId != null)
                {
                    // helpresult = helpresult.Where(top => SubjectId.Contains(top.QuestionSubjectId)).ToList();
                    helpresult = helpresult.Where(top => SubjectId.Contains(top.QuestionSubjectId.Value)).ToList();
                }

                //if (!String.IsNullOrEmpty(_filterCritearea.Status.ToString()))
                //{
                //    if (_filterCritearea.Status.ToString() != "0")
                //    {
                //        int Status = _filterCritearea.Status;
                //        helpresult = helpresult.Where(top => top.Status == Status).ToList();
                //    }
                //}
                if (StatusId != null)
                {
                    helpresult = helpresult.Where(top => StatusId.Contains(top.Status)).ToList();
                }

                //if (!String.IsNullOrEmpty(_filterCritearea.PaymentMethodId.ToString()))
                //{
                //    if (_filterCritearea.PaymentMethodId.ToString() != "0")
                //    {
                //        int PaymentMethodId = _filterCritearea.PaymentMethodId;
                //        helpresult = helpresult.Where(top => top.PaymentMethodId == PaymentMethodId).ToList();
                //    }
                //}
                if (PaymentMethodId != null)
                {
                    helpresult = helpresult.Where(top => PaymentMethodId.Contains((int)(top.PaymentMethodId == null ? 0 : top.PaymentMethodId))).ToList();
                }
            }
            return helpresult;
        }
        public JsonResult CloseQuestion(int id)
        {
            var item = _questionRepository.GetMany(top => top.Id == id).FirstOrDefault();
            if (item != null)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                QuestionFormModel question =
                   Mapper.Map<Question, QuestionFormModel>(item);
                question.Status = (int)QuestionStatus.Closed;
                question.UpdatedBy = efmvcUser.UserId;
                question.UpdatedDate = DateTime.Now;
                CreateOrUpdateQuestionCommand command =
                 Mapper.Map<QuestionFormModel, CreateOrUpdateQuestionCommand>(question);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    var email = _userRepository.GetById(efmvcUser.UserId).Email;
                    string ticketCode = LiveAgent.ReplyTicket(item.Title, item.Description, email, "R", null, (int)UserRole.Advertiser);
                    return Json("success");

                }


            }
            return Json("fail");
        }
        public JsonResult GetScreenshot(int id)
        {

            string imagename = String.Empty;
            var url = ConfigurationManager.AppSettings["siteAddress"];
            foreach (var item in _questionImageRepository.GetMany(top => top.QuestionId == id).ToList())
            {
                if (string.IsNullOrEmpty(imagename))
                {

                    imagename = url + item.UploadImage;
                }
                else
                {

                    var urls = url + item.UploadImage;
                    imagename = imagename + "," + urls;
                }
            }

            return Json(imagename);
        }

        public JsonResult GetCommentScreenshot(int id)
        {

            string imagename = String.Empty;
            var url = ConfigurationManager.AppSettings["siteAddress"];
            foreach (var item in _questioncommentImageRepository.GetMany(top => top.QuestionCommentId == id).ToList())
            {
                if (string.IsNullOrEmpty(imagename))
                {

                    imagename = url + item.UploadImages;
                }
                else
                {

                    var urls = url + item.UploadImages;
                    imagename = imagename + "," + urls;
                }
            }

            return Json(imagename);
        }
        [HttpPost]
        public ActionResult AddComment(QuestionCommentFormModel _model, HttpPostedFileBase[] commentfile)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                if (TempData["questionCommentId"] != null)
                {
                    _model.Id = Convert.ToInt32(TempData["questionCommentId"]);
                    //LiveAgent.DeleteTicket(TempData["TicketId"].ToString());
                }
                _model.UserId = efmvcUser.UserId;
                _model.ResponseDatetime = DateTime.Now;

                var email = _userRepository.GetById(efmvcUser.UserId).Email;

                //string ticketCode = LiveAgent.ReplyTicket(_model.Title, _model.Description, email, "C", null, (int)UserRole.Advertiser);
                //_model.TicketCode = ticketCode;
                _model.TicketCode = null;
                CreateOrUpdateQuestionCommentCommand command =
                Mapper.Map<QuestionCommentFormModel, CreateOrUpdateQuestionCommentCommand>(_model);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //var email = _userRepository.GetById(efmvcUser.UserId).Email;
                    //LiveAgent.CreateTicket(_model.Title, _model.Description, email);
                    if (_model.Id == 0)
                    {
                        foreach (var item in commentfile)
                            UploadQuestionCommentImages(efmvcUser, result, item, 0);
                    }
                    else
                    {
                        if (commentfile[0] != null)
                        {
                            var QuestionComments = _questioncommentImageRepository.GetMany(top => top.QuestionCommentId == _model.Id);
                            foreach (var item in QuestionComments)
                            {
                                var questioncImages = new DeleteQuestionCommentImagesCommand { Id = Convert.ToInt32(item.Id) };
                                if (_commandBus != null) _commandBus.Submit(questioncImages);
                            }
                            foreach (var item in commentfile)
                                UploadQuestionCommentImages(efmvcUser, result, item, _model.Id);
                        }




                    }
                    var status = UpdateResponseDatetimeandStatus(_model.QuestionId);
                    if (status)
                    {
                        //var qNumber = _questionRepository.GetById(Convert.ToInt32(_model.QuestionId)).QNumber;
                        var userName = _userRepository.GetById(efmvcUser.UserId);
                        if (_model.Id == 0)
                        {
                            //TempData["msgsuccess"] = "Comment added successfully.";
                            //TempData["msgsuccess"] = "Comment added successfully for " + qNumber;
                            TempData["msgsuccess"] = "Comment added successfully by " + userName.FirstName + " " + userName.LastName;
                        }
                        else
                        {
                            //TempData["msgsuccess"] = "Comment updated successfully.";
                            //TempData["msgsuccess"] = "Comment updated successfully for " + qNumber;
                            TempData["msgsuccess"] = "Comment updated successfully by " + userName.FirstName + " " + userName.LastName;
                        }
                    }
                    else
                    {
                        TempData["msgerror"] = "Internal Server error.so please try again.";
                    }
                    return RedirectToAction("QuestionDetails", "Help", new { @id = _model.QuestionId });

                }
                TempData["questionCommentId"] = null;
                TempData["TicketId"] = null;
            }
            return View();
        }
        private Boolean UpdateResponseDatetimeandStatus(int? QuestionId)
        {

            var _questionDetails = _questionRepository.GetMany(top => top.Id == QuestionId).FirstOrDefault();
            _questionDetails.LastResponseDateTimeByUser = DateTime.Now;
            QuestionFormModel QuestionCommand =
                Mapper.Map<Question, QuestionFormModel>(_questionDetails);
            CreateOrUpdateQuestionCommand command =
                   Mapper.Map<QuestionFormModel, CreateOrUpdateQuestionCommand>(QuestionCommand);
            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                return true;
            }
            return false;
        }
        private void UploadQuestionCommentImages(EFMVCUser efmvcUser, ICommandResult result, HttpPostedFileBase item, int questioncommentId)
        {
            if (item != null)
            {
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(item.FileName);

                string directoryName = Server.MapPath("~/QuestionComment/");
                directoryName = Path.Combine(directoryName, efmvcUser.UserId.ToString());

                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                string path = Path.Combine(directoryName, fileName + extension);
                item.SaveAs(path);

                string archiveDirectoryName = Server.MapPath("~/QuestionComment/Archive/");

                if (!Directory.Exists(archiveDirectoryName))
                    Directory.CreateDirectory(archiveDirectoryName);

                string archivePath = Path.Combine(archiveDirectoryName, fileName + extension);
                item.SaveAs(archivePath);

                QuestionCommentImagesFormModel _Qimages = new QuestionCommentImagesFormModel();
                if (questioncommentId == 0)
                {
                    _Qimages.QuestionCommentId = result.Id;
                }
                else
                {
                    _Qimages.QuestionCommentId = questioncommentId;
                }
                _Qimages.UploadImages = string.Format("/QuestionComment/{0}/{1}", efmvcUser.UserId.ToString(),
                                                            fileName + extension);
                CreateOrUpdateQuestionCommentImagesCommand qimagescommand =
                 Mapper.Map<QuestionCommentImagesFormModel, CreateOrUpdateQuestionCommentImagesCommand>(_Qimages);
                ICommandResult qimagesresult = _commandBus.Submit(qimagescommand);
                if (qimagesresult.Success)
                {
                }
            }
        }

        [HttpPost]
        public ActionResult UpdateComment(int commentId)
        {

            var commentsDetails = _questioncommentRepository.GetMany(top => top.Id == commentId).FirstOrDefault();
            QuestionCommentFormModel _questioncomment = new QuestionCommentFormModel();
            _questioncomment.Id = commentsDetails.Id;
            _questioncomment.QuestionId = commentsDetails.QuestionId;
            if (commentsDetails.QuestionCommentImages.Count > 0)
            {
                ViewBag.QuestionCommentImages = commentsDetails.QuestionCommentImages;
                ViewBag.QuestionCommentId = commentsDetails.Id;
            }
            else
            {
                ViewBag.QuestionCommentImages = null;
                ViewBag.QuestionCommentId = commentsDetails.Id;

            }
            TempData["questionCommentId"] = commentsDetails.Id;
            TempData["TicketId"] = commentsDetails.TicketCode;
            _questioncomment.QuestionCommentImages = commentsDetails.QuestionCommentImages;
            _questioncomment.ResponseDatetime = commentsDetails.ResponseDatetime;
            _questioncomment.Title = commentsDetails.Title;
            _questioncomment.Description = commentsDetails.Description;

            return PartialView("_AddComment", _questioncomment);
        }
        public ActionResult DeleteComment(int commentId, string ticketCode)
        {
            try
            {
                var QuestionComments = _questioncommentImageRepository.GetMany(top => top.QuestionCommentId == commentId);
                foreach (var item in QuestionComments)
                {
                    var questioncImages = new DeleteQuestionCommentImagesCommand { Id = Convert.ToInt32(item.Id) };
                    if (_commandBus != null) _commandBus.Submit(questioncImages);
                }
                var questioncomment = new DeleteQuestionCommentCommand { Id = Convert.ToInt32(commentId) };
                if (_commandBus != null) _commandBus.Submit(questioncomment);
                if (!string.IsNullOrEmpty(ticketCode)) LiveAgent.DeleteTicket(ticketCode);
                TempData["msgsuccess"] = "comment deleted successfully";
                return Json("success");
            }
            catch (Exception ex)
            {
                TempData["msgerror"] = ex.InnerException.Message;
                return Json("error");

            }

        }
        public ActionResult DeleteQuestion(int? questionId)
        {
            var _questionDetails = _questionRepository.GetMany(top => top.Id == questionId).FirstOrDefault();
            _questionDetails.Status = (int)QuestionStatus.Archived;
            QuestionFormModel QuestionCommand =
                Mapper.Map<Question, QuestionFormModel>(_questionDetails);
            CreateOrUpdateQuestionCommand command =
                   Mapper.Map<QuestionFormModel, CreateOrUpdateQuestionCommand>(QuestionCommand);
            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                return Json("success");
            }
            return Json("fail");
        }
    }
}
