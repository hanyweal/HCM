using HCMDAL;
using System.Collections.Generic;

namespace HCMBLL
{
    public partial class PromotionsRecordsEmployeesEvaluationsDetailsBLL : CommonEntity, IEntity
    {
        public int PromotionRecordEmployeeEvaluationDetailID { get; set; }

        public PromotionsRecordsEmployeesBLL PromotionRecordEmployee { get; set; }

        public int EvaluationYear { get; set; }

        public string Evaluation { get; set; }

        public decimal? Points { get; set; }

        public List<PromotionsRecordsEmployeesEvaluationsDetailsBLL> GetPromotionsRecordsEmployeesEvaluationsDetails()
        {
            List<PromotionsRecordsEmployeesEvaluationsDetails> PromotionsRecordsEmployeesEvaluationsDetailsList = new PromotionsRecordsEmployeesEvaluationsDetailsDAL().GetPromotionsRecordsEmployeesEvaluationsDetails();
            List<PromotionsRecordsEmployeesEvaluationsDetailsBLL> PromotionsRecordsEmployeesEvaluationsDetailsBLLList = new List<PromotionsRecordsEmployeesEvaluationsDetailsBLL>();
            if (PromotionsRecordsEmployeesEvaluationsDetailsList.Count > 0)
                foreach (var item in PromotionsRecordsEmployeesEvaluationsDetailsList)
                    PromotionsRecordsEmployeesEvaluationsDetailsBLLList.Add(new PromotionsRecordsEmployeesEvaluationsDetailsBLL().MapPromotionRecordEmployeeEvaluationDetail(item));

            return PromotionsRecordsEmployeesEvaluationsDetailsBLLList;
        }

        public List<PromotionsRecordsEmployeesEvaluationsDetailsBLL> GetByPromotionRecordEmployeeID(int PromotionRecordEmployeeID)
        {
            try
            {
                List<PromotionsRecordsEmployeesEvaluationsDetails> PromotionsRecordsEmployeesEvaluationsDetailsList =
                    new PromotionsRecordsEmployeesEvaluationsDetailsDAL().GetByPromotionRecordEmployeeID(PromotionRecordEmployeeID);
                List<PromotionsRecordsEmployeesEvaluationsDetailsBLL> PromotionsRecordsEmployeesEvaluationsDetailsBLLList = new List<PromotionsRecordsEmployeesEvaluationsDetailsBLL>();
                if (PromotionsRecordsEmployeesEvaluationsDetailsList.Count > 0)
                    foreach (var item in PromotionsRecordsEmployeesEvaluationsDetailsList)
                        PromotionsRecordsEmployeesEvaluationsDetailsBLLList.Add(new PromotionsRecordsEmployeesEvaluationsDetailsBLL().MapPromotionRecordEmployeeEvaluationDetail(item));

                return PromotionsRecordsEmployeesEvaluationsDetailsBLLList;
            }
            catch
            {
                throw;
            }
        }

        internal PromotionsRecordsEmployeesEvaluationsDetailsBLL MapPromotionRecordEmployeeEvaluationDetail(PromotionsRecordsEmployeesEvaluationsDetails item)
        {
            try
            {
                PromotionsRecordsEmployeesEvaluationsDetailsBLL PromotionRecordEmployeeEvaluationDetail = null;
                if (item != null)
                {
                    PromotionRecordEmployeeEvaluationDetail = new PromotionsRecordsEmployeesEvaluationsDetailsBLL();
                    PromotionRecordEmployeeEvaluationDetail.PromotionRecordEmployeeEvaluationDetailID = item.PromotionRecordEmployeeEvaluationDetailID;
                    PromotionRecordEmployeeEvaluationDetail.PromotionRecordEmployee = new PromotionsRecordsEmployeesBLL().MapPromotionRecordEmployee(item.PromotionsRecordsEmployees);
                    PromotionRecordEmployeeEvaluationDetail.EvaluationYear = item.EvaluationYear;
                    PromotionRecordEmployeeEvaluationDetail.Evaluation = item.Evaluation;
                    PromotionRecordEmployeeEvaluationDetail.Points = item.Points;
                }
                return PromotionRecordEmployeeEvaluationDetail;
            }
            catch
            {
                throw;
            }
        }
    }
}