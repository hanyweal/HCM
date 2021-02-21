using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CommonUtilities;

namespace HCMDAL
{
    public partial class HCMEntities
    {
        public HCMEntities(string temp)        // this arg created just to overload the constructor
             : base(ConfigurationManager.ConnectionStrings["HCMEntities"].ConnectionString)
        //: base(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SecurityEntities"].ConnectionString, "PSA"))
        {

        }

        public override int SaveChanges()
        {
            try
            {
                //AuditRecordsForChange(null);
                SaveChangeLog(null);
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveChanges(int EmployeeCodeID)
        {
            //AuditRecordsForChange(EmployeeCodeID);
            SaveChangeLog(EmployeeCodeID);
            return base.SaveChanges();
        }

        private void SaveChangeLog(int? EmployeeCodeID)
        {
            List<ChangeLogs> ChangeLogList = MapToChangeLog(new DBUtilities().AuditRecordsForChange(EmployeeCodeID, ChangeTracker.Entries().ToList(), ((IObjectContextAdapter)this).ObjectContext));
            if (ChangeLogList.Count > 0)
                ChangeLogs.AddRange(ChangeLogList);
        }

        private List<ChangeLogs> MapToChangeLog(List<CommonUtilities.ChangeLogs> list)
        {
            ChangeLogs ChangeLog = new ChangeLogs();
            ChangeDetailsLogs ChangeDetailLog = new ChangeDetailsLogs();

            List<ChangeLogs> ChangeLogList = new List<ChangeLogs>();
            List<ChangeDetailsLogs> ChangeDetailLogList = new List<ChangeDetailsLogs>();

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.EntityName))
                {
                    ChangeLog = new ChangeLogs();
                    ChangeLog.EntityName = item.EntityName;
                    ChangeLog.DateChange = item.DateChange;
                    ChangeLog.EventTypeID = item.EventTypeID;
                    ChangeLog.EmployeeCodeID = item.EmployeeCodeID;

                    ChangeDetailLogList = new List<ChangeDetailsLogs>();
                    foreach (var detail in item.ChangeDetailsLogs)
                    {
                        ChangeDetailLog = new ChangeDetailsLogs();
                        ChangeDetailLog.PropertyName = detail.PropertyName;
                        ChangeDetailLog.PrimaryKeyValue = detail.PrimaryKeyValue;
                        ChangeDetailLog.OldValue = detail.OldValue;
                        ChangeDetailLog.NewValue = detail.NewValue;

                        ChangeDetailLogList.Add(ChangeDetailLog);
                    }
                    ChangeLog.ChangeDetailsLogs = ChangeDetailLogList;
                    ChangeLogList.Add(ChangeLog);
                }
            }

            return ChangeLogList;
        }
    }
}
