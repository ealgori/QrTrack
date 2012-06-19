using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRTracker.service.Abstract;

namespace QRTracker.service
{
    public class UserService:BaseService
    {
        public int GetUserId(string userName)
        {
            var user = Entities.Users.Where(us => us.name == userName);
            if (user.Count() == 0)
            {
                return AddUser(userName).id;
            }
            return user.First().id;
        }

        public User GetUser(string userName)
        {
            var user = Entities.Users.Where(us => us.name == userName);
            if (user.Count() == 0)
            {
                return AddUser(userName);
            }
            return user.First();
        }

        public User GetUser(int id)
        {
            var user = Entities.Users.Where(us => us.id == id);
            if (user.Count() == 0)
            {
                return null;
            }
            return user.First();
        }


        public User GetUserWithDomain(string userName)
        {
            userName = OtherFunctions.StripDomain(userName);
            var user = Entities.Users.Where(us => us.name == userName);
            if (user.Count() == 0)
            {
                return AddUser(userName);
            }
            return user.First();
        }
        public User AddUser(string userName)
        {
            User user = new User(){name = userName,isManager = false, isViewer = false};
            Entities.Users.AddObject(user);
            Save();
            return user;
            
        }

        public bool AllowView(int userId)
        {
            try
            {
                return Entities.Users.Single(us => us.id == userId).isViewer;
            }
            catch (Exception)
            {

                return false;
            }
            ;
        }


        public bool IsManager(int userId)
        {
            try
            {
                return Entities.Users.Single(us => us.id == userId).isManager;
            }
            catch (Exception)
            {

                return false;
            }
            ;
        }

        public List<User> GetAllUsers()
        {
            return Entities.Users.ToList();
        }
    }
}