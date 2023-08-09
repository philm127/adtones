// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileTimeSettingsHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateUserProfileTimeSettingsHandler.
    /// </summary>
    public class CreateOrUpdateUserProfileTimeSettingsHandler :
        ICommandHandler<CreateOrUpdateUserProfileTimeSettingCommand>
    {
        /// <summary>
        /// The _time settings repository
        /// </summary>
        private readonly ITimeSettingsRepository _timeSettingsRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUserProfileTimeSettingsHandler"/> class.
        /// </summary>
        /// <param name="timeSettingsRepository">The time settings repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUserProfileTimeSettingsHandler(ITimeSettingsRepository timeSettingsRepository,
                                                            IUnitOfWork unitOfWork)
        {
            _timeSettingsRepository = timeSettingsRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateUserProfileTimeSettingCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateUserProfileTimeSettingCommand command)
        {
            var timeSettings = new UserProfileTimeSetting();

            timeSettings.Monday = null;
            foreach (string s in command.MondayPostedTimes.DayIds)
                timeSettings.Monday += s + ",";

            if (!string.IsNullOrEmpty(timeSettings.Monday))
                timeSettings.Monday = timeSettings.Monday.Substring(0, timeSettings.Monday.Length - 1);

            timeSettings.Tuesday = null;
            foreach (string s in command.TuesdayPostedTimes.DayIds)
                timeSettings.Tuesday += s + ",";

            if (!string.IsNullOrEmpty(timeSettings.Tuesday))
                timeSettings.Tuesday = timeSettings.Tuesday.Substring(0, timeSettings.Tuesday.Length - 1);

            timeSettings.Wednesday = null;
            foreach (string s in command.WednesdayPostedTimes.DayIds)
                timeSettings.Wednesday += s + ",";

            if (!string.IsNullOrEmpty(timeSettings.Wednesday))
                timeSettings.Wednesday = timeSettings.Wednesday.Substring(0, timeSettings.Wednesday.Length - 1);

            timeSettings.Thursday = null;
            foreach (string s in command.ThursdayPostedTimes.DayIds)
                timeSettings.Thursday += s + ",";

            if (!string.IsNullOrEmpty(timeSettings.Thursday))
                timeSettings.Thursday = timeSettings.Thursday.Substring(0, timeSettings.Thursday.Length - 1);

            timeSettings.Friday = null;
            foreach (string s in command.FridayPostedTimes.DayIds)
                timeSettings.Friday += s + ",";

            if (!string.IsNullOrEmpty(timeSettings.Friday))
                timeSettings.Friday = timeSettings.Friday.Substring(0, timeSettings.Friday.Length - 1);

            timeSettings.Saturday = null;
            foreach (string s in command.SaturdayPostedTimes.DayIds)
                timeSettings.Saturday += s + ",";

            if (!string.IsNullOrEmpty(timeSettings.Saturday))
                timeSettings.Saturday = timeSettings.Saturday.Substring(0, timeSettings.Saturday.Length - 1);

            timeSettings.Sunday = null;
            foreach (string s in command.SundayPostedTimes.DayIds)
                timeSettings.Sunday += s + ",";

            if (!string.IsNullOrEmpty(timeSettings.Sunday))
                timeSettings.Sunday = timeSettings.Sunday.Substring(0, timeSettings.Sunday.Length - 1);

            timeSettings.UserProfileId = command.UserProfileId;
            timeSettings.UserProfileTimeSettingsId = command.UserProfileTimeSettingsId;

            var ConnString = ConnectionString.GetConnectionStringByOperatorId(command.OperatorId);
            if (timeSettings.UserProfileTimeSettingsId == 0)
            {
                _timeSettingsRepository.Add(timeSettings);
                _unitOfWork.Commit();

                if (ConnString != null && ConnString.Count > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var externalServerUserProfileId = OperatorServer.GetUserProfileIdFromOperatorServer(db, command.UserProfileId);
                        if(externalServerUserProfileId != 0)
                        {
                            var timeSetting2 = new UserProfileTimeSetting();
                            timeSetting2.Monday = timeSettings.Monday;
                            timeSetting2.Thursday = timeSettings.Tuesday;
                            timeSetting2.Wednesday = timeSettings.Wednesday;
                            timeSetting2.Thursday = timeSettings.Thursday;
                            timeSetting2.Friday = timeSettings.Friday;
                            timeSetting2.Saturday = timeSettings.Saturday;
                            timeSetting2.Sunday = timeSettings.Sunday;
                            timeSetting2.UserProfileId = externalServerUserProfileId;
                            db.UserProfileTimeSetting.Add(timeSetting2);
                            db.SaveChanges();
                        }
             
                    }

                }
            }
            else
            {
                _timeSettingsRepository.Update(timeSettings);
                _unitOfWork.Commit();
                if (ConnString != null && ConnString.Count > 0)
                {
                    foreach (var item in ConnString)
                    {
                        EFMVCDataContex db = new EFMVCDataContex(item);
                        var timeProfile = db.UserProfileTimeSetting.Where(s => s.AdtoneServerUserProfileTimeSettingId == command.UserProfileTimeSettingsId).FirstOrDefault();
                        if(timeProfile != null)
                        {
                            var externalServerUserProfileId = OperatorServer.GetUserProfileIdFromOperatorServer(db, command.UserProfileId);
                            if(externalServerUserProfileId != 0)
                            {
                                timeProfile.Monday = timeSettings.Monday;
                                timeProfile.Tuesday = timeSettings.Tuesday;
                                timeProfile.Wednesday = timeSettings.Wednesday;
                                timeProfile.Thursday = timeSettings.Thursday;
                                timeProfile.Friday = timeSettings.Friday;
                                timeProfile.Saturday = timeSettings.Saturday;
                                timeProfile.Sunday = timeSettings.Sunday;
                                timeProfile.UserProfileId = externalServerUserProfileId;
                                db.SaveChanges();
                            }
                           
                        }                      
                    }

                }
            }

           

            return new CommandResult(true);
        }

        #endregion
    }
}