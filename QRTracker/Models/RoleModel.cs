using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRTracker.service;

namespace QRTracker.Models
{
    public class RoleModel
    {
        public List<RoleListModel> roleListModel { get; set; }
        public User User { get; set; }
        public ResultText ResultText { get; set; }
    }

    public class RoleListModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool hasRole { get; set; }
    }
}