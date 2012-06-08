using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace QRTracker.Models
{
    public class ResultAndQRDocModel
    {
        public QrResultModel QrResultModel { get; set; }
        public QrDoc QrDoc { get; set; }
        public string ToJson()
        {
            return new JavaScriptSerializer().Serialize(this);
        }
    }
}