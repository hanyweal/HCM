using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class JobsOperationsTypesBLL : CommonEntity
    {
        public virtual int JobOperationTypeID
        {
            get;
            set;
        }
        public virtual string JobOperationTypeName
        {
            get;
            set;
        }
        public virtual List<JobsOperationsTypesBLL> GetJobsOperationsTypes()
        {
            List<JobsOperationsTypes> JobsOperationsTypesList = new JobsOperationsTypesDAL().GetJobsOperationsTypes();
            List<JobsOperationsTypesBLL> JobsOperationsTypesBLLList = new List<JobsOperationsTypesBLL>();
            if (JobsOperationsTypesList.Count > 0)
            {
                foreach (var item in JobsOperationsTypesList)
                {
                    JobsOperationsTypesBLLList.Add(new JobsOperationsTypesBLL()
                    {
                        JobOperationTypeID = item.JobOperationTypeID,
                        JobOperationTypeName = item.JobOperationTypeName,
                    });
                }
            }

            return JobsOperationsTypesBLLList;
        }
        public JobsOperationsTypesBLL GetByJobOperationTypeID(int JobOperationTypeID)
        {
            return GetJobsOperationsTypes().SingleOrDefault(x => x.JobOperationTypeID.Equals(JobOperationTypeID));
        }
        internal JobsOperationsTypesBLL MapJobsOperationsTypesBLL(JobsOperationsTypes JobsOperationsType)
        {
            JobsOperationsTypesBLL JobsOperationsTypesBLL = null;
            if (JobsOperationsType != null)
            {
                JobsOperationsTypesBLL = new JobsOperationsTypesBLL()
                {


                    JobOperationTypeID = JobsOperationsType.JobOperationTypeID,
                    JobOperationTypeName = JobsOperationsType.JobOperationTypeName,


                };
            }
            return JobsOperationsTypesBLL;
        }
    }
}
