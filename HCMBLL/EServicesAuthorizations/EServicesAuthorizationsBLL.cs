using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class EServicesAuthorizationsBLL : CommonEntity, IEntity
    {
        public int EServiceAuthorizationID { get; set; }

        public EmployeesCodesBLL AuthorizedPerson { get; set; }

        public OrganizationsStructuresBLL Organization { get; set; }

        public EServicesTypesBLL EServiceType { get; set; }

        /// <summary>
        /// Task 256: Adding records in e service authorization after adding new organization node in organizations structure
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Result Add()
        {
            try
            {
                Result result = new Result();
                EServicesAuthorizations EServiceAuthorization = new EServicesAuthorizations()
                {
                    OrganizationID = this.Organization.OrganizationID,
                    EServiceTypeID = this.EServiceType.EServiceTypeID,
                    AuthorizedPersonCodeID = this.AuthorizedPerson.EmployeeCodeID,
                    CreatedDate = DateTime.Now,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID
                };

                this.EServiceAuthorizationID = new EServicesAuthorizationsDAL().Insert(EServiceAuthorization);

                result.Entity = this;
                result.EnumType = typeof(EServicesAuthorizationsEnum);
                result.EnumMember = EServicesAuthorizationsEnum.Done.ToString();
                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// get authorized person to specific organization and e service type
        /// </summary>
        /// <param name="OrganizationID"></param>
        /// <param name="EServiceType"></param>
        /// <returns></returns>
        public EServicesAuthorizationsBLL GetOrganizationAuthorizedPerson(int OrganizationID, EServicesTypesEnum EServiceType)
        {
            try
            {
                EServicesAuthorizations EServiceAuthorization = new EServicesAuthorizationsDAL().Get(OrganizationID, (int)EServiceType).FirstOrDefault();
                EServicesAuthorizationsBLL EServicesAuthorizationBLL = null;
                if (EServiceAuthorization != null)
                    EServicesAuthorizationBLL = new EServicesAuthorizationsBLL()
                    {
                        AuthorizedPerson = new EmployeesCodesBLL().MapEmployeeCode(EServiceAuthorization.AuthorizedPersonNav),
                        Organization = new OrganizationsStructuresBLL().MapOrganizationWithoutManager(EServiceAuthorization.OrganizationsStructures),
                        EServiceType = new EServicesTypesBLL() { EServiceTypeID = EServiceAuthorization.EServiceTypeID, EServiceTypeName = EServiceAuthorization.EServicesTypes.EServiceTypeName }
                    };

                return EServicesAuthorizationBLL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get all organizations under person authorizations and service type
        /// </summary>
        /// <param name="AuthorizedPersonCodeNo"></param>
        /// <param name="EServiceType"></param>
        /// <returns></returns>
        public List<EServicesAuthorizationsBLL> GetBasedOnAuthorizedPerson(string AuthorizedPersonCodeNo, EServicesTypesEnum EServiceType)
        {
            try
            {
                List<EServicesAuthorizations> EServiceAuthorizationList = new EServicesAuthorizationsDAL().GetOrganizations(AuthorizedPersonCodeNo, (int)EServiceType);
                List<EServicesAuthorizationsBLL> EServicesAuthorizationBLLList = new List<EServicesAuthorizationsBLL>();
                foreach (var item in EServiceAuthorizationList)
                {
                    EServicesAuthorizationBLLList.Add(new EServicesAuthorizationsBLL()
                    {
                        AuthorizedPerson = new EmployeesCodesBLL().MapEmployeeCode(item.AuthorizedPersonNav),
                        Organization = new OrganizationsStructuresBLL().MapOrganizationWithoutManager(item.OrganizationsStructures),
                        EServiceType = new EServicesTypesBLL() { EServiceTypeID = item.EServiceTypeID, EServiceTypeName = item.EServicesTypes.EServiceTypeName }
                    });
                }

                return EServicesAuthorizationBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EServicesAuthorizationsBLL> GetFirstLevel(int OrganizationID, EServicesTypesEnum EServiceTypeID)
        {
            try
            {
                List<int> ChildOrganizationIDs = new OrganizationsStructuresBLL().GetOrganizationFirstLevelByID(OrganizationID); //new List<int>(); ChildOrganizationIDs.Add(1803); //
                List<EServicesAuthorizations> EServiceAuthorizationList = new EServicesAuthorizationsDAL().GetOrganizationsByOrganizationIDs(ChildOrganizationIDs, (int)EServiceTypeID);
                List<EServicesAuthorizationsBLL> EServicesAuthorizationBLLList = new List<EServicesAuthorizationsBLL>();

                foreach (var item in EServiceAuthorizationList)
                    EServicesAuthorizationBLLList.Add(new EServicesAuthorizationsBLL()
                    {
                        EServiceAuthorizationID = item.EServiceAuthorizationID,
                        //AuthorizedPerson = new EmployeesCodesBLL()
                        //{
                        //    EmployeeCodeNo = item.AuthorizedPersonNav.EmployeeCodeNo,
                        //    Employee = new EmployeesBLL()
                        //    {
                        //        FirstNameAr = item.AuthorizedPersonNav.Employees.FirstNameAr,
                        //        MiddleNameAr = item.AuthorizedPersonNav.Employees.MiddleNameAr,
                        //        GrandFatherNameAr = item.AuthorizedPersonNav.Employees.GrandFatherNameAr,
                        //        LastNameAr = item.AuthorizedPersonNav.Employees.LastNameAr,
                        //    }
                        //},
                        //Organization = new OrganizationsStructuresBLL()
                        //{
                        //    OrganizationID = item.OrganizationsStructures.OrganizationID,
                        //    ParentOrganization = item.OrganizationsStructures.ParentOrganization == null ? null : new OrganizationsStructuresBLL()
                        //    {
                        //        OrganizationID = item.OrganizationsStructures.ParentOrganization.OrganizationID
                        //    },
                        //    OrganizationName = item.OrganizationsStructures.OrganizationName,
                        //    Branch = new BranchesBLL().MapBranch(item.OrganizationsStructures.Branches),
                        //    OrganizationManager = item.OrganizationsStructures.EmployeesCodes != null ? new EmployeesCodesBLL()
                        //    {
                        //        EmployeeCodeID = item.OrganizationsStructures.EmployeesCodes.EmployeeCodeID,
                        //        EmployeeCodeNo = item.OrganizationsStructures.EmployeesCodes.EmployeeCodeNo,
                        //        Employee = new EmployeesBLL()
                        //        {
                        //            FirstNameAr = item.OrganizationsStructures.EmployeesCodes.Employees.FirstNameAr,
                        //            MiddleNameAr = item.OrganizationsStructures.EmployeesCodes.Employees.MiddleNameAr,
                        //            GrandFatherNameAr = item.OrganizationsStructures.EmployeesCodes.Employees.GrandFatherNameAr,
                        //            LastNameAr = item.OrganizationsStructures.EmployeesCodes.Employees.LastNameAr,
                        //        },
                        //    } : null,
                        //},
                        AuthorizedPerson = new EmployeesCodesBLL().MapEmployeeCode(item.AuthorizedPersonNav),
                        Organization = new OrganizationsStructuresBLL().MapOrganization(item.OrganizationsStructures),
                        EServiceType = new EServicesTypesBLL().MapEServicesTypes(item.EServicesTypes)
                    });

                return EServicesAuthorizationBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EServicesAuthorizationsBLL GetByOrganizationID(int OrganizationID, EServicesTypesEnum EServiceTypeID)
        {
            try
            {
                EServicesAuthorizations EServiceAuthorization = new EServicesAuthorizationsDAL().GetByOrganizationID(OrganizationID, (int)EServiceTypeID);
                EServicesAuthorizationsBLL EServiceAuthorizationBLL = new EServicesAuthorizationsBLL();

                EServiceAuthorizationBLL = new EServicesAuthorizationsBLL()
                {
                    EServiceAuthorizationID = EServiceAuthorization.EServiceAuthorizationID,
                    AuthorizedPerson = new EmployeesCodesBLL().MapEmployeeCode(EServiceAuthorization.AuthorizedPersonNav),
                    Organization = new OrganizationsStructuresBLL().MapOrganization(EServiceAuthorization.OrganizationsStructures),
                    EServiceType = new EServicesTypesBLL().MapEServicesTypes(EServiceAuthorization.EServicesTypes)
                };

                return EServiceAuthorizationBLL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result SetAuthorizations(int EServiceTypeID, int LoginOrganizationID, int OrganizationID, int AuthorizedPersonCodeID)
        {
            Result result = null;

            // validate Login and Authority Organization
            if (LoginOrganizationID == OrganizationID)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(EServicesAuthorizationsEnum);
                result.EnumMember = EServicesAuthorizationsEnum.RejectedBecauseLoginOrganizationIsSameAsAuthorization.ToString();

                return result;
            }
            else if (AuthorizedPersonCodeID == 0)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(EServicesAuthorizationsEnum);
                result.EnumMember = EServicesAuthorizationsEnum.RejectedBecauseThereIsNoManagerForThisOrganization.ToString();

                return result;
            }

            EServicesAuthorizations LoginOrganizationAuth = new EServicesAuthorizationsDAL().GetByOrganizationID(LoginOrganizationID, EServiceTypeID);
            if(LoginOrganizationAuth.AuthorizedPersonCodeID != this.LoginIdentity.EmployeeCodeID)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(EServicesAuthorizationsEnum);
                result.EnumMember = EServicesAuthorizationsEnum.RejectedBecauseOrganizationManagerIsNotAuthorized.ToString();

                return result;
            }

            List<int> ChildOrganizationIDs = new OrganizationsStructuresBLL().GetAllChildernByOrganizationID(OrganizationID);
            int LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
            new EServicesAuthorizationsDAL().SetAuthorizations(EServiceTypeID, ChildOrganizationIDs, AuthorizedPersonCodeID, LastUpdatedBy);

            result = new Result();
            result.Entity = null;
            result.EnumType = typeof(EServicesAuthorizationsEnum);
            result.EnumMember = EServicesAuthorizationsEnum.Done.ToString();

            return result;
        }

        /// <summary>
        /// Task # 255: Changing the manager of any organization must affect on e services authorization module
        /// </summary>
        /// <param name="OrganizationID"></param>
        /// <param name="OldAuthorizedPersonCodeID"></param>
        /// <param name="NewAuthorizedPersonCodeID"></param>
        /// <returns></returns>
        internal Result ChangeAuthorizedPersonForAllChildByOrganizationID(int OrganizationID, int OldAuthorizedPersonCodeID, int NewAuthorizedPersonCodeID)
        {
            Result result = null;
            List<int> ChildOrganizationIDs = new OrganizationsStructuresBLL().GetAllChildernByOrganizationID(OrganizationID);
            int LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
            new EServicesAuthorizationsDAL().ChangeAuthorizedPersonForAllChildByOrganizationID(ChildOrganizationIDs, OldAuthorizedPersonCodeID, NewAuthorizedPersonCodeID, LastUpdatedBy);

            result = new Result();
            result.Entity = null;
            result.EnumType = typeof(EServicesAuthorizationsEnum);
            result.EnumMember = EServicesAuthorizationsEnum.Done.ToString();

            return result;
        }

        internal EServicesAuthorizationsBLL MapEServiceAuthorization(EServicesAuthorizations item)
        {
            return item != null ?
                new EServicesAuthorizationsBLL()
                {
                    EServiceAuthorizationID = item.EServiceAuthorizationID,
                    AuthorizedPerson = new EmployeesCodesBLL().MapEmployeeCode(item.AuthorizedPersonNav),
                    Organization = new OrganizationsStructuresBLL().MapOrganizationWithoutManager(item.OrganizationsStructures),
                    EServiceType = new EServicesTypesBLL().MapEServicesTypes(item.EServicesTypes)
                }
                : null;
        }
    }
}
