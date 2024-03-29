﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRTracker.service;

namespace QRTracker.Controllers
{
    public class TodayController : Controller
    {
        //
        // GET: /Today/
        DocumentService documentService = new DocumentService();
        UserService userService = new UserService();
        public JsonResult GetStatuses()
        {

            User currentUser = userService.GetUser(OtherFunctions.StripDomain(User.Identity.Name));
            
            var tracks = documentService.GetDetailsByUserId(currentUser);
            List<string[]> _aaData = new List<string[]>();
            foreach (var track in tracks)
            {
                _aaData.Add(new string[] { track.docNum,
                    track.statuses,
                    track.statDate.ToString(Constants.DateFormat),
                    track.statDate.ToString(Constants.IncorrectDateFormat),
                    track.userName,
                    track.posted.HasValue?  track.posted.Value.ToString(Constants.DateFormat):"",
                    track.deleted.HasValue?  track.deleted.Value.ToString(Constants.DateFormat): "" });
            }
            JsonResult result = Json(new { aaData = _aaData },JsonRequestBehavior.AllowGet);
            return result;
        }

    }
}
