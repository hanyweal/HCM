using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class EmployeesTypesBLL
    {
        public int EmployeeTypeID { get; set; }

        public string EmployeeTypeName { get; set; }

        public List<EmployeesTypesBLL> GetEmployeesTypes()
        {
            try
            {
                List<EmployeesTypes> EmployeesTypesList = new EmployeesTypesDAL().GetEmployeesTypes();
                List<EmployeesTypesBLL> EmployeesTypesBLLList = new List<EmployeesTypesBLL>();
                if (EmployeesTypesList.Count > 0)
                {
                    foreach (var item in EmployeesTypesList)
                    {
                        EmployeesTypesBLLList.Add(new EmployeesTypesBLL()
                        {
                            EmployeeTypeID = item.EmployeeTypeID,
                            EmployeeTypeName = item.EmployeeTypeName
                        });
                    }
                }

                return EmployeesTypesBLLList;
            }
            catch
            {
                throw;
            }
        }

        public EmployeesTypesBLL GetByEmployeeTypeID(int EmployeeTypeID)
        {
            try
            {
                EmployeesTypes EmployeeType = new EmployeesTypesDAL().GetByEmployeeTypeID(EmployeeTypeID);
                return new EmployeesTypesBLL()
                {
                    EmployeeTypeID = EmployeeType.EmployeeTypeID,
                    EmployeeTypeName = EmployeeType.EmployeeTypeName
                };
            }
            catch
            {
                throw;
            }
        }

        internal EmployeesTypesBLL MapEmployeeType(EmployeesTypes EmployeeType)
        {
            try
            {
                EmployeesTypesBLL EmployeeTypeBLL = null;
                if (EmployeeType != null)
                {
                    EmployeeTypeBLL = new EmployeesTypesBLL()
                    {
                        EmployeeTypeID = EmployeeType.EmployeeTypeID,
                        EmployeeTypeName = EmployeeType.EmployeeTypeName
                    };
                }
                return EmployeeTypeBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
