using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class EServicesTypesBLL
    {
        public int EServiceTypeID { get; set; }

        public string EServiceTypeName { get; set; }

        public List<EServicesTypesBLL> GetEServicesTypes()
        {
            try
            {
                List<EServicesTypes> EServicesTypesList = new EServicesTypesDAL().GetEServicesTypes();
                List<EServicesTypesBLL> EServicesTypesBLLList = new List<EServicesTypesBLL>();
                foreach (var item in EServicesTypesList)
                {
                    EServicesTypesBLLList.Add(new EServicesTypesBLL() { EServiceTypeID = item.EServiceTypeID, EServiceTypeName = item.EServiceTypeName });
                }
                return EServicesTypesBLLList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal EServicesTypesBLL MapEServicesTypes(EServicesTypes item)
        {
            return item != null ?
                new EServicesTypesBLL()
                {
                    EServiceTypeID = item.EServiceTypeID,
                    EServiceTypeName = item.EServiceTypeName
                }
                : null;
        }

    }
}
