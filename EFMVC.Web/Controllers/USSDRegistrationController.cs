using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Model;
using EFMVC.Web.Common;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EFMVC.Domain.Commands;

namespace EFMVC.Web.Controllers
{
    public class USSDRegistrationController : Controller
    {
        private readonly ISoapApiResponseCodeRepository _soapApiResponseCodeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOperatorRepository _operatorRepository;
        private readonly IRewardRepository _rewardRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IFormsAuthentication _formAuthentication;
        private readonly IProfileRepository _profileRepository;
        private readonly ITIBCOResponseCodeRepository _TIBCOResponseCodeRepository;
        private readonly ICommandBus _commandBus;

        public USSDRegistrationController(ISoapApiResponseCodeRepository soapApiResponseCodeRepository, IUserRepository userRepository, IOperatorRepository operatorRepository, IRewardRepository rewardRepository, IUserProfileRepository userProfileRepository, IFormsAuthentication formAuthentication, IProfileRepository profileRepository, ITIBCOResponseCodeRepository TIBCOResponseCodeRepository, ICommandBus commandBus)
        {
            _soapApiResponseCodeRepository = soapApiResponseCodeRepository;
            _userRepository = userRepository;
            _operatorRepository = operatorRepository;
            _rewardRepository = rewardRepository;
            _userProfileRepository = userProfileRepository;
            _formAuthentication = formAuthentication;
            _profileRepository = profileRepository;
            _TIBCOResponseCodeRepository = TIBCOResponseCodeRepository;
            _commandBus = commandBus;
        }

        public class ResponseModel
        {
            public string ResponseCode { get; set; }
            public string ResponseMessage { get; set; }
            public string MessageID { get; set; } = "";

        }

        public static string ErrorCodeUnknownError = "1000";
        public static string ErrorCodeInvalidArguments = "1001";
        public static string ErrorCodePhoneNumberIsEmpty = "1002";
        public static string ErrorCodeFailedToSubscribe = "1106";
        public static string ErrorCodeFailedToUnsubscribe = "1105";
        public static string ErrorCodeUserProfileDoesnotExist = "1101";
        public static string ErrorCodeUserProfileAlreadyExist = "1102";
        public static string ErrorCodeSuccess = "0000";
        public static string ErrorCodeSuccessUpdatedExistingProfile = "0102";
        public static string ErrorCodeSuccesNothingChanged = "0002";

        public static Dictionary<string, string> ErrorCodeMessages =
            new Dictionary<string, string>
            {
                { ErrorCodeUnknownError, "Unknown error" },
                { ErrorCodePhoneNumberIsEmpty, "Phone number is null or empty" },
                { ErrorCodeInvalidArguments, "Invalid arguments" },
                { ErrorCodeFailedToSubscribe, "Internal subscribe failed" },
                { ErrorCodeFailedToUnsubscribe, "Internal unsubscribe failed" },
                { ErrorCodeUserProfileDoesnotExist, "User Profile does not exist" },
                { ErrorCodeUserProfileAlreadyExist, "User Profile already exists" },
                { ErrorCodeSuccess, "Operation completed successfully" },
                { ErrorCodeSuccesNothingChanged, "Operation completed successfully. Nothing is changed." },
                { ErrorCodeSuccessUpdatedExistingProfile, "Updated existing profile." },
            };

        public class SenegalResponseModel
        {
            public string ResponseCode { get; set; }
            public string ResponseMessage { get; set; }
        }

        private class CentralUserIdentity
        {
            public int UserId { get; set; }
            public int UserProfileId { get; set; }
        }

