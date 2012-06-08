using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRTracker.service.Abstract
{
    public abstract class BaseService:IBaseService,IDisposable
    {
        public QRTracker.service.QrTrakerEntities Entities = new QrTrakerEntities();

        public void Save()
        {
            Entities.SaveChanges();
        }



        public void Dispose()
        {
           Entities.Dispose();
        }
    }
}