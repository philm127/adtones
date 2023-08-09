using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using com.paypal.sdk.profiles;
using com.paypal.sdk.services;

namespace EFMVC.Web.Paypal
{
    public class DoDirectPayment
    {
        private static string _OwnerAPIUN= ConfigurationManager.AppSettings["paypalmerchantemail"].ToString();
        private static string _OwnerAPISignature= ConfigurationManager.AppSettings["paypalsecurity"].ToString();
        private static string _OwnerAPIPassword= ConfigurationManager.AppSettings["paypalmerchantpassword"].ToString();
        public static string OwnerAPI
        {
            get { return _OwnerAPIUN; }
            set { _OwnerAPIUN = ConfigurationManager.AppSettings["paypalmerchantemail"].ToString(); }
        }
        public static string OwnerAPISignature
        {
            get { return _OwnerAPISignature; }
            set { _OwnerAPISignature = ConfigurationManager.AppSettings["paypalsecurity"].ToString();}
        }
        public static string OwnerAPIPassword
        {
            get { return _OwnerAPIPassword; }
            set { _OwnerAPIPassword = ConfigurationManager.AppSettings["paypalmerchantpassword"].ToString(); }
        }
        public DoDirectPayment()
        {
        }
        public NVPCodec DoDirectPaymentCode(string paymentAction, string amount, string creditCardType, string creditCardNumber, string expdate_month, string cvv2Number, string firstName, string lastName, string countrycode, string currencycode)
        {
            NVPCallerServices caller = new NVPCallerServices();
            IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();

            if (!string.IsNullOrEmpty(OwnerAPI) && !string.IsNullOrEmpty(OwnerAPISignature) && !string.IsNullOrEmpty(OwnerAPIPassword))
            {
                profile.APIUsername = OwnerAPI.ToString();
                profile.APIPassword = OwnerAPIPassword.ToString();
                profile.APISignature = OwnerAPISignature.ToString();
            }
          
            //profile.Environment = "sandbox";
            profile.Environment = ConfigurationManager.AppSettings["paypalmode"].ToString();
            caller.APIProfile = profile;

            NVPCodec encoder = new NVPCodec();
            //encoder("X-PAYPAL-APPLICATION-ID") = "APP-3WM427375B414890M"
            encoder["VERSION"] = "109.0";
            encoder["METHOD"] = "DoDirectPayment";
            //amount = "0.1";
            // Add request-specific fields to the request.
            encoder["PAYMENTACTION"] = paymentAction;
            encoder["AMT"] = amount;
            encoder["CREDITCARDTYPE"] = creditCardType;
            encoder["ACCT"] = creditCardNumber;
            encoder["EXPDATE"] = expdate_month;
            encoder["CVV2"] = cvv2Number;
            encoder["FIRSTNAME"] = firstName;
            encoder["LASTNAME"] = lastName;
            encoder["STREET"] = "";
            encoder["CITY"] = "";
            encoder["STATE"] = "";
            encoder["ZIP"] = "";
            encoder["COUNTRYCODE"] = countrycode;
            encoder["CURRENCYCODE"] = currencycode;

            // Execute the API operation and obtain the response.
            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = caller.Call(pStrrequestforNvp);

            NVPCodec decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            //  Dim amout As String = decoder("TRANSACTIONID")
            return decoder;

        }

        public NVPCodec refund(string paymentAction, string transcationid, string tp)
        {
            NVPCallerServices caller = new NVPCallerServices();
            IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();

            if (!string.IsNullOrEmpty(OwnerAPI) && !string.IsNullOrEmpty(OwnerAPISignature) && !string.IsNullOrEmpty(OwnerAPIPassword))
            {
                profile.APIUsername = OwnerAPI.ToString();
                profile.APIPassword = OwnerAPIPassword.ToString();
                profile.APISignature = OwnerAPISignature.ToString();
            }
           
            profile.Environment = ConfigurationManager.AppSettings["paypalmode"].ToString();
            //profile.Environment = "live";
            caller.APIProfile = profile;

            NVPCodec encoder = new NVPCodec();
            encoder["VERSION"] = "109.0";
            encoder["METHOD"] = paymentAction;
            
            encoder["TRANSACTIONID"] = transcationid;
            encoder["REFUNDTYPE"] = tp;
            
            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = caller.Call(pStrrequestforNvp);

            NVPCodec decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);
            return decoder;
        }
    }
}