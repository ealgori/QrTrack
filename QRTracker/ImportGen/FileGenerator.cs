using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ExcelLibrary.SpreadSheet;
using QRTracker.service;
using ExcelLibrary;
using QiHe.CodeLib;

namespace QRTracker.ImportGen
{
    
    
    public static class FileGenerator
    {
        
        class StatusesModel
        {
            public StatusesModel()
            {
                DictStat = new Dictionary<int, int>();
                StatsList = new List<string>();
            }
            public Dictionary<int, int> DictStat { get; set; }
            public List<string> StatsList { get; set; }
        }

        class DocumentModel
        {
            public DocumentModel()
            {
                DictDoc = new Dictionary<string, int>();
                DocList = new List<string>();
            }
            public Dictionary<string, int> DictDoc { get; set; }
            public List<string> DocList { get; set; }
        } 
        
        
        private static bool PeformImport(string fileName,string Type, bool deleted, ImportWorker worker)
        {
            //string fileName = "WoDelete(1)({0}).xls";
            //worker.Tick();
            for (int i = 0; i < 1000; i++)
                if (!File.Exists(Constants.ImportFolder + string.Format(fileName, i)))
                {
                    fileName = string.Format(fileName, i);
                    break;

                }
            //получаем статусы
            var statModel = GetStatusHashDictionary();

            //получаем данные
            //var detailData = GetData(true, "WO");
            var detailData = GetData(deleted, Type);

            if (detailData.Count() == 0)
                return false;

            var docModel = GetDocumentHashDictionart(detailData);
            List<List<string>> sheet = PeformConvert(statModel, docModel, detailData);
            //выполняем преобразование
            //создаем файл
            string filePath = FileGen(fileName, sheet);
            if (filePath == null)
                return false;
            // копируем файл в прогрузку
            string endFile = Constants.ImportFolder + Path.GetFileName(fileName);
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(endFile)))
                    Directory.CreateDirectory(Path.GetDirectoryName(endFile));
                File.Copy(filePath, endFile);
            }
            catch (Exception exc)
            {
                return false;
            }

            while (File.Exists(endFile))
            {
                System.Threading.Thread.Sleep(Constants.FileCheckInterval);
                worker.Tick();
            }

            // ищем

            // коммитим
            Commit(detailData);
            worker.Tick();
            return true;
        }
        
        public static bool PeformWoDeleteImport(ImportWorker worker)
        {
            string fileName = "WoDelete(1)({0}).xls";
            return PeformImport(fileName, "WO", true,worker);
        }

        public static bool PeformWoInsertImport(ImportWorker worker)
        {
            string fileName = "WoInsert(1)({0}).xls";
            return PeformImport(fileName, "WO", false,worker);
        }


        public static bool PeformPoDeleteImport(ImportWorker worker)
        {
            string fileName = "PODelete(1)({0}).xls";
            return PeformImport(fileName, "PO", true,worker);
        }

        public static bool PeformPoInsertImport(ImportWorker worker)
        {
            string fileName = "POInsert(1)({0}).xls";
            return PeformImport(fileName, "PO", false,worker);
        }

      
        
        private static StatusesModel GetStatusHashDictionary()
        {
            StatusesModel statusesModel = new StatusesModel();
            
            using (StatusService statusService = new StatusService())
            {
                var statuses = statusService.GetStatuses();
                // таблица соответствий. Ключ - ид статуса, значение - будущая позиция в файле
                int i = 1;
                foreach (var status in statuses)
                {
                   
                    statusesModel.DictStat.Add( status.id,i);
                    statusesModel.StatsList.Add(status.name);
                    i++;
                }

                return statusesModel;
            }
        }

        private static DocumentModel GetDocumentHashDictionart(IEnumerable<Detail> data)
        {
            DocumentModel documentModel = new DocumentModel();
           
            var _docNums = data.Select(doc => new {doc.docNum, doc.hash}).Distinct();
            int i = 1;
            foreach (var detail in _docNums)
            {
               
                documentModel.DictDoc.Add(detail.docNum,i);
                documentModel.DocList.Add(detail.docNum);
                i++;
            }
            return documentModel;
        }

        private static List<List<string>> PeformConvert(StatusesModel statusesModel,DocumentModel documentModel ,IEnumerable<Detail> details)
        {
            List<List<string>> sheet = new List<List<string>>();
            int statusCount = statusesModel.StatsList.Count;
            // header
            List<string> statuses = new List<string>();
            statuses.Add("");
            statuses.AddRange(statusesModel.StatsList);
            sheet.Add(statuses);
            // инициализация столбца документов
            for (int i = 1; i < documentModel.DictDoc.Count+1; i++)
            {
                List<string> row = new List<string>(new string[statusCount+1]);// +1 так как статусы должны быть смещены вправо на одну клетку.
                row[0] = documentModel.DocList[i-1];
                sheet.Add(row);
            }
            // пошла жара

            foreach (var detail in details)
            {
                try
                {
                int row = documentModel.DictDoc[detail.docNum];
                int column = statusesModel.DictStat[detail.StatusId];
                sheet[row][column] = detail.statDate.ToString();
                }
                catch (Exception)
                {
                    // не придумал чего делать
                    throw;
                }
            }
            return sheet;

        }

        private static IEnumerable<Detail> GetData(bool deleted, string Type)
        {
            IEnumerable<Detail> details;
            using (DocumentService documentService = new DocumentService())
            {
                details = documentService.GetDetails(deleted, Type);
                
            }
            return details;
        }
        
        private static void Commit(IEnumerable<Detail> details )
        {
            using (DocumentService documentService=new DocumentService())
            {
                
            
                foreach (var detail in details)
                {
                    documentService.CommitTrackById(detail.id);
                }
            }
        }
        
        
        //private DocumentService documentService =new DocumentService();


















        public static string FileGen(string fileName,List<List<string>> sheet )
        {
            if (!Directory.Exists(Constants.ImportTempFolder))
                Directory.CreateDirectory(Constants.ImportTempFolder);
            string file = Constants.ImportTempFolder+fileName;
            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception)
                {

                    return null;
                }
            }
            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet("First Sheet");
            for (int row=0; row<sheet.Count;row++)
            {
                for (var col = 0; col < sheet[row].Count;col++ )
                {
                    worksheet.Cells[row, col] = new Cell(sheet[row][col]);
                }
            }
            workbook.Worksheets.Add(worksheet);
            workbook.Save(file);

            return file;

        }


        
    }
}