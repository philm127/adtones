// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 01-03-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 01-03-2014
// ***********************************************************************
// <copyright file="ViewModelToDomainMappingProfile.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using AutoMapper;
using EFMVC.Domain.Commands;
using EFMVC.Domain.Commands.Advert;
using EFMVC.Domain.Commands.Billing;
using EFMVC.Domain.Commands.BillingDetails;
using EFMVC.Domain.Commands.BlockedNumber;
using EFMVC.Domain.Commands.Campaign;
using EFMVC.Domain.Commands.Clients;
using EFMVC.Domain.Commands.CompanyDetails;
using EFMVC.Domain.Commands.Contacts;
using EFMVC.Domain.Commands.OperatorAdmin;
using EFMVC.Domain.Commands.ProfileAdmin;
using EFMVC.Domain.Commands.ProfileMatchInfo;
using EFMVC.Domain.Commands.Security;
using EFMVC.Web.Areas.Admin.ViewModel;
using EFMVC.Web.Models;
using EFMVC.Web.ViewModels;

/// <summary>
/// The Mappers namespace.
/// </summary>

namespace EFMVC.Web.Mappers
{
    /// <summary>
    /// Class ViewModelToDomainMappingProfile.
    /// </summary>
    internal class ViewModelToDomainMappingProfile : Profile
    {
        /// <summary>
        /// Gets the name of the profile.
        /// </summary>
        /// <value>The name of the profile.</value>
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        protected override void Configure()
        {
//            Mapper.CreateMap<CategoryFormModel, CreateOrUpdateCategoryCommand>();
//            Mapper.CreateMap<ExpenseFormModel, CreateOrUpdateExpenseCommand>();
            Mapper.CreateMap<UserFormModel, UserRegisterCommand>();

            Mapper.CreateMap<UserProfileFormModel, CreateOrUpdateProfileCommand>()
                .ForMember(dest => dest.UserProfileAttitudes, src => src.UseDestinationValue())
                .ForMember(dest => dest.UserProfileAdverts, src => src.UseDestinationValue())
                .ForMember(dest => dest.UserProfileCinemas, src => src.UseDestinationValue())
                .ForMember(dest => dest.UserProfileInternets, src => src.UseDestinationValue())
                .ForMember(dest => dest.UserProfileMobiles, src => src.UseDestinationValue())
                .ForMember(dest => dest.UserProfilePresses, src => src.UseDestinationValue())
                .ForMember(dest => dest.UserProfileProductsServices, src => src.UseDestinationValue())
                .ForMember(dest => dest.UserProfileRadios, src => src.UseDestinationValue())
                .ForMember(dest => dest.UserProfileTimeSettings, src => src.UseDestinationValue())
                .ForMember(dest => dest.UserProfileTvs, src => src.UseDestinationValue());

            Mapper.CreateMap<UserProfileAdvertFormModel, CreateOrUpdateUserProfileAdvertCommand>();
            Mapper.CreateMap<UserProfileAttitudeFormModel, CreateOrUpdateUserProfileAttitudeCommand>();
            Mapper.CreateMap<UserProfileCinemaFormModel, CreateOrUpdateUserProfileCinemaCommand>();
            Mapper.CreateMap<UserProfileInternetFormModel, CreateOrUpdateUserProfileInternetCommand>();
            Mapper.CreateMap<UserProfileMobileFormModel, CreateOrUpdateUserProfileMobileCommand>();
            Mapper.CreateMap<UserProfilePressFormModel, CreateOrUpdateUserProfilePressCommand>();
            Mapper.CreateMap<UserProfileProductsServiceFormModel, CreateOrUpdateUserProfileProductsServiceCommand>();
            Mapper.CreateMap<UserProfileRadioFormModel, CreateOrUpdateUserProfileRadioCommand>();
            Mapper.CreateMap<UserProfileTimeSettingFormModel, CreateOrUpdateUserProfileTimeSettingCommand>();
            Mapper.CreateMap<UserProfileTvFormModel, CreateOrUpdateUserProfileTvCommand>();
            Mapper.CreateMap<UserProfileCreditsReceivedFormModel, CreateOrUpdateUserProfileCreditsReceivedCommand>();
            Mapper.CreateMap<UserProfileAdvertsReceivedFromModel, CreateOrUpdateUserProfileAdvertsReceivedCommand>();
            Mapper.CreateMap<BlockedNumberFormModel, CreateOrUpdateBlockedNumberCommand>();
            Mapper.CreateMap<ContactsFormModel, CreateOrUpdateContactsCommand>();
            Mapper.CreateMap<CompanyDetailsFormModel, CreateOrUpdateCompanyDetailsCommand>();
            Mapper.CreateMap<SystemConfigFormModel, CreateOrUpdateSystemConfigCommand>();
            Mapper.CreateMap<CampaignProfileFormModel, CreateOrUpdateCampaignProfileCommand>()
                .ForMember(dest => dest.CampaignProfileAttitudes, src => src.UseDestinationValue())
                .ForMember(dest => dest.CampaignProfileAdverts, src => src.UseDestinationValue())
                .ForMember(dest => dest.CampaignProfileCinemas, src => src.UseDestinationValue())
                .ForMember(dest => dest.CampaignProfileInternets, src => src.UseDestinationValue())
                .ForMember(dest => dest.CampaignProfileMobiles, src => src.UseDestinationValue())
                .ForMember(dest => dest.CampaignProfilePresses, src => src.UseDestinationValue())
                .ForMember(dest => dest.CampaignProfileProductsServices, src => src.UseDestinationValue())
                .ForMember(dest => dest.CampaignProfileRadios, src => src.UseDestinationValue())
                .ForMember(dest => dest.CampaignProfileTimeSettings, src => src.UseDestinationValue())
                .ForMember(dest => dest.CampaignProfileTvs, src => src.UseDestinationValue());

            Mapper.CreateMap<CampaignProfileGeographicFormModel, CreateOrUpdateCampaignProfileGeographicCommand>();
            Mapper.CreateMap<CampaignProfileAdvertFormModel, CreateOrUpdateCampaignProfileAdvertCommand>();
            Mapper.CreateMap<CampaignProfileAttitudeFormModel, CreateOrUpdateCampaignProfileAttitudeCommand>();
            Mapper.CreateMap<CampaignProfileCinemaFormModel, CreateOrUpdateCampaignProfileCinemaCommand>();
            Mapper.CreateMap<CampaignProfileInternetFormModel, CreateOrUpdateCampaignProfileInternetCommand>();
            Mapper.CreateMap<CampaignProfileMobileFormModel, CreateOrUpdateCampaignProfileMobileCommand>();
            Mapper.CreateMap<CampaignProfilePressFormModel, CreateOrUpdateCampaignProfilePressCommand>();
            Mapper.CreateMap
                <CampaignProfileProductsServiceFormModel, CreateOrUpdateCampaignProfileProductsServiceCommand>();
            Mapper.CreateMap<CampaignProfileRadioFormModel, CreateOrUpdateCampaignProfileRadioCommand>();
            Mapper.CreateMap<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>();
            Mapper.CreateMap<CampaignProfileTvFormModel, CreateOrUpdateCampaignProfileTvCommand>();
            Mapper.CreateMap<CampaignProfileDemographicsFormModel, CreateOrUpdateCampaignProfileDemographicsCommand>();
            Mapper.CreateMap<ClientModel, CreateOrUpdateClientCommand>();
            Mapper.CreateMap<CampaignAdvertFormModel, CreateOrUpdateCampaignAdvertCommand>();

            Mapper.CreateMap<AdvertFormModel, CreateOrUpdateAdvertCommand>()
           .ForMember(dest => dest.OperatorId, opt => opt.MapFrom(src => src.OperatorId == 0 ? null : src.OperatorId));

            Mapper.CreateMap<AdminAdvertFormModel, CreateOrUpdateAdvertCommand>();

            Mapper.CreateMap<PostedTimesModel, PostedTimes>();

            Mapper.CreateMap<CampaignAuditFormModel, CreateOrUpdateCampaignAuditCommand>();

            Mapper.CreateMap<BillingFormModel, CreateOrUpdateBillingCommand>();
            Mapper.CreateMap<SagepayFormModel, CreateOrUpdateBillingCommand>();
            Mapper.CreateMap<MpesaFormModel, CreateOrUpdateBillingCommand>();
            Mapper.CreateMap<BillingDetailsFormModel, CreateOrUpdateBillingDetailsCommand>();

            Mapper.CreateMap<UsersCreditFormModel, CreateOrUpdateUsersCreditCommand>();
            Mapper.CreateMap<UsersCreditPaymentFormModel, CreateOrUpdateUsersCreditPaymentCommand>();

            Mapper.CreateMap<QuestionFormModel, CreateOrUpdateQuestionCommand>();
            Mapper.CreateMap<QuestionCommentFormModel, CreateOrUpdateQuestionCommentCommand>();
            Mapper.CreateMap<QuestionCommentImagesFormModel, CreateOrUpdateQuestionCommentImagesCommand>();
            Mapper.CreateMap<QuestionImagesFormModel, CreateOrUpdateQuestionImagesCommand>();

            Mapper.CreateMap<CountryFormModel, CreateOrUpdateCountryCommand>();
            Mapper.CreateMap<CountryTaxFormModel, CreateOrUpdateCountryTaxCommand>();

            Mapper.CreateMap<OperatorFormModel, CreateOrUpdateOperatorCommand>();
            Mapper.CreateMap<AreaFormModel, CreateOrUpdateAreaCommand>();

            Mapper.CreateMap<ProfileMatchInformationFormModel, CreateOrUpdateProfileMatchInfoCommand>();
            Mapper.CreateMap<NewCampaignProfileFormModel, CreateOrUpdateCopyCampaignProfileCommand>();
            Mapper.CreateMap<NewClientFormModel, CreateOrUpdateCopyClientCommand>();
            Mapper.CreateMap<NewCampaignProfileFormModel, DeleteCamapignProfileCommand>()
                .ForMember(dest => dest.Id, src => src.MapFrom(top => top.CampaignProfileId));
            Mapper.CreateMap<NewClientFormModel, DeleteClientCommand>()
                .ForMember(dest => dest.Id, src => src.MapFrom(top => top.ClientId));
            Mapper.CreateMap<NewAdvertFormModel, CreateOrUpdateCopyAdvertCommand>();
            Mapper.CreateMap<CampaignAdvertFormModel, DeleteCamapignAdvertCommand>()
                .ForMember(dest => dest.CampaignAdvertId, src => src.MapFrom(top => top.CampaignAdvertId));
            Mapper.CreateMap<NewAdvertFormModel, DeleteAdvertCommand>()
                .ForMember(dest => dest.Id, src => src.MapFrom(top => top.AdvertId));
            Mapper.CreateMap<CampaignProfilePreferenceFormModel, DeleteCampaignProfilePreferenceCommand>()
                .ForMember(dest => dest.CampaignProfilePreferenceId, src => src.MapFrom(top => top.CampaignProfilePreferenceId));
            Mapper.CreateMap<RewardFormModel, CreateOrUpdateRewardCommand>();

            Mapper.CreateMap<RewardFormModel, DeleteRewardCommand>()
                .ForMember(dest => dest.Id, src => src.MapFrom(top => top.Id));

            Mapper.CreateMap<NewAdvertRejectionFormModel, DeleteAdvertRejectionCommand>()
                .ForMember(dest => dest.Id, src => src.MapFrom(top => top.AdvertRejectionId));

            Mapper.CreateMap<UserProfileDemographicFormModel, CreateOrUpdateUserProfileDemographicsCommand>();

            Mapper.CreateMap<AdvertCategoryFormModel, CreateOrUpdateAdvertCategoryCommand>();
            Mapper.CreateMap<AdvertCategoryFormModel, DeleteAdvertCategoryCommand>();
            Mapper.CreateMap<CopyRightFormModel, CreateOrUpdateCopyRightCommand>();
            Mapper.CreateMap<OperatorAdminFormModel, CreateOrUpdateOperatorAdminRegistrationCommand>();
            Mapper.CreateMap<OperatorAdminFormModel, CreateOrUpdateContactsCommand>();
            Mapper.CreateMap<CampaignCreditFormModel, CreateOrUpdateCampaignCreditPeriodCommand>();
            Mapper.CreateMap<ProfileAdminRegistrationFormModel, CreateOrUpdateProfileAdminRegistrationCommand>();
            Mapper.CreateMap<ProfileAdminRegistrationFormModel, CreateOrUpdateContactsCommand>();
            Mapper.CreateMap<PromotionalCampaignFormModel, CreateOrUpdatePromotionalCampaignCommand>();
            Mapper.CreateMap<PromotionalAdvertFormModel, CreateOrUpdatePromotionalAdvertCommand>();
            Mapper.CreateMap<UserPasswordHistoryFormModel, UserPasswordHistoryCommand>();
        }
    }
}