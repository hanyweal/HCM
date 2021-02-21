using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HCMDAL;
using HCMBLL.Enums;

namespace HCMBLL
{
    public class PlacementBLL : CommonEntity, IEntity
    {
        public virtual EmployeesCareersHistoryBLL EmployeeCareerHistory
        {
            get;
            set;
        }

        public OrganizationsStructuresBLL Organization { get; set; }

        public JobsBLL Job { get; set; }

        public EmployeesCodesBLL Manager { get; set; }

        public int EmployeesPlacedCount { get; set; }

        public virtual List<PlacementBLL> GetEmployeesInPlaced()
        {
            try
            {
                List<PlacementBLL> EmployeesInPlacedBLLList = new List<PlacementBLL>();
                List<EmployeesCareersHistory> Employees = new EmployeesCareersHistoryDAL().GetActiveEmployeesCareersHistory();
                List<Assignings> Assignings = new AssigningsDAL().GetAssignings();
                List<OrganizationsStructures> OrganizationsStructures = new OrganizationsStructuresDAL().GetOrganizationStructure();
                int[] EmployeeCareerHistoryIDs = Assignings.Select(c => (int)c.EmployeeCareerHistoryID).ToArray();
                int[] ManagerCodeIDs = OrganizationsStructures.Where(p => p.ManagerCodeID.HasValue == true).Select(c => c.ManagerCodeID.Value).ToArray(); //.Select(c => (int)c.ManagerCodeID).ToArray();
                Employees.RemoveAll(x => EmployeeCareerHistoryIDs.Contains(x.EmployeeCareerHistoryID));
                Employees.RemoveAll(o => ManagerCodeIDs.Contains(o.EmployeeCodeID));

                foreach (var item in Employees)
                {
                    EmployeesInPlacedBLLList.Add(new PlacementBLL() { EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item) });
                    //new OrganizationsStructuresBLL().GetOrganizationNameTillLastParentExceptPresident(((InternalAssigningBLL)AssigningBLL).Organization.OrganizationID);
                    EmployeesInPlacedBLLList.ForEach(x => x.EmployeeCareerHistory.OrganizationJob.OrganizationStructure.FullOrganizationName = new OrganizationsStructuresBLL().GetOrganizationNameTillLastParentExceptPresident(x.EmployeeCareerHistory.OrganizationJob.OrganizationStructure.OrganizationID));
                }
                return EmployeesInPlacedBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PlacementBLL> GetOrganizationsPlacement(int OrgnizationID)
        {
            try
            {
                List<PlacementBLL> PlacementBLLList = new List<PlacementBLL>();
                List<InternalAssigningBLL> AssigningBLLList = new InternalAssigningBLL().Get();
                List<OrganizationsStructuresBLL> OrganizationsBLLList = new OrganizationsStructuresBLL().GetByOrganizationIDWithhAllChilds(OrgnizationID);
                foreach (var item in OrganizationsBLLList)
                {
                    PlacementBLLList.Add(new PlacementBLL()
                    {
                        Organization = item,
                        EmployeesPlacedCount = AssigningBLLList.Where(x => x.Organization.OrganizationID == item.OrganizationID).Count()
                    });
                }
                return PlacementBLLList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// get Actual Org and Actual Job from organization structure module if he is a manager, but if he is a normal employee, get from internal assignings module
        /// </summary>
        /// <param name="EmployeeCodeNo"></param>
        /// <returns></returns>
        public PlacementBLL GetCurrentActualOrgAndActualJob(string EmployeeCodeNo)
        {
            try
            {
                PlacementBLL CurrentOrgAndJob = null;
                OrganizationsStructuresBLL OrganizationsStructureBasedOnManagement = new OrganizationsStructuresBLL().GetAllOrganizationsForManagerByManagerCodeNo(EmployeeCodeNo)?.FirstOrDefault();
                if (OrganizationsStructureBasedOnManagement != null)
                {
                    OrganizationsStructureBasedOnManagement.FullOrganizationName = new OrganizationsStructuresBLL().GetOrganizationNameTillLastParentExceptPresident(OrganizationsStructureBasedOnManagement.OrganizationID);
                    CurrentOrgAndJob = new PlacementBLL() { Organization = OrganizationsStructureBasedOnManagement, Job = new JobsBLL() { JobName = Globalization.ManagerText + " " + OrganizationsStructureBasedOnManagement.OrganizationName } };
                }
                else
                {
                    BaseAssigningsBLL CurrentAssigningRecord = new InternalAssigningBLL().GetEmployeeActiveAssigning(EmployeeCodeNo);
                    if (CurrentAssigningRecord != null)
                        CurrentOrgAndJob = new PlacementBLL() { Organization = ((InternalAssigningBLL)CurrentAssigningRecord).Organization, Job = ((InternalAssigningBLL)CurrentAssigningRecord).Job };
                }

                return CurrentOrgAndJob;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
