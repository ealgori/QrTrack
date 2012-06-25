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
        // Максимальное время которое ипорт может не подавать признаки жизни. в секундах
        public static int MaxImportDownTime = 60;
        // интервал для проверки того что файл пропал из папки импорта
        public static int FileCheckInterval = 10000;


        public static string DateFormat { get { return "dd.MM.yyyy hh:mm"; }}

        public static string IncorrectDateFormat
        {
            get { return "yyyyMMddhhmm"; }
        }

    }
}