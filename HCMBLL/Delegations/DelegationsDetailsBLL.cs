using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class DelegationsDetailsBLL : CommonEntity, IPassengerOrderTypeDetails
    {
        public int DelegationDetailID { get; set; }

        public BaseDelegationsBLL Delegation
        {
            get;
            set;
            //get
            //{ return new InternalDelegationBLL(); }
            //set
            //{ value = new InternalDelegationBLL(); }
        }

        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public bool IsPassengerOrderCompensation { get; set; }

        public List<DelegationsDetailsBLL> GetDelegationsDetails()
        {
            List<DelegationsDetails> DelegationsDetailsList = new DelegationsDetailsDAL().GetDelegationsDetails();
            List<DelegationsDetailsBLL> DelegationsDetailsBLLList = new List<DelegationsDetailsBLL>();
            if (DelegationsDetailsList.Count > 0)
            {
                foreach (var item in DelegationsDetailsList)
                {
                    DelegationsDetailsBLLList.Add(new DelegationsDetailsBLL().MapDelegationDetail(item));
                }
            }
            return DelegationsDetailsBLLList.ToList();//.Select( x => new { x.EmployeeCode.EmployeeCodeNo });
        }

        public List<DelegationsDetailsBLL> GetDelegationsDetailsByDelegationID(int DelegationID)
        {
            try
            {
                List<DelegationsDetails> DelegationsDetailsList = new DelegationsDetailsDAL().GetDelegationsDetailsByDelegationID(DelegationID);
                List<DelegationsDetailsBLL> DelegationsDetailsBLLList = new List<DelegationsDetailsBLL>();
                if (DelegationsDetailsList.Count > 0)
                {
                    foreach (var item in DelegationsDetailsList)
                    {
                        DelegationsDetailsBLLList.Add(new DelegationsDetailsBLL().MapDelegationDetail(item));
                    }
                }
                return DelegationsDetailsBLLList;
            }
            catch
            {
                throw;
            }
        }

        public DelegationsDetailsBLL GetDelegationsDetailsByDelegationDetailID(int DelegationDetailID)
        {
            DelegationsDetails DelegationsDetails = new DelegationsDetailsDAL().GetDelegationsDetailsByDelegationDetailID(DelegationDetailID);
            DelegationsDetailsBLL DelegationsDetailsBLL = new DelegationsDetailsBLL();
            if (DelegationsDetails != null)
            {
                DelegationsDetailsBLL = new DelegationsDetailsBLL().MapDelegationDetail(DelegationsDetails);
            }

            return DelegationsDetailsBLL;
        }

        public List<DelegationsDetailsBLL> GetDelegationsDetailsByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                List<DelegationsDetails> DelegationsDetailsList = new DelegationsDetailsDAL().GetDelegationsDetailsByEmployeeCodeID(EmployeeCodeID);
                List<DelegationsDetailsBLL> DelegationsDetailsBLLList = new List<DelegationsDetailsBLL>();
                if (DelegationsDetailsList.Count > 0)
                {
                    foreach (var item in DelegationsDetailsList)
                    {
                        DelegationsDetailsBLLList.Add(new DelegationsDetailsBLL().MapDelegationDetail(item));
                    }
                }
                return DelegationsDetailsBLLList;
            }
            catch
            {
                throw;
            }
        }

        public int GetDelegationConsumedByEmployeeCodeID(int DelegationFiscalYear, int EmployeeCodeID, int DelegationKindID)
        {
            return this.GetDelegationsDetailsByEmployeeCodeID(EmployeeCodeID)
                 .Where(d => d.Delegation.DelegationStartDate.Year == DelegationFiscalYear
                                && d.Delegation.DelegationEndDate.Year == DelegationFiscalYear
                                && d.Delegation.DelegationKind.DelegationKindID == DelegationKindID)
                .ToList().Sum(d => d.Delegation.DelegationPeriod);
        }

        public Result Add(DelegationsDetailsBLL DelegationDetailBLL)
        {
            Result result = new Result();

            // validate employee already exists
            DelegationsDetails employee = new DelegationsDetailsDAL().GetDelegationsDetailsByDelegationID(DelegationDetailBLL.Delegation.DelegationID)
                        .Where(d => d.EmployeeCareerHistoryID == DelegationDetailBLL.EmployeeCareerHistory.EmployeeCareerHistoryID).FirstOrDefault();
            if (employee != null)
            {
                result.Entity = null;
                result.EnumType = typeof(DelegationsValidationEnum);
                result.EnumMember = DelegationsValidationEnum.RejectedBecauseAlreadyExist.ToString();

                return result;
            }

            // other validation
            result = new EmployeeDelegationBLL(DelegationDetailBLL.Delegation.DelegationStartDate, DelegationDetailBLL.Delegation.DelegationEndDate, DelegationDetailBLL.Delegation.DelegationKind.DelegationKindID, new EmployeesCodesBLL()
            {
                EmployeeCodeID = DelegationDetailBLL.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID
            }).IsValid();

            if (result.EnumMember == DelegationsValidationEnum.Done.ToString())
            {
                result = new Result();
                DelegationsDetails DelegationDetail = new DelegationsDetails()
                {
                    DelegationID = DelegationDetailBLL.Delegation.DelegationID,
                    EmployeeCareerHistoryID = DelegationDetailBLL.EmployeeCareerHistory.EmployeeCareerHistoryID,
                    CreatedDate = DateTime.Now,
                    CreatedBy = DelegationDetailBLL.LoginIdentity.EmployeeCodeID,
                    IsPassengerOrderCompensation = DelegationDetailBLL.IsPassengerOrderCompensation
                };
                this.DelegationDetailID = new DelegationsDetailsDAL().Insert(DelegationDetail);
                if (this.DelegationDetailID != 0)
                {
                    result.Entity = null;
                    result.EnumType = typeof(DelegationsValidationEnum);
                    result.EnumMember = DelegationsValidationEnum.Done.ToString();
                }
            }
            return result;
        }

        public Result Remove(int DelegationDetailID)
        {
            try
            {
                Result result = null;
                new DelegationsDetailsDAL().Delete(DelegationDetailID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(DelegationsValidationEnum),
                    EnumMember = DelegationsValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        internal DelegationsDetailsBLL MapDelegationDetail(DelegationsDetails DelegationDetail)
        {
            try
            {
                DelegationsDetailsBLL DelegationDetailBLL = null;
                if (DelegationDetail != null)
                {
                    DelegationDetailBLL = new DelegationsDetailsBLL()
                    {
                        DelegationDetailID = DelegationDetail.DelegationDetailID,
                        Delegation = new BaseDelegationsBLL().MapDelegation(DelegationDetail.Delegations),
                        EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(DelegationDetail.EmployeesCareersHistory),
                        IsPassengerOrderCompensation = DelegationDetail.IsPassengerOrderCompensation
                    };
                }
                return DelegationDetailBLL;
            }
            catch
            {
                throw;
            }
        }

        public int DetailID
        {
            get
            {
                return DelegationDetailID;
            }
            set
            {
                DelegationDetailID = value;
            }
        }
    }
}