using HCMDAL;
using System.Collections.Generic;

namespace HCMBLL
{
    public class PromotionsRecordsActionsTypesBLL : CommonEntity
    {
        public int PromotionActionTypeID
        {
            get;
            set;
        }

        public string PromotionActionTypeName
        {
            get;
            set;
        }

        public List<PromotionsRecordsActionsTypesBLL> GetPromotionsRecordsActionsTypes()
        {
            List<PromotionsRecordsActionsTypes> PromotionsRecordsActionsTypesList = new PromotionsRecordsActionsTypesDAL().GetPromotionsRecordsActionsTypes();
            List<PromotionsRecordsActionsTypesBLL> PromotionsRecordsActionsTypesBLLList = new List<PromotionsRecordsActionsTypesBLL>();
            if (PromotionsRecordsActionsTypesList.Count > 0)
            {
                foreach (var item in PromotionsRecordsActionsTypesList)
                {
                    PromotionsRecordsActionsTypesBLLList.Add(MapPromotionActionType(item));
                }
            }

            return PromotionsRecordsActionsTypesBLLList;
        }

        internal PromotionsRecordsActionsTypesBLL MapPromotionActionType(PromotionsRecordsActionsTypes item)
        {
            return item != null ?
                new PromotionsRecordsActionsTypesBLL()
                {
                    PromotionActionTypeID = item.PromotionActionTypeID,
                    PromotionActionTypeName = item.PromotionActionTypeName
                }
                : null;
        }
    }
}