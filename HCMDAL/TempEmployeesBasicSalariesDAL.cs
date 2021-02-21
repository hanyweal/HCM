using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
    public class TempEmployeesBasicSalariesDAL
    { 
        public bool Insert(TempEmployeesBasicSalaries obj)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.TempEmployeesBasicSalaries.Add(obj);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                }
                throw;
            }
        }

        public List<TempEmployeesBasicSalaries> GetEmployeesBasicSalaries()
        {
            try
            {
                HCMEntities db = new HCMEntities();
                return db.TempEmployeesBasicSalaries.ToList();

            }
            catch
            {
                throw;
            }
        }
    }
}
