using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using QRTracker.service;

namespace QRTracker.Models
{
    public class ActionModel
    {
        public TrackModel Tracks { get; set; }
        public ResultText Result { get; set; }
        public string ToJson()
        {
            return new JavaScriptSerializer().Serialize(Result);
        }
    }
}