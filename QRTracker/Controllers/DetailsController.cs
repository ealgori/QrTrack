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
        public string GetLastSuccess()
        {
            string goodPattern =
                @"<a data-ajax='true' data-ajax-mode='replace' data-ajax-update='#lastImp' href='/Details/GetLastSuccess'>{0}</a>";
            string badPattern =
               @"<a  style='color:red' data-ajax='true' data-ajax-mode='replace' data-ajax-update='#lastImp' href='/Details/GetLastSuccess'>{0}</a>";



            if (ImportGen.ImportWorker.LastSuccess == null)
            {
                if (ImportGen.ImportWorker.Freeze)
                {
                    return string.Format(badPattern, "never" + " :" + ImportGen.ImportWorker.State.ToString());
                }
                else
                {
                    return string.Format(goodPattern, "never" + " :" + ImportGen.ImportWorker.State.ToString());
                }
            }
            else
            {


                if (ImportGen.ImportWorker.Freeze)
                {
                    return string.Format(badPattern,
                                         ImportGen.ImportWorker.LastSuccess.ToString() + " :" +
                                         ImportGen.ImportWorker.State.ToString());
                }
                else
                {
                    return string.Format(goodPattern,
                                         ImportGen.ImportWorker.LastSuccess.ToString() + " :" +
                                         ImportGen.ImportWorker.State.ToString());
                }
            }
        }



    }
}
