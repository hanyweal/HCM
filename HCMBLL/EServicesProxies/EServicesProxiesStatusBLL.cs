using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class EServicesProxiesStatusBLL
    {
        public int EServiceProxyStatusID { get; set; }

        public string EServiceProxyStatus { get; set; }

        public List<EServicesProxiesStatusBLL> GetEServicesProxiesStatusBLL()
        {
            try
            {
                List<EServicesProxiesStatus> EServicesTypesList = new EServicesProxiesStatusDAL().GetEServicesProxiesStatus();
                List<EServicesProxiesStatusBLL> EServicesProxiesStatusBLLList = new List<EServicesProxiesStatusBLL>();
                foreach (var item in EServicesTypesList)
                    EServicesProxiesStatusBLLList.Add(this.MapEServicesProxiesStatus(item));

                return EServicesProxiesStatusBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal EServicesProxiesStatusBLL MapEServicesProxiesStatus(EServicesProxiesStatus item)
        {
            return item != null ?
                new EServicesProxiesStatusBLL()
                {
                    EServiceProxyStatusID = item.EServiceProxyStatusID,
                    EServiceProxyStatus = item.EServiceProxyStatus
                }
                : null;
        }
    }
}