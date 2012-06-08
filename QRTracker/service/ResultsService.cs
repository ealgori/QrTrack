using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRTracker.service.Abstract;

namespace QRTracker.service
{
    public class ResultsService:BaseService
    {
        public ResultText GetResultText(Constants.ResultTypes type)
        {
            try
            {
                string stype = type.ToString();
                return Entities.ResultTexts.FirstOrDefault(res => res.id == stype);
            }
            catch (Exception exc)
            {

                return new ResultText(){success = false,text = "unknown error"};
            }
        }

        public ResultText GetResultText(string type)
        {
            try
            {
                return Entities.ResultTexts.Single(res => res.id == type);
            }
            catch (Exception exc)
            {

                return new ResultText() { success = false, text = "unknown error" };
            }
        }
    }
}