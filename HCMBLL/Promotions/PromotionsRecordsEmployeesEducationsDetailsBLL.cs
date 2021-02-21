using HCMDAL;
using System.Collections.Generic;

namespace HCMBLL
{
    public partial class PromotionsRecordsEmployeesEducationsDetailsBLL : CommonEntity, IEntity
    {
        public int PromotionRecordEmployeeEducationDetailID { get; set; }

        public PromotionsRecordsEmployeesBLL PromotionRecordEmployee { get; set; }

        public QualificationsDegreesBLL QualificationDegree { get; set; }

        public QualificationsBLL Qualification { get; set; }

        public GeneralSpecializationsBLL GeneralSpecialization { get; set; }

        public int Weight { get; set; }

        public decimal? Points { get; set; }

        public bool IsIncluded { get; set; }

        public List<PromotionsRecordsEmployeesEducationsDetailsBLL> GetPromotionsRecordsEmployeesEducationsDetails()
        {
            List<PromotionsRecordsEmployeesEducationsDetails> PromotionsRecordsEmployeesEducationsDetailsList = new PromotionsRecordsEmployeesEducationsDetailsDAL().GetPromotionsRecordsEmployeesEducationsDetails();
            List<PromotionsRecordsEmployeesEducationsDetailsBLL> PromotionsRecordsEmployeesEducationsDetailsBLLList = new List<PromotionsRecordsEmployeesEducationsDetailsBLL>();
            if (PromotionsRecordsEmployeesEducationsDetailsList.Count > 0)
                foreach (var item in PromotionsRecordsEmployeesEducationsDetailsList)
                    PromotionsRecordsEmployeesEducationsDetailsBLLList.Add(new PromotionsRecordsEmployeesEducationsDetailsBLL().MapPromotionRecordEmployeeEducationDetail(item));

            return PromotionsRecordsEmployeesEducationsDetailsBLLList;
        }

        public List<PromotionsRecordsEmployeesEducationsDetailsBLL> GetByPromotionRecordEmployeeID(int PromotionRecordEmployeeID)
        {
            try
            {
                List<PromotionsRecordsEmployeesEducationsDetails> PromotionsRecordsEmployeesEducationsDetailsList =
                    new PromotionsRecordsEmployeesEducationsDetailsDAL().GetByPromotionRecordEmployeeID(PromotionRecordEmployeeID);
                List<PromotionsRecordsEmployeesEducationsDetailsBLL> PromotionsRecordsEmployeesEducationsDetailsBLLList = new List<PromotionsRecordsEmployeesEducationsDetailsBLL>();
                if (PromotionsRecordsEmployeesEducationsDetailsList.Count > 0)
                    foreach (var item in PromotionsRecordsEmployeesEducationsDetailsList)
                        PromotionsRecordsEmployeesEducationsDetailsBLLList.Add(new PromotionsRecordsEmployeesEducationsDetailsBLL().MapPromotionRecordEmployeeEducationDetail(item));

                return PromotionsRecordsEmployeesEducationsDetailsBLLList;
            }
            catch
            {
                throw;
            }
        }

        internal PromotionsRecordsEmployeesEducationsDetailsBLL MapPromotionRecordEmployeeEducationDetail(PromotionsRecordsEmployeesEducationsDetails item)
        {
            try
            {
                PromotionsRecordsEmployeesEducationsDetailsBLL PromotionRecordEmployeeEducationDetail = null;
                if (item != null)
                {
                    PromotionRecordEmployeeEducationDetail = new PromotionsRecordsEmployeesEducationsDetailsBLL();
                    PromotionRecordEmployeeEducationDetail.PromotionRecordEmployeeEducationDetailID = item.PromotionRecordEmployeeEducationDetailID;
                    PromotionRecordEmployeeEducationDetail.PromotionRecordEmployee = new PromotionsRecordsEmployeesBLL().MapPromotionRecordEmployee(item.PromotionsRecordsEmployees);
                    PromotionRecordEmployeeEducationDetail.QualificationDegree = new QualificationsDegreesBLL().MapQualificationDegree(item.QualificationsDegrees);
                    PromotionRecordEmployeeEducationDetail.Qualification = new QualificationsBLL().MapQualification(item.Qualifications);
                    PromotionRecordEmployeeEducationDetail.GeneralSpecialization = new GeneralSpecializationsBLL().MapGeneralSpecialization(item.GeneralSpecializations);
                    PromotionRecordEmployeeEducationDetail.Points = item.Points;
                    PromotionRecordEmployeeEducationDetail.Weight = item.Weight;
                    PromotionRecordEmployeeEducationDetail.IsIncluded = item.IsIncluded;
                }
                return PromotionRecordEmployeeEducationDetail;
            }
            catch
            {
                throw;
            }
        }
    }
}