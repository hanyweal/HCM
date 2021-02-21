﻿using HCMDAL;
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Configuration;
using HCMBLL.Enums;

namespace HCMBLL
{
    public class SMSLogsBLL : CommonEntity
    {
        public int SMSLogID { get; set; }

        public BusinessSubCategoriesEnum BusinssSubCategory { get; set; }

        public int DetailID { get; set; }

        public string Message { get; set; }

        public string MobileNo { get; set; }

        public virtual int Add()
        {
            try
            {
                SMSLogsDAL SMSLogDal = new SMSLogsDAL();
                SMSLogs SMSLog = new SMSLogs()
                {
                    Message = this.Message,
                    MobileNo = this.MobileNo,
                    BusinssSubCategoryID = (int)this.BusinssSubCategory,
                    DetailID = this.DetailID,
                    CreatedBy = this.CreatedBy.EmployeeCodeID,
                    CreatedDate = DateTime.Now
                };
                SMSLogDal.Insert(SMSLog);

                return this.SMSLogID;
            }
            catch
            {
                throw;
            }
        }

        internal SMSLogsBLL MapSMSLog(SMSLogs SMSLog)
        {
            try
            {
                SMSLogsBLL SMSLogBLL = null;
                if (SMSLog != null)
                {
                    SMSLogBLL = new SMSLogsBLL()
                    {
                        SMSLogID = SMSLog.SMSLogID,
                        BusinssSubCategory = (BusinessSubCategoriesEnum)SMSLog.BusinssSubCategoryID,
                        DetailID = DetailID,
                        Message = Message,
                        MobileNo = MobileNo,
                        CreatedDate = CreatedDate,
                        CreatedBy = CreatedBy
                    };
                }
                return SMSLogBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}

