using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRTracker.Models;
using QRTracker.service;

namespace QRTracker.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/
        UserService userService = new UserService();
        RightService rightService = new RightService();
        ResultsService resultsService = new ResultsService();
        public ActionResult Index()
        {
            List<QRTracker.service.User> users = userService.GetAllUsers();
            return View(users);
        }

        
        //private RoleModel GetRoleModel(string id, ResultText resultText)
        //{
            
        //    /return roleModel;
        //}
        
        
        public ActionResult GetRoles(string id, ResultText resultText)
        {

            List<Role> roles = rightService.GetAllRoles();
            List<int> rights = rightService.GetRoles(int.Parse(id)).Select(rig => rig.id).ToList();
            RoleModel roleModel = new RoleModel();
            roleModel.ResultText = resultText;
            roleModel.User = userService.GetUser(int.Parse(id));
            roleModel.roleListModel = new List<RoleListModel>();
            foreach (var role in roles)
            {
                bool hasRole = rights.Any(rol => rights.Contains(role.id));
                roleModel.roleListModel.Add(new RoleListModel() { id = role.id, name = role.name, hasRole = hasRole });
            }

            return PartialView(roleModel);
        }
        [HttpPost]
        public ActionResult GetRoles(FormCollection collection)
        {
            ResultText resultText = new ResultText();
            int userId = int.Parse(collection["userId"]);
            if (userService.GetUserWithDomain(User.Identity.Name).isManager)
            {

               
                User user = userService.GetUser(userId);
                bool isManager = collection["isManager"] == "true,false";
                user.isManager = isManager;

                bool isViewer = collection["isViewer"] == "true,false";
                user.isViewer = isViewer;

                userService.ApplyCurrentValues(user);
                foreach (string key in collection.Keys)
                {
                    int id = -1;
                    try
                    {
                        id = int.Parse(key);
                        bool val = collection[key] == "true,false";
                        if (val)
                        {
                            rightService.AddRole(user.id,id);
                        }
                        else
                        {
                            rightService.RemoveRole(user.id,id);
                        }

                    }
                    catch (Exception)
                    {
                        
                        
                    }
                   
                }


                resultText = resultsService.GetResultText(Constants.ResultTypes.SuccessAction);

            }
            else
            {
                resultText = resultsService.GetResultText(Constants.ResultTypes.HasNoRight);
            }

            //return GetRoles(userId.ToString(),resultText);//return "loaded" + id;
            return GetRoles(userId.ToString(), resultText);
            
        }
    }
}
