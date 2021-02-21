using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class AllowancesCalculationTypesBLL : CommonEntity
    {
        public int AllowanceCalculationTypeID
        {
            get;
            set;
        }

        public string AllowanceCalculationTypeName
        {
            get;
            set;
        }

        public int Add()
        {
            throw new System.NotImplementedException();
        }

        public int Update()
        {
            throw new System.NotImplementedException();
        }

        public int Remove()
        {
            throw new System.NotImplementedException();
        }

        public List<AllowancesCalculationTypesBLL> GetAllowancesCalculationTypes()
        {
            List<AllowancesCalculationTypes> AllowancesCalculationTypesList = new AllowancesCalculationTypesDAL().GetAllowancesCalculationTypes();
            List<AllowancesCalculationTypesBLL> AllowancesCalculationTypesBLLList = new List<AllowancesCalculationTypesBLL>();
            if (AllowancesCalculationTypesList.Count > 0)
            {
                foreach (var item in AllowancesCalculationTypesList)
                {
                    AllowancesCalculationTypesBLLList.Add(new AllowancesCalculationTypesBLL()
                    {
                        AllowanceCalculationTypeID = item.AllowanceCalculationTypeID,
                        AllowanceCalculationTypeName = item.AllowanceCalculationTypeName
                    });
                }
            }

            return AllowancesCalculationTypesBLLList;
        }

        internal AllowancesCalculationTypesBLL MapAllowanceCalculationType(AllowancesCalculationTypes item)
        {
            return item != null ?
                new AllowancesCalculationTypesBLL()
                {
                    AllowanceCalculationTypeID = item.AllowanceCalculationTypeID,
                    AllowanceCalculationTypeName = item.AllowanceCalculationTypeName
                }
                : null;
        }
    }
}
