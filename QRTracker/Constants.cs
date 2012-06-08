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
            UnknownError

        };
    }
}