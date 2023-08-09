using System;
using System.Collections.Generic;
using System.Configuration;
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
using EFMVC.Web.Helpers;
using EFMVC.Web.Models;
using EFMVC.Web.SearchClass;
using EFMVC.Web.ViewModels;
using System.IO;
namespace EFMVC.Web.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    public class AdminQuestionController : Controller
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
        public AdminQuestionController(ICommandBus commandBus, IUserRepository userRepository, IQuestionRepository questionRepository, IClientRepository clientRepository, IQuestionSubjectRepository questionSubjectRepository, IPaymentMethodRepository paymentMethodRepository, IQuestionImagesRepository questionImageRepository, IQuestionCommentRepository questioncommentRepository, IQuestionCommentImagesRepository questioncommentImageRepository)
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
        public ActionResult Index()
        {
            List<HelpAdminResult> _result = FillHelpResult();
            HelpAdminFilter _filterCritearea = new HelpAdminFilter();
            FillClientDropdown(0);
            FillQuestionSubjectDropdown(0);
            FillQuestionStatus();
            FillPaymentDropdown(0);
            FillUserDropdown();
            return View(Tuple.Create(_result, _filterCritearea));

        }
        public ActionResult QuestionDetails(int id)
        {
            QuestionCommentFormModel _comment = new QuestionCommentFormModel();
            var _questionDetails = _questionRepository.GetMany(top => top.Id == id).FirstOrDefault();
            _comment.QuestionId = id;
            _comment.Title = _questionDetails.Title;
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            ViewBag.userId = efmvcUser.UserId;
            return View(Tuple.Create(_comment, _questionDetails));

        }
        public ActionResult SearchQuestion([Bind(Prefix = "Item2")]HelpAdminFilter _filterCritearea)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<HelpAdminResult> _result = new List<HelpAdminResult>();
                var finalresult = new List<HelpAdminResult>();
                if (_filterCritearea != null)
                {
                    _result = FillHelpResult();
                    finalresult = gethelpResult(_result, _filterCritearea);
                }
                else
                {
                    _result = FillHelpResult();
                    finalresult = gethelpResult(_result, _filterCritearea);
                }

                return PartialView("_HelpList", finalresult);
            }
            else
            {
                return PartialView("_HelpList", "notauthorise");
            }
        }
        public List<HelpAdminResult> gethelpResult(List<HelpAdminResult> helpresult, HelpAdminFilter _filterCritearea)
        {
            if (helpresult != null && _filterCritearea != null)
            {
                if (!String.IsNullOrEmpty(_filterCritearea.ID))
                {
                    int id = Convert.ToInt32(_filterCritearea.ID);
                    helpresult = helpresult.Where(top => top.Id == id).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.UserId.ToString()))
                {
                    if (_filterCritearea.UserId.ToString() != "0")
                    {
                        int userId = _filterCritearea.UserId;
                        helpresult = helpresult.Where(top => top.userId == userId).ToList();
                    }
                }
                if (!String.IsNullOrEmpty(_filterCritearea.ClientId.ToString()))
                {
                    if (_filterCritearea.ClientId.ToString() != "0")
                    {
                        int clientid = _filterCritearea.ClientId;
                        helpresult = helpresult.Where(top => top.ClientId == clientid).ToList();
                    }
                }

                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    helpresult = helpresult.Where(top => top.QuestionDateTime.Value.Date >= _filterCritearea.Fromdate.Value.Date && top.QuestionDateTime.Value.Date <= _filterCritearea.Todate.Value.Date).ToList();
                }
                if ((_filterCritearea.LastResponseFromdate != null && _filterCritearea.LastResponseTodate != null))
                {
                    helpresult = helpresult.Where(top => top.LastResponseDatetime != null).ToList();
                    helpresult = helpresult.Where(top => top.LastResponseDatetime.Value.Date >= _filterCritearea.LastResponseFromdate.Value.Date && top.LastResponseDatetime.Value.Date <= _filterCritearea.LastResponseTodate.Value.Date).ToList();
                }

                if (!String.IsNullOrEmpty(_filterCritearea.SubjectId.ToString()))
                {
                    if (_filterCritearea.SubjectId.ToString() != "0")
                    {
                        int SubjectId = _filterCritearea.SubjectId;
                        helpresult = helpresult.Where(top => top.QuestionSubjectId == SubjectId).ToList();
                    }
                }
                if (!String.IsNullOrEmpty(_filterCritearea.Status.ToString()))
                {
                    if (_filterCritearea.Status.ToString() != "0")
                    {
                        int Status = _filterCritearea.Status;
                        helpresult = helpresult.Where(top => top.Status == Status).ToList();
                    }
                }
                if (!String.IsNullOrEmpty(_filterCritearea.PaymentMethodId.ToString()))
                {
                    if (_filterCritearea.PaymentMethodId.ToString() != "0")
                    {
                        int PaymentMethodId = _filterCritearea.PaymentMethodId;
                        helpresult = helpresult.Where(top => top.PaymentMethodId == PaymentMethodId).ToList();
                    }
                }
            }
            return helpresult;
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
            questionstatus.Insert(0, new SelectListItem { Text = "--Select Status--", Value = "0" });
            ViewBag.questionstatus = questionstatus;
        }

        public void FillUserDropdown()
        {

            var userdetails = _userRepository.GetAll().Select(top => new SelectListItem
            {
                Text = top.FirstName + " " + top.LastName,
                Value = top.UserId.ToString(),
            });
            var user = userdetails.ToList();
            user.Insert(0, new SelectListItem { Text = "--Select User--", Value = "0" });
            ViewBag.user = user;
        }
        public void FillClientDropdown(int type)
        {

            var clientdetails = _clientRepository.GetAll().Select(top => new SelectListItem
            {
                Text = top.Name,
                Value = top.Id.ToString(),
            });
            var client = clientdetails.ToList();
            if (type == 0)
            {
                client.Insert(0, new SelectListItem { Text = "--Select Client--", Value = "0" });
            }
            ViewBag.client = client;
        }
        public void FillQuestionSubjectDropdown(int type)
        {

            var Qsubjectdetails = _questionSubjectRepository.GetAll().Select(top => new SelectListItem
            {
                Text = top.Name,
                Value = top.SubjectId.ToString(),
            });
            var Qsubject = Qsubjectdetails.ToList();
            if (type == 0)
            {
                Qsubject.Insert(0, new SelectListItem { Text = "--Select Subject--", Value = "0" });
            }
            ViewBag.Qsubject = Qsubject;
        }
        public void FillPaymentDropdown(int type)
        {

            var paymentMethoddetails = _paymentMethodRepository.GetAll().Select(top => new SelectListItem
            {
                Text = top.Description,
                Value = top.Id.ToString(),
            });
            var paymentMethod = paymentMethoddetails.ToList();
            if (type == 0)
            {
                paymentMethod.Insert(0, new SelectListItem { Text = "--Select Method--", Value = "0" });
            }
            ViewBag.paymentMethod = paymentMethod;
        }
        public List<HelpAdminResult> FillHelpResult()
        {
            List<HelpAdminResult> _helpResult = new List<HelpAdminResult>();
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var result = _questionRepository.GetAll().OrderBy(top=>top.Status).ThenByDescending(top=>top.LastResponseDateTimeByUser);
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
                _helpResult.Add(new HelpAdminResult { userName = item.User.FirstName + " " + item.User.LastName, userEmail = item.User.Email, userId = item.UserId, QANumber = item.QNumber, Id = item.Id, ClientId = clientId, ClientName = clientname, CampaignProfileId = campaingnId, CampaignName = campaignname, QuestionDateTime = item.CreatedDate, QuestionTitle = item.Title, QuestionSubject = subject, QuestionSubjectId = questionSubjectId, Status = item.Status, LastResponseDatetime = item.LastResponseDateTime, LastResponseDateTimeByUser = item.LastResponseDateTimeByUser, PaymentMethodId = paymentMethodId, fStatus = status });
            }
            return _helpResult;
        }
        public JsonResult CloseQuestion(int id)
        {
            var item = _questionRepository.GetMany(top => top.Id == id).FirstOrDefault();
            if (item != null)
            {
                QuestionFormModel question =
                   Mapper.Map<Question, QuestionFormModel>(item);
                question.Status = 3;
                CreateOrUpdateQuestionCommand command =
                 Mapper.Map<QuestionFormModel, CreateOrUpdateQuestionCommand>(question);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
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
                }
                _model.UserId = efmvcUser.UserId;
                _model.ResponseDatetime = DateTime.Now;

                CreateOrUpdateQuestionCommentCommand command =
                Mapper.Map<QuestionCommentFormModel, CreateOrUpdateQuestionCommentCommand>(_model);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
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
                        if (_model.Id == 0)
                        {
                            TempData["msgsuccess"] = "Comment added successfully.";
                        }
                        else
                        {
                            TempData["msgsuccess"] = "Comment updated successfully.";
                        }
                    }
                    else
                    {
                        TempData["msgerror"] = "Internal Server error.so please try again.";
                    }
                    return RedirectToAction("QuestionDetails", "AdminQuestion", new { @id = _model.QuestionId });

                }

                TempData["questionCommentId"] = null;
            }
            return View();
        }
        private Boolean UpdateResponseDatetimeandStatus(int? QuestionId)
        {

            var _questionDetails = _questionRepository.GetMany(top => top.Id == QuestionId).FirstOrDefault();
            _questionDetails.LastResponseDateTime = DateTime.Now;
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
            _questioncomment.QuestionCommentImages = commentsDetails.QuestionCommentImages;
            _questioncomment.ResponseDatetime = commentsDetails.ResponseDatetime;
            _questioncomment.Title = commentsDetails.Title;
            _questioncomment.Description = commentsDetails.Description;

            return PartialView("_AddComment", _questioncomment);
        }
        public ActionResult DeleteComment(int commentId)
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
            _questionDetails.LastResponseDateTime = DateTime.Now;
            _questionDetails.Status = 4;
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
