using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using HCMBLL;

namespace HCMDisactiveEmployeeBasedOnBusinessProcess
{
    public partial class DeactivateEmployeesSrv : ServiceBase
    {
        public int Hour
        {
            get
            {
                string HoursMins = ConfigurationManager.AppSettings["ServiceStartsEveryDayAt"];
                return Convert.ToInt32(HoursMins.Split(':')[0]);
            }
            set { }
        }

        public int Min
        {
            get
            {
                string HoursMins = ConfigurationManager.AppSettings["ServiceStartsEveryDayAt"];
                return Convert.ToInt32(HoursMins.Split(':')[1]);
            }
            set { }
        }

        System.Timers.Timer Timer = new System.Timers.Timer();
        DateTime ScheduleTime;

        public DeactivateEmployeesSrv()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LogServiceEvents("OnStart @ " + DateTime.Now);

                this.Timer = new System.Timers.Timer();
                this.ScheduleTime = DateTime.Now.Date.AddHours(Hour).AddMinutes(Min); //DateTime.Now.AddDays(0).AddMinutes(3);

                LogServiceEvents("First Run after start @ " + this.ScheduleTime);

                this.Timer.Enabled = true;
                SetTimerInterval();
                this.Timer.Elapsed += Timer_Elapsed;
            }
            catch (Exception ex)
            {
                LogServiceEvents("OnStart Exception: " + ex.Message + (ex.InnerException != null ? " Inner Exception : " + ex.InnerException.Message : ""));
                throw ex;
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                LogServiceEvents("Timer started @ " + DateTime.Now);

                //set next run
                this.ScheduleTime = this.ScheduleTime.AddDays(1).Date.AddHours(Hour).AddMinutes(Min);
                this.SetTimerInterval();
                
                #region Transfer
                try
                {                    
                    new BaseTransfersBLL().StartProcess();                    
                }
                catch (Exception ex)
                {
                    LogServiceEvents(" Exception Message = " + ex.Message +
                       (ex.InnerException != null ? " Inner Exception : " + ex.InnerException.Message : ""));
                }
                #endregion

                #region End of service
                try
                {
                    new EndOfServicesBLL().StartProcess();
                }
                catch (Exception ex)
                {
                    LogServiceEvents(" Exception Message = " + ex.Message +
                       (ex.InnerException != null ? " Inner Exception : " + ex.InnerException.Message : ""));
                }
                #endregion

                LogServiceEvents("Timer completed and Task done @ " + DateTime.Now);

                LogServiceEvents("Next Run @ " + this.ScheduleTime);
            }
            catch (TaskCanceledException ex)
            {
                LogServiceEvents("TaskCanceledException: IsCancellationRequested = " + ex.CancellationToken.IsCancellationRequested.ToString() +
                    " Message = " + ex.Message +
                    (ex.InnerException != null ? " Inner Exception : " + ex.InnerException.Message : ""));
                //throw ex;
            }
            catch (Exception ex)
            {
                LogServiceEvents("Exception: " + ex.Message +
                    (ex.InnerException != null ? " Inner Exception : " + ex.InnerException.Message : ""));
                //throw ex;
            }
        }

        protected override void OnStop()
        {
            LogServiceEvents("OnStop @ " + DateTime.Now);
            this.Timer.Enabled = false;
        }

        private void SetTimerInterval()
        {
            double tillNextInterval = this.ScheduleTime.Subtract(DateTime.Now).TotalSeconds * 1000;
            if (tillNextInterval < 0)
                tillNextInterval += new TimeSpan(24, this.Hour, this.Min).TotalSeconds * 1000;
            this.Timer.Interval = tillNextInterval;
        }

        private void LogServiceEvents(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog.txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
