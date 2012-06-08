using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRTracker.Models;
using QRTracker.service;

namespace QRTracker.Controllers
{
    public class ActionsController : Controller
    {
        //
        // GET: /Actions/
        DocumentService documentService = new DocumentService();
        ResultsService resultsService = new ResultsService();
        RightService rightService = new RightService();
       
        public ActionResult Index()
        {
         QrDoc model = (QrDoc)TempData["qrDoc"];

        using (UserService userService = new UserService())
        {
                model.ActionUser = userService.GetUser(OtherFunctions.StripDomain(User.Identity.Name));
        }
         
            
            
         ActionModel actionModel = new ActionModel();
         actionModel.Tracks= new TrackModel();
         actionModel.Tracks.qrDoc = model;
         {
             // 
             // нулевое действие
             if (model.ActionId == 0)
             {
                 actionModel.Result = resultsService.GetResultText(Constants.ResultTypes.SucessDecode);
             }
             else
             
             // проверяем, разрешено ли делать то, что они хотят
             
             if (!rightService.HasChance(model))
             {
                 actionModel.Result = resultsService.GetResultText(Constants.ResultTypes.HasNoChance);
             }
             // проверяем, разрешено ли это данному пользователю
             else if (!rightService.HasRight(model))
             {
                 actionModel.Result = resultsService.GetResultText(Constants.ResultTypes.HasNoRight);
             }

             // проверяем существует ли документ
             else
             {
                 if (!documentService.DocExist(model))
                 {
                     documentService.AddDocument(model);
                 }


                 // проверяем, нет ли уже данной записаи
                 if (documentService.ExistTrack(model))
                 {
                     actionModel.Result = resultsService.GetResultText(Constants.ResultTypes.AlreadyExist);
                 }
                 else
                     // выполняем экшн
                     try
                     {
                         documentService.CreateTrack(model);
                         actionModel.Result = resultsService.GetResultText(Constants.ResultTypes.SuccessAction);
                     }
                     catch (Exception)
                     {

                         actionModel.Result = resultsService.GetResultText("какая то проблема при работе с базой");
                     }


             }



            
         }
            return PartialView(actionModel);



        }

        
        public ActionResult ShowError(string resultType)
        {

            ActionModel actionModel = new ActionModel();
            actionModel.Result = resultsService.GetResultText(resultType);
            
            return PartialView("Index",actionModel);



        }

        public ActionResult DeleteTrack(int trackId)
        {

            ActionModel actionModel = new ActionModel();

            int docId = documentService.GetDocumentIdByTrackId(trackId);
            if (docId == -1)
            {
                actionModel.Result = resultsService.GetResultText(Constants.ResultTypes.UnknownError);

                // документ не найден... редирект с неизвестной ошибкой
            }
            else
            {

                // получить ид владельца трека
                int trackOwnerId = documentService.GetTrackOwnerId(trackId);

                User currentUser;
                using (UserService userService = new UserService())
                {
                    // получить ид текущего пользователя
                    currentUser = userService.GetUser(OtherFunctions.StripDomain(User.Identity.Name));
                }
                actionModel.Tracks = new TrackModel();
                actionModel.Tracks.qrDoc = new QrDoc(docId, currentUser);

               

                actionModel.Tracks.qrDoc.ActionUser = currentUser;
                bool isManager = false;
                using (UserService userService = new UserService())
                {
                     isManager = userService.IsManager(currentUser.id);
                }
                if ((trackOwnerId != currentUser.id) && (!isManager))
                {
                    actionModel.Result = resultsService.GetResultText(Constants.ResultTypes.RightsError);
                }
                else
                {
                    // удалить трек
                    if (documentService.DeleteTrackById(trackId))
                    {
                        actionModel.Result = resultsService.GetResultText(Constants.ResultTypes.SuccessAction);
                    }
                    else
                    {
                        actionModel.Result = resultsService.GetResultText(Constants.ResultTypes.UnknownError);
                    }
                }

            }
            return PartialView("Index", actionModel);
            
            // новая трек модел с результатом, а так же ид текущего юзера и полуинициализированным qrdoc
        }

    }



}
