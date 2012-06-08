using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRTracker.Models.Abstract
{
    interface IResult
    {
        bool Success { get; set;}
        Constants.ResultTypes ResultType { get; set; }
        string ResultText { get; set; }

    }
}
