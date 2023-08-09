$(document).ready(function () {

    if (window.location.href.toLowerCase().indexOf("admin/usermanagement") > -1) {
        $("#liuser").addClass("active opened active");
        $("#liuser > ul").addClass("active");
        $("#adminusermanagement").addClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/campaigncredit") > -1) {
        $("#licampaign").addClass("active opened active");
        $("#licampaign > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").addClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/campaign") > -1) {
        $("#licampaign").addClass("active opened active");
        $("#licampaign > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").addClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/useradvert") > -1) {
        $("#licampaign").addClass("active opened active");
        $("#licampaign > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").addClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/usercredit") > -1) {
        $("#liuser").addClass("active opened active");
        $("#liuser > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").addClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/userpayment") > -1) {
        $("#liinvoice").addClass("active opened active");
        $("#liinvoice > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").addClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/question") > -1) {
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").addClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/userprofile") > -1) {
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#admin_userprofile").addClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/countrytax") > -1) {
        $("#licountry").addClass("active opened active");
        $("#licountry > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").addClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/country") > -1) {
        $("#licountry").addClass("active opened active");
        $("#licountry > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").addClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/invoice") > -1) {
        $("#liinvoice").addClass("active opened active");
        $("#liinvoice > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").addClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/systemconfig") > -1) {
        $("#lisystem").addClass("active opened active");
        $("#lisystem > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").addClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/adminuserprofile") > -1) {
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").addClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/importcsv") > -1) {
        $("#liuser").addClass("active opened active");
        $("#liuser > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").addClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    //Add 12-04-2019
    else if (window.location.href.toLowerCase().indexOf("admin/operatorregistration") > -1) {
        $("#lioperator").addClass("active opened active");
        $("#lioperator > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").addClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }

    else if (window.location.href.toLowerCase().indexOf("admin/operator") > -1) {
        $("#lioperator").addClass("active opened active");
        $("#lioperator > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").addClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/profilematchinformation") > -1) {
        $("#liuser").addClass("active opened active");
        $("#liuser > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").addClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/area") > -1) {
        $("#licountry").addClass("active opened active");
        $("#licountry > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").addClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/ImportFileTrack") > -1) {
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").addClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }

    //Add 08-02-2019
    else if (window.location.href.toLowerCase().indexOf("admin/managementreport") > -1) {
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").addClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");

    }

    //Add 13-02-2019
    else if (window.location.href.toLowerCase().indexOf("admin/reward") > -1) {
        $("#lisystem").addClass("active opened active");
        $("#lisystem > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").addClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }

    //Add 22-03-2019
    else if (window.location.href.toLowerCase().indexOf("admin/advertcategory") > -1) {
        $("#licampaign").addClass("active opened active");
        $("#licampaign > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").addClass("active");
        $("#copyRight").removeClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }

    //Add 01-04-2019
    else if (window.location.href.toLowerCase().indexOf("admin/copyright") > -1) {
        $("#lisystem").addClass("active opened active");
        $("#lisystem > ul").addClass("active");
        $("#adminusermanagement").removeClass("active");
        $("#admincampaign").removeClass("active");
        $("#adminadvert").removeClass("active");
        $("#admincredit").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp").removeClass("active");
        $("#userprofile").removeClass("active");
        $("#admincountryTaxmanagement").removeClass("active");
        $("#admincountrymanagement").removeClass("active");
        $("#adminuserinvoice").removeClass("active");
        $("#adminSystemConfig").removeClass("active");
        $("#admin_userprofile").removeClass("active");
        $("#adminImportCsv").removeClass("active");
        $("#adminOperator").removeClass("active");
        $("#adminProfileMatchInformation").removeClass("active");
        $("#adminArea").removeClass("active");
        $("#importFileTrack").removeClass("active");
        $("#rewards").removeClass("active");
        $("#advertcategory").removeClass("active");
        $("#copyRight").addClass("active");
        $("#adminOperatorRegistration").removeClass("active");
        $("#adminCreditPeriod").removeClass("active");
    }

    if (window.location.href.toLowerCase().indexOf("users/accountoverview") > -1) {
        $("#useraccountoverview").addClass("active");
        $("#userpersonalinfo").removeClass("active");
        $("#userblockednumber").removeClass("active");
        $("#usermobile").removeClass("active");
        $("#usertimesettings").removeClass("active");
        $("#user_profile").removeClass("active");
        $("#userTicket").removeClass("active");
        $("#user_account").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("users/personalinfo") > -1) {
        $("#useraccountoverview").removeClass("active");
        $("#userpersonalinfo").addClass("active");
        $("#userblockednumber").removeClass("active");
        $("#usermobile").removeClass("active");
        $("#usertimesettings").removeClass("active");
        $("#user_profile").removeClass("active");
        $("#userTicket").removeClass("active");
        $("#user_account").removeClass("active");
    }

    else if (window.location.href.toLowerCase().indexOf("users/blockednumber") > -1) {
        $("#useraccountoverview").removeClass("active");
        $("#userpersonalinfo").removeClass("active");
        $("#userblockednumber").addClass("active");
        $("#usermobile").removeClass("active");
        $("#usertimesettings").removeClass("active");
        $("#user_profile").removeClass("active");
        $("#userTicket").removeClass("active");
        $("#user_account").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("users/mobile") > -1) {
        $("#useraccountoverview").removeClass("active");
        $("#userpersonalinfo").removeClass("active");
        $("#userblockednumber").removeClass("active");
        $("#usermobile").addClass("active");
        $("#usertimesettings").removeClass("active");
        $("#user_profile").removeClass("active");
        $("#userTicket").removeClass("active");
        $("#user_account").removeClass("active");

    }
    else if (window.location.href.toLowerCase().indexOf("users/timesettings") > -1) {
        $("#useraccountoverview").removeClass("active");
        $("#userpersonalinfo").removeClass("active");
        $("#userblockednumber").removeClass("active");
        $("#usermobile").removeClass("active");
        $("#usertimesettings").addClass("active");
        $("#user_profile").removeClass("active");
        $("#userTicket").removeClass("active");
        $("#user_account").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("users/profile") > -1) {
        $("#useraccountoverview").removeClass("active");
        $("#userpersonalinfo").removeClass("active");
        $("#userblockednumber").removeClass("active");
        $("#usermobile").removeClass("active");
        $("#usertimesettings").removeClass("active");
        $("#user_profile").addClass("active");
        $("#userTicket").removeClass("active");
        $("#user_account").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("users/account") > -1) {
        $("#useraccountoverview").removeClass("active");
        $("#userpersonalinfo").removeClass("active");
        $("#userblockednumber").removeClass("active");
        $("#usermobile").removeClass("active");
        $("#usertimesettings").removeClass("active");
        $("#user_profile").removeClass("active");
        $("#userTicket").removeClass("active");
        $("#user_account").addClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("users/ticket") > -1) {
        $("#useraccountoverview").removeClass("active");
        $("#userpersonalinfo").removeClass("active");
        $("#userblockednumber").removeClass("active");
        $("#usermobile").removeClass("active");
        $("#usertimesettings").removeClass("active");
        $("#user_profile").removeClass("active");
        $("#user_account").removeClass("active");
        $("#userTicket").addClass("active");
    }

    if (window.location.href.toLowerCase().indexOf("usersadmin/useraccount") > -1) {
        $("#useradmin_account").removeClass("active");
        $("#useradminAccountOverview").removeClass("active");
        $("#useradminProfileManagement").removeClass("active");
        $("#useradminPersonalInfo").removeClass("active");
        $("#useradminBlockedNumber").removeClass("active");
        $("#useradminticket").removeClass("active");
        $("#useradminIndex").addClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("usersadmin/account") > -1) {
        $("#useradmin_account").addClass("active");
        $("#useradminIndex").removeClass("active");
        $("#useradmin_account").removeClass("active");
        $("#useradminAccountOverview").removeClass("active");
        $("#useradminProfileManagement").removeClass("active");
        $("#useradminPersonalInfo").removeClass("active");
        $("#useradminticket").removeClass("active");
        $("#useradminBlockedNumber").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("usersadmin/useradminaccountoverview") > -1) {
        $("#useradminAccountOverview").addClass("active");
        $("#useradmin_account").removeClass("active");
        $("#useradminIndex").removeClass("active");
        $("#useradmin_account").removeClass("active");
        $("#useradminProfileManagement").removeClass("active");
        $("#useradminPersonalInfo").removeClass("active");
        $("#useradminticket").removeClass("active");
        $("#useradminBlockedNumber").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("usersadmin/useradminblockednumber") > -1) {
        $("#useradminAccountOverview").removeClass("active");
        $("#useradmin_account").removeClass("active");
        $("#useradminIndex").removeClass("active");
        $("#useradmin_account").removeClass("active");
        $("#useradminProfileManagement").removeClass("active");
        $("#useradminPersonalInfo").removeClass("active");
        $("#useradminticket").removeClass("active");
        $("#useradminBlockedNumber").addClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("usersadmin/useradminprofile") > -1) {
        $("#useradminAccountOverview").removeClass("active");
        $("#useradmin_account").removeClass("active");
        $("#useradminIndex").removeClass("active");
        $("#useradmin_account").removeClass("active");
        $("#useradminProfileManagement").addClass("active");
        $("#useradminPersonalInfo").removeClass("active");
        $("#useradminticket").removeClass("active");
        $("#useradminBlockedNumber").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("usersadmin/useradminpersonalinfo") > -1) {
        $("#useradminAccountOverview").removeClass("active");
        $("#useradmin_account").removeClass("active");
        $("#useradminIndex").removeClass("active");
        $("#useradmin_account").removeClass("active");
        $("#useradminProfileManagement").removeClass("active");
        $("#useradminPersonalInfo").addClass("active");
        $("#useradminticket").removeClass("active");
        $("#useradminBlockedNumber").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("usersadmin/ticket") > -1) {
        $("#useradminAccountOverview").removeClass("active");
        $("#useradmin_account").removeClass("active");
        $("#useradminIndex").removeClass("active");
        $("#useradmin_account").removeClass("active");
        $("#useradminProfileManagement").removeClass("active");
        $("#useradminPersonalInfo").removeClass("active");
        $("#useradminBlockedNumber").removeClass("active");
        $("#useradminticket").addClass("active");
    }

    if (window.location.href.toLowerCase().indexOf("advertadmin/ticket") > -1) {
        $("#advertadmin-userprofile").removeClass("active");
        $("#advertadmin-useradvert").removeClass("active");
        $("#advertadmin-ticket").addClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("advertadmin/useradvert") > -1) {
        $("#advertadmin-userprofile").removeClass("active");
        $("#advertadmin-ticket").removeClass("active");
        $("#advertadmin-useradvert").addClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("advertadmin/userprofile") > -1) {
        $("#advertadmin-ticket").removeClass("active");
        $("#advertadmin-useradvert").removeClass("active");
        $("#advertadmin-userprofile").addClass("active");
    }

    if (window.location.href.toLowerCase().indexOf("operatoradmin/ticket") > -1) {
        $("#operatoradmin-userprofile").removeClass("active");
        $("#operatoradmin-useradvert").removeClass("active");
        $("#operatoradmin-user").removeClass("active");
        $("#operatoradmin-ticket").addClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("operatoradmin/useradvert") > -1) {
        $("#operatoradmin-ticket").removeClass("active");
        $("#operatoradmin-userprofile").removeClass("active");
        $("#operatoradmin-user").removeClass("active");
        $("#operatoradmin-useradvert").addClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("operatoradmin/userprofile") > -1) {
        $("#operatoradmin-ticket").removeClass("active");
        $("#operatoradmin-useradvert").removeClass("active");
        $("#operatoradmin-user").removeClass("active");
        $("#operatoradmin-userprofile").addClass("active");

    }
    else if (window.location.href.toLowerCase().indexOf("operatoradmin/advertisersmanagement") > -1) {
        $("#operatoradmin-ticket").removeClass("active");
        $("#operatoradmin-useradvert").removeClass("active");
        $("#operatoradmin-user").addClass("active");
        $("#operatoradmin-userprofile").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("operatoradmin/usercampaign") > -1) {
        $("#operatoradmin-ticket").removeClass("active");
        $("#operatoradmin-userprofile").removeClass("active");
        $("#operatoradmin-user").removeClass("active");
        $("#operatoradmin-useradvert").removeClass("active");
        $("#operatoradmin-campaign").addClass("active");
    }
});

