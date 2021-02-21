using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class EmployeesTypesDAL
    {
        public List<EmployeesTypes> GetEmployeesTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesTypes.ToList();
            }
            catch
            {
                throw;
            }
        }

        public EmployeesTypes GetByEmployeeTypeID(int EmployeeTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EmployeesTypes.SingleOrDefault(x => x.EmployeeTypeID.Equals(EmployeeTypeID));
            }
            catch
            {
                throw;
            }
        }
    }
}
