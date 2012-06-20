using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRTracker.Models;
using QRTracker.Models.Abstract;
using QRTracker.service.Abstract;

namespace QRTracker.service
{
    public class RightService:BaseService
    {
        public bool HasRight(BaseQrDoc doc)
        {

            Role role = new Role();
            try
            {
                role = Entities.Roles.FirstOrDefault(rol => rol.DocType.id == doc.docType && rol.statusId == doc.ActionId);
                if (role == null)
                    return false;


                var right = Entities.Rights.FirstOrDefault(righ => righ.Role.id == role.id && righ.userId == doc.ActionUser.id);
                if (right == null)
                    return false;
                else
                {
                    return true;
                }

            }
            catch (Exception)
            {
                
                throw;
            }
            
               
           
            
            return false;
        }

        public bool HasChance(BaseQrDoc doc)
        {

            Role role = new Role();
            try
            {
                role = Entities.Roles.FirstOrDefault(rol => rol.DocType.id == doc.docType && rol.statusId == doc.ActionId);
                if (role != null)
                    return true;
            }
            catch (Exception exc)
            {
               

            }
            return false;
           
        }

        public void AddRole(int userId, int roleId)
        {
            var role = Entities.Rights.FirstOrDefault(rol => (rol.roleId == roleId) && (rol.userId == userId));
            if (role == null)
            {
                Right right = new Right();
                right.roleId = roleId;
                right.userId = userId;
                Entities.Rights.AddObject(right);
                Save();
            }
        }

        public void RemoveRole(int userId, int roleId)
        {
            var role = Entities.Rights.FirstOrDefault(rol => (rol.roleId == roleId) && (rol.userId == userId));
            if (role != null)
            {
                Entities.Rights.DeleteObject(role);
                Save();
            }
        }
        public List<RoleListModel> GetRoles(int userId)
        {
            // получаем все права данного пользователя
            var rigths = Entities.Rights.Where(rig => rig.userId == userId).Select(righ=>righ.roleId);
            // получаем роли из этих прав
            var _roles =
               Entities.Roles.Where(rol => rigths.Contains(rol.id));
            
            List<RoleListModel> roles = _roles.Select(role => new RoleListModel() {id = role.id, name = role.Status.name}).ToList();
            return roles;
        }
        public List<Role> GetAllRoles()
        {
           
            // получаем роли из этих прав
            var roles =
               Entities.Roles.ToList();

          
            return roles;
        }

    }
}