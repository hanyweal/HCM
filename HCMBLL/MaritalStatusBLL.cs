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
    public class MaritalStatusBLL : CommonEntity
    {
        public int MaritalStatusID
        {
            get;
            set;
        }

        public string MaritalStatusName
        {
            get;
            set;
        }

        public int Add()
        {
            throw new System.NotImplementedException();
        }

        public int Update()
        {
            throw new System.NotImplementedException();
        }

        public int Remove()
        {
            throw new System.NotImplementedException();
        }

        public List<MaritalStatusBLL> GetMaritalStatus()
        {
            List<MaritalStatus> MaritalStatusList = new MaritalStatusDAL().GetMaritalStatus();
            List<MaritalStatusBLL> MaritalStatusBLLList = new List<MaritalStatusBLL>();
            if (MaritalStatusList.Count > 0)
            {
                foreach (var item in MaritalStatusList)
                {
                    MaritalStatusBLLList.Add(new MaritalStatusBLL()
                    {
                        MaritalStatusID = item.MaritalStatusID,
                        MaritalStatusName = item.MaritalStatusName
                    });
                }
            }

            return MaritalStatusBLLList;
        }

        public MaritalStatusBLL GetByMaritalStatusID(int MaritalStatusID)
        {
            return GetMaritalStatus().SingleOrDefault(x => x.MaritalStatusID.Equals(MaritalStatusID));
        }

        internal MaritalStatusBLL MapMaritalStatus(MaritalStatus MaritalStatus)
        {
            try
            {
                MaritalStatusBLL MaritalStatusBLL = null;
                if (MaritalStatus != null)
                {
                    MaritalStatusBLL = new MaritalStatusBLL()
                    {
                        MaritalStatusID = MaritalStatus.MaritalStatusID,
                        MaritalStatusName = MaritalStatus.MaritalStatusName
                    };
                }
                return MaritalStatusBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}