﻿using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN_COSO.Extension
{
    public class PaypalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;

        static PaypalConfiguration()
        {
            var config = getconfig();
            clientId = "ARet3hh2cJSQDv2j6dEhHKYrMfMc44i5L3sCGXyuz1xWan7P_4Y5FYRs-voXqGyjGfRTjzNBeDUKEsSu";
            clientSecret = "EHtmP3kbJZT9W81g2PvYJLqwUw2MldMCgaboT8dbhRENdp40I_xe0XDCyFTN3wd-HY5o-Gvi9cjTh6kJ";
        }

        private static Dictionary<string, string> getconfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            // getting accesstocken from paypal  
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken  
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}