﻿using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Web.Areas.Users.Models;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.Users.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "User")]
    [RouteArea("Users")]
    [RoutePrefix("Ticket")]
    public class TicketController : Controller
    {
        //
        // GET: /AdminQuestion/

        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;
        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IQuestionSubjectRepository _questionSubjectRepository;
        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        /// <summary>
        /// The _question repository
        /// </summary>
        private readonly IQuestionRepository _questionRepository;

        /// <summary>
        /// The _question repository
        /// </summary>
        private readonly IQuestionImagesRepository _questionImageRepository;
        /// <summary>
        /// The _question repository
        /// </summary>
        private readonly IQuestionCommentRepository _questioncommentRepository;
        /// <summary>
        /// The _questioncommentImage repository
        /// </summary>
        private readonly IQuestionCommentImagesRepository _questioncommentImageRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public TicketController(ICommandBus commandBus, IUserRepository userRepository, IQuestionRepository questionRepository, IClientRepository clientRepository, IQuestionSubjectRepository questionSubjectRepository, IPaymentMethodRepository paymentMethodRepository, IQuestionImagesRepository questionImageRepository, IQuestionCommentRepository questioncommentRepository, IQuestionCommentImagesRepository questioncommentImageRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _clientRepository = clientRepository;
            _questionSubjectRepository = questionSubjectRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _questionImageRepository = questionImageRepository;
            _questioncommentRepository = questioncommentRepository;
            _questioncommentImageRepository = questioncommentImageRepository;
        }
        [Route("Index")]
        public ActionResult Index()
        {
            //List<TicketResult> _result = FillHelpResult();
            List<TicketResult> _result = new List<TicketResult>();
            TicketFilter _filterCritearea = new TicketFilter();
            FillQuestionSubjectDropdown();
            FillQuestionStatus();
            return View(Tuple.Create(_result, _filterCritearea));
        }

        public List<TicketResult> FillHelpResult()
        {
            List<TicketResult> _helpResult = new List<TicketResult>();
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            // IOrderedEnumerable<Question> result = _questionRepository.GetMany(top => top.UserId == efmvcUser.UserId).OrderBy(top => top.Status).ThenByDescending(top => top.LastResponseDateTimeByUser);
            IOrderedEnumerable<Question> result = _questionRepository.GetMany(top => top.UserId == efmvcUser.UserId).OrderByDescending(top => top.Id);
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
                //_helpResult.Add(new TicketResult { userId = (int)(item.UserId), ClientId = (item.ClientId), PaymentMethodId = (item.PaymentMethodId), userName = item.User.FirstName + " " + item.User.LastName, userEmail = item.User.Email, fuserId = item.UserId, QANumber = item.QNumber, Id = item.Id, fClientId = clientId, ClientName = clientname, CampaignProfileId = campaingnId, CampaignName = campaignname, QuestionDateTime = item.CreatedDate, QuestionTitle = item.Title, QuestionSubject = subject, fQuestionSubjectId = questionSubjectId, Status = item.Status, LastResponseDatetime = item.LastResponseDateTime, LastResponseDateTimeByUser = item.LastResponseDateTimeByUser, fPaymentMethodId = paymentMethodId, fStatus = status, Organisation = item.User.Organisation });
                _helpResult.Add(new TicketResult { userId = (int)(item.UserId), ClientId = (item.ClientId), PaymentMethodId = (item.PaymentMethodId), userName = item.User.FirstName + " " + item.User.LastName, userEmail = item.User.Email, fuserId = item.UserId, QANumber = item.QNumber, Id = item.Id, fClientId = clientId, ClientName = clientname, CampaignProfileId = campaingnId, CampaignName = campaignname, QuestionDateTime = item.CreatedDate == null ? null : item.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), QuestionDateTimeSort = item.CreatedDate == null ? null : item.CreatedDate, QuestionTitle = item.Title, QuestionSubject = subject, fQuestionSubjectId = questionSubjectId, Status = item.Status, LastResponseDatetime = item.LastResponseDateTime == null ? null : item.LastResponseDateTime.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), LastResponseDatetimeSort = item.LastResponseDateTime == null ? null : item.LastResponseDateTime, LastResponseDateTimeByUser = item.LastResponseDateTimeByUser == null ? null : item.LastResponseDateTimeByUser.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), LastResponseDateTimeByUserSort = item.LastResponseDateTimeByUser == null ? null : item.LastResponseDateTimeByUser, fPaymentMethodId = paymentMethodId, fStatus = status, Organisation = item.User.Organisation });
            }
            return _helpResult;
        }

        //Add 26-06-2019
        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                List<TicketResult> _result = new List<TicketResult>();
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
                    int[] StatusId = new int[cnt];
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
                            var data = columnSearch[1].Split(',').ToArray();
                            CreatedDatefromdate = Convert.ToDateTime(data[0]);
                            CreatedDatetodate = Convert.ToDateTime(data[1]);
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
                            LastResponseDatefromdate = Convert.ToDateTime(data[0]);
                            LastResponseDatetodate = Convert.ToDateTime(data[1]);
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
                            StatusId = columnSearch[3].Split(',').Select(int.Parse).ToArray();
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
                            SubjectId = columnSearch[4].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[4] = null;
                        }
                    }

                    help = _questionRepository.GetMany(top => top.SubjectId != (int)QuestionSubjectStatus.AdvertError && top.UserId == efmvcUser.UserId).OrderByDescending(top => top.Id).ToList();
                    if (columnSearch[0] != null)
                    {
                        help = help.Where(top => top.QNumber == TicketNo).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        help = help.Where(top => (top.CreatedDate >= CreatedDatefromdate && top.CreatedDate <= CreatedDatetodate)).ToList();
                    }
                    if (columnSearch[2] != null)
                    {
                        help = help.Where(top => (top.LastResponseDateTime >= LastResponseDatefromdate && top.LastResponseDateTime <= LastResponseDatetodate)).ToList();
                    }
                    if (columnSearch[3] != null)
                    {
                        help = help.Where(top => (StatusId.Contains((int)top.Status))).ToList();
                    }
                    if (columnSearch[4] != null)
                    {
                        help = help.Where(top => (SubjectId.Contains((int)top.QuestionSubject.SubjectId))).ToList();
                    }

                    cnt = help.Count();

                    help = help.Skip(param.Start).Take(param.Length);

                    #endregion
                }
                else
                {
                    help = _questionRepository.GetMany(top => top.SubjectId != (int)QuestionSubjectStatus.AdvertError && top.UserId == efmvcUser.UserId).OrderByDescending(top => top.Id).ToList();
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

                        _result.Add(new TicketResult { userId = (int)(item.UserId), ClientId = (item.ClientId), PaymentMethodId = (item.PaymentMethodId), userName = item.User.FirstName + " " + item.User.LastName, userEmail = item.User.Email, fuserId = item.UserId, QANumber = item.QNumber, Id = item.Id, fClientId = clientId, ClientName = clientname, CampaignProfileId = campaingnId, CampaignName = campaignname, QuestionDateTime = item.CreatedDate == null ? null : item.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), QuestionDateTimeSort = item.CreatedDate == null ? null : item.CreatedDate, QuestionTitle = item.Title, QuestionSubject = subject, fQuestionSubjectId = questionSubjectId, Status = item.Status, LastResponseDatetime = item.LastResponseDateTime == null ? null : item.LastResponseDateTime.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), LastResponseDatetimeSort = item.LastResponseDateTime == null ? null : item.LastResponseDateTime, LastResponseDateTimeByUser = item.LastResponseDateTimeByUser == null ? null : item.LastResponseDateTimeByUser.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), LastResponseDateTimeByUserSort = item.LastResponseDateTimeByUser == null ? null : item.LastResponseDateTimeByUser, fPaymentMethodId = paymentMethodId, fStatus = fstatus, Organisation = item.User.Organisation });
                    }
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<TicketResult> result = new DTResult<TicketResult>
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
        private static List<TicketResult> ApplySorting(DTParameters param, List<TicketResult> result)
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
                        result = result.OrderBy(top => top.QuestionDateTimeSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.QuestionDateTimeSort).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.QuestionTitle).ToList();
                    else
                        result = result.OrderByDescending(top => top.QuestionTitle).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.QuestionSubject).ToList();
                    else
                        result = result.OrderByDescending(top => top.QuestionSubject).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.fStatus).ToList();
                    else
                        result = result.OrderByDescending(top => top.fStatus).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.LastResponseDateTimeByUserSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.LastResponseDateTimeByUserSort).ToList();
                }
            }
            return result;
        }

        public void FillQuestionSubjectDropdown()
        {
            //Commented 04-03-2019
            //var Qsubjectdetails = _questionSubjectRepository.GetAll().Select(top => new
            //{
            //    Name = top.Name,
            //    SubjectId = top.SubjectId,
            //}).ToList();

            //Add 04-03-2019
            string[] questionList = { "Technical", "Billing", "Rewards & Credits", "Inappropriate Content", "Other" };
            var Qsubjectdetails = _questionSubjectRepository.GetMany(question => questionList.Contains(question.Name)).Select(top => new
            {
                Name = top.Name,
                SubjectId = top.SubjectId,
            }).ToList();

            ViewBag.Qsubject = new MultiSelectList(Qsubjectdetails, "SubjectId", "Name");
        }

        public void FillQuestionStatus()
        {
            IEnumerable<Common.QuestionStatus> questionstatusTypes = Enum.GetValues(typeof(Common.QuestionStatus))
                                                     .Cast<Common.QuestionStatus>();
            var questionstatus = (from action in questionstatusTypes
                                  select new SelectListItem
                                  {
                                      Text = action.ToString(),
                                      Value = ((int)action).ToString()
                                  }).ToList();


            ViewBag.questionstatus = new MultiSelectList(questionstatus, "Value", "Text");

        }

        [Route("AddQuestion")]
        public ActionResult AddQuestion()
        {
            var campaign = new List<SelectListItem>();
            campaign.Add(new SelectListItem() { Text = "--Select campaign--", Value = " " });
            ViewBag.campaign = campaign;
            FillQuestionSubjectDropdown();
            return View();
        }

        [Route("AddQuestion")]
        [HttpPost]
        public ActionResult AddQuestion(QuestionFormModel _model, HttpPostedFileBase[] questionfile)
        {
            if (ModelState.IsValid)
            {

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
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //TempData["msgsuccess"] = "Record added successfully.";
                        TempData["msgsuccess"] = "Ticket ID " + _model.QNumber + " added successfully.";
                        return RedirectToAction("Index");
                    }
                }

            }
            return RedirectToAction("Index");
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


        [Route("SearchTicket")]
        [HttpPost]
        public ActionResult SearchTicket([Bind(Prefix = "Item2")]TicketFilter _filterCritearea, int[] UserId, int[] ClientId, int[] SubjectId, int[] StatusId, int[] PaymentMethodId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<TicketResult> _result = new List<TicketResult>();
                var finalresult = new List<TicketResult>();
                if (_filterCritearea != null)
                {
                    _result = FillHelpResult();
                    finalresult = gethelpResult(_result, _filterCritearea, UserId, ClientId, SubjectId, StatusId, PaymentMethodId);
                }
                else
                {
                    _result = FillHelpResult();
                    finalresult = gethelpResult(_result, _filterCritearea, UserId, ClientId, SubjectId, StatusId, PaymentMethodId);
                }

                return PartialView("_TicketList", finalresult);
            }
            else
            {
                return PartialView("_TicketList", "notauthorise");
            }
        }

        [Route("TicketDetails")]
        public ActionResult TicketDetails(int id)
        {
            QuestionCommentFormModel _comment = new QuestionCommentFormModel();
            var _questionDetails = _questionRepository.GetMany(top => top.Id == id).FirstOrDefault();
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
            _comment.QuestionId = id;
            _comment.Title = _questionDetails.Title;
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            ViewBag.userId = efmvcUser.UserId;
            return View(Tuple.Create(_comment, _questionDetails));

        }

        [Route("CloseTicket")]
        public JsonResult CloseTicket(int id)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var item = _questionRepository.GetMany(top => top.Id == id).FirstOrDefault();
            if (item != null)
            {
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
                    string ticketCode = LiveAgent.ReplyTicket(item.Title, item.Description, email, "R", null, (int)UserRole.User);
                    return Json("success");
                }
            }
            return Json("fail");
        }
        [Route("GetScreenshot")]
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
        [Route("GetCommentScreenshot")]
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

        public List<TicketResult> gethelpResult(List<TicketResult> helpresult, TicketFilter _filterCritearea, int[] UserId, int[] ClientId, int[] SubjectId, int[] StatusId, int[] PaymentMethodId)
        {
            if (helpresult != null && _filterCritearea != null)
            {
                if (!String.IsNullOrEmpty(_filterCritearea.ID))
                {
                    //int id = Convert.ToInt32(_filterCritearea.ID);
                    helpresult = helpresult.Where(top => top.QANumber == _filterCritearea.ID).ToList();

                }
                if (UserId != null)
                {
                    helpresult = helpresult.Where(top => UserId.Contains(top.userId)).ToList();
                }
                //if (!String.IsNullOrEmpty(_filterCritearea.UserId.ToString()))
                //{
                //    if (_filterCritearea.UserId.ToString() != "0")
                //    {
                //        int userId = _filterCritearea.UserId;
                //        helpresult = helpresult.Where(top => top.userId == userId).ToList();
                //    }
                //}
                if (ClientId != null)
                {
                    helpresult = helpresult.Where(top => ClientId.Contains((int)(top.ClientId == null ? 0 : top.ClientId))).ToList();
                }
                //if (!String.IsNullOrEmpty(_filterCritearea.ClientId.ToString()))
                //{
                //    if (_filterCritearea.ClientId.ToString() != "0")
                //    {
                //        int clientid = _filterCritearea.ClientId;
                //        helpresult = helpresult.Where(top => top.ClientId == clientid).ToList();
                //    }
                //}

                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    //Comment 04-06-2019
                    ////  helpresult = helpresult.Where(top => top.QuestionDateTime.Value.Date >= _filterCritearea.Fromdate.Value.Date && top.QuestionDateTime.Value.Date <= _filterCritearea.Todate.Value.Date).ToList();
                    //string strTodate = _filterCritearea.Todate;
                    //DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //string strFromdate = _filterCritearea.Fromdate;
                    //DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //helpresult = helpresult.Where(top => top.QuestionDateTime.Value.Date >= Fromdate && top.QuestionDateTime.Value.Date <= Todate).ToList();

                    //Add 04-06-2019
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
                    //    //Todate = DateTime.ParseExact(strTodate.Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //    Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //}
                    //else
                    //{
                    //strTodate = _filterCritearea.Todate.Substring(0, 11) + DateTime.Parse(_filterCritearea.Todate.Substring(11, 8)).ToString().Substring(11, 8);
                    //Todate = DateTime.ParseExact(strTodate.Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
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
                    //    //Fromdate = DateTime.ParseExact(strFromdate.Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //}
                    //else
                    //{
                    //strFromdate = _filterCritearea.Fromdate.Substring(0, 11) + DateTime.Parse(_filterCritearea.Fromdate.Substring(11, 8)).ToString().Substring(11, 8);
                    //Fromdate = DateTime.ParseExact(strFromdate.Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    DateTime from = DateTime.Parse(_filterCritearea.Fromdate.Substring(11, 8));
                    string fromdatetime = _filterCritearea.Fromdate.Substring(0, 11) + Convert.ToString(from.ToString("HH:mm:ss"));
                    Fromdate = DateTime.ParseExact(fromdatetime, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    //}

                    //helpresult = helpresult.Where(top => DateTime.ParseExact(top.QuestionDateTimeSort.Value.ToString("dd/MM/yyyy HH:mm").Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) >= Fromdate && DateTime.ParseExact(top.QuestionDateTimeSort.Value.ToString("dd/MM/yyyy HH:mm").Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) <= Todate).ToList();
                    helpresult = helpresult.Where(top => top.QuestionDateTimeSort.Value >= Fromdate && top.QuestionDateTimeSort.Value <= Todate).ToList();
                }
                if ((_filterCritearea.LastResponseFromdate != null && _filterCritearea.LastResponseTodate != null))
                {
                    //Comment 04-06-2019
                    //helpresult = helpresult.Where(top => top.LastResponseDatetime != null).ToList();
                    //// helpresult = helpresult.Where(top => top.LastResponseDatetime.Value.Date >= _filterCritearea.LastResponseFromdate.Value.Date && top.LastResponseDatetime.Value.Date <= _filterCritearea.LastResponseTodate.Value.Date).ToList();
                    //string strTodate = _filterCritearea.LastResponseTodate;
                    //DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //string strFromdate = _filterCritearea.LastResponseFromdate;
                    //DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //helpresult = helpresult.Where(top => top.LastResponseDatetime.Value.Date >= Fromdate && top.LastResponseDatetime.Value.Date <= Todate).ToList();

                    //Add 04-06-2019
                    string strTodate = "";
                    string strFromdate = "";
                    DateTime? Todate = null;
                    DateTime? Fromdate = null;
                    helpresult = helpresult.Where(top => top.LastResponseDateTimeByUser != null).ToList();
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

                    //helpresult = helpresult.Where(top => DateTime.ParseExact(top.LastResponseDateTimeByUserSort.Value.ToString("dd/MM/yyyy HH:mm").Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) >= Fromdate && DateTime.ParseExact(top.LastResponseDateTimeByUserSort.Value.ToString("dd/MM/yyyy HH:mm").Substring(0, 16), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) <= Todate).ToList();
                    helpresult = helpresult.Where(top => top.LastResponseDateTimeByUserSort.Value >= Fromdate && top.LastResponseDateTimeByUserSort.Value <= Todate).ToList();
                }
                if (SubjectId != null)
                {
                    // helpresult = helpresult.Where(top => SubjectId.Contains(top.QuestionSubjectId)).ToList();
                    helpresult = helpresult.Where(top => SubjectId.Contains(top.fQuestionSubjectId.Value)).ToList();
                }
                //if (!String.IsNullOrEmpty(_filterCritearea.SubjectId.ToString()))
                //{
                //    if (_filterCritearea.SubjectId.ToString() != "0")
                //    {
                //        int SubjectId = _filterCritearea.SubjectId;
                //        helpresult = helpresult.Where(top => top.QuestionSubjectId == SubjectId).ToList();
                //    }
                //}
                if (StatusId != null)
                {
                    helpresult = helpresult.Where(top => StatusId.Contains(top.Status)).ToList();
                }
                //if (!String.IsNullOrEmpty(_filterCritearea.Status.ToString()))
                //{
                //    if (_filterCritearea.Status.ToString() != "0")
                //    {
                //        int Status = _filterCritearea.Status;
                //        helpresult = helpresult.Where(top => top.Status == Status).ToList();
                //    }
                //}
                if (PaymentMethodId != null)
                {
                    helpresult = helpresult.Where(top => PaymentMethodId.Contains((int)(top.PaymentMethodId == null ? 0 : top.PaymentMethodId))).ToList();
                }
                //if (!String.IsNullOrEmpty(_filterCritearea.PaymentMethodId.ToString()))
                //{
                //    if (_filterCritearea.PaymentMethodId.ToString() != "0")
                //    {
                //        int PaymentMethodId = _filterCritearea.PaymentMethodId;
                //        helpresult = helpresult.Where(top => top.PaymentMethodId == PaymentMethodId).ToList();
                //    }
                //}
            }
            return helpresult;
        }

        [Route("GetClientsUser")]
        [HttpPost]
        public ActionResult GetClientsUser(int[] userId)
        {
            try
            {
                if (userId != null)
                {

                    var clientdetails = _clientRepository.GetAll().Where(top => userId.Contains((int)(top.UserId))).Select(top => new
                    {
                        Name = top.Name,
                        Id = top.Id
                    }).ToList();
                    return Json(clientdetails);

                }
                else
                {
                    var clientdetails = _clientRepository.GetAll().Select(top => new
                    {
                        Name = top.Name,
                        Id = top.Id
                    }).ToList();
                    return Json(clientdetails);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }

        [Route("AddComment")]
        [HttpPost]
        public ActionResult AddComment(QuestionCommentFormModel _model, HttpPostedFileBase[] commentfile)
        {

            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                if (TempData["questionCommentId"] != null)
                {
                    _model.Id = Convert.ToInt32(TempData["questionCommentId"]);
                    // LiveAgent.DeleteTicket(TempData["TicketId"].ToString());
                }
                _model.UserId = efmvcUser.UserId;
                _model.ResponseDatetime = DateTime.Now;
                var email = _userRepository.GetById(efmvcUser.UserId).Email;

                //string ticketCode = LiveAgent.ReplyTicket(_model.Title, _model.Description, email, "C", null, (int)UserRole.User);
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
                        var userName = _userRepository.GetById(efmvcUser.UserId);
                        if (_model.Id == 0)
                        {
                            //TempData["msgsuccess"] = "Comment added successfully.";
                            TempData["msgsuccess"] = "Comment added successfully by " + userName.FirstName + " " + userName.LastName;
                        }
                        else
                        {
                            //TempData["msgsuccess"] = "Comment updated successfully.";
                            TempData["msgsuccess"] = "Comment updated successfully by " + userName.FirstName + " " + userName.LastName;
                        }
                    }
                    else
                    {
                        TempData["msgerror"] = "Internal Server error.so please try again.";
                    }
                    return RedirectToAction("TicketDetails", "Ticket", new { @id = _model.QuestionId });

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
            _questionDetails.Status = 2;
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

        [Route("DeleteTicket")]
        public ActionResult DeleteTicket(int? questionId)
        {
            var _questionDetails = _questionRepository.GetMany(top => top.Id == questionId).FirstOrDefault();
            _questionDetails.LastResponseDateTime = DateTime.Now;
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
        [Route("UpdateTicket")]
        [HttpPost]
        public ActionResult UpdateTicket(int commentId)
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

        [Route("DeleteComment")]
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
    }
}