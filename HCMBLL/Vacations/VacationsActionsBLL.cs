using HCMBLL.Enums;
using HCMDAL;
using PSADTO;
using PSADTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class VacationsActionsBLL
    {
        public int VacationActionID { get; set; }

        public string VacationActionName { get; set; }

        public List<VacationsActionsBLL> GetVacationsActions()
        {
            try
            {
                List<VacationsActions> VacationsActionsList = new VacationsActionsDAL().GetVacationsActions();
                List<VacationsActionsBLL> VacationsActionsBLLList = new List<VacationsActionsBLL>();

                foreach (var item in VacationsActionsList)
                    VacationsActionsBLLList.Add(new VacationsActionsBLL() { VacationActionID = item.VacationActionID, VacationActionName = item.VacationActionName });

                return VacationsActionsBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VacationsActionsBLL> GetVacationsActions(AuthenticationResult Authentication)
        {
            try
            {
                List<VacationsActions> VacationsActionsList = new VacationsActionsDAL().GetVacationsActions();
                List<VacationsActionsBLL> VacationsActionsBLLList = new List<VacationsActionsBLL>();

                if (Authentication != null && Authentication.User != null && Authentication.User.IsAdmin)
                {
                    foreach (var item in VacationsActionsList)
                        VacationsActionsBLLList.Add(new VacationsActionsBLL() { VacationActionID = item.VacationActionID, VacationActionName = item.VacationActionName });
                }
                else
                {
                    foreach (var item in VacationsActionsList)
                    {
                        GroupsObjects Privilage = Authentication.Privilages.FirstOrDefault(e => e.Object.ObjectID == (int)ObjectsEnum.HCMVacations);

                        if (Privilage != null)
                        {
                            if (Privilage.Creating && item.VacationActionID == (int)VacationsActionsEnum.Add)
                                VacationsActionsBLLList.Add(new VacationsActionsBLL() { VacationActionID = item.VacationActionID, VacationActionName = item.VacationActionName });
                            else if (Privilage.Updating && (item.VacationActionID == (int)VacationsActionsEnum.Extend || item.VacationActionID == (int)VacationsActionsEnum.Break))
                                VacationsActionsBLLList.Add(new VacationsActionsBLL() { VacationActionID = item.VacationActionID, VacationActionName = item.VacationActionName });
                            else if (Privilage.Deleting && item.VacationActionID == (int)VacationsActionsEnum.Cancel)
                                VacationsActionsBLLList.Add(new VacationsActionsBLL() { VacationActionID = item.VacationActionID, VacationActionName = item.VacationActionName });
                        }
                    }
                }

                return VacationsActionsBLLList;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
