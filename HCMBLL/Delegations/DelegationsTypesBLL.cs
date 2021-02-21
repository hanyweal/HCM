using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class DelegationsTypesBLL
    {
        public int DelegationTypeID { get; set; }

        public string DelegationTypeName { get; set; }

        public List<DelegationsTypesBLL> GetDelegationsTypes()
        {
            try
            {
                List<DelegationsTypes> DelegationsTypesList = new DelegationsTypesDAL().GetDelegationsTypes();
                List<DelegationsTypesBLL> DelegationsTypesBLLList = new List<DelegationsTypesBLL>();
                if (DelegationsTypesList.Count > 0)
                {
                    foreach (var item in DelegationsTypesList)
                    {
                        DelegationsTypesBLLList.Add(new DelegationsTypesBLL()
                        {
                            DelegationTypeID = item.DelegationTypeID,
                            DelegationTypeName = item.DelegationTypeName
                        });
                    }
                }

                return DelegationsTypesBLLList;
            }
            catch
            {
                throw;
            }
        }

        public DelegationsTypesBLL GetByDelegationTypeID(int DelegationTypeID)
        {
            try
            {
                DelegationsTypes DelegationType = new DelegationsTypesDAL().GetByDelegationTypeID(DelegationTypeID);
                return new DelegationsTypesBLL()
                {
                    DelegationTypeID = DelegationType.DelegationTypeID,
                    DelegationTypeName = DelegationType.DelegationTypeName
                };
            }
            catch
            {
                throw;
            }
        }

        internal DelegationsTypesBLL MapDelegationType(DelegationsTypes DelegationType)
        {
            try
            {
                DelegationsTypesBLL DelegationTypeBLL = null;
                if (DelegationType != null)
                {
                    DelegationTypeBLL = new DelegationsTypesBLL()
                    {
                        DelegationTypeID = DelegationType.DelegationTypeID,
                        DelegationTypeName = DelegationType.DelegationTypeName
                    };
                }
                return DelegationTypeBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
