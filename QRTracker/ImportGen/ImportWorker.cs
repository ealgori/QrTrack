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
                    
                    bool woDeleteResult = FileGenerator.PeformWoDeleteImport();
                    bool woInsertResult = FileGenerator.PeformWoInsertImport();
                    bool poDeleteResult = FileGenerator.PeformPoDeleteImport();
                    bool poInsertResult = FileGenerator.PeformPoInsertImport();
                    if (woDeleteResult||woInsertResult||poInsertResult||poDeleteResult)
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