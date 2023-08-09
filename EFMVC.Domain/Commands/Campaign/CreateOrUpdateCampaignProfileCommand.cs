// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class CreateOrUpdateCampaignProfileCommand.
    /// </summary>
    public class CreateOrUpdateCampaignProfileCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the name of the campaign.
        /// </summary>
        /// <value>The name of the campaign.</value>
        public string CampaignName { get; set; }

        /// <summary>
        /// Gets or sets the campaign description.
        /// </summary>
        /// <value>The campaign description.</value>
        public string CampaignDescription { get; set; }

     
        /// <summary>
        /// Gets or sets the total budget.
        /// </summary>
        /// <value>The total budget.</value>
        public decimal TotalBudget { get; set; }

        /// <summary>
        /// Gets or sets the maximum daily budget.
        /// </summary>
        /// <value>The maximum daily budget.</value>
        public float MaxDailyBudget { get; set; }

        /// <summary>
        /// Gets or sets the maximum bid.
        /// </summary>
        /// <value>The maximum bid.</value>
        public float MaxBid { get; set; }

        public float MaxMonthBudget { get; set; }

        public float MaxWeeklyBudget { get; set; }
        public float MaxHourlyBudget { get; set; }
        public decimal TotalCredit { get; set; }

        public float SpendToDate { get; set; }

        /// <summary>
        /// Gets or sets the available credit.
        /// </summary>
        /// <value>The available credit.</value>
        public decimal AvailableCredit { get; set; }

        /// <summary>
        /// Gets or sets the plays to date.
        /// </summary>
        /// <value>The plays to date.</value>
        public int PlaysToDate { get; set; }

        /// <summary>
        /// Gets or sets the plays last month.
        /// </summary>
        /// <value>The plays last month.</value>
        public int PlaysLastMonth { get; set; }

        /// <summary>
        /// Gets or sets the plays current month.
        /// </summary>
        /// <value>The plays current month.</value>
        public int PlaysCurrentMonth { get; set; }

        /// <summary>
        /// Gets or sets the cancelled to date.
        /// </summary>
        /// <value>The cancelled to date.</value>
        public int CancelledToDate { get; set; }

        /// <summary>
        /// Gets or sets the cancelled last month.
        /// </summary>
        /// <value>The cancelled last month.</value>
        public int CancelledLastMonth { get; set; }

        /// <summary>
        /// Gets or sets the cancelled current month.
        /// </summary>
        /// <value>The cancelled current month.</value>
        public int CancelledCurrentMonth { get; set; }

        /// <summary>
        /// Gets or sets the SMS to date.
        /// </summary>
        /// <value>The SMS to date.</value>
        public int SmsToDate { get; set; }

        /// <summary>
        /// Gets or sets the SMS last month.
        /// </summary>
        /// <value>The SMS last month.</value>
        public int SmsLastMonth { get; set; }

        /// <summary>
        /// Gets or sets the SMS current month.
        /// </summary>
        /// <value>The SMS current month.</value>
        public int SmsCurrentMonth { get; set; }

        /// <summary>
        /// Gets or sets the email to date.
        /// </summary>
        /// <value>The email to date.</value>
        public int EmailToDate { get; set; }

        /// <summary>
        /// Gets or sets the emails last month.
        /// </summary>
        /// <value>The emails last month.</value>
        public int EmailsLastMonth { get; set; }

        /// <summary>
        /// Gets or sets the emails current month.
        /// </summary>
        /// <value>The emails current month.</value>
        public int EmailsCurrentMonth { get; set; }

        public string EmailFileLocation { get; set; }
       

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CreateOrUpdateCampaignProfileCommand"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the number of plays.
        /// </summary>
        /// <value>The number of plays.</value>
        public int NumberOfPlays { get; set; }

        /// <summary>
        /// Gets or sets the average daily plays.
        /// </summary>
        /// <value>The average daily plays.</value>
        public int AverageDailyPlays { get; set; }

        /// <summary>
        /// Gets or sets the SMS requests.
        /// </summary>
        /// <value>The SMS requests.</value>
        public int SmsRequests { get; set; }

        /// <summary>
        /// Gets or sets the emails delievered.
        /// </summary>
        /// <value>The emails delievered.</value>
        public int EmailsDelievered { get; set; }

        /// <summary>
        /// Gets or sets the email subject.
        /// </summary>
        /// <value>The email subject.</value>
        public string EmailSubject { get; set; }

        /// <summary>
        /// Gets or sets the email body.
        /// </summary>
        /// <value>The email body.</value>
        public string EmailBody { get; set; }

        /// <summary>
        /// Gets or sets the email sender address.
        /// </summary>
        /// <value>The email sender address.</value>
        public string EmailSenderAddress { get; set; }

        /// <summary>
        /// Gets or sets the SMS originator.
        /// </summary>
        /// <value>The SMS originator.</value>
        public string SmsOriginator { get; set; }

        /// <summary>
        /// Gets or sets the SMS body.
        /// </summary>
        /// <value>The SMS body.</value>
        public string SmsBody { get; set; }

        /// <summary>
        /// Gets or sets the SMS FileLocation.
        /// </summary>
        /// <value>The SMS FileLocation.</value>
        public string SMSFileLocation { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        public DateTime UpdatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime ? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime? EndDate { get; set; }

        public int Status { get; set; }

        public int NumberInBatch { get; set; }
        public int CountryId { get; set; }
        public bool IsAdminApproval { get; set; }

        public bool NextStatus { get; set; }

        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the adverts.
        /// </summary>
        /// <value>The adverts.</value>
        public ICollection<CreateOrUpdateAdvertCommand> Adverts { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile adverts.
        /// </summary>
        /// <value>The campaign profile adverts.</value>
        public ICollection<CreateOrUpdateCampaignProfileAdvertCommand> CampaignProfileAdverts { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile attitudes.
        /// </summary>
        /// <value>The campaign profile attitudes.</value>
        public ICollection<CreateOrUpdateCampaignProfileAttitudeCommand> CampaignProfileAttitudes { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile cinemas.
        /// </summary>
        /// <value>The campaign profile cinemas.</value>
        public ICollection<CreateOrUpdateCampaignProfileCinemaCommand> CampaignProfileCinemas { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile internets.
        /// </summary>
        /// <value>The campaign profile internets.</value>
        public ICollection<CreateOrUpdateCampaignProfileInternetCommand> CampaignProfileInternets { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile mobiles.
        /// </summary>
        /// <value>The campaign profile mobiles.</value>
        public ICollection<CreateOrUpdateCampaignProfileMobileCommand> CampaignProfileMobiles { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile presses.
        /// </summary>
        /// <value>The campaign profile presses.</value>
        public ICollection<CreateOrUpdateCampaignProfilePressCommand> CampaignProfilePresses { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile products services.
        /// </summary>
        /// <value>The campaign profile products services.</value>
        public ICollection<CreateOrUpdateCampaignProfileProductsServiceCommand> CampaignProfileProductsServices { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile radios.
        /// </summary>
        /// <value>The campaign profile radios.</value>
        public ICollection<CreateOrUpdateCampaignProfileRadioCommand> CampaignProfileRadios { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile time settings.
        /// </summary>
        /// <value>The campaign profile time settings.</value>
        public ICollection<CreateOrUpdateCampaignProfileTimeSettingCommand> CampaignProfileTimeSettings { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile TVS.
        /// </summary>
        /// <value>The campaign profile TVS.</value>
        public ICollection<CreateOrUpdateCampaignProfileTvCommand> CampaignProfileTvs { get; set; }

        /// <summary>
        /// Gets or sets the campaign adverts.
        /// </summary>
        /// <value>The campaign adverts.</value>
        public virtual ICollection<CreateOrUpdateCampaignAdvertCommand> CampaignAdverts { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile date settings.
        /// </summary>
        /// <value>The campaign profile date settings.</value>
        public virtual ICollection<CreateOrUpdateCampaignDateSettingsCommand> CampaignProfileDateSettings { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile demographics.
        /// </summary>
        /// <value>The campaign profile demographics.</value>
        public virtual ICollection<CreateOrUpdateCampaignProfileDemographicsCommand> CampaignProfileDemographics { get; set; }

        public virtual ICollection<CreateOrUpdateCampaignProfileGeographicCommand> CreateOrUpdateCampaignProfileGeographic { get; set; }
    }
}