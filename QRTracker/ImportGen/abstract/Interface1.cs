using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace QRTracker.ImportGen.Abstract
{
	public interface IWorker
	{
	    void DoWork();
	}

    public enum  WorkerState
    {
        Started = 1,
        Stopped = 2,
        Working = 3

    }


    public class Worker:IWorker
    {
        public static WorkerState State { get; set; }

        public virtual void DoWork()
        {
           
            //Thread.Sleep(Constants.ImportInterval);
        }
    }
}
