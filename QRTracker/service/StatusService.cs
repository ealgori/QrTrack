using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRTracker.Models.Abstract;
using QRTracker.service.Abstract;

namespace QRTracker.service
{
    public class StatusService:BaseService
    {
        public bool AllowedAction(BaseQrDoc doc)
        {
            return false;
        }

        public IEnumerable<Status> GetStatuses()
        {
            return Entities.Statuses.OrderBy(stat => stat.id);
        }
    }
}