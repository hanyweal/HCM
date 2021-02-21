using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HCMDisactiveEmployeeBasedOnBusinessProcess
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new Service1()
            //};
            //ServiceBase.Run(ServicesToRun);
#if DEBUG

            DeactivateEmployeesSrv objService = new DeactivateEmployeesSrv();
            objService.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

#else

                                    ServiceBase[] ServicesToRun;
                                    ServicesToRun = new ServiceBase[]
                                    {
                                        new Service1()
                                    };
                                    ServiceBase.Run(ServicesToRun);
                                    System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#endif
        }
    }
}
