using HCMDAL;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class PromotionsDecisionsBLL
    {
        public int PromotionDecisionID { get; set; }

        public string PromotionDecisionName { get; set; }

        public List<PromotionsDecisionsBLL> GetPromotionsDecisions()
        {
            List<PromotionsDecisions> PromotionDecisionList = new PromotionsDecisionsDAL().GetPromotionsDecisions();
            List<PromotionsDecisionsBLL> PromotionsDecisionsBLLList = new List<PromotionsDecisionsBLL>();
            if (PromotionDecisionList.Count > 0)
                foreach (var item in PromotionDecisionList)
                    PromotionsDecisionsBLLList.Add(MapPromotionDecision(item));

            return PromotionsDecisionsBLLList;
        }

        public PromotionsDecisionsBLL GetByPromotionDecisionID(int PromotionDecisionID)
        {
            return GetPromotionsDecisions().FirstOrDefault(x => x.PromotionDecisionID.Equals(PromotionDecisionID));
        }

        internal PromotionsDecisionsBLL MapPromotionDecision(PromotionsDecisions PromotionRecordStatus)
        {
            try
            {
                PromotionsDecisionsBLL PromotionsCandidatesAddedWayBLL = null;
                if (PromotionRecordStatus != null)
                {
                    PromotionsCandidatesAddedWayBLL = new PromotionsDecisionsBLL()
                    {
                        PromotionDecisionID = PromotionRecordStatus.PromotionDecisionID,
                        PromotionDecisionName = PromotionRecordStatus.PromotionDecisionName
                    };
                }
                return PromotionsCandidatesAddedWayBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}