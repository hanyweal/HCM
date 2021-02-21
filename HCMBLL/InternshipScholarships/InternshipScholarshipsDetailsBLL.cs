using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class InternshipScholarshipsDetailsBLL : CommonEntity, IPassengerOrderTypeDetails
    {
        public int InternshipScholarshipDetailID { get; set; }

        public BaseInternshipScholarshipsBLL InternshipScholarship
        {
            get;
            set; 
        }

        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public bool IsPassengerOrderCompensation { get; set; }

        public List<InternshipScholarshipsDetailsBLL> GetInternshipScholarshipsDetails()
        {
            List<InternshipScholarshipsDetails> InternshipScholarshipsDetailsList = new InternshipScholarshipsDetailsDAL().GetInternshipScholarshipsDetails();
            List<InternshipScholarshipsDetailsBLL> InternshipScholarshipsDetailsBLLList = new List<InternshipScholarshipsDetailsBLL>();
            if (InternshipScholarshipsDetailsList.Count > 0)
            {
                foreach (var item in InternshipScholarshipsDetailsList)
                {
                    InternshipScholarshipsDetailsBLLList.Add(new InternshipScholarshipsDetailsBLL().MapInternshipScholarshipDetail(item));
                }
            }
            return InternshipScholarshipsDetailsBLLList.ToList();
        }

        public List<InternshipScholarshipsDetailsBLL> GetInternshipScholarshipsDetailsByInternshipScholarshipID(int InternshipScholarshipID)
        {
            try
            {
                List<InternshipScholarshipsDetails> InternshipScholarshipsDetailsList = new InternshipScholarshipsDetailsDAL().GetInternshipScholarshipsDetailsByInternshipScholarshipID(InternshipScholarshipID);
                List<InternshipScholarshipsDetailsBLL> InternshipScholarshipsDetailsBLLList = new List<InternshipScholarshipsDetailsBLL>();
                if (InternshipScholarshipsDetailsList.Count > 0)
                {
                    foreach (var item in InternshipScholarshipsDetailsList)
                    {
                        InternshipScholarshipsDetailsBLLList.Add(new InternshipScholarshipsDetailsBLL().MapInternshipScholarshipDetail(item));
                    }
                }
                return InternshipScholarshipsDetailsBLLList;
            }
            catch
            {
                throw;
            }
        }

        public InternshipScholarshipsDetailsBLL GetInternshipScholarshipsDetailsByInternshipScholarshipDetailID(int InternshipScholarshipDetailID)
        {
            InternshipScholarshipsDetails InternshipScholarshipsDetails = new InternshipScholarshipsDetailsDAL().GetInternshipScholarshipsDetailsByInternshipScholarshipDetailID(InternshipScholarshipDetailID);
            InternshipScholarshipsDetailsBLL InternshipScholarshipsDetailsBLL = new InternshipScholarshipsDetailsBLL();
            if (InternshipScholarshipsDetails != null)
            {
                InternshipScholarshipsDetailsBLL = new InternshipScholarshipsDetailsBLL().MapInternshipScholarshipDetail(InternshipScholarshipsDetails);
            }

            return InternshipScholarshipsDetailsBLL;
        }

        public List<InternshipScholarshipsDetailsBLL> GetInternshipScholarshipsDetailsByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                List<InternshipScholarshipsDetails> InternshipScholarshipsDetailsList = new InternshipScholarshipsDetailsDAL().GetInternshipScholarshipsDetailsByEmployeeCodeID(EmployeeCodeID);
                List<InternshipScholarshipsDetailsBLL> InternshipScholarshipsDetailsBLLList = new List<InternshipScholarshipsDetailsBLL>();
                if (InternshipScholarshipsDetailsList.Count > 0)
                {
                    foreach (var item in InternshipScholarshipsDetailsList)
                    {
                        InternshipScholarshipsDetailsBLLList.Add(new InternshipScholarshipsDetailsBLL().MapInternshipScholarshipDetail(item));
                    }
                }
                return InternshipScholarshipsDetailsBLLList;
            }
            catch
            {
                throw;
            }
        }

        public Result Add(InternshipScholarshipsDetailsBLL InternshipScholarshipDetailBLL)
        {
            Result result = new Result();

            // validate employee already exists
            InternshipScholarshipsDetails employee = new InternshipScholarshipsDetailsDAL().GetInternshipScholarshipsDetailsByInternshipScholarshipID(InternshipScholarshipDetailBLL.InternshipScholarship.InternshipScholarshipID)
                        .Where(d => d.EmployeeCareerHistoryID == InternshipScholarshipDetailBLL.EmployeeCareerHistory.EmployeeCareerHistoryID).FirstOrDefault();
            if (employee != null)
            {
                result.Entity = null;
                result.EnumType = typeof(InternshipScholarshipsValidationEnum);
                result.EnumMember = InternshipScholarshipsValidationEnum.RejectedBecauseAlreadyExist.ToString();

                return result;
            }

            result = new Result();
            InternshipScholarshipsDetails InternshipScholarshipDetail = new InternshipScholarshipsDetails()
            {
                InternshipScholarshipID = InternshipScholarshipDetailBLL.InternshipScholarship.InternshipScholarshipID,
                EmployeeCareerHistoryID = InternshipScholarshipDetailBLL.EmployeeCareerHistory.EmployeeCareerHistoryID,
                CreatedDate = DateTime.Now,
                CreatedBy = InternshipScholarshipDetailBLL.LoginIdentity.EmployeeCodeID,
                IsPassengerOrderCompensation = InternshipScholarshipDetailBLL.IsPassengerOrderCompensation
            };
            this.InternshipScholarshipDetailID = new InternshipScholarshipsDetailsDAL().Insert(InternshipScholarshipDetail);
            if (this.InternshipScholarshipDetailID != 0)
            {
                result.Entity = null;
                result.EnumType = typeof(InternshipScholarshipsValidationEnum);
                result.EnumMember = InternshipScholarshipsValidationEnum.Done.ToString();
            }

            return result;
        }

        public Result Remove(int InternshipScholarshipDetailID)
        {
            try
            {
                Result result = null;
                new InternshipScholarshipsDetailsDAL().Delete(InternshipScholarshipDetailID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(InternshipScholarshipsValidationEnum),
                    EnumMember = InternshipScholarshipsValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        internal InternshipScholarshipsDetailsBLL MapInternshipScholarshipDetail(InternshipScholarshipsDetails InternshipScholarshipDetail)
        {
            try
            {
                InternshipScholarshipsDetailsBLL InternshipScholarshipDetailBLL = null;
                if (InternshipScholarshipDetail != null)
                {
                    InternshipScholarshipDetailBLL = new InternshipScholarshipsDetailsBLL()
                    {
                        InternshipScholarshipDetailID = InternshipScholarshipDetail.InternshipScholarshipDetailID,
                        InternshipScholarship = new BaseInternshipScholarshipsBLL().MapInternshipScholarship(InternshipScholarshipDetail.InternshipScholarships),
                        EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(InternshipScholarshipDetail.EmployeesCareersHistory),
                        IsPassengerOrderCompensation=InternshipScholarshipDetail.IsPassengerOrderCompensation
                    };
                }
                return InternshipScholarshipDetailBLL;
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
                return InternshipScholarshipDetailID;
            }
            set
            {
                InternshipScholarshipDetailID = value;
            }
        }
    }
}