function adminmenujs() {
    if (window.location.href.toLowerCase().indexOf("admin/usermanagement") > -1) {
        $("#liuser").addClass("active opened active");
        $("#liuser > ul").addClass("active");
        $("#adminusermanagement1").addClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    //Add 23-01-2020
    if (window.location.href.toLowerCase().indexOf("admin/promotionaluser") > -1) {
        $("#liuser").addClass("active opened active");
        $("#liuser > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").addClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/campaigncredit") > -1) {
        $("#licampaign").addClass("active opened active");
        $("#licampaign > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").addClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/campaign") > -1) {
        $("#licampaign").addClass("active opened active");
        $("#licampaign > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").addClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    //Add 24-01-2020
    else if (window.location.href.toLowerCase().indexOf("admin/promotionalcampaign") > -1) {
        $("#licampaign").addClass("active opened active");
        $("#licampaign > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").addClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/useradvert") > -1) {
        $("#licampaign").addClass("active opened active");
        $("#licampaign > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").addClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/usercredit") > -1) {
        $("#liuser").addClass("active opened active");
        $("#liuser > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").addClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/userpayment") > -1) {
        $("#liinvoice").addClass("active opened active");
        $("#liinvoice > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").addClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/question") > -1) {
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").addClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/userprofile") > -1) {
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#admin_userprofile1").addClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/countrytax") > -1) {
        $("#licountry").addClass("active opened active");
        $("#licountry > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").addClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/country") > -1) {
        $("#licountry").addClass("active opened active");
        $("#licountry > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").addClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/invoice") > -1) {
        $("#liinvoice").addClass("active opened active");
        $("#liinvoice > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").addClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/systemconfig") > -1) {
        $("#lisystem").addClass("active opened active");
        $("#lisystem > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").addClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/adminuserprofile") > -1) {
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").addClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/importcsv") > -1) {
        $("#liuser").addClass("active opened active");
        $("#liuser > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").addClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }

    //Add 12-04-2019
    else if (window.location.href.toLowerCase().indexOf("admin/operatorregistration") > -1) {
        $("#lioperator").addClass("active opened active");
        $("#lioperator > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").addClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }

    //Add 01-01-2020
    else if (window.location.href.toLowerCase().indexOf("admin/operatormaxadverts") > -1) {
        $("#lioperator").addClass("active opened active");
        $("#lioperator > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").addClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }

    //Add 06-01-2020
    else if (window.location.href.toLowerCase().indexOf("admin/operatorconfiguration") > -1) {
        $("#lioperator").addClass("active opened active");
        $("#lioperator > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").addClass("active");
    }

    else if (window.location.href.toLowerCase().indexOf("admin/operator") > -1) {
        $("#lioperator").addClass("active opened active");
        $("#lioperator > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").addClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/profilematchinformation") > -1) {
        $("#liuser").addClass("active opened active");
        $("#liuser > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").addClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/area") > -1) {
        $("#licountry").addClass("active opened active");
        $("#licountry > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoic1e").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").addClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }
    else if (window.location.href.toLowerCase().indexOf("admin/ImportFileTrack") > -1) {
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").addClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }

    //Add 08-02-2019
    else if (window.location.href.toLowerCase().indexOf("admin/managementreport") > -1) {
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").addClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");

    }

    //Add 13-02-2019
    else if (window.location.href.toLowerCase().indexOf("admin/reward") > -1) {
        $("#lisystem").addClass("active opened active");
        $("#lisystem > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").addClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }

    //Add 22-03-2019
    else if (window.location.href.toLowerCase().indexOf("admin/advertcategory") > -1) {
        $("#licampaign").addClass("active opened active");
        $("#licampaign > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").addClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }

    //Add 01-04-2019
    else if (window.location.href.toLowerCase().indexOf("admin/copyright") > -1) {
        $("#lisystem").addClass("active opened active");
        $("#lisystem > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").addClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").removeClass("active");
    }

    //Add 28-11-2019
    else if (window.location.href.toLowerCase().indexOf("admin/profileadminregistration") > -1) {
        $("#liuser").addClass("active opened active");
        $("#liuser > ul").addClass("active");
        $("#adminusermanagement1").removeClass("active");
        $("#adminpromotionalusermanagement1").removeClass("active");
        $("#admincampaign1").removeClass("active");
        $("#adminpromotionalcampaign1").removeClass("active");
        $("#adminadvert1").removeClass("active");
        $("#admincredit1").removeClass("active");
        $("#adminuserPay1").removeClass("active");
        $("#adminHelp1").removeClass("active");
        $("#userprofile1").removeClass("active");
        $("#admincountryTaxmanagement1").removeClass("active");
        $("#admincountrymanagement1").removeClass("active");
        $("#adminuserinvoice1").removeClass("active");
        $("#adminSystemConfig1").removeClass("active");
        $("#admin_userprofile1").removeClass("active");
        $("#adminImportCsv1").removeClass("active");
        $("#adminOperator1").removeClass("active");
        $("#adminProfileMatchInformation1").removeClass("active");
        $("#adminArea1").removeClass("active");
        $("#importFileTrack1").removeClass("active");
        $("#rewards1").removeClass("active");
        $("#advertcategory1").removeClass("active");
        $("#copyRight1").removeClass("active");
        $("#adminOperatorRegistration1").removeClass("active");
        $("#adminCreditPeriod1").removeClass("active");
        $("#adminOperatorMaxAdvert1").removeClass("active");
        $("#adminOperatorConfiguration1").removeClass("active");
        $("#adminProfileAdminRegistration1").addClass("active");
    }
}
//Add 28-05-2019 Check Decimal Value
function isNumber(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (
        //(charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;
    return true;
}

//Add 30-07-2019
$(".onlyDigit").keypress(function (e) {
    //if the letter is not digit then display error and don't type anything
    $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
    if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});

//Add 30-07-2019
$(".only-numeric").keypress(function (e) {
    var keyCode = e.which ? e.which : e.keyCode

    if (!(keyCode >= 48 && keyCode <= 57)) {
        $(".error").css("display", "inline");
        return false;
    } else {
        $(".error").css("display", "none");
    }
});

//Add 15-05-2019
//Function for Admin, Advert-Admin, Operator-Admin, User-Admin
function multipleTabLogout() {
    function lsTest() {
        var test = 'test';
        try {
            localStorage.setItem(test, test);
            localStorage.removeItem(test);
            return true;
        } catch (e) {
            return false;
        }
    }

    // listen to storage event
    window.addEventListener('storage', function (event) {
        // do what you want on logout-event
        if (event.key == 'logout-event') {
            //$('#console').html('Received logout event! Insert logout script here.');
            //window.location = "logout.php";
            window.location.pathname = "Admin/Login/Index";
        }
    }, false);

    $(document).ready(function () {
        if (lsTest()) {
            $('#logoutbtn').on('click', function () {
                // change logout-event and therefore send an event
                localStorage.setItem('logout-event', 'logout' + Math.random());
                return true;
            });
        } else {
            // setInterval or setTimeout
        }
    });
}

function popUpMultipleTabLogout() {
    function lsTest() {
        var test = 'test';
        try {
            localStorage.setItem(test, test);
            localStorage.removeItem(test);
            return true;
        } catch (e) {
            return false;
        }
    }

    // listen to storage event
    window.addEventListener('storage', function (event) {
        // do what you want on logout-event
        if (event.key == 'logout-event') {
            //$('#console').html('Received logout event! Insert logout script here.');
            //window.location = "logout.php";
            window.location.pathname = "Admin/Login/Index";
        }
    }, false);

    $(document).ready(function () {
        if (lsTest()) {
            // change logout-event and therefore send an event
            localStorage.setItem('logout-event', 'logout' + Math.random());
            return true;
        } else {
            // setInterval or setTimeout
        }
    });
}

//Add 15-05-2019
//Function for Users
function usersMultipleTabLogout() {
    function lsTest() {
        var test = 'test';
        try {
            localStorage.setItem(test, test);
            localStorage.removeItem(test);
            return true;
        } catch (e) {
            return false;
        }
    }

    // listen to storage event
    window.addEventListener('storage', function (event) {
        // do what you want on logout-event
        if (event.key == 'logout-event') {
            //$('#console').html('Received logout event! Insert logout script here.');
            //window.location = "logout.php";
            window.location.pathname = "Users/Login/Index";
        }
    }, false);

    $(document).ready(function () {
        if (lsTest()) {
            $('#logoutbtn').on('click', function () {
                // change logout-event and therefore send an event
                localStorage.setItem('logout-event', 'logout' + Math.random());
                return true;
            });
        } else {
            // setInterval or setTimeout
        }
    });
}

function popupUsersMultipleTabLogout() {
    function lsTest() {
        var test = 'test';
        try {
            localStorage.setItem(test, test);
            localStorage.removeItem(test);
            return true;
        } catch (e) {
            return false;
        }
    }

    // listen to storage event
    window.addEventListener('storage', function (event) {
        // do what you want on logout-event
        if (event.key == 'logout-event') {
            //$('#console').html('Received logout event! Insert logout script here.');
            //window.location = "logout.php";
            window.location.pathname = "Users/Login/Index";
        }
    }, false);

    $(document).ready(function () {
        if (lsTest()) {
            // change logout-event and therefore send an event
            localStorage.setItem('logout-event', 'logout' + Math.random());
            return true;
        } else {
            // setInterval or setTimeout
        }
    });
}

//Add 24-04-2019
//Function for Admin, Advert-Admin, Operator-Admin, User-Admin
//var intervalHolder = null;
//var instances = 0;
//var pocTimeoutDialogAdmin;
//function adminSessionTimeOut(sessiontimeout, valuestop, difference) {
//    var url = $("#dialog").data('request-url');
//    var time_out_url = '/Admin/UserManagement/SessionTimeout';
//    var minutes = difference.minutes();
//    var seconds = difference.seconds();
//    var finaltime = (minutes * 60) + seconds;

//    String.prototype.format = function () {
//        var s = this,
//            i = arguments.length;
//        while (i--) {
//            s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
//        }
//        return s;
//    };
//    var vsc = valuestop.seconds();
//    var pocTimeoutDialog = {
//        calculateTimer: 0,
//        settings: {
//            timeout: sessiontimeout.minutes(), //2000000 //1200000000 //20 minutes
//            countdown: valuestop.seconds(), //20 seconds
//            title: 'Your session is about to expire!',
//            message: 'You will be logged out in {0} seconds.',
//            question: 'Do you want to continue with your session?',
//            keep_alive_url: window.location.href,
//            //logout_redirect_url: '@Url.Action("Index", "Login", new { area = "Admin" })'
//            logout_redirect_url: 'Admin/Login/Index'
//        },
//        init: function () {
//            // alert('Hi');
//            this.setupDialogTimer();
//        },
//        setupDialogTimer: function () {
//            var self = this;
//            self.setTimer = window.setTimeout(function () {
//                self.setupDialog();
//            }, (finaltime - vsc) * 1000);
//        },
//        clearDialogTimer: function () {
//            var self = this;
//            window.clearTimeout(self.setTimer);
//            self.init();
//        },
//        setupDialog: function () {
//            var self = this;
//            self.destroyDialog();
//            $("#dialog").html("");
//            $('<div class="modal-dialog modal-sm">' +
//                '<div class="modal-content">' +
//                '<div class="modal-header">' +
//                '<div class="row">' +
//                '<label class="col-lg-12 control-label" style="text-align: center;font-size: medium;">' + this.settings.title + '</label>' +
//                '</div>' +
//                '</div>' +
//                '<div class="modal-body">' +
//                '<form class="form-horizontal">' +
//                '<div class="form-group">' +
//                '<div class="row">' +
//                '<label class="col-lg-12 control-label" style="text-align: center;">' + this.settings.message.format(' <span id = "sessionTimeoutCountdown" class= "session-timeout-countdown" > ' + this.settings.countdown + '</span>') + '</label>' +
//                '</div>' +
//                '<div class="row">' +
//                '<label class="col-lg-12 control-label" style="text-align: center;">' + this.settings.question + '</label>' +
//                '</div>' +
//                '</div>' +
//                '</form>' +
//                '</div>' +
//                '<div class="modal-footer">' +
//                '<button id="dialogKeepSession" data-dismiss="modal" class="dialog-keep-session-btn btn btn-sm btn-blue">Yes, Keep me signed in</button> &nbsp;' +
//                '<button id="dialogSignOut" data-dismiss="modal" class="dialog-sign-out-btn btn btn-sm btn-blue">No, Sign me out</button>' +
//                '</div>' +
//                '</div>' +
//                '</div>').appendTo('#dialog');

//            $("#dialog").modal();
//            $('#dialogKeepSession').on('click', function () {
//                $.ajax({
//                    url: time_out_url,
//                    type: "GET",
//                    cache: false,
//                    success: function () {
//                        self.keepAlive();
//                    },
//                    error: function () {
//                        self.signOut();
//                    }
//                });
//                //self.keepAlive();
//                $("#dialog").hide();
//            });
//            $('#dialogSignOut').on('click', function () {
//                self.signOut();
//                $("#dialog").hide();
//            });
//            ///self.clearCountdown();
//            self.startCountdown();
//        },
//        destroyDialog: function () {
//            if ($("#sessionTimeoutDialog").length) {
//                $('#sessionTimeoutDialog').remove();
//            }
//        },
//        startCountdown: function () {
//            var self = this,
//                counter = this.settings.countdown;
//            self.clearCountdown();
//            this.calculateTimer = window.setInterval(function () {
//                counter -= 1;
//                $("#sessionTimeoutCountdown").html(counter);
//                if (counter <= 0) {
//                    self.clearCountdown();
//                    self.signOut();
//                }
//            }, 1000);
//        },
//        clearCountdown: function () {
//            window.clearInterval(this.calculateTimer);
//            this.calculateTimer = undefined;
//        },
//        keepAlive: function () {
//            sessionTimeoutStatus = true;
//            var self = this;
//            self.destroyDialog();
//            $.ajax({
//                url: this.settings.keep_alive_url,
//                type: "GET",
//                cache: false,
//                success: function () {
//                    self.clearCountdown();
//                    self.init();
//                },
//                error: function () {
//                    self.signOut();
//                }
//            });
//        },
//        signOut: function () {
//            var self = this;
//            self.destroyDialog();
//            $.ajax({
//                type: "GET",
//                url: url,
//                cache: false,
//                success: function () {
//                    self.redirectLogout();
//                },
//                error: function () {
//                    self.signOut();
//                }
//            });
//            //self.redirectLogout();
//        },
//        redirectLogout: function () {
//            //window.location = this.settings.logout_redirect_url;
//            var url = 'Admin/Login/Index';
//            if (typeof (history.pushState) != "undefined") {
//                var obj = { Page: 'Logout', Url: url };
//                history.pushState(obj, obj.Page, obj.Url);
//            } else {
//                alert("Browser does not support HTML5.");
//            }

//            window.location.pathname = this.settings.logout_redirect_url;

//            //Add 13-05-2019 for logout multiple tab
//            function lsTest() {
//                var test = 'test';
//                try {
//                    localStorage.setItem(test, test);
//                    localStorage.removeItem(test);
//                    return true;
//                } catch (e) {
//                    return false;
//                }
//            }
//            // listen to storage event
//            window.addEventListener('storage', function (event) {
//                // do what you want on logout-event
//                if (event.key == 'logout-event') {
//                    //$('#console').html('Received logout event! Insert logout script here.');
//                    //window.location = "logout.php";
//                    window.location.pathname = this.settings.logout_redirect_url;
//                }
//            }, false);
//            $(document).ready(function () {
//                if (lsTest()) {
//                    //$('#logout').on('click', function () {
//                        // change logout-event and therefore send an event
//                        localStorage.setItem('logout-event', 'logout' + Math.random());
//                        return true;
//                    //});
//                } else {
//                    // setInterval or setTimeout
//                }
//            });
//        }
//    };

//    $(function () {
//        pocTimeoutDialog.init();
//    });
//}

////Add 30-04-2019
////Function for Advert-Admin
//var intervalHolder = null;
//function advertAdminSessionTimeOut(sessiontimeout, valuestop, difference) {
//    var adverturl = $("#dialog").data('request-url');
//    var advert_time_out_url = '/AdvertAdmin/Ticket/SessionTimeout';
//    var minutes = difference.minutes();
//    var seconds = difference.seconds();
//    var finaltime = (minutes * 60) + seconds;
//    String.prototype.format = function () {
//        var s = this,
//            i = arguments.length;
//        while (i--) {
//            s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
//        }
//        return s;
//    };
//    var widgetId = "0";

//    var pocTimeoutDialog = {
//        calculateTimer: 0,
//        settings: {
//            timeout: sessiontimeout.minutes(), //2000000 //1200000000 //20 minutes
//            countdown: valuestop.seconds(), //20 seconds
//            title: 'Your session is about to expire!',
//            message: 'You will be logged out in {0} seconds.',
//            question: 'Do you want to continue with your session?',
//            keep_alive_url: window.location.href,
//            //logout_redirect_url: '@Url.Action("Index", "Login", new { area = "Admin" })'
//            logout_redirect_url: 'Admin/Login/Index'
//        },
//        init: function () {
//            // alert('Hi');            
//            var aaa = "sessionTimeoutCountdown_" + widgetId.toString();
//            if ($("[id=" + aaa + "]").length > 0) {
//                widgetId = new Date().getMilliseconds().toString()
//                $("[id=" + aaa + "]").attr("id", "sessionTimeoutCountdown_" + widgetId);
//            }
//            if (this.calculateTimer != null) clearInterval(this.calculateTimer);
//            this.setupDialogTimer();
//        },
//        setupDialogTimer: function () {
//            window.clearInterval(intervalHolder);
//            intervalHolder = null;
//            var self = this;
//            self.setTimer = window.setTimeout(function () {
//                self.setupDialog();
//            }, (finaltime - this.settings.countdown) * 1000);
//            /*(difference.hours() + ":" + difference.minutes() + ":" + difference.seconds()) * 1000);*/
//            /*(this.settings.timeout - this.settings.countdown) * 1000);*/
//        },
//        clearDialogTimer: function () {
//            var self = this;
//            window.clearTimeout(self.setTimer);
//            self.init();
//        },
//        setupDialog: function () {
//            var self = this;
//            self.destroyDialog();
//            if (widgetId == "0") {
//                $("#dialog").html("");
//                $('<div class="modal-dialog modal-sm">' +
//                    '<div class="modal-content">' +
//                    '<div class="modal-header">' +
//                    '<div class="row">' +
//                    '<label class="col-lg-12 control-label" style="text-align: center;font-size: medium;">' + this.settings.title + '</label>' +
//                    '</div>' +
//                    '</div>' +
//                    '<div class="modal-body">' +
//                    '<form class="form-horizontal">' +
//                    '<div class="form-group">' +
//                    '<div class="row">' +
//                    '<label class="col-lg-12 control-label" style="text-align: center;">' + this.settings.message.format(' <span id = "sessionTimeoutCountdown_' + widgetId.toString() + '" class= "session-timeout-countdown" > ' + this.settings.countdown + '</span>') + '</label>' +
//                    '</div>' +
//                    '<div class="row">' +
//                    '<label class="col-lg-12 control-label" style="text-align: center;">' + this.settings.question + '</label>' +
//                    '</div>' +
//                    '</div>' +
//                    '</form>' +
//                    '</div>' +
//                    '<div class="modal-footer">' +
//                    '<button id="dialogKeepSession" data-dismiss="modal" class="dialog-keep-session-btn btn btn-sm btn-blue">Yes, Keep me signed in</button> &nbsp;' +
//                    '<button id="dialogSignOut" data-dismiss="modal" class="dialog-sign-out-btn btn btn-sm btn-blue">No, Sign me out</button>' +
//                    '</div>' +
//                    '</div>' +
//                    '</div>').appendTo('#dialog');
//            }

//            $("#dialog").modal();
//            //$("#seconds").html(this.settings.countdown);
//            $('#dialogKeepSession').on('click', function () {
//                $.ajax({
//                    url: advert_time_out_url,
//                    type: "GET",
//                    cache: false,
//                    success: function () {
//                        self.keepAlive();
//                    },
//                    error: function () {
//                        self.signOut();
//                    }
//                });
//                //self.keepAlive();
//                $("#dialog").hide();
//            });
//            $('#dialogSignOut').on('click', function () {
//                self.signOut();
//                $("#dialog").hide();
//            });
//            self.startCountdown();
//        },
//        destroyDialog: function () {
//            if ($("#sessionTimeoutDialog").length) {
//                $('#sessionTimeoutDialog').remove();
//            }
//        },
//        startCountdown: function () {
//            var self = this,
//                counter = this.settings.countdown;
//            self.clearCountdown();
//            intervalHolder = window.setInterval(function () {
//                counter -= 1;
//                var wid = widgetId;
//                console.log(widgetId);
//                var aaa = "sessionTimeoutCountdown_" + wid.toString();
//                $("[id=" + aaa + "]").html(counter);
//                if (counter <= 0) {
//                    self.clearCountdown();
//                    self.signOut();
//                }
//            }, 1000);
//        },
//        clearCountdown: function () {
//            window.clearInterval(intervalHolder);
//            //window.clearInterval(this.calculateTimer);
//            intervalHolder = null;
//            this.calculateTimer = null;
//        },
//        keepAlive: function () {
//            sessionTimeoutStatus = true;
//            var self = this;
//            self.destroyDialog();
//            $.ajax({
//                url: this.settings.keep_alive_url,
//                type: "GET",
//                cache: false,
//                success: function () {
//                    self.clearCountdown();
//                    // self.init();
//                },
//                error: function () {
//                    self.signOut();
//                }
//            });
//        },
//        signOut: function () {
//            var self = this;
//            self.destroyDialog();
//            $.ajax({
//                type: "GET",
//                url: adverturl,
//                cache: false,
//                success: function () {
//                    self.redirectLogout();
//                },
//                error: function () {
//                    self.signOut();
//                }
//            });
//            //self.redirectLogout();
//        },
//        redirectLogout: function () {
//            //window.location = this.settings.logout_redirect_url;
//            var url = 'Admin/Login/Index';
//            if (typeof (history.pushState) != "undefined") {
//                var obj = { Page: 'Logout', Url: url };
//                history.pushState(obj, obj.Page, obj.Url);
//            } else {
//                alert("Browser does not support HTML5.");
//            }

//            window.location.pathname = this.settings.logout_redirect_url;
//        }
//    };

//    $(function () {
//        pocTimeoutDialog.init();
//    });
//}

////Add 24-04-2019
////Function for Users
//function userSessionTimeOut() {
//    var url = $("#dialog").data('request-url');

//    String.prototype.format = function () {
//        var s = this,
//            i = arguments.length;
//        while (i--) {
//            s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
//        }
//        return s;
//    };

//    var pocTimeoutDialog = {
//        calculateTimer: 0,
//        settings: {
//            timeout: 1200000000, //2000000 //20 minutes
//            countdown: 20, //20 seconds
//            title: 'Your session is about to expire!',
//            message: 'You will be logged out in {0} seconds.',
//            question: 'Do you want to continue with your session?',
//            keep_alive_url: window.location.href,
//            //logout_redirect_url: '@Url.Action("Index", "Login", new { area = "Admin" })'
//            logout_redirect_url: 'Users/Login/Index'
//        },
//        init: function () {
//            // alert('Hi');
//            this.setupDialogTimer();
//        },
//        setupDialogTimer: function () {
//            var self = this;
//            self.setTimer = window.setTimeout(function () {
//                self.setupDialog();
//            }, (this.settings.timeout - this.settings.countdown) * 1000);
//        },
//        clearDialogTimer: function () {
//            var self = this;
//            window.clearTimeout(self.setTimer);
//            self.init();
//        },
//        setupDialog: function () {
//            var self = this;
//            self.destroyDialog();

//            $('<div class="modal-dialog modal-sm">' +
//                '<div class="modal-content">' +
//                '<div class="modal-header">' +
//                '<div class="row">' +
//                '<label class="col-lg-12 control-label" style="text-align: center;font-size: medium;">' + this.settings.title + '</label>' +
//                '</div>' +
//                '</div>' +
//                '<div class="modal-body">' +
//                '<form class="form-horizontal">' +
//                '<div class="form-group">' +
//                '<div class="row">' +
//                '<label class="col-lg-12 control-label" style="text-align: center;">' + this.settings.message.format(' <span id = "sessionTimeoutCountdown" class= "session-timeout-countdown" > ' + this.settings.countdown + '</span>') + '</label>' +
//                '</div>' +
//                '<div class="row">' +
//                '<label class="col-lg-12 control-label" style="text-align: center;">' + this.settings.question + '</label>' +
//                '</div>' +
//                '</div>' +
//                '</form>' +
//                '</div>' +
//                '<div class="modal-footer">' +
//                '<button id="dialogKeepSession" data-dismiss="modal" class="dialog-keep-session-btn btn btn-sm btn-blue">Yes, Keep me signed in</button> &nbsp;' +
//                '<button id="dialogSignOut" data-dismiss="modal" class="dialog-sign-out-btn btn btn-sm btn-blue">No, Sign me out</button>' +
//                '</div>' +
//                '</div>' +
//                '</div>').appendTo('#dialog');

//            $("#dialog").modal();
//            $("#seconds").html(this.settings.countdown);
//            $('#dialogKeepSession').on('click', function () {
//                self.keepAlive();
//                $("#dialog").hide();
//            });
//            $('#dialogSignOut').on('click', function () {
//                self.signOut();
//                $("#dialog").hide();
//            });
//            self.startCountdown();
//        },
//        destroyDialog: function () {
//            if ($("#sessionTimeoutDialog").length) {
//                $('#sessionTimeoutDialog').remove();
//            }
//        },
//        startCountdown: function () {
//            var self = this,
//                counter = this.settings.countdown;
//            self.clearCountdown();
//            this.calculateTimer = window.setInterval(function () {
//                counter -= 1;
//                $("#sessionTimeoutCountdown").html(counter);
//                if (counter <= 0) {
//                    self.clearCountdown();
//                    self.signOut();
//                }
//            }, 1000);
//        },
//        clearCountdown: function () {
//            window.clearInterval(this.calculateTimer);
//            this.calculateTimer = undefined;
//        },
//        keepAlive: function () {
//            sessionTimeoutStatus = true;
//            var self = this;
//            self.destroyDialog();
//            $.ajax({
//                url: this.settings.keep_alive_url,
//                type: "GET",
//                cache: false,
//                success: function () {
//                    self.clearCountdown();
//                    // self.init();
//                },
//                error: function () {
//                    self.signOut();
//                }
//            });
//        },
//        signOut: function () {
//            var self = this;
//            self.destroyDialog();
//            $.ajax({
//                type: "GET",
//                url: url,
//                cache: false,
//                success: function () {
//                    self.redirectLogout();
//                },
//                error: function () {
//                    self.signOut();
//                }
//            });
//            //self.redirectLogout();
//        },
//        redirectLogout: function () {
//            //window.location = this.settings.logout_redirect_url;
//            var url = 'Users/Login/Index';
//            if (typeof (history.pushState) != "undefined") {
//                var obj = { Page: 'Logout', Url: url };
//                history.pushState(obj, obj.Page, obj.Url);
//            } else {
//                alert("Browser does not support HTML5.");
//            }

//            window.location.pathname = this.settings.logout_redirect_url;
//        }
//    };

//    $(function () {
//        pocTimeoutDialog.init();
//    });
//}