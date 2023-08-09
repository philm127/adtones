﻿using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Web.Areas.OperatorAdmin.Models;
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

namespace EFMVC.Web.Areas.OperatorAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "OperatorAdmin")]
    [RouteArea("OperatorAdmin")]
    [RoutePrefix("Ticket")]
    public class TicketController : Controller
    {
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
        private readonly IContactsRepository _contactsRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public TicketController(ICommandBus commandBus, IUserRepository userRepository, IQuestionRepository questionRepository, IClientRepository clientRepository, IQuestionSubjectRepository questionSubjectRepository, IPaymentMethodRepository paymentMethodRepository, IQuestionImagesRepository questionImageRepository, IQuestionCommentRepository questioncommentRepository, IQuestionCommentImagesRepository questioncommentImageRepository, IContactsRepository contactsRepository)
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
            _contactsRepository = contactsRepository;
        }

        // GET: OperatorAdmin/Ticket
        [Route("Index")]
        public ActionResult Index(int? userId)
        {
            //Add 16-04-2019
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var operatorAdminId = efmvcUser.UserId;
            //List<TicketResult> _result = FillHelpResult(userId, operatorAdminId);
            List<TicketResult> _result = new List<TicketResult>();
            TempData["UserId"] = userId;
            //Comment 16-04-2019
            //List<TicketResult> _result = FillHelpResult(userId);

            TicketFilter _filterCritearea = new TicketFilter();
            FillQuestionSubjectDropdown();
            FillQuestionStatus();
            FillUserDropdown(userId, operatorAdminId);
            return View(Tuple.Create(_result, _filterCritearea));
        }

        //Add 02-07-2019
        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                List<TicketResult> _result = new List<TicketResult>();
                IEnumerable<Question> help = null;
                string status = string.Empty;
                ViewBag.SearchResult = false;
                var cnt = 10;
                int userId = 0;

                if (TempData["UserId"] == null)
                {
                    userId = 0;
                }
                else
                {
                    userId = Convert.ToInt32(TempData["UserId"].ToString());
                }

                var operatorAdminId = efmvcUser.UserId;
                var userData = _userRepository.GetById(operatorAdminId);

                var operatorId = userData.OperatorId;
                var countryId = userData.Operator.CountryId;

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
                    int[] UserId = new int[cnt];
                    int[] CountryId = new int[cnt];
                    int[] OperatorId = new int[cnt];
                    int[] StatusId = new int[cnt];
                    int[] SubjectId = new int[cnt];
                    DateTime CreatedDatefromdate = new DateTime();
                    DateTime CreatedDatetodate = new DateTime();
                    DateTime LastResponseDatefromdate = new DateTime();
                    DateTime LastResponseDatetodate = new DateTime();

                    if (!String.IsNullOrEmpty(columnSearch[1]))
                    {
                        if (columnSearch[1] != "null")
                        {
                            UserId = columnSearch[1].Split(',').Select(a => (int)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[1] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[3]))
                    {
                        if (columnSearch[3] != "null")
                        {
                            TicketNo = columnSearch[3].ToString();
                        }
                        else
                        {
                            columnSearch[3] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[5]))
                    {
                        if (columnSearch[5] != "null")
                        {
                            var data = columnSearch[5].Split(',').ToArray();
                            CreatedDatefromdate = Convert.ToDateTime(data[0]);
                            CreatedDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else
                        {
                            columnSearch[5] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[7]))
                    {
                        if (columnSearch[7] != "null")
                        {
                            SubjectId = columnSearch[7].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[7] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[8]))
                    {
                        if (columnSearch[8] != "null")
                        {
                            StatusId = columnSearch[8].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[8] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[9]))
                    {
                        if (columnSearch[9] != "null")
                        {
                            var data = columnSearch[9].Split(',').ToArray();
                            LastResponseDatefromdate = Convert.ToDateTime(data[0]);
                            LastResponseDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else
                        {
                            columnSearch[9] = null;
                        }
                    }

                    if (userId != 0)
                    {
                        help = _questionRepository.GetMany(top => top.UserId == userId && (top.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || top.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest || top.SubjectId == (int)QuestionSubjectStatus.AdvertError || top.SubjectId == (int)QuestionSubjectStatus.Adreview)).OrderByDescending(top => top.CreatedDate);
                    }
                    else
                    {
                        help = _questionRepository.GetMany(s => s.UserId != null && (s.User.OperatorId == operatorId || s.User.OperatorId == 0) && (s.User.RoleId == 2 || s.User.RoleId == 3) &&
                        (s.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || s.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest || s.SubjectId == (int)QuestionSubjectStatus.AdvertError || s.SubjectId == (int)QuestionSubjectStatus.Adreview))
                        .OrderByDescending(top => top.CreatedDate);
                    }

                    if (columnSearch[0] != null)
                    {
                        help = help.Where(top => (UserId.Contains(Convert.ToInt32(top.UserId)))).ToList();
                    }
                    if (columnSearch[3] != null)
                    {
                        help = help.Where(top => top.QNumber == TicketNo).ToList();
                    }
                    if (columnSearch[5] != null)
                    {
                        help = help.Where(top => (top.CreatedDate >= CreatedDatefromdate && top.CreatedDate <= CreatedDatetodate)).ToList();
                    }
                    if (columnSearch[7] != null)
                    {
                        help = help.Where(top => (SubjectId.Contains((int)top.QuestionSubject.SubjectId))).ToList();
                    }
                    if (columnSearch[8] != null)
                    {
                        help = help.Where(top => (StatusId.Contains((int)top.Status))).ToList();
                    }
                    if (columnSearch[9] != null)
                    {
                        help = help.Where(top => (top.LastResponseDateTime >= LastResponseDatefromdate && top.LastResponseDateTime <= LastResponseDatetodate)).ToList();
                    }

                    cnt = help.Count();
                    help = help.Skip(param.Start).Take(param.Length);

                    #endregion
                }
                else
                {
                    if (userId != 0)
                    {
                        help = _questionRepository.GetMany(top => top.UserId == userId && (top.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || top.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest || top.SubjectId == (int)QuestionSubjectStatus.AdvertError || top.SubjectId == (int)QuestionSubjectStatus.Adreview)).OrderByDescending(top => top.CreatedDate);
                    }
                    else
                    {
                        // Code commented on 11/07/2019
                        //help = _questionRepository.GetMany(s => s.UserId != null && (s.User.OperatorId == operatorId || s.User.OperatorId == 0) && (s.User.RoleId == 2 || s.User.RoleId == 3) &&
                        //(s.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || s.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest || s.SubjectId == (int)QuestionSubjectStatus.AdvertError || s.SubjectId == (int)QuestionSubjectStatus.Adreview))
                        //.OrderByDescending(top => top.CreatedDate);

                        var userIdList = _contactsRepository.GetMany(s => s.CountryId == countryId).Select(s => s.UserId).ToList();
                        var advertiserIdList = _userRepository.GetMany(s => userIdList.Contains(s.UserId) && s.RoleId == 3).Select(s => s.UserId).ToList();

                        help = _questionRepository.GetMany(s => s.UserId != null && (s.User.OperatorId == operatorId || advertiserIdList.Contains((int)s.UserId)) &&
                       (s.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || s.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest || s.SubjectId == (int)QuestionSubjectStatus.AdvertError || s.SubjectId == (int)QuestionSubjectStatus.Adreview))
                       .OrderByDescending(top => top.CreatedDate);
                    }
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

                        _result.Add(new TicketResult { userId = (int)(item.UserId), ClientId = (item.ClientId), PaymentMethodId = (item.PaymentMethodId), userName = item.User.FirstName + " " + item.User.LastName, userEmail = item.User.Email, fuserId = item.UserId, QANumber = item.QNumber, Id = item.Id, fClientId = clientId, ClientName = clientname, CampaignProfileId = campaingnId, CampaignName = campaignname, QuestionDateTime = item.CreatedDate == null ? null : item.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), QuestionDateTimeSort = item.CreatedDate, QuestionTitle = item.Title, QuestionSubject = subject, QuestionSubjectId = int.Parse(questionSubjectId.Value.ToString()), fQuestionSubjectId = questionSubjectId, Status = item.Status, LastResponseDatetime = item.LastResponseDateTime == null ? null : item.LastResponseDateTime.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), LastResponseDatetimeSort = item.LastResponseDateTime, LastResponseDateTimeByUser = item.LastResponseDateTimeByUser == null ? null : item.LastResponseDateTimeByUser.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), LastResponseDateTimeByUserSort = item.LastResponseDateTimeByUser, fPaymentMethodId = paymentMethodId, fStatus = fstatus, Organisation = item.User.Organisation == null ? "-" : item.User.Organisation });
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

        //Add 02-07-2019
        // Function For Filter/Sorting Ticket Data
        private static List<TicketResult> ApplySorting(DTParameters param, List<TicketResult> result)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.userName).ToList();
                    else
                        result = result.OrderByDescending(top => top.userName).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.userEmail).ToList();
                    else
                        result = result.OrderByDescending(top => top.userEmail).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Organisation).ToList();
                    else
                        result = result.OrderByDescending(top => top.Organisation).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.QANumber).ToList();
                    else
                        result = result.OrderByDescending(top => top.QANumber).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CampaignName).ToList();
                    else
                        result = result.OrderByDescending(top => top.CampaignName).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.QuestionDateTimeSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.QuestionDateTimeSort).ToList();
                }
                else if (paramOrderDetails.Column == 6)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.QuestionTitle).ToList();
                    else
                        result = result.OrderByDescending(top => top.QuestionTitle).ToList();
                }
                else if (paramOrderDetails.Column == 7)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.fQuestionSubjectId).ToList();
                    else
                        result = result.OrderByDescending(top => top.fQuestionSubjectId).ToList();
                }
                else if (paramOrderDetails.Column == 8)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Status).ToList();
                    else
                        result = result.OrderByDescending(top => top.Status).ToList();
                }
                else if (paramOrderDetails.Column == 9)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.LastResponseDatetimeSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.LastResponseDatetimeSort).ToList();
                }
            }
            return result;
        }

        public List<TicketResult> FillHelpResult(int? userId, int operatorAdminId)
        {
            List<TicketResult> _helpResult = new List<TicketResult>();
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            IOrderedEnumerable<Question> result;

            //Add 16-04-2019
            var userData = _userRepository.GetById(operatorAdminId);
            var operatorId = userData.OperatorId;
            var countryId = userData.Operator.CountryId;


            if (userId != null)
            {
                //result = _questionRepository.GetAll().Where(top => top.UserId == userId && (top.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || top.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest)).OrderBy(top => top.Status).ThenByDescending(top => top.LastResponseDateTimeByUser);
                result = _questionRepository.GetMany(top => top.UserId == userId && (top.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || top.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest || top.SubjectId == (int)QuestionSubjectStatus.AdvertError || top.SubjectId == (int)QuestionSubjectStatus.Adreview)).OrderByDescending(top => top.CreatedDate);
            }
            else
            {
                //var userIdData = _userRepository.GetMany(top => (top.OperatorId == operatorId || top.OperatorId == 0) && (top.RoleId == 2 || top.RoleId == 3)).Select(top => top.UserId).ToList();

                // result = _questionRepository.GetMany(s => (s.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || s.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest)).OrderBy(top => top.Status).ThenByDescending(top => top.LastResponseDateTimeByUser);
                //result = _questionRepository.GetMany(s => userIdData.Contains((int)s.UserId) && (s.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || s.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest || s.SubjectId == (int)QuestionSubjectStatus.AdvertError || s.SubjectId == (int)QuestionSubjectStatus.Adreview)).OrderBy(top => top.Id);
                //result = _questionRepository.GetMany(s => s.UserId != null && userIdData.Contains((int)s.UserId) && (s.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || s.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest || s.SubjectId == (int)QuestionSubjectStatus.AdvertError || s.SubjectId == (int)QuestionSubjectStatus.Adreview)).OrderBy(top => top.Id);

                //Code commented on 11/07/2018/
                //result = _questionRepository.GetMany(s => s.UserId != null && (s.User.OperatorId == operatorId || s.User.OperatorId == 0) && (s.User.RoleId == 2 || s.User.RoleId == 3) &&
                //(s.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || s.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest || s.SubjectId == (int)QuestionSubjectStatus.AdvertError || s.SubjectId == (int)QuestionSubjectStatus.Adreview))
                //.OrderByDescending(top => top.CreatedDate);

                var userIdList = _contactsRepository.GetMany(s => s.CountryId == countryId).Select(s => s.UserId).ToList();
                var advertiserIdList = _userRepository.GetMany(s => userIdList.Contains(s.UserId) && s.RoleId == 3).Select(s => s.UserId).ToList();

                result = _questionRepository.GetMany(s => s.UserId != null && (s.User.OperatorId == operatorId || advertiserIdList.Contains((int)s.UserId)) &&
               (s.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || s.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest || s.SubjectId == (int)QuestionSubjectStatus.AdvertError || s.SubjectId == (int)QuestionSubjectStatus.Adreview))
               .OrderByDescending(top => top.CreatedDate);
            }
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

                //Commented on 18-02-2019
                //_helpResult.Add(new TicketResult { userId = (int)(item.UserId), ClientId = (item.ClientId), PaymentMethodId = (item.PaymentMethodId), userName = item.User.FirstName + " " + item.User.LastName, userEmail = item.User.Email, fuserId = item.UserId, QANumber = item.QNumber, Id = item.Id, fClientId = clientId, ClientName = clientname, CampaignProfileId = campaingnId, CampaignName = campaignname, QuestionDateTime = item.CreatedDate, QuestionTitle = item.Title, QuestionSubject = subject, fQuestionSubjectId = questionSubjectId, Status = item.Status, LastResponseDatetime = item.LastResponseDateTime, LastResponseDateTimeByUser = item.LastResponseDateTimeByUser, fPaymentMethodId = paymentMethodId, fStatus = status, Organisation = item.User.Organisation });

                //Add 18-02-2019
                _helpResult.Add(new TicketResult { userId = (int)(item.UserId), ClientId = (item.ClientId), PaymentMethodId = (item.PaymentMethodId), userName = item.User.FirstName + " " + item.User.LastName, userEmail = item.User.Email, fuserId = item.UserId, QANumber = item.QNumber, Id = item.Id, fClientId = clientId, ClientName = clientname, CampaignProfileId = campaingnId, CampaignName = campaignname, QuestionDateTime = item.CreatedDate == null ? null : item.CreatedDate.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), QuestionDateTimeSort = item.CreatedDate, QuestionTitle = item.Title, QuestionSubject = subject, QuestionSubjectId = int.Parse(questionSubjectId.Value.ToString()), fQuestionSubjectId = questionSubjectId, Status = item.Status, LastResponseDatetime = item.LastResponseDateTime == null ? null : item.LastResponseDateTime.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), LastResponseDatetimeSort = item.LastResponseDateTime, LastResponseDateTimeByUser = item.LastResponseDateTimeByUser == null ? null : item.LastResponseDateTimeByUser.Value.ToString("dd/MM/yyyy hh:mm:ss tt"), LastResponseDateTimeByUserSort = item.LastResponseDateTimeByUser, fPaymentMethodId = paymentMethodId, fStatus = status, Organisation = item.User.Organisation == null ? "-" : item.User.Organisation });
            }
            return _helpResult;
        }


        public void FillQuestionSubjectDropdown()
        {
            var Qsubjectdetails = _questionSubjectRepository.GetMany(s => (s.SubjectId == (int)QuestionSubjectStatus.OperatorAdreview || s.SubjectId == (int)QuestionSubjectStatus.OperatorCreditRequest || s.SubjectId == (int)QuestionSubjectStatus.AdvertError || s.SubjectId == (int)QuestionSubjectStatus.Adreview)).Select(top => new
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

        public void FillUserDropdown(int? userId, int operatorAdminId)
        {
            //Add 16-04-2019
            var operatorId = _userRepository.GetById(operatorAdminId).OperatorId;

            if (userId != null)
            {
                var userdetails = _userRepository.GetMany(top => top.UserId == userId && top.OperatorId == operatorId && (top.RoleId == 2 || top.RoleId == 3)).Select(top => new
                {
                    Name = top.FirstName + " " + top.LastName,
                    UserId = top.UserId,
                }).ToList();
                ViewBag.user = new MultiSelectList(userdetails, "UserId", "Name");
            }
            else
            {
                var questionUserList = _questionRepository.GetAll().Select(s => s.UserId).ToList();
                var userdetails = _userRepository.GetMany(top => questionUserList.Contains(top.UserId) && top.OperatorId == operatorId && (top.RoleId == 2 || top.RoleId == 3)).Select(top => new
                {
                    Name = top.FirstName + " " + top.LastName,
                    UserId = top.UserId,
                }).ToList();
                ViewBag.user = new MultiSelectList(userdetails, "UserId", "Name");
            }
        }

        [Route("SearchTicket")]
        public ActionResult SearchTicket([Bind(Prefix = "Item2")]TicketFilter _filterCritearea, int[] UserId, int[] ClientId, int[] SubjectId, int[] StatusId, int[] PaymentMethodId)
        {
            if (User.Identity.IsAuthenticated)
            {
                //Add 16-04-2019
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var operatorAdminId = efmvcUser.UserId;

                List<TicketResult> _result = new List<TicketResult>();
                var finalresult = new List<TicketResult>();
                if (_filterCritearea != null)
                {
                    //Add 16-04-2019
                    _result = FillHelpResult(null, operatorAdminId);

                    //Comment 16-04-2019
                    //_result = FillHelpResult(null);
                    finalresult = gethelpResult(_result, _filterCritearea, UserId, ClientId, SubjectId, StatusId, PaymentMethodId);
                }
                else
                {
                    //Add 16-04-2019
                    _result = FillHelpResult(null, operatorAdminId);

                    //Comment 16-04-2019
                    //_result = FillHelpResult(null);
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
                    string email = "";
                    var userIdList = _questioncommentRepository.GetMany(s => s.QuestionId == id && s.UserId != efmvcUser.UserId).Select(s => s.UserId).Distinct().ToList();
                    if (userIdList.Count() > 0)
                        email = _userRepository.GetMany(s => userIdList.Contains(s.UserId) && s.RoleId != (int)UserRole.UserAdmin && s.RoleId != (int)UserRole.Admin && s.RoleId != (int)UserRole.AdvertAdmin && s.RoleId != (int)UserRole.OperatorAdmin).FirstOrDefault().Email;
                    else
                        email = _questionRepository.GetMany(s => s.Id == id).FirstOrDefault().User.Email;

                    string agentEmail = LiveAgent.GetAgent();
                    string ticketCode = LiveAgent.ReplyTicket(item.Title, item.Description, email, "R", agentEmail, (int)UserRole.Admin);
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
                    //Comment 03-06-2019
                    //string strTodate = _filterCritearea.Todate;
                    //DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //string strFromdate = _filterCritearea.Fromdate;
                    //DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //helpresult = helpresult.Where(top => top.QuestionDateTime.Value.Date >= Fromdate && top.QuestionDateTime.Value.Date <= Todate).ToList();
                    //// helpresult = helpresult.Where(top => top.QuestionDateTime.Value.Date >= _filterCritearea.Fromdate.Value.Date && top.QuestionDateTime.Value.Date <= _filterCritearea.Todate.Value.Date).ToList();

                    //Add 03-06-2019
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
                    //    Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
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
                    helpresult = helpresult.Where(top => top.LastResponseDatetimeSort.Value >= Fromdate.Value && top.LastResponseDatetimeSort.Value <= Todate.Value).ToList();
                }
                if ((_filterCritearea.LastResponseFromdate != null && _filterCritearea.LastResponseTodate != null))
                {
                    //Comment 03-06-2019
                    ////helpresult = helpresult.Where(top => top.LastResponseDatetime != null).ToList();
                    //helpresult = helpresult.Where(top => top.LastResponseDateTimeByUser != null).ToList();
                    //// helpresult = helpresult.Where(top => top.LastResponseDatetime.Value.Date >= _filterCritearea.LastResponseFromdate.Value.Date && top.LastResponseDatetime.Value.Date <= _filterCritearea.LastResponseTodate.Value.Date).ToList();
                    //string strTodate = _filterCritearea.LastResponseTodate;
                    //DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //string strFromdate = _filterCritearea.LastResponseFromdate;
                    //DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    //helpresult = helpresult.Where(top => top.LastResponseDateTimeByUser.Value >= Fromdate && top.LastResponseDateTimeByUser.Value <= Todate).ToList();
                    ////helpresult = helpresult.Where(top => top.LastResponseDatetime.Value.Date >= Fromdate && top.LastResponseDatetime.Value.Date <= Todate).ToList();
                    ////  helpresult = helpresult.Where(top => top.LastResponseDatetime.Value.Date >= _filterCritearea.LastResponseFromdate.Value.Date && top.LastResponseDatetime.Value.Date <= _filterCritearea.LastResponseTodate.Value.Date).ToList();

                    //Add 03-06-2019
                    string strTodate = "";
                    string strFromdate = "";
                    DateTime? Todate = null;
                    DateTime? Fromdate = null;
                    helpresult = helpresult.Where(top => top.LastResponseDatetimeSort != null).ToList();
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
                    helpresult = helpresult.Where(top => top.LastResponseDatetimeSort.Value >= Fromdate.Value && top.LastResponseDatetimeSort.Value <= Todate.Value).ToList();
                }
                if (SubjectId != null)
                {
                    helpresult = helpresult.Where(top => SubjectId.Contains(top.QuestionSubjectId)).ToList();
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
                    //LiveAgent.DeleteTicket(TempData["TicketId"].ToString());
                }
                _model.UserId = efmvcUser.UserId;
                _model.ResponseDatetime = DateTime.Now;


                //string email = "";
                //var userIdList = _questioncommentRepository.GetMany(s => s.QuestionId == _model.QuestionId && s.UserId != efmvcUser.UserId).Select(s => s.UserId).Distinct().ToList();
                //if (userIdList.Count() > 0)
                //    email = _userRepository.GetMany(s => userIdList.Contains(s.UserId) && s.RoleId != (int)UserRole.UserAdmin && s.RoleId != (int)UserRole.Admin && s.RoleId != (int)UserRole.AdvertAdmin && s.RoleId != (int)UserRole.OperatorAdmin).FirstOrDefault().Email;
                //else
                //    email = _questionRepository.GetMany(s => s.Id == _model.QuestionId).FirstOrDefault().User.Email;

                //string agentEmail = LiveAgent.GetAgent();
                //string ticketCode = LiveAgent.ReplyTicket(_model.Title, _model.Description, email, "C", agentEmail, (int)UserRole.Admin);
                // _model.TicketCode = ticketCode;
                _model.TicketCode = null;

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
        [Route("FillUserDropdownAJAX")]
        [HttpPost]
        public ActionResult FillUserDropdownAJAX(string UserName)
        {
            try
            {
                //Add 16-04-2019
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var operatorAdminId = efmvcUser.UserId;
                var operatorId = _userRepository.GetById(operatorAdminId).OperatorId;

                var userData = _userRepository.GetById(operatorAdminId);
                var countryId = userData.Operator.CountryId;
                //var userIdList = _contactsRepository.GetMany(s => s.CountryId == countryId).Select(s => s.UserId).ToList();
                //var advertiserIdList = _userRepository.GetMany(s => s.OperatorId == operatorId && (s.RoleId == 2 || s.RoleId == 3)).Select(s => s.UserId).ToList();
                var userIdList = _contactsRepository.GetMany(s => s.CountryId == countryId).Select(s => s.UserId).ToList();
                var advertiserIdList = _userRepository.GetMany(s => userIdList.Contains(s.UserId) && s.RoleId == 3).Select(s => s.UserId).ToList();

                if (!string.IsNullOrEmpty(UserName))
                {
                    var userdetails = _userRepository.GetMany(top => top.FirstName.Contains(UserName) || top.LastName.Contains(UserName) && advertiserIdList.Contains(top.UserId) && top.OperatorId == operatorId).Select(top => new
                    //var userdetails = _userRepository.GetMany(top => (top.FirstName + " " + top.LastName).Contains(UserName) && top.OperatorId == operatorId && (top.RoleId == 2 || top.RoleId == 3)).Select(top => new
                    // var userdetails = _userRepository.GetMany(top => top.FirstName.Contains(UserName) || top.LastName.Contains(UserName)).Select(top => new
                    {
                        Name = top.FirstName + " " + top.LastName,
                        UserId = top.UserId,
                    }).ToList();
                    ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
                    var data = userdetails.Select(x => new { id = x.UserId, name = x.Name }).ToArray();
                    return Json(data);
                }
                else
                {
                    return Json("");
                }
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }
    }
}