using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class ScholarshipsDetailsBLL : CommonEntity
    {
        public virtual BaseScholarshipsBLL Scholarship { get; set; }

        public virtual int ScholarshipDetailID { get; set; }

        public virtual DateTime FromDate { get; set; }

        public virtual DateTime ToDate { get; set; }

        public virtual string Notes { get; set; }

        public virtual int ScholarshipPeriod
        {
            get
            {
                return this.ToDate.Subtract(this.FromDate).Days + 1;
            }
        }

        public virtual ScholarshipsActionsBLL ScholarshipAction { get; set; }

        public List<ScholarshipsDetailsBLL> GetScholarshipDetailsByScholarshipID(int ScholarshipID)
        {
            List<ScholarshipsDetailsBLL> ScholarshipsDetailsBLLList = new List<ScholarshipsDetailsBLL>();
            List<ScholarshipsDetails> ScholarshipsDetailsList = new ScholarshipsDetailsDAL().GetByScholarshipID(ScholarshipID);
            foreach (var item in ScholarshipsDetailsList)
            {
                ScholarshipsDetailsBLLList.Add(new ScholarshipsDetailsBLL().MapScholarshipDetail(item));
            }
            return ScholarshipsDetailsBLLList;
        }

        public ScholarshipsDetailsBLL GetScholarshipsDetailsByScholarshipDetailID(int ScholarshipDetailID)
        {
            ScholarshipsDetailsBLL ScholarshipDetailBLL = new ScholarshipsDetailsBLL();
            ScholarshipsDetails ScholarshipDetail = new ScholarshipsDetailsDAL().GetByScholarshipDetailID(ScholarshipDetailID);
            return new ScholarshipsDetailsBLL().MapScholarshipDetail(ScholarshipDetail);
        }

        public Result Remove(int ScholarshipDetailID)
        {
            Result result = null;
            ScholarshipsDetailsBLL ScholarshipDetail = this.GetScholarshipsDetailsByScholarshipDetailID(ScholarshipDetailID);

            new ScholarshipsDetailsDAL().Delete(ScholarshipDetailID, this.LoginIdentity.EmployeeCodeID);
            ScholarshipsDetailsBLL FirstScholarshipDetail = new ScholarshipsDetailsBLL().GetScholarshipDetailsByScholarshipID(ScholarshipDetail.Scholarship.ScholarshipID).FirstOrDefault();
            ScholarshipsDetailsBLL LastScholarshipDetail = new ScholarshipsDetailsBLL().GetScholarshipDetailsByScholarshipID(ScholarshipDetail.Scholarship.ScholarshipID).LastOrDefault();
            if (LastScholarshipDetail == null)
                new BaseScholarshipsBLL()
                {
                    ScholarshipID = ScholarshipDetail.Scholarship.ScholarshipID,
                    LoginIdentity = this.LoginIdentity,
                }.Remove(ScholarshipDetail.Scholarship.ScholarshipID);
            else
            {
                if (ScholarshipDetail.ScholarshipAction.ScholarshipActionID == (int)ScholarshipsActionsEnum.Joining)
                {
                    new BaseScholarshipsBLL()
                    {
                        ScholarshipID = LastScholarshipDetail.Scholarship.ScholarshipID,
                        ScholarshipStartDate = FirstScholarshipDetail.FromDate,
                        ScholarshipEndDate = LastScholarshipDetail.ToDate,
                        ScholarshipJoinDate = null,
                        IsPassed = null,
                    }.Update();
                }
                else if (ScholarshipDetail.ScholarshipAction.ScholarshipActionID == (int)ScholarshipsActionsEnum.Cancel)
                {
                    new BaseScholarshipsBLL()
                    {
                        ScholarshipID = LastScholarshipDetail.Scholarship.ScholarshipID,
                        ScholarshipStartDate = FirstScholarshipDetail.FromDate,
                        ScholarshipEndDate = LastScholarshipDetail.ToDate,
                        IsCanceled = false,
                    }.Update();
                }
                else
                {
                    new BaseScholarshipsBLL()
                    {
                        ScholarshipID = LastScholarshipDetail.Scholarship.ScholarshipID,
                        ScholarshipStartDate = FirstScholarshipDetail.FromDate,
                        ScholarshipEndDate = LastScholarshipDetail.ToDate,
                    }.Update();
                }
            }

            result = new Result();
            result.EnumType = typeof(VacationsValidationEnum);
            result.EnumMember = VacationsValidationEnum.Done.ToString();
            return result;
        }

        internal ScholarshipsDetailsBLL MapScholarshipDetail(ScholarshipsDetails ScholarshipDetail)
        {
            try
            {
                ScholarshipsDetailsBLL ScholarshipDetailBLL = null;
                if (ScholarshipDetail != null)
                {
                    ScholarshipDetailBLL = new ScholarshipsDetailsBLL()
                    {
                        Scholarship = ScholarshipDetail.Scholarships != null ? new BaseScholarshipsBLL().MapScholarship(ScholarshipDetail.Scholarships) : null,
                        ScholarshipDetailID = ScholarshipDetail.ScholarshipDetailID,
                        FromDate = ScholarshipDetail.FromDate,
                        ToDate = ScholarshipDetail.ToDate,
                        ScholarshipAction = new ScholarshipsActionsBLL()
                        {
                            ScholarshipActionID = ScholarshipDetail.ScholarshipActionID,
                            ScholarshipActionName = ScholarshipDetail.ScholarshipsActions.ScholarshipActionName
                        },
                        Notes = ScholarshipDetail.Notes,
                        CreatedDate = ScholarshipDetail.CreatedDate,
                        CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(ScholarshipDetail.CreatedByNav),
                        LastUpdatedDate = ScholarshipDetail.LastUpdatedDate,
                        LastUpdatedBy = new EmployeesCodesBLL().MapEmployeeCode(ScholarshipDetail.LastUpdatedByNav),
                    };
                }
                return ScholarshipDetailBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}