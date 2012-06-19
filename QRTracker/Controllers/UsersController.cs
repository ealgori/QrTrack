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
        public ActionResult Index()
        {
            List<QRTracker.service.User> users = userService.GetAllUsers();
            return View(users);
        }

        public ActionResult GetRoles(string id)
        {
            List<Role> roles = rightService.GetAllRoles();
            List<int> rights = rightService.GetRoles(int.Parse(id)).Select(rig=>rig.id).ToList();
            RoleModel roleModel = new RoleModel();
            roleModel.User = userService.GetUser(int.Parse(id));
            roleModel.roleListModel = new List<RoleListModel>();
            foreach (var role in roles)
            {
                roleModel.roleListModel.Add(new RoleListModel() { id = role.id, name = role.name, hasRole = rights.Any(rol=>rights.Contains(role.id))});
            }
            return PartialView(roleModel);
        }
        [HttpPost]
        public ActionResult GetRoles(FormCollection collection)
        {
            return View();//return "loaded" + id;
        }
    }
}
