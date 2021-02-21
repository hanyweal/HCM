using HCMDAL;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class AssigningsTypesBLL
    {
        public int AssigningTypeID { get; set; }

        public string AssigningTypeName { get; set; }

        public List<AssigningsTypesBLL> GetAssigningsTypes()
        {
            try
            {
                List<AssigningsTypes> AssigningsTypesList = new AssigningsTypesDAL().GetAssigningsTypes();
                List<AssigningsTypesBLL> AssigningsTypesBLLList = new List<AssigningsTypesBLL>();
                if (AssigningsTypesList.Count > 0)
                {
                    foreach (var item in AssigningsTypesList)
                    {
                        AssigningsTypesBLLList.Add(new AssigningsTypesBLL()
                        {
                            AssigningTypeID = item.AssigningTypID,
                            AssigningTypeName = item.AssigningTypeName
                        });
                    }
                }

                return AssigningsTypesBLLList;
            }
            catch
            {

                throw;
            }
        }

        public AssigningsTypesBLL GetByAssigningTypeID(int AssigningTypeID)
        {
            return this.GetAssigningsTypes().SingleOrDefault(x => x.AssigningTypeID.Equals(AssigningTypeID));
        }

        internal AssigningsTypesBLL MapAssigningType(AssigningsTypes AssigningType)
        {
            try
            {
                AssigningsTypesBLL AssigningTypeBLL = null;
                if (AssigningType != null)
                {
                    AssigningTypeBLL = new AssigningsTypesBLL()
                    {
                        AssigningTypeID = AssigningType.AssigningTypID,
                        AssigningTypeName = AssigningType.AssigningTypeName
                    };
                }
                return AssigningTypeBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
