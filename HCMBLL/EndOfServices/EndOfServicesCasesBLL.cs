using HCMDAL;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class EndOfServicesCasesBLL : CommonEntity
    {
        public virtual int EndOfServiceCaseID
        {
            get;
            set;
        }

        public virtual string EndOfServiceCase
        {
            get;
            set;
        }
         
        public virtual List<EndOfServicesCasesBLL> GetEndOfServicesCases()
        {
            List<EndOfServicesCases> EndOfServicesCasesList = new EndOfServicesCasesDAL().GetEndOfServicesCases();
            List<EndOfServicesCasesBLL> EndOfServicesCasesBLLList = new List<EndOfServicesCasesBLL>();
            if (EndOfServicesCasesList.Count > 0)
            {
                foreach (var item in EndOfServicesCasesList)
                {
                    EndOfServicesCasesBLLList.Add(MapEndOfServiceCase(item));
                }
            }

            return EndOfServicesCasesBLLList;
        }

        public EndOfServicesCasesBLL GetByEndOfServiceCaseID(int EndOfServiceCaseID)
        {
            return GetEndOfServicesCases().SingleOrDefault(x => x.EndOfServiceCaseID.Equals(EndOfServiceCaseID));
        }

        internal EndOfServicesCasesBLL MapEndOfServiceCase(EndOfServicesCases item)
        {
            return item != null ?
                new EndOfServicesCasesBLL()
                {
                    EndOfServiceCaseID = item.EndOfServiceCaseID,
                    EndOfServiceCase = item.EndOfServiceCase, 
                }
                : null;
        }

        public virtual int Add()
        {
            throw new System.NotImplementedException();
        }

        public virtual int Remove()
        {
            throw new System.NotImplementedException();
        }

        public virtual int Update()
        {
            throw new System.NotImplementedException();
        }

    }
}

