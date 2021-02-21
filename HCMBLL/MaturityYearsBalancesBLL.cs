using HCMDAL;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class MaturityYearsBalancesBLL : CommonEntity
    {
        public int MaturityYearID { get; set; }

        public int MaturityYear { get; set; }

        public int Balance { get; set; }

        public List<MaturityYearsBalancesBLL> GetMaturityYearsBalances()
        {
            List<MaturityYearsBalances> MaturityYearsBalancesList = new MaturityYearsBalancesDAL().GetMaturityYearsBalances();
            List<MaturityYearsBalancesBLL> MaturityYearsBalancesBLLList = new List<MaturityYearsBalancesBLL>();
            if (MaturityYearsBalancesList.Count > 0)
            {
                foreach (var item in MaturityYearsBalancesList)
                {
                    MaturityYearsBalancesBLLList.Add(new MaturityYearsBalancesBLL()
                    {
                        MaturityYearID = item.MaturityYearID,
                        MaturityYear = item.MaturityYear,
                        Balance = item.Balance
                    });
                }
            }

            return MaturityYearsBalancesBLLList;
        }

        public MaturityYearsBalancesBLL GetByMaturityYearID(int MaturityYearID)
        {
            return GetMaturityYearsBalances().SingleOrDefault(x => x.MaturityYearID.Equals(MaturityYearID));
        }

        internal MaturityYearsBalancesBLL MapMaturityYearBalance(MaturityYearsBalances item)
        {
            return item != null ?
                new MaturityYearsBalancesBLL()
                {
                    MaturityYearID = item.MaturityYearID,
                    MaturityYear = item.MaturityYear,
                    Balance = item.Balance
                }
                : null;
        }
    }
}
