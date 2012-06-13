using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRTracker.Models;
using QRTracker.Models.Abstract;
using QRTracker.service.Abstract;


namespace QRTracker.service
{
    public class DocumentService:BaseService
    {
        #region baseQrmethods

        public bool DocExist(BaseQrDoc doc)
        {
            var eDoc = Entities.Documents.FirstOrDefault(docu => docu.hash == doc.Hash);
            if (eDoc == null)
                return false;
            else
                return true;
        }

        public int GetDocumentId(BaseQrDoc doc)
        {
            var eDoc = Entities.Documents.FirstOrDefault(docu => docu.hash == doc.Hash);
            if (eDoc != null)
                return eDoc.id;
            else
            {
                return AddDocument(doc);
            }
            return 0;
        }

        public bool RecordExist(BaseQrDoc doc)
        {
            return true;
        }

        public int AddDocument(BaseQrDoc doc)
        {
            
            Document docu = new Document();
            docu.hash = doc.Hash;
            docu.typeId = doc.docType;
            docu.docNum = doc.docNum;
            Entities.Documents.AddObject(docu);
            Save();
            Track track = new Track();
            track.statId = 1;
            track.docId = docu.id;
            track.userId = doc.userId;
            track.statDate = doc.creationDate;
            Entities.Tracks.AddObject(track);
            Save();
            //AddActionToBuffer(track);


            return docu.id;
        }
        public void Create(BaseQrDoc doc)
        {
            
        }

        public bool ExistTrack(BaseQrDoc doc)
        {
            var document = Entities.Documents.FirstOrDefault(docu => docu.hash == doc.Hash);
            var track =
                Entities.Tracks.FirstOrDefault(
                    trac => (trac.docId == document.id) && (trac.Status.id == doc.ActionId)&&(trac.deleted==null));
            if (track!=null)

                return true;
            else
            {
                return false;
            }
        }

        public void CreateTrack(BaseQrDoc doc)
        {
            int docId = GetDocumentId(doc);
            Track track = new Track();
            track.docId = docId;
            
            track.statDate = DateTime.Now;
            track.userId = doc.ActionUser.id;
            track.statId = doc.ActionId;
            Entities.Tracks.AddObject(track);
            Save();
            //AddActionToBuffer(track);
        }


        [Obsolete]
        public TrackModel GetTracks(BaseQrDoc doc)
        {
            TrackModel tracks = new TrackModel();
            tracks.qrDoc = doc;
            int docId = GetDocumentId(doc);
            bool isManager = new UserService().IsManager(doc.ActionUser.id);
            tracks.Tracks = Entities.Tracks.Where(tr => tr.docId == docId).OrderBy(doct => doct.statDate).Select(tra => new TrackExtend { Track = tra, AllowAction = (doc.ActionUser.id == tra.userId) || isManager }).ToList();

            return tracks;

        }
        /// <summary>
        /// Получает массив qrExtenden
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public List<TrackExtend> GetTracksByQrDoc(BaseQrDoc doc)
        {
            Document document = Entities.Documents.FirstOrDefault(docu => docu.hash == doc.Hash);
            if (document == null)
            {
               int id = AddDocument(doc);
               document = Entities.Documents.FirstOrDefault(docu => docu.id == id);
            }
               
            bool isManager = new UserService().IsManager(doc.ActionUser.id);
            var tracks = Entities.Tracks.Where(tr => tr.docId == document.id).Select(trac => new TrackExtend() {Track = trac,AllowAction = (doc.ActionUser.id == trac.userId) || isManager }).ToList();
            return tracks;
        }
        #endregion
        #region idMethods
        public Document GetDocumentById(int id)
        {
            Document doc = Entities.Documents.FirstOrDefault(docs => docs.id == id);
            if (doc == null)
                return null;
            else
                return doc;
        }

        public bool DocExistById(int id)
        {
            Document doc = GetDocumentById(id);
            if (doc == null)
                return false;
            else
            return true;
        }

        public int GetTrackOwnerId(int trackId)
        {
            Track track = GetTrackById(trackId);
            if (track == null)
                return -1;
            else
            {
                return track.userId;
            }
        }
        /// <summary>
        /// получение треков документа для конкретного пользователя для отображения истории действий
        /// </summary>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public IEnumerable<Detail> GetDetailsByUserId(User curUser)
        {
            //var user = Entities.Users.FirstOrDefault(us => us.id == userId);
            if (curUser==null)
                return null;
            string userName = curUser.name;
            DateTime now = DateTime.Now;
            DateTime start = now.GetStartOfDay();
            DateTime end = now.GetEndOfDay();
            //var details = Entities.Details.Where(detai => (detai.statDate > start && detai.statDate < end) || (detai.deleted > start && detai.deleted < end)).OrderBy(deta => deta.statDate).AsEnumerable();
            var details = Entities.Details.OrderByDescending(deta => deta.statDate).AsEnumerable();
            if (!curUser.isManager)
                details = details.Where(det => det.userName == userName);
            return details;
        }

        public int GetDocumentIdByTrackId(int trackId)
        {
            Track track = GetTrackById(trackId);
            if (track != null)
                return track.docId;
            else
            {
                return -1;
            }
        }

        public Track GetTrackById(int trackId)
        {
            Track track = Entities.Tracks.FirstOrDefault(tr => tr.id == trackId);
            return track;
        }


        public bool DeleteTrackById(int trackId)
        {
            try
            {
                Track track = GetTrackById(trackId);
                track.deleted = DateTime.Now;
                track.posted = null;
                Save();
                //RemoveActionToBuffer(track);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
         
        }

        

        #endregion

        public int GetDocTypeId(string docTypeName)
        {
            var docType = Entities.DocTypes.FirstOrDefault(dt => dt.name == docTypeName);
            if (docType != null)
                return docType.id;
            else
            {
                return AddDocType(docTypeName);
            }
        }

        public int AddDocType(string docTypeName)
        {
            DocType docType= new DocType();
            docType.name = docTypeName;
            Entities.DocTypes.AddObject(docType);
            Save();
            return docType.id;

        }

        //public void AddActionToBuffer(Track track)
        //{
        //    // написать вьюшку по ид трека
        //    // номер дока статус дата пользователь и флаг очистки
        //    // добавить в таблицу (но тут данные получаются отораннвые от таблицы. то есть не ид документа, а именно текстовый номер. не ид пользователя, а его имя и т.д.)
        //}

        //public void RemoveActionToBuffer(Track track)
        //{

        //}



    }
}