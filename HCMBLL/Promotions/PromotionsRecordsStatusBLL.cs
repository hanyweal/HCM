﻿using HCMDAL;
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class PromotionsRecordsStatusBLL : CommonEntity, IEntity
    {
        public int PromotionRecordStatusID { get; set; }

        public string PromotionRecordStatusName { get; set; }

        public List<PromotionsRecordsStatusBLL> GetPromotionsRecordsStatus()
        {
            List<PromotionsRecordsStatus> PromotionsRecordsStatusList = new PromotionsRecordsStatusDAL().GetPromotionsRecordsStatus();
            List<PromotionsRecordsStatusBLL> PromotionsRecordsStatusBLLList = new List<PromotionsRecordsStatusBLL>();
            if (PromotionsRecordsStatusList.Count > 0)
            {
                foreach (var item in PromotionsRecordsStatusList)
                {
                    PromotionsRecordsStatusBLLList.Add(new PromotionsRecordsStatusBLL()
                    {
                        PromotionRecordStatusID = item.PromotionRecordStatusID,
                        PromotionRecordStatusName = item.PromotionRecordStatusName
                    });
                }
            }

            return PromotionsRecordsStatusBLLList;
        }

        public PromotionsRecordsStatusBLL GetByPromotionRecordStatusID(int PromotionRecordStatusID)
        {
            return GetPromotionsRecordsStatus().SingleOrDefault(x => x.PromotionRecordStatusID.Equals(PromotionRecordStatusID));
        }

        internal PromotionsRecordsStatusBLL MapPromotionRecordStatus(PromotionsRecordsStatus PromotionRecordStatus)
        {
            try
            {
                PromotionsRecordsStatusBLL PromotionRecordStatusBLL = null;
                if (PromotionRecordStatus != null)
                {
                    PromotionRecordStatusBLL = new PromotionsRecordsStatusBLL()
                    {
                        PromotionRecordStatusID = PromotionRecordStatus.PromotionRecordStatusID,
                        PromotionRecordStatusName = PromotionRecordStatus.PromotionRecordStatusName
                    };
                }
                return PromotionRecordStatusBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}