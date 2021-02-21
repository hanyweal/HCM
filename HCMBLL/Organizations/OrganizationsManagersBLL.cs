using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class OrganizationsManagersBLL : CommonEntity
    {
        public int OrganizationMangerID { get; set; }

        public EmployeesCodesBLL ManagerCode { get; set; }

        public OrganizationsStructuresBLL Organization { get; set;}

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public List<OrganizationsManagersBLL> GetOrganizationsManagers(int OrganizationID)
        {
            try
            {
                List<OrganizationsManagers> OrganizationsManagersList = new OrganizationsManagersDAL().GetOrganizationsManagers(OrganizationID);
                List<OrganizationsManagersBLL> OrganizationsManagersBLLList = new List<OrganizationsManagersBLL>();
                OrganizationsManagersList.ForEach(x => OrganizationsManagersBLLList.Add(new OrganizationsManagersBLL()
                {
                    OrganizationMangerID = x.OrganizationManagerID,
                    ManagerCode = new EmployeesCodesBLL().MapEmployeeCode(x.ManagersCodesNav),
                    Organization = new OrganizationsStructuresBLL().MapOrganization(x.OrganizationsStructures),
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(x.CreatedByNav),
                    CreatedDate = x.CreatedDate,
                }));

                return OrganizationsManagersBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OrganizationsManagersBLL> GetOrganizationsOfManager(int ManagerCodeID)
        {
            try
            {
                List<OrganizationsManagers> OrganizationsManagersList = new OrganizationsManagersDAL().GetOrganizationsOfManager(ManagerCodeID);
                List<OrganizationsManagersBLL> OrganizationsManagersBLLList = new List<OrganizationsManagersBLL>();
                OrganizationsManagersList.ForEach(x => OrganizationsManagersBLLList.Add(new OrganizationsManagersBLL()
                {
                    OrganizationMangerID = x.OrganizationManagerID,
                    ManagerCode = new EmployeesCodesBLL().MapEmployeeCode(x.ManagersCodesNav),
                    Organization = new OrganizationsStructuresBLL().MapOrganization(x.OrganizationsStructures),
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(x.CreatedByNav),
                    CreatedDate = x.CreatedDate,
                }));

                return OrganizationsManagersBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
