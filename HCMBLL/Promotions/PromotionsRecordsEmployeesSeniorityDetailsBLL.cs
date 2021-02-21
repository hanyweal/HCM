using HCMDAL;
using System.Collections.Generic;

namespace HCMBLL
{
    public partial class PromotionsRecordsEmployeesSeniorityDetailsBLL : CommonEntity, IEntity
    {
        public int PromotionRecordEmployeeSeniorityDetailID { get; set; }

        public PromotionsRecordsEmployeesBLL PromotionRecordEmployee { get; set; }

        public int Months { get; set; }

        public decimal? Points { get; set; }

        public List<PromotionsRecordsEmployeesSeniorityDetailsBLL> GetPromotionsRecordsEmployeesSeniorityDetails()
        {
            List<PromotionsRecordsEmployeesSeniorityDetails> PromotionsRecordsEmployeesSeniorityDetailsList = new PromotionsRecordsEmployeesSeniorityDetailsDAL().GetPromotionsRecordsEmployeesSeniorityDetails();
            List<PromotionsRecordsEmployeesSeniorityDetailsBLL> PromotionsRecordsEmployeesSeniorityDetailsBLLList = new List<PromotionsRecordsEmployeesSeniorityDetailsBLL>();
            if (PromotionsRecordsEmployeesSeniorityDetailsList.Count > 0)
                foreach (var item in PromotionsRecordsEmployeesSeniorityDetailsList)
                    PromotionsRecordsEmployeesSeniorityDetailsBLLList.Add(new PromotionsRecordsEmployeesSeniorityDetailsBLL().MapPromotionRecordEmployeeSeniorityDetail(item));

            return PromotionsRecordsEmployeesSeniorityDetailsBLLList;
        }

        public List<PromotionsRecordsEmployeesSeniorityDetailsBLL> GetByPromotionRecordEmployeeID(int GetByPromotionRecordEmployeeID)
        {
            try
            {
                List<PromotionsRecordsEmployeesSeniorityDetails> PromotionsRecordsEmployeesSeniorityDetailsList =
                    new PromotionsRecordsEmployeesSeniorityDetailsDAL().GetByPromotionRecordEmployeeID(GetByPromotionRecordEmployeeID);
                List<PromotionsRecordsEmployeesSeniorityDetailsBLL> PromotionsRecordsEmployeesSeniorityDetailsBLLList = new List<PromotionsRecordsEmployeesSeniorityDetailsBLL>();
                if (PromotionsRecordsEmployeesSeniorityDetailsList.Count > 0)
                    foreach (var item in PromotionsRecordsEmployeesSeniorityDetailsList)
                        PromotionsRecordsEmployeesSeniorityDetailsBLLList.Add(new PromotionsRecordsEmployeesSeniorityDetailsBLL().MapPromotionRecordEmployeeSeniorityDetail(item));

                return PromotionsRecordsEmployeesSeniorityDetailsBLLList;
            }
            catch
            {
                throw;
            }
        }

        internal PromotionsRecordsEmployeesSeniorityDetailsBLL MapPromotionRecordEmployeeSeniorityDetail(PromotionsRecordsEmployeesSeniorityDetails item)
        {
            try
            {
                PromotionsRecordsEmployeesSeniorityDetailsBLL PromotionRecordEmployeeSeniorityDetail = null;
                if (item != null)
                {
                    PromotionRecordEmployeeSeniorityDetail = new PromotionsRecordsEmployeesSeniorityDetailsBLL();
                    PromotionRecordEmployeeSeniorityDetail.PromotionRecordEmployeeSeniorityDetailID = item.PromotionRecordEmployeeSeniorityDetailID;
                    PromotionRecordEmployeeSeniorityDetail.PromotionRecordEmployee = new PromotionsRecordsEmployeesBLL().MapPromotionRecordEmployee(item.PromotionsRecordsEmployees);
                    PromotionRecordEmployeeSeniorityDetail.Months = item.Months;
                    PromotionRecordEmployeeSeniorityDetail.Points = item.Points;
                }
                return PromotionRecordEmployeeSeniorityDetail;
            }
            catch
            {
                throw;
            }
        }
    }
}