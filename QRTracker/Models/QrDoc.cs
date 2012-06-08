using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using QRTracker.Models.Abstract;
using QRTracker.service;

namespace QRTracker.Models
{
    public class QrDoc:BaseQrDoc
    {
      
        /// <summary>
        /// конструктор по куаркоду
        /// </summary>
        /// <param name="coded"></param>
        public QrDoc(string coded)
        {
            IsInited = false;
            string decoded = Decode(coded);
            Parse(decoded);
            this.qtText = coded;

        }
        /// <summary>
        /// конструктор по ид документа
        /// </summary>
        /// <param name="docId"></param>
        public QrDoc(int docId,User user)
        {
            InitFromBase(docId, user);
        }


      

        

        

       
    }
}