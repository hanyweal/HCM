using HCMBLL.DTO;
using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class EServicesProxiesBLL : CommonEntity, IEntity
    {
        public int EServiceProxyID { get; set; }

        public EmployeesCodesBLL FromEmployee { get; set; }

        public EmployeesCodesBLL ToEmployee { get; set; }

        public EServicesTypesBLL EServiceType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public EServicesProxiesStatusBLL EServiceProxyStatus { get; set; }

        public bool IsActive { get; set; }

        public string Notes { get; set; }

        public virtual Result Add()
        {
            try
            {
                Result result = new Result();

                if(this.StartDate.ToString().Contains("0001"))
                {
                    result = new Result();
                    result.Entity = null;
                    result.EnumType = typeof(EServicesProxiesEnum);
                    result.EnumMember = EServicesProxiesEnum.RejectedBecauseStartDateRequried.ToString();

                    return result;
                }
                else if (this.LoginIdentity.EmployeeCodeID == this.ToEmployee.EmployeeCodeID)
                {
                    result = new Result();
                    result.Entity = null;
                    result.EnumType = typeof(EServicesProxiesEnum);
                    result.EnumMember = EServicesProxiesEnum.RejectedBecauseLoginManagerIsSameAsProxyEmployee.ToString();

                    return result;
                }
                else
                {
                    EServicesProxiesBLL bll = this.GetActiveByFromEmployeeCodeID(this.FromEmployee.EmployeeCodeID, (EServicesTypesEnum)this.EServiceType.EServiceTypeID);
                    if (bll != null && bll.EServiceProxyID > 0)
                    {
                        result = new Result();
                        result.Entity = bll;
                        result.EnumType = typeof(EServicesProxiesEnum);
                        result.EnumMember = EServicesProxiesEnum.RejectedBecauseThereIsAlreadyActiveProxyExist.ToString();

                        return result;
                    }

                    // Validate if ToEmployee has already EVacation Request Pending Exist
                    EmployeesCodes ToEmployee = new EmployeesCodesDAL().GetByEmployeeCodeID(this.ToEmployee.EmployeeCodeID);
                    result = new EVacationsRequestsBLL().IsEVacationRequestPendingExist(ToEmployee.EmployeeCodeNo);
                    if (result != null)
                    {
                        result = new Result();
                        result.Entity = null;
                        result.EnumType = typeof(EServicesProxiesEnum);
                        result.EnumMember = EServicesProxiesEnum.RejectedBecauseThereIsPendingEVacationRequestExist.ToString();

                        return result;
                    }

                    // Validate if ToEmployee has already Vacation Exist
                    List<BaseVacationsBLL> lst = new BaseVacationsBLL().GetByEmployeeCodeID(this.ToEmployee.EmployeeCodeID, this.StartDate, this.StartDate.AddDays(this.DaysCountInGregYear));
                    if (lst.Count > 0)
                    {
                        result = new Result();
                        result.Entity = null;
                        result.EnumType = typeof(EServicesProxiesEnum);
                        result.EnumMember = EServicesProxiesEnum.RejectedBecauseThereIsVacationExist.ToString();

                        return result;
                    }
                }

                EServicesProxies EServiceProxy = new EServicesProxies()
                {
                    EServiceTypeID = this.EServiceType.EServiceTypeID,
                    FromEmployeeCodeID = this.FromEmployee.EmployeeCodeID,
                    ToEmployeeCodeID = this.ToEmployee.EmployeeCodeID,
                    StartDate = this.StartDate,
                    EndDate = this.EndDate,
                    EServiceProxyStatusID = (int)EServicesProxiesStatusEnum.Valid,
                    IsActive = true,
                    Notes = this.Notes,
                    CreatedDate = DateTime.Now,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID
                };

                this.EServiceProxyID = new EServicesProxiesDAL().Insert(EServiceProxy);

                #region Send SMS on creation
                EmployeesCodesBLL FromEmployeeCodeBLL = new EmployeesCodesBLL().GetByEmployeeCodeID(this.FromEmployee.EmployeeCodeID);
                EmployeesCodesBLL ToEmployeeCodeBLL = new EmployeesCodesBLL().GetByEmployeeCodeID(this.ToEmployee.EmployeeCodeID);
                SMSBLL SMSObj = new SMSBLL();
                SMSObj.SendSMS(new SMSLogsBLL()
                {
                    BusinssSubCategory = BusinessSubCategoriesEnum.EServicesProxies,
                    DetailID = this.EServiceProxyID,
                    MobileNo = ToEmployeeCodeBLL.Employee.EmployeeMobileNo,
                    Message = string.Format(Globalization.SMSEServicesProxiesCreationText,
                                ToEmployeeCodeBLL.Employee.EmployeeNameAr, FromEmployeeCodeBLL.Employee.EmployeeNameAr),
                    CreatedBy = this.LoginIdentity,
                });
                #endregion

                result = new Result();
                result.Entity = this;
                result.EnumType = typeof(EServicesProxiesEnum);
                result.EnumMember = EServicesProxiesEnum.Done.ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result RevokeEServiceProxy(int EServiceProxyID, EServicesProxiesStatusEnum Status, string CancellationReason = "")
        {
            try
            {
                Result result = new Result();

                EServicesProxies EServiceProxy = new EServicesProxiesDAL().GetActiveByEServiceProxyID(EServiceProxyID);
                if (EServiceProxy == null || EServiceProxy.EServiceProxyID <= 0)
                {
                    result = new Result();
                    result.Entity = null;
                    result.EnumType = typeof(EServicesAuthorizationsEnum);
                    result.EnumMember = EServicesProxiesEnum.RejectedBecauseEServiceProxyIsNotActive.ToString();

                    return result;
                }

                int ToEmployeeCodeID = EServiceProxy.ToEmployeeCodeID;
                int FromEmployeeCodeID = EServiceProxy.FromEmployeeCodeID;
                EServiceProxy = new EServicesProxies()
                {
                    EServiceProxyID = EServiceProxyID,
                    EndDate = DateTime.Now,
                    EServiceProxyStatusID = (int)Status,
                    IsActive = false,
                    CancellationReason = CancellationReason,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
                };

                this.EServiceProxyID = new EServicesProxiesDAL().Revoke(EServiceProxy);

                #region Send SMS on revoke to "ToEmployee"
                EmployeesCodesBLL FromEmployeeCodeBLL = new EmployeesCodesBLL().GetByEmployeeCodeID(FromEmployeeCodeID);
                EmployeesCodesBLL ToEmployeeCodeBLL = new EmployeesCodesBLL().GetByEmployeeCodeID(ToEmployeeCodeID);
                SMSBLL SMSObj = new SMSBLL();
                SMSObj.SendSMS(new SMSLogsBLL()
                {
                    BusinssSubCategory = BusinessSubCategoriesEnum.EServicesProxies,
                    DetailID = this.EServiceProxyID,
                    MobileNo = ToEmployeeCodeBLL.Employee.EmployeeMobileNo,
                    Message = string.Format(Globalization.SMSEServicesProxiesRevokeText,
                                ToEmployeeCodeBLL.Employee.EmployeeNameAr, FromEmployeeCodeBLL.Employee.EmployeeNameAr),
                    CreatedBy = this.LoginIdentity,
                });
                #endregion

                #region Send SMS on revoke to "FromEmployee" in case of Proxy cancelled by System
                if (Status == EServicesProxiesStatusEnum.CancelledBySystem)
                {
                    SMSObj.SendSMS(new SMSLogsBLL()
                    {
                        BusinssSubCategory = BusinessSubCategoriesEnum.EServicesProxies,
                        DetailID = this.EServiceProxyID,
                        MobileNo = FromEmployeeCodeBLL.Employee.EmployeeMobileNo,
                        Message = string.Format(Globalization.SMSEServicesProxiesRevokeBySytemText,
                                FromEmployeeCodeBLL.Employee.EmployeeNameAr, CancellationReason),
                        CreatedBy = this.LoginIdentity,
                    });
                }
                #endregion                

                result.Entity = this;
                result.EnumType = typeof(EServicesProxiesEnum);
                result.EnumMember = EServicesProxiesEnum.Done.ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Task 318: Service to Cancel EServicesProxies By System
        /// </summary>
        /// <param name="EmployeeCodeID"></param>
        /// <param name="Status"></param>
        /// <param name="CancellationReason"></param>
        /// <returns></returns>
        public Result RevokeEServiceProxyByEmployeeCodeID(int EmployeeCodeID, EServicesProxiesStatusEnum Status, string CancellationReason)
        {
            try
            {
                Result result = new Result();

                foreach (EServicesProxies item in new EServicesProxiesDAL().GetActiveByFromEmployeeCodeID(EmployeeCodeID))                
                    result = this.RevokeEServiceProxy(item.EServiceProxyID, EServicesProxiesStatusEnum.CancelledBySystem, CancellationReason);
                
                result.Entity = this;
                result.EnumType = typeof(EServicesProxiesEnum);
                result.EnumMember = EServicesProxiesEnum.Done.ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EServicesProxiesBLL> Get(EServicesTypesEnum EServiceType)
        {
            try
            {
                List<EServicesProxies> EServiceProxyList = new EServicesProxiesDAL().Get((int)EServiceType).ToList();
                List<EServicesProxiesBLL> EServiceProxyBLLList = new List<EServicesProxiesBLL>();

                foreach (EServicesProxies item in EServiceProxyList)                
                    EServiceProxyBLLList.Add(this.MapEServiceProxy(item));                

                return EServiceProxyBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get All Proxies based on Creator Employee 
        /// </summary>
        /// <param name="FromEmployeeCodeID"></param>
        /// <param name="EServiceType"></param>
        /// <returns></returns>
        public List<EServicesProxiesBLL> GetByFromEmployeeCodeID(int FromEmployeeCodeID, EServicesTypesEnum? EServiceType)
        {
            try
            {
                List<EServicesProxies> EServiceProxyList = new EServicesProxiesDAL().GetByFromEmployeeCodeID(FromEmployeeCodeID, (int?)EServiceType).ToList();
                List<EServicesProxiesBLL> EServiceProxyBLLList = new List<EServicesProxiesBLL>();

                foreach (EServicesProxies item in EServiceProxyList)
                    EServiceProxyBLLList.Add(this.MapEServiceProxy(item));

                return EServiceProxyBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get All Proxies based on Proxied(To) Employee 
        /// </summary>
        /// <param name="ToEmployeeCodeID"></param>
        /// <param name="EServiceType"></param>
        /// <returns></returns>
        public List<EServicesProxiesBLL> GetByToEmployeeCodeID(int ToEmployeeCodeID, EServicesTypesEnum? EServiceType)
        {
            try
            {
                List<EServicesProxies> EServiceProxyList = new EServicesProxiesDAL().GetByToEmployeeCodeID(ToEmployeeCodeID, (int?)EServiceType).ToList();
                List<EServicesProxiesBLL> EServiceProxyBLLList = new List<EServicesProxiesBLL>();

                foreach (EServicesProxies item in EServiceProxyList)
                    EServiceProxyBLLList.Add(this.MapEServiceProxy(item));

                return EServiceProxyBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get All Active Proxies based on Proxied(To) Employee 
        /// </summary>
        /// <param name="ToEmployeeCodeID"></param>
        /// <returns></returns>
        public List<EServicesProxiesBLL> GetActiveByToEmployeeCodeID(int ToEmployeeCodeID)
        {
            try
            {
                List<EServicesProxies> EServiceProxyList = new EServicesProxiesDAL().GetActiveByToEmployeeCodeID(ToEmployeeCodeID).ToList();
                List<EServicesProxiesBLL> EServiceProxyBLLList = new List<EServicesProxiesBLL>();

                foreach (EServicesProxies item in EServiceProxyList)
                    EServiceProxyBLLList.Add(this.MapEServiceProxy(item));

                return EServiceProxyBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Active Proxy based on Creator Employee and ServiceType 
        /// </summary>
        /// <param name="FromEmployeeCodeID"></param>
        /// <param name="EServiceType"></param>
        /// <returns></returns>
        public EServicesProxiesBLL GetActiveByFromEmployeeCodeID(int FromEmployeeCodeID, EServicesTypesEnum EServiceType)
        {
            try
            {
                EServicesProxies EServiceProxy
                    = new EServicesProxiesDAL().GetByFromEmployeeCodeID(FromEmployeeCodeID, (int)EServiceType)
                    .FirstOrDefault(x=>x.IsActive == true);

                return this.MapEServiceProxy(EServiceProxy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal EServicesProxiesBLL MapEServiceProxy(EServicesProxies item)
        {
            return item != null ?
                new EServicesProxiesBLL()
                {
                    EServiceProxyID = item.EServiceProxyID,
                    FromEmployee = new EmployeesCodesBLL().MapEmployeeCode(item.FromEmployeeCode),
                    ToEmployee = new EmployeesCodesBLL().MapEmployeeCode(item.ToEmployeeCode),
                    EServiceType = new EServicesTypesBLL().MapEServicesTypes(item.EServicesTypes),
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    IsActive = item.IsActive,
                    EServiceProxyStatus = new EServicesProxiesStatusBLL().MapEServicesProxiesStatus(item.EServicesProxiesStatus),
                    Notes = item.Notes
                }
                : null;
        }
    }
}
