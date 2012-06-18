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
                FileGenerator.PeformWoDeleteImport();
                FileGenerator.PeformWoInsertImport();
                FileGenerator.PeformPoDeleteImport();
                FileGenerator.PeformPoInsertImport();
                //!!!!!!!!!!!!!!
                State = WorkerState.Started;
                Thread.Sleep(Constants.ImportInterval);
            }
           
        }
    }
}