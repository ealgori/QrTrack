using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using QRTracker.service;

namespace QRTracker.Models
{
    public class QrResultModel
    {
        public QrResultModel(Constants.ResultTypes resultType)
        {
            using (ResultsService service = new ResultsService())
            {
                Result = service.GetResultText(resultType);
            }
        }

        public ResultText Result { get; set;}
        public QrDoc QrDoc { get; set;}
        public string ToJson()
        {
            return new JavaScriptSerializer().Serialize(this);
        }

    }
}