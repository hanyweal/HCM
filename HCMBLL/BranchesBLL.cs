using HCMDAL;
using System.Collections.Generic;

namespace HCMBLL
{
    public class BranchesBLL
    {
        public int BranchID { get; set; }

        public string BranchName { get; set; }

        public List<BranchesBLL> GetBranches()
        {
            List<Branches> BranchesList = new BranchesDAL().GetBranches();
            List<BranchesBLL> BranchesListBLL = new List<BranchesBLL>();
            foreach (var item in BranchesList)
            {
                BranchesListBLL.Add(new BranchesBLL().MapBranch(item));
            }
            return BranchesListBLL;
        }

        internal BranchesBLL MapBranch(Branches Branch)
        {
            try
            {
                BranchesBLL BrancheBLL = null;
                if (Branch != null)
                {
                    BrancheBLL = new BranchesBLL()
                    {
                        BranchID = Branch.BranchID,
                        BranchName = Branch.BranchName
                    };
                }
                return BrancheBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
