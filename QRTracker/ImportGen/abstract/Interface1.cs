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
        Working = 3,
        Error = 4

    }


    public abstract class Worker:IWorker
    {
        public static WorkerState State { get; set; }
        public static DateTime? LastSuccess { get; set; }
        public static bool Freeze { get; set; }
        public static DateTime? tick{get; set;}

        public  void Tick()
        {
            tick = DateTime.Now;
        }
        
        public  void Control()
        {
            while (true)
            {
                if (tick.HasValue)
                {
                    TimeSpan span = DateTime.Now - tick.Value;
                    if (span.Seconds > Constants.MaxImportDownTime)
                        Freeze = true;
                    else
                    {
                        Freeze = false;
                    }
                }
                System.Threading.Thread.Sleep(10000);
            }

        }
        
        public virtual void DoWork()
        {
           
            //Thread.Sleep(Constants.ImportInterval);
        }
    }
}
