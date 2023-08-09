using System.Collections.ObjectModel;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Data;
using System.Linq;
using EFMVC.Domain.Commands.Campaign;
using EFMVC.CommandProcessor.Command;
using EFMVC.Domain.OperatorServerData;

namespace EFMVC.Domain.Handlers
{
    public class CreateOrUpdateCopyCampaignProfileHandler : ICommandHandler<CreateOrUpdateCopyCampaignProfileCommand>
    {
        /// <summary>
        /// The _profile campaign repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileCampaignRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileHandler"/> class.
        /// </summary>
        /// <param name="profileCampaignRepository">The profile campaign repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCopyCampaignProfileHandler(ICampaignProfileRepository profileCampaignRepository,
                                                    IUnitOfWork unitOfWork)
        {
            _profileCampaignRepository = profileCampaignRepository;
            _unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateCopyCampaignProfileCommand command)
        {
            var ConnString = ConnectionString.GetConnectionStringByCountryId(command.CountryId);

            CampaignProfile campaignProfile = null;
            campaignProfile = _profileCampaignRepository.GetById(command.CampaignProfileId);
            if (campaignProfile != null)
            {
                campaignProfile.UserId = command.UserId;
                campaignProfile.ClientId = command.ClientId;
                campaignProfile.CampaignName = command.CampaignName;
                campaignProfile.CampaignDescription = command.CampaignDescription;
                campaignProfile.TotalBudget = command.TotalBudget;
                campaignProfile.MaxDailyBudget = command.MaxDailyBudget;
                campaignProfile.MaxBid = command.MaxBid;
                campaignProfile.MaxMonthBudget = command.MaxMonthBudget;
                campaignProfile.MaxWeeklyBudget = command.MaxWeeklyBudget;
                campaignProfile.MaxHourlyBudget = command.MaxHourlyBudget;
                campaignProfile.TotalCredit = command.TotalCredit;
                campaignProfile.SpendToDate = command.SpendToDate;
                campaignProfile.AvailableCredit = command.AvailableCredit;
                campaignProfile.PlaysToDate = command.PlaysToDate;
                campaignProfile.PlaysLastMonth = command.PlaysLastMonth;
                campaignProfile.PlaysCurrentMonth = command.PlaysCurrentMonth;
                campaignProfile.CancelledToDate = command.CancelledToDate;
                campaignProfile.CancelledLastMonth = command.CancelledLastMonth;
                campaignProfile.CancelledCurrentMonth = command.CancelledCurrentMonth;
                campaignProfile.SmsToDate = command.SmsToDate;
                campaignProfile.SmsLastMonth = command.SmsLastMonth;
                campaignProfile.SmsCurrentMonth = command.SmsCurrentMonth;
                campaignProfile.EmailToDate = command.EmailToDate;
                campaignProfile.EmailsLastMonth = command.EmailsLastMonth;
                campaignProfile.EmailsCurrentMonth = command.EmailsCurrentMonth;
                campaignProfile.EmailFileLocation = command.EmailFileLocation;
                campaignProfile.Active = command.Active;
                campaignProfile.NumberOfPlays = command.NumberOfPlays;
                campaignProfile.AverageDailyPlays = command.AverageDailyPlays;
                campaignProfile.SmsRequests = command.SmsRequests;
                campaignProfile.EmailsDelievered = command.EmailsDelievered;
                campaignProfile.EmailSubject = command.EmailSubject;
                campaignProfile.EmailBody = command.EmailBody;
                campaignProfile.EmailSenderAddress = command.EmailSenderAddress;
                campaignProfile.SmsOriginator = command.SmsOriginator;
                campaignProfile.SmsBody = command.SmsBody;
                campaignProfile.SMSFileLocation = command.SMSFileLocation;
                campaignProfile.CreatedDateTime = System.DateTime.Now;
                campaignProfile.UpdatedDateTime = System.DateTime.Now;
                campaignProfile.Status = command.Status;
                campaignProfile.StartDate = command.StartDate;
                campaignProfile.EndDate = command.EndDate;
                campaignProfile.NumberInBatch = command.NumberInBatch;
                campaignProfile.CountryId = command.CountryId;
                campaignProfile.IsAdminApproval = command.IsAdminApproval;
                campaignProfile.RemainingMaxDailyBudget = command.RemainingMaxDailyBudget;
                campaignProfile.RemainingMaxHourlyBudget = command.RemainingMaxHourlyBudget;
                campaignProfile.RemainingMaxWeeklyBudget = command.RemainingMaxWeeklyBudget;
                campaignProfile.RemainingMaxMonthBudget = command.RemainingMaxMonthBudget;
                campaignProfile.ProvidendSpendAmount = command.ProvidendSpendAmount;
                campaignProfile.BucketCount = command.BucketCount;
                campaignProfile.PhoneticAlphabet = command.PhoneticAlphabet;
                campaignProfile.NextStatus = command.NextStatus;
                campaignProfile.CurrencyCode = command.CurrencyCode;
            }
            else
            {
                campaignProfile = new Model.CampaignProfile
                {
                    CampaignProfileId = command.CampaignProfileId,
                    UserId = command.UserId,
                    ClientId = command.ClientId,
                    CampaignName = command.CampaignName,
                    CampaignDescription = command.CampaignDescription,
                    TotalBudget = command.TotalBudget,
                    MaxDailyBudget = command.MaxDailyBudget,
                    MaxBid = command.MaxBid,
                    MaxMonthBudget = command.MaxMonthBudget,
                    MaxWeeklyBudget = command.MaxWeeklyBudget,
                    MaxHourlyBudget = command.MaxHourlyBudget,
                    TotalCredit = command.TotalCredit,
                    SpendToDate = command.SpendToDate,
                    AvailableCredit = command.AvailableCredit,
                    PlaysToDate = command.PlaysToDate,
                    PlaysLastMonth = command.PlaysLastMonth,
                    PlaysCurrentMonth = command.PlaysCurrentMonth,
                    CancelledToDate = command.CancelledToDate,
                    CancelledLastMonth = command.CancelledLastMonth,
                    CancelledCurrentMonth = command.CancelledCurrentMonth,
                    SmsToDate = command.SmsToDate,
                    SmsLastMonth = command.SmsLastMonth,
                    SmsCurrentMonth = command.SmsCurrentMonth,
                    EmailToDate = command.EmailToDate,
                    EmailsLastMonth = command.EmailsLastMonth,
                    EmailsCurrentMonth = command.EmailsCurrentMonth,
                    EmailFileLocation = command.EmailFileLocation,
                    Active = command.Active,
                    NumberOfPlays = command.NumberOfPlays,
                    AverageDailyPlays = command.AverageDailyPlays,
                    SmsRequests = command.SmsRequests,
                    EmailsDelievered = command.EmailsDelievered,
                    EmailSubject = command.EmailSubject,
                    EmailBody = command.EmailBody,
                    EmailSenderAddress = command.EmailSenderAddress,
                    SmsOriginator = command.SmsOriginator,
                    SmsBody = command.SmsBody,
                    SMSFileLocation = command.SMSFileLocation,
                    CreatedDateTime = System.DateTime.Now,
                    UpdatedDateTime = System.DateTime.Now,
                    Status = command.Status,
                    StartDate = command.StartDate,
                    EndDate = command.EndDate,
                    NumberInBatch = command.NumberInBatch,
                    CountryId = command.CountryId,
                    IsAdminApproval = command.IsAdminApproval,
                    RemainingMaxDailyBudget = command.RemainingMaxDailyBudget,
                    RemainingMaxHourlyBudget = command.RemainingMaxHourlyBudget,
                    RemainingMaxWeeklyBudget = command.RemainingMaxWeeklyBudget,
                    RemainingMaxMonthBudget = command.RemainingMaxMonthBudget,
                    ProvidendSpendAmount = command.ProvidendSpendAmount,
                    BucketCount = command.BucketCount,
                    PhoneticAlphabet = command.PhoneticAlphabet,
                    NextStatus = command.NextStatus,
                    CurrencyCode = command.CurrencyCode,
                    //CampaignProfileTimeSettings = new Collection<CampaignProfileTimeSetting> { new CampaignProfileTimeSetting() },
                };
            }

            if (campaignProfile.CampaignProfileId == 0)
            {
                _profileCampaignRepository.Add(campaignProfile);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        int clientId = 0;
                        if (command.ClientId == null)
                        {
                            clientId = 0;
                        }
                        else
                        {
                            clientId = command.ClientId.Value;
                        }
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                        //var externalServerClientId = OperatorServer.GetUserIdFromOperatorServer(db, clientId);
                        var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, clientId);
                        var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                        if (externalServerUserId != 0 && externalServerCountryId != 0)
                        {
                            var campaignProfile2 = new Model.CampaignProfile
                            {
                                CampaignProfileId = command.CampaignProfileId,
                                UserId = externalServerUserId,
                                ClientId = externalServerClientId,
                                CampaignName = command.CampaignName,
                                CampaignDescription = command.CampaignDescription,
                                TotalBudget = command.TotalBudget,
                                MaxDailyBudget = command.MaxDailyBudget,
                                MaxBid = command.MaxBid,
                                MaxMonthBudget = command.MaxMonthBudget,
                                MaxWeeklyBudget = command.MaxWeeklyBudget,
                                MaxHourlyBudget = command.MaxHourlyBudget,
                                TotalCredit = command.TotalCredit,
                                SpendToDate = command.SpendToDate,
                                AvailableCredit = command.AvailableCredit,
                                PlaysToDate = command.PlaysToDate,
                                PlaysLastMonth = command.PlaysLastMonth,
                                PlaysCurrentMonth = command.PlaysCurrentMonth,
                                CancelledToDate = command.CancelledToDate,
                                CancelledLastMonth = command.CancelledLastMonth,
                                CancelledCurrentMonth = command.CancelledCurrentMonth,
                                SmsToDate = command.SmsToDate,
                                SmsLastMonth = command.SmsLastMonth,
                                SmsCurrentMonth = command.SmsCurrentMonth,
                                EmailToDate = command.EmailToDate,
                                EmailsLastMonth = command.EmailsLastMonth,
                                EmailsCurrentMonth = command.EmailsCurrentMonth,
                                EmailFileLocation = command.EmailFileLocation,
                                Active = command.Active,
                                NumberOfPlays = command.NumberOfPlays,
                                AverageDailyPlays = command.AverageDailyPlays,
                                SmsRequests = command.SmsRequests,
                                EmailsDelievered = command.EmailsDelievered,
                                EmailSubject = command.EmailSubject,
                                EmailBody = command.EmailBody,
                                EmailSenderAddress = command.EmailSenderAddress,
                                SmsOriginator = command.SmsOriginator,
                                SmsBody = command.SmsBody,
                                SMSFileLocation = command.SMSFileLocation,
                                CreatedDateTime = System.DateTime.Now,
                                UpdatedDateTime = System.DateTime.Now,
                                Status = command.Status,
                                StartDate = command.StartDate,
                                EndDate = command.EndDate,
                                NumberInBatch = command.NumberInBatch,
                                CountryId = externalServerCountryId,
                                IsAdminApproval = command.IsAdminApproval,
                                RemainingMaxDailyBudget = command.RemainingMaxDailyBudget,
                                RemainingMaxHourlyBudget = command.RemainingMaxHourlyBudget,
                                RemainingMaxWeeklyBudget = command.RemainingMaxWeeklyBudget,
                                RemainingMaxMonthBudget = command.RemainingMaxMonthBudget,
                                ProvidendSpendAmount = command.ProvidendSpendAmount,
                                BucketCount = command.BucketCount,
                                PhoneticAlphabet = command.PhoneticAlphabet,
                                NextStatus = command.NextStatus,
                                AdtoneServerCampaignProfileId = campaignProfile.CampaignProfileId,
                                CurrencyCode = command.CurrencyCode
                                ////CampaignProfileTimeSettings = new Collection<CampaignProfileTimeSetting> { new CampaignProfileTimeSetting() }
                            };
                            db.CampaignProfiles.Add(campaignProfile2);
                            db.SaveChanges();

                            //Comment 19-11-2019
                            ////Add 22-03-2019
                            //var timeSetting2 = new CampaignProfileTimeSetting();
                            //timeSetting2.Monday = null;
                            //timeSetting2.Tuesday = null;
                            //timeSetting2.Wednesday = null;
                            //timeSetting2.Thursday = null;
                            //timeSetting2.Friday = null;
                            //timeSetting2.Saturday = null;
                            //timeSetting2.Sunday = null;
                            //timeSetting2.CampaignProfileId = campaignProfile2.CampaignProfileId;
                            //timeSetting2.AdtoneServerCampaignProfileTimeId = campaignProfile.CampaignProfileTimeSettings.FirstOrDefault().CampaignProfileTimeSettingsId;

                            //db.CampaignProfileTimeSettings.Add(timeSetting2);
                            //db.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                _profileCampaignRepository.Update(campaignProfile);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        int clientId = 0;
                        if (command.ClientId == null)
                        {
                            clientId = 0;
                        }
                        else
                        {
                            clientId = command.ClientId.Value;
                        }
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var campaignProfileDetails = db.CampaignProfiles.Where(s => s.AdtoneServerCampaignProfileId == command.CampaignProfileId).FirstOrDefault();
                        if (campaignProfileDetails != null)
                        {
                            var externalServerUserId = OperatorServer.GetUserIdFromOperatorServer(db, command.UserId);
                            //var externalServerClientId = OperatorServer.GetUserIdFromOperatorServer(db, clientId);
                            var externalServerClientId = OperatorServer.GetClientIdFromOperatorServer(db, clientId);
                            var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                            if (externalServerUserId != 0 && externalServerCountryId != 0)
                            {
                                campaignProfileDetails.UserId = externalServerUserId;
                                campaignProfileDetails.ClientId = externalServerClientId;
                                campaignProfileDetails.CampaignName = command.CampaignName;
                                campaignProfileDetails.CampaignDescription = command.CampaignDescription;
                                campaignProfileDetails.TotalBudget = command.TotalBudget;
                                campaignProfileDetails.MaxDailyBudget = command.MaxDailyBudget;
                                campaignProfileDetails.MaxBid = command.MaxBid;
                                campaignProfileDetails.MaxMonthBudget = command.MaxMonthBudget;
                                campaignProfileDetails.MaxWeeklyBudget = command.MaxWeeklyBudget;
                                campaignProfileDetails.MaxHourlyBudget = command.MaxHourlyBudget;
                                campaignProfileDetails.TotalCredit = command.TotalCredit;
                                campaignProfileDetails.SpendToDate = command.SpendToDate;
                                campaignProfileDetails.AvailableCredit = command.AvailableCredit;
                                campaignProfileDetails.PlaysToDate = command.PlaysToDate;
                                campaignProfileDetails.PlaysLastMonth = command.PlaysLastMonth;
                                campaignProfileDetails.PlaysCurrentMonth = command.PlaysCurrentMonth;
                                campaignProfileDetails.CancelledToDate = command.CancelledToDate;
                                campaignProfileDetails.CancelledLastMonth = command.CancelledLastMonth;
                                campaignProfileDetails.CancelledCurrentMonth = command.CancelledCurrentMonth;
                                campaignProfileDetails.SmsToDate = command.SmsToDate;
                                campaignProfileDetails.SmsLastMonth = command.SmsLastMonth;
                                campaignProfileDetails.SmsCurrentMonth = command.SmsCurrentMonth;
                                campaignProfileDetails.EmailToDate = command.EmailToDate;
                                campaignProfileDetails.EmailsLastMonth = command.EmailsLastMonth;
                                campaignProfileDetails.EmailsCurrentMonth = command.EmailsCurrentMonth;
                                campaignProfileDetails.EmailFileLocation = command.EmailFileLocation;
                                campaignProfileDetails.Active = command.Active;
                                campaignProfileDetails.NumberOfPlays = command.NumberOfPlays;
                                campaignProfileDetails.AverageDailyPlays = command.AverageDailyPlays;
                                campaignProfileDetails.SmsRequests = command.SmsRequests;
                                campaignProfileDetails.EmailsDelievered = command.EmailsDelievered;
                                campaignProfileDetails.EmailSubject = command.EmailSubject;
                                campaignProfileDetails.EmailBody = command.EmailBody;
                                campaignProfileDetails.EmailSenderAddress = command.EmailSenderAddress;
                                campaignProfileDetails.SmsOriginator = command.SmsOriginator;
                                campaignProfileDetails.SmsBody = command.SmsBody;
                                campaignProfileDetails.SMSFileLocation = command.SMSFileLocation;
                                campaignProfileDetails.CreatedDateTime = System.DateTime.Now;
                                campaignProfileDetails.UpdatedDateTime = System.DateTime.Now;
                                campaignProfileDetails.Status = command.Status;
                                campaignProfileDetails.StartDate = command.StartDate;
                                campaignProfileDetails.EndDate = command.EndDate;
                                campaignProfileDetails.NumberInBatch = command.NumberInBatch;
                                campaignProfileDetails.CountryId = externalServerCountryId;
                                campaignProfileDetails.IsAdminApproval = command.IsAdminApproval;
                                campaignProfileDetails.RemainingMaxDailyBudget = command.RemainingMaxDailyBudget;
                                campaignProfileDetails.RemainingMaxHourlyBudget = command.RemainingMaxHourlyBudget;
                                campaignProfileDetails.RemainingMaxWeeklyBudget = command.RemainingMaxWeeklyBudget;
                                campaignProfileDetails.RemainingMaxMonthBudget = command.RemainingMaxMonthBudget;
                                campaignProfileDetails.ProvidendSpendAmount = command.ProvidendSpendAmount;
                                campaignProfileDetails.BucketCount = command.BucketCount;
                                campaignProfileDetails.PhoneticAlphabet = command.PhoneticAlphabet;
                                campaignProfileDetails.NextStatus = command.NextStatus;
                                campaignProfileDetails.AdtoneServerCampaignProfileId = campaignProfile.CampaignProfileId;
                                campaignProfileDetails.CurrencyCode = campaignProfile.CurrencyCode;
                                db.SaveChanges();
                            }

                        }
                    }
                }
            }

            return new CommandResult(true);
        }
    }
}
