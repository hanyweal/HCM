using HCMDAL;
using System;
using System.Collections.Generic;

namespace HCMBLL
{
    public class ScholarshipsActionsBLL
    {
        public int ScholarshipActionID { get; set; }

        public string ScholarshipActionName { get; set; }

        public List<ScholarshipsActionsBLL> GetScholarshipsActions()
        {
            try
            {
                List<ScholarshipsActions> ScholarshipsActionsList = new ScholarshipsActionsDAL().GetScholarshipsActions();
                List<ScholarshipsActionsBLL> ScholarshipsActionsBLLList = new List<ScholarshipsActionsBLL>();
                foreach (var item in ScholarshipsActionsList)
                {
                    ScholarshipsActionsBLLList.Add(new ScholarshipsActionsBLL() { ScholarshipActionID = item.ScholarshipActionID, ScholarshipActionName = item.ScholarshipActionName });
                }
                return ScholarshipsActionsBLLList;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
