using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRTracker.service;

namespace QRTracker.Models
{
    public class IsManager : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //PermissionManager permissionManager = new PermissionManager();
            string action = filterContext.ActionDescriptor.ActionName;
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string user = filterContext.HttpContext.User.Identity.Name;
            bool isManager = false;
            using (UserService userService = new UserService())
            {
                var _user = userService.GetUserWithDomain(user);
                isManager = _user.isManager;
            }
            if (!isManager)
            {
                throw new UnauthorizedAccessException("Пользователю не разрешено использование данного действия");
            }
        }
    }
}