        private async Task<CentralUserIdentity> GetUserIdAndProfileIdByMsisdn(string msisdn)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = "select UserId,UserProfileId from UserProfile where MSISDN = @MSISDN";
                    command.Parameters.AddWithValue("@MSISDN", msisdn);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            int userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                            int profileId = reader.GetInt32(reader.GetOrdinal("UserProfileId"));
                            return new CentralUserIdentity { UserId = userId, UserProfileId = profileId };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// If user does not exist - it creates User and Profile and returns their IDs.
        /// </summary>
        /// <param name="msisdn">MSISDN.</param>
        /// <returns>JSON with UserId, UserProfileId, Success flag and a Message.</returns>
        public async Task<JsonResult> AddUserFromSafaricom(string msisdn)
        {
            if (string.IsNullOrWhiteSpace(msisdn))
                return Json(new
                {
                    UserProfileId = default(int?),
                    UserId = default(int?),
                    Success = false,
                    Message = ErrorCodeMessages[ErrorCodePhoneNumberIsEmpty]
                }, JsonRequestBehavior.AllowGet);

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        try
                        {
                            command.CommandText = "exec AddUserFromSafaricom @MSISDN";
                            command.Parameters.AddWithValue("@MSISDN", msisdn);
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                if (reader.HasRows && await reader.ReadAsync())
                                {
                                    int userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                                    int profileId = reader.GetInt32(reader.GetOrdinal("UserProfileId"));
                                    return Json(new
                                    {
                                        UserId = userId,
                                        UserProfileId = profileId,
                                        Success = true,
                                        Message = ErrorCodeMessages[ErrorCodeSuccess]
                                    }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        catch (SqlException e)
                        {
                            if (e.Number == 51101)
                            {
                                var uInfo = await GetUserIdAndProfileIdByMsisdn(msisdn);
                                return Json(new
                                {
                                    UserId = uInfo?.UserId,
                                    UserProfileId = uInfo?.UserProfileId,
                                    Success = true,
                                    Message = ErrorCodeMessages[ErrorCodeUserProfileAlreadyExist]
                                }, JsonRequestBehavior.AllowGet);
                            }
                            return Json(new
                            {
                                UserId = default(int?),
                                UserProfileId = default(int?),
                                Success = false,
                                Message = e.Message,
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new
                {
                    UserId = default(int?),
                    UserProfileId = default(int?),
                    Success = false,
                    Message = ErrorCodeMessages[ErrorCodeUnknownError]
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    UserId = default(int?),
                    UserProfileId = default(int?),
                    Success = false,
                    Message = e.Message,
                }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// If user does not exist - it creates User and Profile and returns their IDs.
        /// </summary>
        /// <param name="msisdn">MSISDN.</param>
        /// <returns>JSON with UserId, UserProfileId, Success flag and a Message.</returns>
        public async Task<JsonResult> DeactivateUser(string msisdn)
        {
            if (string.IsNullOrWhiteSpace(msisdn))
                return Json(new
                {
                    UserProfileId = default(int?),
                    UserId = default(int?),
                    Success = false,
                    Message = ErrorCodeMessages[ErrorCodePhoneNumberIsEmpty]
                }, JsonRequestBehavior.AllowGet);

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFMVCDataContex"].ConnectionString))
                {
                    await conn.OpenAsync();
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        try
                        {
                            command.CommandText = "exec DeactivateUser @msisdn";
                            command.Parameters.AddWithValue("@msisdn", msisdn);
                            var affected = await command.ExecuteNonQueryAsync();
                            return Json(new
                            {
                                Success = true,
                                Message = ErrorCodeMessages[ErrorCodeSuccess]
                            }, JsonRequestBehavior.AllowGet);
                        }
                        catch (SqlException e)
                        {
                            if (e.Number == 51100)
                                return Json(new
                                {
                                    UserId = default(int?),
                                    UserProfileId = default(int?),
                                    Success = true,
                                    Message = ErrorCodeMessages[ErrorCodeUserProfileDoesnotExist]
                                }, JsonRequestBehavior.AllowGet);
                            return Json(new
                            {
                                UserId = default(int?),
                                UserProfileId = default(int?),
                                Success = false,
                                Message = e.Message,
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    UserId = default(int?),
                    UserProfileId = default(int?),
                    Success = false,
                    Message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Safaricom Correct way to Subscribe/Unsubscribe

        public JsonResult SubscribeSafaricomUser(string phoneNumber, string subscribeMessageId)
        {
            //return Json(new ResponseModel
            //{
            //    ResponseCode = "9998",
            //    MessageID = subscribeMessageId,
            //    ResponseMessage = "THIS METHOD IS DEPRECATED AND IS NOT USED ANYMORE!!!",
            //}, JsonRequestBehavior.AllowGet);
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    return Json(new ResponseModel { MessageID = subscribeMessageId, ResponseCode = ErrorCodePhoneNumberIsEmpty, ResponseMessage = ErrorCodeMessages[ErrorCodePhoneNumberIsEmpty] }, JsonRequestBehavior.AllowGet);
                using (EFMVCDataContex db = new EFMVCDataContex())
                {
                    var existingProfile = db.Userprofiles.Include(nameof(UserProfile.User)).FirstOrDefault(p => p.MSISDN == phoneNumber);
                    if (existingProfile == null)
                    {
                        Trace.TraceInformation($"Creating new subscriber for MSISDN: [{phoneNumber}]...");
                        // subscribe new
                        var subscribeResult = OperatorProcess(phoneNumber, subscribeMessageId);
                        return Json(new ResponseModel
                        {
                            ResponseCode = ErrorCodeSuccess,
                            MessageID = subscribeMessageId,
                            ResponseMessage = ErrorCodeMessages[ErrorCodeSuccess]
                        }, JsonRequestBehavior.AllowGet);
                    }

                    // if already activated - return success nothing changed.
                    if (existingProfile.User.Activated == 1)
                    {
                        Trace.TraceInformation($"Already existed and activated User for MSISDN: [{phoneNumber}]");
                        return Json(new ResponseModel
                        {
                            ResponseCode = ErrorCodeSuccess,
                            MessageID = subscribeMessageId,
                            ResponseMessage = ErrorCodeMessages[ErrorCodeSuccesNothingChanged]
                        }, JsonRequestBehavior.AllowGet);
                    }
                    Trace.TraceInformation($"Already existed but not activated user for MSISDN: [{phoneNumber}]. Activating...");
                    // Activate existing profile
                    ChangeActiveStatusCommand updCommand = new ChangeActiveStatusCommand
                    {
                        UserId = existingProfile.UserId,
                        Activated = 1
                    };

                    var result = _commandBus.Submit(updCommand);
                    if (result.Success)
                    {
                        // after updating profile we need to rebuild PreMatches.
                        OperatorFunctionality operatorFunctionality = new OperatorFunctionality(
                            _userRepository, _operatorRepository, _rewardRepository, _userProfileRepository, _formAuthentication, _profileRepository, _commandBus);
                        operatorFunctionality.UpdatePreMatchesForUser(existingProfile.User);

                        return Json(new ResponseModel
                        {
                            ResponseCode = ErrorCodeSuccess,
                            MessageID = subscribeMessageId,
                            ResponseMessage = ErrorCodeMessages[ErrorCodeSuccessUpdatedExistingProfile]
                        }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new ResponseModel
                    {
                        ResponseCode = ErrorCodeFailedToSubscribe,
                        MessageID = subscribeMessageId,
                        ResponseMessage = ErrorCodeMessages[ErrorCodeFailedToSubscribe]
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError($"SubscribeSafaricomUser() Failed. Error: {e}");
                return Json(new ResponseModel { ResponseCode = ErrorCodeUnknownError, MessageID = subscribeMessageId, ResponseMessage = ErrorCodeMessages[ErrorCodeUnknownError] }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UnSubscribeSafaricomUser(string phoneNumber)
        {
            //return Json(new ResponseModel
            //{
            //    ResponseCode = "9998",
            //    ResponseMessage = "THIS METHOD IS DEPRECATED AND IS NOT USED ANYMORE!!!",
            //}, JsonRequestBehavior.AllowGet);
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return Json(new ResponseModel { MessageID = string.Empty, ResponseCode = ErrorCodePhoneNumberIsEmpty, ResponseMessage = ErrorCodeMessages[ErrorCodePhoneNumberIsEmpty] }, JsonRequestBehavior.AllowGet);
            try
            {
                using (EFMVCDataContex db = new EFMVCDataContex())
                {
                    var userinfo = db.Userprofiles.Include(nameof(UserProfile.User)).FirstOrDefault(s => s.MSISDN == phoneNumber);
                    if (userinfo == null)
                    {
                        Trace.TraceWarning($"UnSubscribeSafaricomUser(). User does not exist for MSISDN: [{phoneNumber}]");
                        return Json(new ResponseModel
                        {
                            ResponseCode = ErrorCodeSuccess,
                            ResponseMessage = ErrorCodeMessages[ErrorCodeUserProfileDoesnotExist]
                        }, JsonRequestBehavior.AllowGet);
                    }
                    if (userinfo.User.Activated == 3)
                    {
                        return Json(new ResponseModel
                        {
                            ResponseCode = ErrorCodeSuccess,
                            ResponseMessage = ErrorCodeMessages[ErrorCodeSuccesNothingChanged]
                        }, JsonRequestBehavior.AllowGet);
                    }
                    UserDeleteProcess(userinfo.User, db);
                    return Json(new ResponseModel
                    {
                        ResponseCode = ErrorCodeSuccess,
                        ResponseMessage = ErrorCodeMessages[ErrorCodeSuccess]
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError($"UnSubscribeSafaricomUser() Failed. Error: {e}");
                return Json(new ResponseModel { ResponseCode = ErrorCodeUnknownError, ResponseMessage = ErrorCodeMessages[ErrorCodeUnknownError] }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Subscribe User

        public JsonResult SubscribeUserFromTibco(string returnCode, string phoneNumber, string messageId)
        {
            var soapResonseCodeData = _soapApiResponseCodeRepository.GetMany(s => s.ReturnCode == returnCode).FirstOrDefault();
            if (soapResonseCodeData != null)
            {
                if (soapResonseCodeData.ReturnCode == "000000" || soapResonseCodeData.ReturnCode == "307181")
                {
                    var userId = OperatorProcess(phoneNumber, messageId);
                    //var msgId = _userRepository.GetById(userId).TibcoMessageId;
                    return Json(new ResponseModel { ResponseCode = soapResonseCodeData.ReturnCode, ResponseMessage = soapResonseCodeData.Description, MessageID = messageId }, JsonRequestBehavior.AllowGet);
                }
                return Json(new ResponseModel { ResponseCode = soapResonseCodeData.ReturnCode, ResponseMessage = soapResonseCodeData.Description, MessageID = messageId }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ResponseModel { ResponseCode = returnCode, ResponseMessage = "Unknown errors.", MessageID = messageId }, JsonRequestBehavior.AllowGet);
        }


        //private ResponseModel AddSafaricomUser(string phoneNumber)
        //{
        //    //var returnCode = SoapApiProcess.AddSoapUser(MSISDN);
        //    var returnCode = SoapApiProcess.AddCorpUser(phoneNumber);
        //    var soapResonseCodeData = _soapApiResponseCodeRepository.GetMany(s => s.ReturnCode == returnCode).FirstOrDefault();
        //    //if (returnCode.Contains("?"))
        //    //{
        //    //    return "100001"; // Unknown error

        //    //}
        //    if (soapResonseCodeData != null)
        //    {
        //        if (soapResonseCodeData.ReturnCode == "000000")
        //        {
        //            OperatorProcess(phoneNumber, "Tset");
        //        }
        //        return new ResponseModel { ResponseCode = soapResonseCodeData.ReturnCode, ResponseMessage = soapResonseCodeData.Description };
        //    }
        //    else
        //    {
        //        return new ResponseModel { ResponseCode = "100001", ResponseMessage = "Unknown errors." };
        //    }
        //    // return 0;
        //}

        private int OperatorProcess(string phoneNumber, string messageId)
        {

            UserFormModel userFormModel = new UserFormModel();
            userFormModel.MSISDN = phoneNumber;
            userFormModel.TibcoMessageId = messageId;
            OperatorFunctionality objOperator = new OperatorFunctionality(_userRepository, _operatorRepository, _rewardRepository, _userProfileRepository, _formAuthentication, _profileRepository, _commandBus);
            var userId = objOperator.AddUserByUSSD(userFormModel);
            return userId;
        }

        #endregion

        #region Unsubscribe User

        //public JsonResult Unsubscribe(string MSISDN)
        //{
        //var responseCode = SoapApiProcess.TIBCOSubscribeUser(MSISDN);
        //var TIBCOResonseCodeData = _TIBCOResponseCodeRepository.GetMany(s => s.ReturnCode == responseCode).FirstOrDefault();
        //if (TIBCOResonseCodeData != null)
        //{
        //    if (TIBCOResonseCodeData.ReturnCode == "0")
        //    {
        //EFMVCDataContex db = new EFMVCDataContex();
        //var userProfile = db.Userprofiles.Where(s => s.MSISDN == MSISDN).FirstOrDefault();
        //if (userProfile != null)
        //{
        //    var userinfo = db.Users.Where(s => s.UserId == userProfile.UserId).FirstOrDefault();
        //    var returnCode = SoapApiProcess.DeleteCorpUser(userinfo);
        //    var soapResonseCodeData = _soapApiResponseCodeRepository.GetMany(s => s.ReturnCode == returnCode).FirstOrDefault();
        //    if (soapResonseCodeData != null)
        //    {
        //        if (soapResonseCodeData.ReturnCode == "000000")
        //        {
        //            UserDeleteProcess(userinfo, db);
        //            return Json(new ResponseModel { ResponseCode = soapResonseCodeData.ReturnCode, ResponseMessage = soapResonseCodeData.Description, MessageID = userinfo.TibcoMessageId == null ? userinfo.TibcoMessageId : "" }, JsonRequestBehavior.AllowGet);
        //        }
        //        return Json(new ResponseModel { ResponseCode = soapResonseCodeData.ReturnCode, ResponseMessage = soapResonseCodeData.Description }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new ResponseModel { ResponseCode = "100001", ResponseMessage = "Unknown errors." }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //return Json(new ResponseModel { ResponseCode = "100001", ResponseMessage = "Unknown errors." }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new ResponseModel { ResponseCode = TIBCOResonseCodeData.ReturnCode, ResponseMessage = TIBCOResonseCodeData.Description }, JsonRequestBehavior.AllowGet);
        //}
        //return Json(new ResponseModel { ResponseCode = responseCode, ResponseMessage = "Response code not added in database" }, JsonRequestBehavior.AllowGet);
        // }

        public JsonResult UnsubscribeUserFromTibco(string returnCode, int userId)
        {
            var soapResonseCodeData = _soapApiResponseCodeRepository.GetMany(s => s.ReturnCode == returnCode).FirstOrDefault();
            if (soapResonseCodeData != null)
            {
                if (soapResonseCodeData.ReturnCode == "000000")
                {
                    EFMVCDataContex db = new EFMVCDataContex();
                    var userinfo = db.Users.Where(s => s.UserId == userId).FirstOrDefault();
                    UserDeleteProcess(userinfo, db);
                    return Json(new ResponseModel { ResponseCode = soapResonseCodeData.ReturnCode, ResponseMessage = soapResonseCodeData.Description, MessageID = userinfo.TibcoMessageId }, JsonRequestBehavior.AllowGet);
                }
                return Json(new ResponseModel { ResponseCode = soapResonseCodeData.ReturnCode, ResponseMessage = soapResonseCodeData.Description, MessageID = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResponseModel { ResponseCode = returnCode, ResponseMessage = "Unknown errors.", MessageID = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        private void UserDeleteProcess(User userinfo, EFMVCDataContex db)
        {

            #region User Data Delete

            var userProfileData = db.Userprofiles.Where(s => s.UserId == userinfo.UserId).FirstOrDefault();

            if (userProfileData != null)
            {

                var userProfileId = userProfileData.UserProfileId;
                DeletePrematchData(userProfileId, userinfo.OperatorId);

                var emailVerificationCode = db.EmailVerificationCode.Where(s => s.UserId == userinfo.UserId).ToList();
                foreach (var item in emailVerificationCode)
                {
                    db.EmailVerificationCode.Remove(item);
                    db.SaveChanges();
                }

                var ConnString = ConnectionString.GetConnectionStringByOperatorId(userinfo.OperatorId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db2 = new EFMVCDataContex(item);

                        var userinfofromOP = db2.Users.Where(s => s.AdtoneServerUserId == userinfo.UserId).FirstOrDefault();

                        if (userinfofromOP != null)
                        {
                            //var userProfileDatafromOP = db2.Userprofiles.Where(s => s.UserId == userinfofromOP.UserId).FirstOrDefault();
                            //if (userProfileDatafromOP != null)
                            //{
                            //    //db2.Userprofiles.Remove(userProfileDatafromOP);
                            //    userProfileDatafromOP.MSISDN = null;
                            //    db2.SaveChanges();
                            //}

                            userinfofromOP.Activated = (int)UserStatus.Deleted;
                            userinfofromOP.IsMsisdnMatch = false;
                            //db2.Users.Remove(userinfofromOP);
                            db2.SaveChanges();
                        }

                    }
                }

                userinfo.Activated = (int)UserStatus.Deleted;
                userinfo.IsMsisdnMatch = false;
                // db.Users.Remove(userinfo);
                db.SaveChanges();


            }
            #endregion
        }

        private void DeletePrematchData(int userProfId, int operatorId)
        {
            if (operatorId != 0)
            {
                var ConnString = ConnectionString.GetConnectionStringByOperatorId(operatorId);

                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var userProfileData = db.Userprofiles.Where(s => s.AdtoneServerUserProfileId == userProfId).FirstOrDefault();
                        if (userProfileData != null)
                        {
                            var userProfileId = userProfileData.UserProfileId.ToString();
                            var PrematchData = db.PreMatch.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData.Count() > 0)
                            {
                                db.PreMatch.RemoveRange(PrematchData);
                                db.SaveChanges();
                            }

                            var PrematchData2 = db.PreMatch2.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData2.Count() > 0)
                            {
                                db.PreMatch2.RemoveRange(PrematchData2);
                                db.SaveChanges();
                            }

                            var PrematchData3 = db.PreMatch3.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData3.Count() > 0)
                            {
                                db.PreMatch3.RemoveRange(PrematchData3);
                                db.SaveChanges();
                            }

                            var PrematchData4 = db.PreMatch4.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData4.Count() > 0)
                            {
                                db.PreMatch4.RemoveRange(PrematchData4);
                                db.SaveChanges();
                            }

                            var PrematchData5 = db.PreMatch5.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData5.Count() > 0)
                            {
                                db.PreMatch5.RemoveRange(PrematchData5);
                                db.SaveChanges();
                            }

                            var PrematchData6 = db.PreMatch6.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData6.Count() > 0)
                            {
                                db.PreMatch6.RemoveRange(PrematchData6);
                                db.SaveChanges();
                            }

                            var PrematchData7 = db.PreMatch7.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData7.Count() > 0)
                            {
                                db.PreMatch7.RemoveRange(PrematchData7);
                                db.SaveChanges();
                            }

                            var PrematchData8 = db.PreMatch8.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData8.Count() > 0)
                            {
                                db.PreMatch8.RemoveRange(PrematchData8);
                                db.SaveChanges();
                            }

                            var PrematchData9 = db.PreMatch9.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData9.Count() > 0)
                            {
                                db.PreMatch9.RemoveRange(PrematchData9);
                                db.SaveChanges();
                            }

                            var PrematchData10 = db.PreMatch10.Where(s => s.MsUserProfileId == userProfileId).ToList();
                            if (PrematchData10.Count() > 0)
                            {
                                db.PreMatch10.RemoveRange(PrematchData10);
                                db.SaveChanges();
                            }

                            db.SaveChanges();
                        }

                    }

                }

            }

        }

        #endregion


        #region Senegal Subscribe User
        public JsonResult SenegalSubscribeUser(string returnCode, string phoneNumber)
        {
            if (returnCode == "0001")
            {
                var userId = SenegalOperatorProcess(phoneNumber);
                return Json(new SenegalResponseModel { ResponseCode = returnCode, ResponseMessage = null }, JsonRequestBehavior.AllowGet);
            }
            return Json(new SenegalResponseModel { ResponseCode = returnCode, ResponseMessage = null }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateSenegalSubscribeUser(string returnCode, string userId, string phoneNumber)
        {
            EFMVCDataContex db = new EFMVCDataContex();
            if (returnCode == "0001")
            {
                int UserId = Convert.ToInt32(userId);
                var user = db.Users.Where(top => top.UserId == UserId).FirstOrDefault();
                if (user != null)
                {
                    var ConnString = ConnectionString.GetConnectionStringByOperatorId(user.OperatorId);
                    if (ConnString != null && ConnString.Count() > 0)
                    {
                        foreach (var item in ConnString)
                        {
                            EFMVCDataContex db2 = new EFMVCDataContex(item);
                            var userinfofromOP = db2.Users.Where(s => s.AdtoneServerUserId == user.UserId).FirstOrDefault();
                            if (userinfofromOP != null)
                            {
                                userinfofromOP.Activated = (int)UserStatus.Approved;
                                userinfofromOP.IsMsisdnMatch = true;
                                db2.SaveChanges();
                            }
                        }
                    }
                    user.Activated = (int)UserStatus.Approved;
                    user.IsMsisdnMatch = true;
                    db.SaveChanges();
                    return Json(new SenegalResponseModel { ResponseCode = returnCode, ResponseMessage = null }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new SenegalResponseModel { ResponseCode = "404", ResponseMessage = "User is not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new SenegalResponseModel { ResponseCode = "500", ResponseMessage = "Internal Server Error." }, JsonRequestBehavior.AllowGet);
        }

        private int SenegalOperatorProcess(string phoneNumber)
        {

            UserFormModel userFormModel = new UserFormModel();
            userFormModel.MSISDN = phoneNumber;
            OperatorFunctionality objOperator = new OperatorFunctionality(_userRepository, _operatorRepository, _rewardRepository, _userProfileRepository, _formAuthentication, _profileRepository, _commandBus);
            var userId = objOperator.AddUserBySenegalProvision(userFormModel);
            return userId;
        }

        #endregion

        #region Senegal UnSubscribe User
        [HttpGet]
        public JsonResult DeleteUserForSenegal(string returnCode, string userId)
        {
            if (returnCode == "0001")
            {
                EFMVCDataContex db = new EFMVCDataContex();
                int uId = Convert.ToInt32(userId);
                var userinfo = db.Users.Where(s => s.UserId == uId).FirstOrDefault();
                UserDeleteProcess(userinfo, db);
                return Json(new ResponseModel { ResponseCode = returnCode, ResponseMessage = null }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ResponseModel { ResponseCode = returnCode, ResponseMessage = null }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}