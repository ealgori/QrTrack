using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRTracker.Models.Abstract;
using QRTracker.service;

namespace QRTracker.Models
{
    public class TrackExtend
    {
        public Track Track { get; set; }
        public bool AllowAction { get; set; }

    }

    public class TrackModel
    {
        public List<TrackExtend> Tracks { get; set; }
       
        public BaseQrDoc qrDoc { get; set; }

        
    }
}