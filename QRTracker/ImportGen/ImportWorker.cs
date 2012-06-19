using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using QRTracker.ImportGen.Abstract;

namespace QRTracker.ImportGen
{
    public class ImportWorker:Worker
    {
        public ImportWorker()
        {
            State = WorkerState.Started;
        }
        
        public override void DoWork()
        {
            while (true)
            {
                State= WorkerState.Working;
                try
                {
                    FileGenerator.PeformWoDeleteImport();
                    FileGenerator.PeformWoInsertImport();
                    FileGenerator.PeformPoDeleteImport();
                    FileGenerator.PeformPoInsertImport();
                    LastSuccess = DateTime.Now;
                }
                catch (Exception)
                {
                    
                    throw;
                }
                
                //!!!!!!!!!!!!!!
                State = WorkerState.Started;
                Thread.Sleep(Constants.ImportInterval);
            }
           
        }
    }
}