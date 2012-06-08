using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRTracker.Models;
using QRTracker.service;

namespace QRTracker.Controllers
{
    public class DetailsController : Controller
    {
        //
        // GET: /Details/
       DocumentService documentService = new DocumentService();
        public ActionResult Details(TrackModel trackModel )
        {



            // если контекст успел обновиться, получаем новые треки
            
            trackModel.Tracks = documentService.GetTracksByQrDoc(trackModel.qrDoc);

            {

                return PartialView("Details", trackModel);
            }
        }




    }
}
