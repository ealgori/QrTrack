using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRTracker.Models.Abstract
{
    public interface  IBaseQrDoc
    {
         string docNum { get; set; }
         int docType { get; set; }
         int userId { get; set; }
         DateTime creationDate { get; set; }
         int ActionId { get; set; }
         bool IsInited { get; set; }
         
    }
}