using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using QRTracker.service;
using System.Web.Mvc;

namespace QRTracker.Models.Abstract
{
    public abstract class BaseQrDoc:IBaseQrDoc
    {
        /// <summary>
        /// номерд документа. с куаркода
        /// </summary>
        public string docNum { get; set; }
        /// <summary>
        /// тип документа. с куаркода
        /// </summary>
        public int docType { get; set; }
        /// <summary>
        ///  ид создателя документа. c куаркода
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// дата создания документа. с куаркода
        /// </summary>
 
        public DateTime creationDate { get; set; }
        /// <summary>
        /// ид статуса в который хотят перевести документ
        /// </summary>
        public int ActionId { get; set; }
        /// <summary>
        /// ид пользователя который производит это перевод
        /// </summary>
 
        public User ActionUser { get; set; }
        /// <summary>
        /// флаг, успешно ли распарсился документ
        /// </summary>
 
        public bool IsInited { get; set; }
        /// <summary>
        ///  фактически, ключ документа
        /// </summary>

        public string Hash { get; set; }
        /// <summary>
        ///  текст, прочитанный с куаркода
        /// </summary>

        public string qtText { get; set; }

        public  void Parse(string decoded)
        {
            try
            {
                string[] parts = decoded.Split(new string[] { ";", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                // номер документа
                this.docNum = parts[0];
                // тип документа
                this.docType = (new QRTracker.service.DocumentService().GetDocTypeId(parts[1]));
                // дата создания
                this.creationDate = DateTime.Parse(parts[3], CultureInfo.CreateSpecificCulture("ru-RU"));
                string userName = parts[2];
                this.userId = (new QRTracker.service.UserService().GetUserId(userName));
             
                this.Hash = parts[4];
                if (OtherFunctions.verifyMd5Hash(decoded))
                {
                    IsInited = true;
                }
                
                

            }
            catch (Exception exc)
            {
                IsInited = false;

            }
        }

        public override string ToString()
        {
            return qtText;
        }

        public string ToJson()
        {
            return new JavaScriptSerializer().Serialize(this);
        }







        public string Decode(string coded)
        {
            string decoded = coded;
            return decoded;
        }
        
        public void InitFromBase(int id,User user)
        {
            
            using (DocumentService docService = new DocumentService())
            {
                Document doc = docService.GetDocumentById(id);
                this.docNum = doc.docNum;
                this.docType = doc.DocType.id;
                this.ActionUser = user;
                this.Hash = doc.hash;
                // ну и допишите что хотите
                IsInited = true;
            }
          

        }
    }
}