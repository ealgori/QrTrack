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
            tick = DateTime.Now;
        }
        
        public override void DoWork()
        {
            while (true)
            {
                State= WorkerState.Working;
                try
                {
                    
                    //throw new Exception();
                    bool woDeleteResult = FileGenerator.PeformWoDeleteImport(this);
                    bool woInsertResult = FileGenerator.PeformWoInsertImport(this);
                    bool poDeleteResult = FileGenerator.PeformPoDeleteImport(this);
                    bool poInsertResult = FileGenerator.PeformPoInsertImport(this);
                    if (woDeleteResult||woInsertResult||poInsertResult||poDeleteResult)
                        LastSuccess = DateTime.Now;
                    this.Tick();
                    State = WorkerState.Started;
                }
                catch (Exception exception)
                {

                    State = WorkerState.Error;
                }
                
                //!!!!!!!!!!!!!!
               
                Thread.Sleep(Constants.ImportInterval);
            }
           
        }
    }
}