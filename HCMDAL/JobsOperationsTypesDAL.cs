using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMDAL
{
    public class JobsOperationsTypesDAL
    {
       public List<JobsOperationsTypes> GetJobsOperationsTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsOperationsTypes
                    .ToList();

            }
            catch
            {
                throw;
            }
        }

        public JobsOperationsTypes GetByJobOperationTypeID(int JobOperationTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.JobsOperationsTypes
                        .SingleOrDefault(x => x.JobOperationTypeID.Equals(JobOperationTypeID));

            }
            catch
            {
                throw;
            }
        }
    }
}
