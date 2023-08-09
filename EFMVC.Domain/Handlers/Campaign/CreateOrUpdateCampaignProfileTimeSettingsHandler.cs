// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileTimeSettingsHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using EFMVC.Model;
using System.Linq;
/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class CreateOrUpdateCampaignProfileTimeSettingsHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileTimeSettingsHandler :
        ICommandHandler<CreateOrUpdateCampaignProfileTimeSettingCommand>
    {
        /// <summary>
        /// The _time settings repository
        /// </summary>
        private readonly ICampaignTimeSettingsRepository _timeSettingsRepository;
        private readonly ICampaignProfileRepository _profileRepository;
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileTimeSettingsHandler"/> class.
        /// </summary>
        /// <param name="timeSettingsRepository">The time settings repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileTimeSettingsHandler(ICampaignTimeSettingsRepository timeSettingsRepository, ICampaignProfileRepository profileRepository,
                                                                IUnitOfWork unitOfWork)
        {
            _timeSettingsRepository = timeSettingsRepository;
            _profileRepository = profileRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfileTimeSettingCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignProfileTimeSettingCommand command)
        {
            var timeSettings = new CampaignProfileTimeSetting();
            timeSettings = _timeSettingsRepository.GetById(command.CampaignProfileTimeSettingsId);
            if (timeSettings != null)
            {
                timeSettings.Monday = null;
                timeSettings.Tuesday = null;
                timeSettings.Wednesday = null;
                timeSettings.Thursday = null;
                timeSettings.Friday = null;
                timeSettings.Saturday = null;
                timeSettings.Sunday = null;
                foreach (string s in command.MondayPostedTimes.DayIds)
                    timeSettings.Monday += s + ",";
                if (!string.IsNullOrEmpty(timeSettings.Monday))
                    timeSettings.Monday = timeSettings.Monday.Substring(0, timeSettings.Monday.Length - 1);
                foreach (string s in command.TuesdayPostedTimes.DayIds)
                    timeSettings.Tuesday += s + ",";
                if (!string.IsNullOrEmpty(timeSettings.Tuesday))
                    timeSettings.Tuesday = timeSettings.Tuesday.Substring(0, timeSettings.Tuesday.Length - 1);
                foreach (string s in command.WednesdayPostedTimes.DayIds)
                    timeSettings.Wednesday += s + ",";
                if (!string.IsNullOrEmpty(timeSettings.Wednesday))
                    timeSettings.Wednesday = timeSettings.Wednesday.Substring(0, timeSettings.Wednesday.Length - 1);
                foreach (string s in command.ThursdayPostedTimes.DayIds)
                    timeSettings.Thursday += s + ",";
                if (!string.IsNullOrEmpty(timeSettings.Thursday))
                    timeSettings.Thursday = timeSettings.Thursday.Substring(0, timeSettings.Thursday.Length - 1);
                foreach (string s in command.FridayPostedTimes.DayIds)
                    timeSettings.Friday += s + ",";
                if (!string.IsNullOrEmpty(timeSettings.Friday))
                    timeSettings.Friday = timeSettings.Friday.Substring(0, timeSettings.Friday.Length - 1);
                foreach (string s in command.SaturdayPostedTimes.DayIds)
                    timeSettings.Saturday += s + ",";
                if (!string.IsNullOrEmpty(timeSettings.Saturday))
                    timeSettings.Saturday = timeSettings.Saturday.Substring(0, timeSettings.Saturday.Length - 1);
                foreach (string s in command.SundayPostedTimes.DayIds)
                    timeSettings.Sunday += s + ",";
                if (!string.IsNullOrEmpty(timeSettings.Sunday))
                    timeSettings.Sunday = timeSettings.Sunday.Substring(0, timeSettings.Sunday.Length - 1);
                timeSettings.CampaignProfileId = command.CampaignProfileId;
            }
            else
            {
                timeSettings = new CampaignProfileTimeSetting();
                timeSettings.Monday = string.Empty;
                foreach (string s in command.MondayPostedTimes.DayIds)
                    timeSettings.Monday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Monday))
                    timeSettings.Monday = timeSettings.Monday.Substring(0, timeSettings.Monday.Length - 1);

                timeSettings.Tuesday = string.Empty;
                foreach (string s in command.TuesdayPostedTimes.DayIds)
                    timeSettings.Tuesday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Tuesday))
                    timeSettings.Tuesday = timeSettings.Tuesday.Substring(0, timeSettings.Tuesday.Length - 1);

                timeSettings.Wednesday = string.Empty;
                foreach (string s in command.WednesdayPostedTimes.DayIds)
                    timeSettings.Wednesday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Wednesday))
                    timeSettings.Wednesday = timeSettings.Wednesday.Substring(0, timeSettings.Wednesday.Length - 1);

                timeSettings.Thursday = string.Empty;
                foreach (string s in command.ThursdayPostedTimes.DayIds)
                    timeSettings.Thursday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Thursday))
                    timeSettings.Thursday = timeSettings.Thursday.Substring(0, timeSettings.Thursday.Length - 1);

                timeSettings.Friday = string.Empty;
                foreach (string s in command.FridayPostedTimes.DayIds)
                    timeSettings.Friday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Friday))
                    timeSettings.Friday = timeSettings.Friday.Substring(0, timeSettings.Friday.Length - 1);

                timeSettings.Saturday = string.Empty;
                foreach (string s in command.SaturdayPostedTimes.DayIds)
                    timeSettings.Saturday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Saturday))
                    timeSettings.Saturday = timeSettings.Saturday.Substring(0, timeSettings.Saturday.Length - 1);

                timeSettings.Sunday = string.Empty;
                foreach (string s in command.SundayPostedTimes.DayIds)
                    timeSettings.Sunday += s + ",";

                if (!string.IsNullOrEmpty(timeSettings.Sunday))
                    timeSettings.Sunday = timeSettings.Sunday.Substring(0, timeSettings.Sunday.Length - 1);

                timeSettings.CampaignProfileId = command.CampaignProfileId;
                timeSettings.CampaignProfileTimeSettingsId = command.CampaignProfileTimeSettingsId;
            }

            int countryId = 0;
            var campaignProfile = _profileRepository.Get(x => x.CampaignProfileId == command.CampaignProfileId);
            if (campaignProfile != null)
            {
                countryId = (int)campaignProfile.CountryId;
            }
            if (timeSettings.CampaignProfileTimeSettingsId == 0)
            {
                _timeSettingsRepository.Add(timeSettings);
                _unitOfWork.Commit();
                var ConnString = ConnectionString.GetConnectionStringByCountryId(campaignProfile.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);

                        var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, command.CampaignProfileId);
                        if(externalServerCampaignProfileId != 0)
                        {
                            var timeSetting2 = new CampaignProfileTimeSetting();
                            timeSetting2.Monday = timeSettings.Monday;
                            timeSetting2.Tuesday = timeSettings.Tuesday;
                            timeSetting2.Wednesday = timeSettings.Wednesday;
                            timeSetting2.Thursday = timeSettings.Thursday;
                            timeSetting2.Friday = timeSettings.Friday;
                            timeSetting2.Saturday = timeSettings.Saturday;
                            timeSetting2.Sunday = timeSettings.Sunday;
                            timeSetting2.CampaignProfileId = externalServerCampaignProfileId;
                            timeSetting2.AdtoneServerCampaignProfileTimeId = timeSettings.CampaignProfileTimeSettingsId;


                            db.CampaignProfileTimeSettings.Add(timeSetting2);
                            db.SaveChanges();
                        }
              
                    }
                }

            }               
            else
            {
                _timeSettingsRepository.Update(timeSettings);
                _unitOfWork.Commit();
                var ConnString = ConnectionString.GetConnectionStringByCountryId(campaignProfile.CountryId);
                if (ConnString != null && ConnString.Count() > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var campaignTimeSettingData = db.CampaignProfileTimeSettings.Where(s => s.AdtoneServerCampaignProfileTimeId == command.CampaignProfileTimeSettingsId).FirstOrDefault();
                        if(campaignTimeSettingData != null)
                        {
                            var externalServerCampaignProfileId = OperatorServer.GetCampaignProfileIdFromOperatorServer(db, command.CampaignProfileId);
                            if(externalServerCampaignProfileId != 0)
                            {
                                campaignTimeSettingData.Monday = timeSettings.Monday;
                                campaignTimeSettingData.Tuesday = timeSettings.Tuesday;
                                campaignTimeSettingData.Wednesday = timeSettings.Wednesday;
                                campaignTimeSettingData.Thursday = timeSettings.Thursday;
                                campaignTimeSettingData.Friday = timeSettings.Friday;
                                campaignTimeSettingData.Saturday = timeSettings.Saturday;
                                campaignTimeSettingData.Sunday = timeSettings.Sunday;
                                campaignTimeSettingData.CampaignProfileId = externalServerCampaignProfileId;
                            }
                           
                            db.SaveChanges();
                        }
                    }
                }
            }
              

          

            return new CommandResult(true);
        }

        #endregion
    }
}