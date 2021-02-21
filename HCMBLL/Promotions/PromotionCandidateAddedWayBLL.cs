using HCMDAL;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class PromotionCandidateAddedWayBLL
    {
        public int PromotionCandidateAddedWayID { get; set; }

        public string PromotionCandidateAddedWayName { get; set; }

        public List<PromotionCandidateAddedWayBLL> GetPromotionCandidateAddedWay()
        {
            List<PromotionsCandidatesAddedWays> PromotionCandidateAddedWayList = new PromotionsCandidatesAddedWaysDAL().GetPromotionsCandidatesAddedWays();
            List<PromotionCandidateAddedWayBLL> PromotionCandidateAddedWayBLLList = new List<PromotionCandidateAddedWayBLL>();
            if (PromotionCandidateAddedWayList.Count > 0)
            {
                foreach (var item in PromotionCandidateAddedWayList)
                {
                    PromotionCandidateAddedWayBLLList.Add(new PromotionCandidateAddedWayBLL()
                    {
                        PromotionCandidateAddedWayID = item.PromotionCandidateAddedWayID,
                        PromotionCandidateAddedWayName = item.PromotionCandidateAddedWayName
                    });
                }
            }

            return PromotionCandidateAddedWayBLLList;
        }

        public PromotionCandidateAddedWayBLL GetByPromotionCandidateAddedWayID(int PromotionCandidateAddedWayID)
        {
            return GetPromotionCandidateAddedWay().FirstOrDefault(x => x.PromotionCandidateAddedWayID.Equals(PromotionCandidateAddedWayID));
        }

        internal PromotionCandidateAddedWayBLL MapPromotionCandidateAddedWay(PromotionsCandidatesAddedWays PromotionRecordStatus)
        {
            try
            {
                PromotionCandidateAddedWayBLL PromotionsCandidatesAddedWayBLL = null;
                if (PromotionRecordStatus != null)
                {
                    PromotionsCandidatesAddedWayBLL = new PromotionCandidateAddedWayBLL()
                    {
                        PromotionCandidateAddedWayID = PromotionRecordStatus.PromotionCandidateAddedWayID,
                        PromotionCandidateAddedWayName = PromotionRecordStatus.PromotionCandidateAddedWayName
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