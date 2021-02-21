using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMDAL;

namespace HCMBLL
{
    public class CareersHistoryTypesBLL
    {
        public int CareerHistoryTypeID { get; set; }

        public string CareerHistoryTypeName { get; set; }

        public List<CareersHistoryTypesBLL> GetCareersHistoryTypes()
        {
            List<CareersHistoryTypes> CareersHistoryTypesList = new CareersHistoryTypesDAL().GetCareersHistoryTypes();
            List<CareersHistoryTypesBLL> CareersHistoryTypesBLLList = new List<CareersHistoryTypesBLL>();
            foreach (var item in CareersHistoryTypesList)
            {
                CareersHistoryTypesBLLList.Add(new CareersHistoryTypesBLL().MapCareerHistoryType(item));
            }
            return CareersHistoryTypesBLLList;
        }

        internal CareersHistoryTypesBLL MapCareerHistoryType(CareersHistoryTypes CareerHistoryType)
        {
            try
            {
                CareersHistoryTypesBLL CareerHistoryTypeBLL = null;
                if (CareerHistoryType != null)
                {
                    CareerHistoryTypeBLL = new CareersHistoryTypesBLL()
                    {
                        CareerHistoryTypeID = CareerHistoryType.CareerHistoryTypeID,
                        CareerHistoryTypeName = CareerHistoryType.CareerHistoryTypeName
                    };
                }
                return CareerHistoryTypeBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
