using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRTracker
{
    public static class Constants
    {
   

      

        public enum ResultTypes
        {
            DecodeError,
            ParseError,
            RightsError,
            UnAuthorized,
            SuccessAction,
            HasNoChance,
            AlreadyExist,
            HasNoRight,
            SucessDecode,
            UnknownError,
            ImportNow

        };

        public static string ImportFolder = @"\\EV001B78BFE400\Import$\upload\";
        public static string ImportTempFolder = @"C:\Import\temp\";

        public static int ImportInterval = 1*60*1000;
    }
}