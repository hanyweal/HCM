using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class DelegationsKindsBLL
    {
        public int DelegationKindID { get; set; }

        public string DelegationKindName { get; set; }

        public List<DelegationsKindsBLL> GetDelegationsKinds()
        {
            try
            {
                List<DelegationsKinds> DelegationsKindsList = new DelegationsKindsDAL().GetDelegationsKinds();
                List<DelegationsKindsBLL> DelegationsKindsBLLList = new List<DelegationsKindsBLL>();
                if (DelegationsKindsList.Count > 0)
                {
                    foreach (var item in DelegationsKindsList)
                    {
                        DelegationsKindsBLLList.Add(new DelegationsKindsBLL()
                        {
                            DelegationKindID = item.DelegationKindID,
                            DelegationKindName = item.DelegationKindName
                        });
                    }
                }

                return DelegationsKindsBLLList;
            }
            catch
            {

                throw;
            }
        }

        public DelegationsKindsBLL GetByDelegationKindID(int DelegationKindID)
        {
            try
            {
                DelegationsKinds DelegationsKind = new DelegationsKindsDAL().GetByDelegationkindID(DelegationKindID);

                return new DelegationsKindsBLL()
                {
                    DelegationKindID = DelegationsKind.DelegationKindID,
                    DelegationKindName = DelegationsKind.DelegationKindName
                };
            }
            catch
            {

                throw;
            }
        }

        internal DelegationsKindsBLL MapDelegationKind(DelegationsKinds DelegationKind)
        {
            try
            {
                DelegationsKindsBLL DelegationkindBLL = null;
                if (DelegationKind != null)
                {
                    DelegationkindBLL = new DelegationsKindsBLL()
                    {
                        DelegationKindID = DelegationKind.DelegationKindID,
                        DelegationKindName = DelegationKind.DelegationKindName
                    };
                }
                return DelegationkindBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
