using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Core.Common;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Web.Areas.UsersAdmin.Models;
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
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.OperatorAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "OperatorAdmin")]
    [RouteArea("OperatorAdmin")]
    [RoutePrefix("UserAccount")]
    public class UserAccountController : Controller
    {
        // GET: UsersAdmin/UserAccount
        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly IProfileRepository _profileRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IContactsRepository _contactsRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public UserAccountController(ICommandBus commandBus, IUserRepository userRepository, IProfileRepository profileRepository, IUserProfileRepository userProfileRepository, IContactsRepository contactsRepository)
        {
            _commandBus = commandBus;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _contactsRepository = contactsRepository;
        }
        public void FillUserRole()
        {
            IEnumerable<Common.UserRole> userroleTypes = Enum.GetValues(typeof(Common.UserRole))
                                                     .Cast<Common.UserRole>();
            var userrole = (from action in userroleTypes
                            select new SelectListItem
                            {
                                Text = action.ToString(),
                                Value = ((int)action).ToString()
                            }).ToList();
            ViewBag.userrole = userrole;
        }
        [Route("Index")]
        public ActionResult Index()
        {
            // var result = _userRepository.GetAll().Where(top => top.VerificationStatus == true && top.RoleId == 2).OrderByDescending(top => top.DateCreated);

            var userdropdown = _userRepository.GetMany(top => top.VerificationStatus == true && (top.RoleId == 2 || top.RoleId == 3)).OrderByDescending(top => top.DateCreated).Select(s => new { Id = s.UserId, Name = s.FirstName + " " + s.LastName }).Take(1000).ToList();
            //var userdropdown = (from item in result
            //                    select new
            //                    {
            //                        Id = item.UserId,
            //                        Name = item.FirstName + " " + item.LastName
            //                    }).ToList();
            ViewBag.userdetails = new MultiSelectList(userdropdown, "Id", "Name");
            //List<UserResult> _result = FillUserResult();
            List<UserResult> _result = new List<UserResult>();
            FillUserStatus();
            EFMVC.Web.Areas.UsersAdmin.SearchClass.UserFilter _filterCritearea = new EFMVC.Web.Areas.UsersAdmin.SearchClass.UserFilter();
            return View(Tuple.Create(_result, _filterCritearea));
        }
        public void FillUserStatus()
        {
            IEnumerable<Common.UserStatus> userTypes = Enum.GetValues(typeof(Common.UserStatus))
                                                     .Cast<Common.UserStatus>();
            var userstatus = (from action in userTypes
                              select new
                              {
                                  Text = action.ToString(),
                                  Value = ((int)action).ToString()
                              }).ToList();
            ViewBag.userStatus = new MultiSelectList(userstatus, "Value", "Text");
        }

        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                //Add 17-04-2019
                var operatorAdminId = efmvcUser.UserId;

                var operatorId = _userRepository.GetMany(s => s.UserId == efmvcUser.UserId).FirstOrDefault().OperatorId;
                bool searchValue = false;
                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                    if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
                        searchValue = true;
                }
                List<Model.User> userResult = null;

                //Add 17-04-2019
                var cnt = _userRepository.Count(top => top.VerificationStatus == true && top.OperatorId == operatorId && (top.RoleId == 2 || top.RoleId == 3));

                //Comment 17-04-2019
                //var cnt = _userRepository.Count(top => top.VerificationStatus == true && top.RoleId == 2 && top.OperatorId == operatorId);

                if (searchValue == true)
                {


                    int[] UserId = new int[cnt];
                    int[] UserStatus = new int[cnt];
                    DateTime fromdate = new DateTime();
                    DateTime todate = new DateTime();
                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] == "null")
                        {
                            columnSearch[0] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[1]))
                    {
                        if (columnSearch[1] == "null")
                        {
                            columnSearch[1] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[2]))
                    {
                        if (columnSearch[2] != "null")
                        {
                            UserId = columnSearch[2].Split(',').Select(int.Parse).ToArray();
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
                            fromdate = Convert.ToDateTime(data[0]);
                            todate = Convert.ToDateTime(data[1]);
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
                            UserStatus = columnSearch[4].Split(',').Select(int.Parse).ToArray();
                        }
                        else
                        {
                            columnSearch[4] = null;
                        }
                    }

                    var email = columnSearch[0] == null ? null : columnSearch[0].ToLower();
                    var MSISDN = columnSearch[1] == null ? null : columnSearch[1];

                    //Commented on 18-02-2019
                    //userResult = _userRepository.GetMany(p => p.VerificationStatus == true && p.OperatorId == operatorId && p.RoleId == 2 &&
                    //   (p.Email.ToLower() != null && p.Email.ToLower().Contains(email))
                    //  || ((p.UserProfiles.FirstOrDefault().MSISDN != null && p.UserProfiles.FirstOrDefault().MSISDN.Contains(MSISDN)))
                    // || ((UserId.Contains(p.UserId)))
                    // || ((p.DateCreated >= fromdate && p.DateCreated <= todate))
                    // || ((UserStatus.Contains(p.Activated)))
                    //)
                    //.OrderByDescending(top => top.DateCreated).ToList();

                    //cnt = userResult.Count();
                    //userResult = userResult.Skip(param.Start).Take(param.Length).ToList();

                    //Add 17-04-2019
                    //userResult = _userRepository.GetMany(top => top.VerificationStatus == true && top.OperatorId == operatorId && (top.RoleId == 2 || top.RoleId == 3)).OrderByDescending(top => top.DateCreated).ToList();
                    userResult = _userRepository.GetAll().Where(top => top.VerificationStatus == true && top.OperatorId == operatorId && (top.RoleId == 2 || top.RoleId == 3)).OrderByDescending(top => top.UserId).ToList();

                    //Comment 17-04-2019
                    //Add 18-02-2019
                    //userResult = _userRepository.GetMany(top => top.VerificationStatus == true && top.OperatorId == operatorId && top.RoleId == 2).OrderByDescending(top => top.DateCreated).ToList();

                    if (email != null)
                    {
                        userResult = userResult.Where(top => top.Email.ToLower() != null && top.Email.ToLower().Contains(email)).OrderByDescending(top => top.UserId).ToList();
                    }
                    if (MSISDN != null)
                    {
                        userResult = userResult.Where(top => top.UserProfiles.FirstOrDefault().MSISDN != null && top.UserProfiles.FirstOrDefault().MSISDN.Contains(MSISDN)).OrderByDescending(top => top.UserId).ToList();
                    }
                    if (columnSearch[2] != null)
                    {
                        userResult = userResult.Where(top => UserId.Contains(top.UserId)).OrderByDescending(top => top.UserId).ToList();
                    }
                    if (columnSearch[3] != null)
                    {
                        userResult = userResult.Where(top => top.DateCreated >= fromdate && top.DateCreated <= todate).OrderByDescending(top => top.UserId).ToList();
                    }
                    if (columnSearch[4] != null)
                    {
                        userResult = userResult.Where(top => UserStatus.Contains(top.Activated)).OrderByDescending(top => top.UserId).ToList();
                    }

                    cnt = userResult.Count();
                    userResult = userResult.Skip(param.Start).Take(param.Length).ToList();
                }
                else
                {
                    //Add 17-04-2019
                    //userResult = _userRepository.GetMany(top => top.VerificationStatus == true && (top.RoleId == 2 || top.RoleId == 3) && top.OperatorId == operatorId).OrderByDescending(top => top.DateCreated).ToList();
                    userResult = _userRepository.GetAll().Where(top => top.VerificationStatus == true && (top.RoleId == 2 || top.RoleId == 3) && top.OperatorId == operatorId).OrderByDescending(top => top.UserId).ToList();

                    //Comment 17-04-2019
                    //userResult = _userRepository.GetMany(top => top.VerificationStatus == true && top.RoleId == 2 && top.OperatorId == operatorId).OrderByDescending(top => top.DateCreated).ToList();
                    cnt = userResult.Count();
                    userResult = userResult.Skip(param.Start).Take(param.Length).ToList();
                }


                //Code End on 21-Nov-2017


                Mapper.CreateMap<User, UserResult>();
                var dtsource =
                  Mapper.Map<List<Model.User>, List<UserResult>>(userResult);
                
                if (dtsource.Count() > 0)
                {
                    foreach (var item in dtsource)
                    {
                        UserRoles userRole = (UserRoles)item.RoleId;
                        item.RoleName = userRole.ToString();

                        var GetMsisdn = _profileRepository.GetMany(s => s.UserId == item.UserId).Select(a => a.MSISDN).FirstOrDefault();
                        if (GetMsisdn != null)
                        {
                            item.MSISDN = GetMsisdn;
                        }
                        else
                        {
                            item.MSISDN = "-";
                        }

                        if(item.Email == null || item.Email == "")
                        {
                            item.Email = "-";
                        }
                    }
                }
                
                dtsource = ApplySorting(param, dtsource);
                //dtsource = dtsource.Skip(param.Start).Take(param.Length).ToList();

                DTResult<UserResult> result = new DTResult<UserResult>
                {
                    draw = param.Draw,
                    // data = data,
                    //recordsFiltered = count,
                    //recordsTotal = count

                    data = dtsource,
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

        // Function For Filter/Sorting Campaign Profile Data
        private static List<UserResult> ApplySorting(DTParameters param, List<UserResult> dtsource)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.Email).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.Email).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.MSISDN).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.MSISDN).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.Name).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.Name).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.RoleName).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.RoleName).ToList();
                }
                else if (paramOrderDetails.Column == 4)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.DateCreated).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.DateCreated).ToList();
                }
                else if (paramOrderDetails.Column == 5)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        dtsource = dtsource.OrderBy(top => top.Status).ToList();
                    else
                        dtsource = dtsource.OrderByDescending(top => top.Status).ToList();
                }
            }
            return dtsource;
        }

        public List<UserResult> FillUserResult()
        {
            //Add 17-04-2019
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var operatorAdminId = efmvcUser.UserId;
            var operatorId = _userRepository.GetById(operatorAdminId).OperatorId;

            var result = _userRepository.GetAll().Where(top => top.VerificationStatus == true && top.OperatorId == operatorId && (top.RoleId == 2 || top.RoleId == 3)).OrderByDescending(top => top.UserId).ToList();
            Mapper.CreateMap<User, UserResult>();
            var dtsource =
              Mapper.Map<List<Model.User>, List<UserResult>>(result);

            //Add 17-04-2019
            foreach (var item in dtsource)
            {
                UserRoles userRole = (UserRoles)item.RoleId;
                item.RoleName = userRole.ToString();

                var GetMsisdn = _profileRepository.GetMany(s => s.UserId == item.UserId).Select(a => a.MSISDN).FirstOrDefault();
                if (GetMsisdn != null)
                {
                    item.MSISDN = GetMsisdn;
                }
                else
                {
                    item.MSISDN = "-";
                }

                if (item.Email == null || item.Email == "")
                {
                    item.Email = "-";
                }
            }

            var userdropdown = (from item in result
                                select new
                                {
                                    Id = item.UserId,
                                    Name = item.FirstName + " " + item.LastName
                                }).ToList();
            ViewBag.userdetails = new MultiSelectList(userdropdown, "Id", "Name");
            return dtsource;
        }
       
    
        [Route("ApproveORSuspendUser")]
        public ActionResult ApproveORSuspendUser(int id, int status)
        {
            ChangeActiveStatusCommand command = new ChangeActiveStatusCommand();
            command.UserId = id;
            command.Activated = status;
            ICommandResult result = _commandBus.Submit(command);
            var user = _userRepository.GetById(id);
            if (status == 1)
            {
                TempData["status"] = "User is approved successfully.";
                //271191
                // CheckUserExistSoapApi(user);
            }
            else
            {
                TempData["status"] = "User is suspended successfully.";
                //271191
                //DeleteUserExistFromSoapApi(user);
            }
            return Json("success");
        }
      
        [Route("DeleteUser")]
        public ActionResult DeleteUser(int id)
        {
            ChangeActiveStatusCommand command = new ChangeActiveStatusCommand();
            command.UserId = id;
            command.Activated = 3;
            ICommandResult result = _commandBus.Submit(command);
            TempData["status"] = "User is deleted successfully.";
            return RedirectToAction("Index");

        }

        [Route("SearchUsers")]
        public ActionResult SearchUsers([Bind(Prefix = "Item2")]EFMVC.Web.Areas.UsersAdmin.SearchClass.UserFilter _filterCritearea, int[] UserId, int[] UserStatusId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<UserResult> _result = new List<UserResult>();
                var finalresult = new List<UserResult>();
                if (_filterCritearea != null)
                {
                    _result = FillUserResult();
                    finalresult = getuserResult(_result, _filterCritearea, UserId, UserStatusId);
                }
                else
                {
                    //UserStatusId = new int[] { 1 };
                    _result = FillUserResult();
                    finalresult = getuserResult(_result, _filterCritearea, UserId, UserStatusId);
                }

                return PartialView("_UserDetails", finalresult);
            }
            else
            {
                return PartialView("_UserDetails", "notauthorise");
            }
        }
        public List<UserResult> getuserResult(List<UserResult> userresult, EFMVC.Web.Areas.UsersAdmin.SearchClass.UserFilter _filterCritearea, int[] UserId, int[] UserStatusId)
        {
            if (userresult != null && _filterCritearea != null)
            {
                if (!String.IsNullOrEmpty(_filterCritearea.Email))
                {
                    userresult = userresult.Where(top => top.Email.Contains(_filterCritearea.Email)).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.MSISDN))
                {
                    userresult = userresult.Where(top => top.MSISDN.Contains(_filterCritearea.MSISDN)).ToList();

                }
                if (UserId != null)
                {
                    userresult = userresult.Where(top => UserId.Contains(top.Id)).ToList();
                }
                if (UserStatusId != null)
                {
                    userresult = userresult.Where(top => UserStatusId.Contains(top.Activated)).ToList();
                }
                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    //  userresult = userresult.Where(top => top.DateCreated.Value.Date >= _filterCritearea.Fromdate.Value.Date && top.DateCreated.Value.Date <= _filterCritearea.Todate.Value.Date).ToList();
                    string strTodate = _filterCritearea.Todate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.Fromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    userresult = userresult.Where(top => top.DateCreated.Value.Date >= Fromdate && top.DateCreated.Value.Date <= Todate).ToList();
                }
            }
            else
            {
                if (UserId != null)
                {
                    userresult = userresult.Where(top => UserId.Contains(top.Id)).ToList();
                }
                if (UserStatusId != null)
                {
                    userresult = userresult.Where(top => UserStatusId.Contains(top.Activated)).ToList();
                }
            }
            return userresult;
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
                //var countryId = userData.Operator.CountryId;
                //var userIdList = _contactsRepository.GetMany(s => s.CountryId == countryId).Select(s => s.UserId).ToList();
                var advertiserIdList = _userRepository.GetMany(s => s.RoleId == 2).Select(s => s.UserId).ToList();

                if (!string.IsNullOrEmpty(UserName))
                {
                    var userdetails = _userRepository.GetMany(top => top.FirstName.Contains(UserName) || top.LastName.Contains(UserName) && top.RoleId == 2).Select(top => new
                    //var userdetails = _userRepository.GetMany(top => top.FirstName.Contains(UserName) || top.LastName.Contains(UserName) && advertiserIdList.Contains(top.UserId)).Select(top => new
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

        [Route("ActivateUserSection")]
        public ActionResult ActivateUserSection(int id)
        {
            Session["userId"] = id;
            return Json("success");
        }
    }
}