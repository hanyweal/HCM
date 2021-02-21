using HCMBLL.Enums;
using HCMDAL;

namespace HCMBLL
{
    public class EmployeesVacationsOpeningBalancesBLL : IEntity
    {
        public int EmployeeVacationOpeningBalanceID { get; set; }

        public EmployeesCodesBLL EmployeeCode { get; set; }

        public VacationsTypesBLL VacationType { get; set; }

        public int OpeningBalance { get; set; }

        public virtual Result Add()
        {
            Result result = new Result();
            EmployeesVacationsOpeningBalances EmployeeVacationOpeningBalance = new EmployeesVacationsOpeningBalances();
            EmployeeVacationOpeningBalance.EmployeeCodeID = this.EmployeeCode.EmployeeCodeID;
            EmployeeVacationOpeningBalance.OpeningBalance = this.OpeningBalance;
            EmployeeVacationOpeningBalance.VacationTypeID = this.VacationType.VacationTypeID;
            this.EmployeeVacationOpeningBalanceID = new EmployeesVacationsOpeningBalancesDAL().Insert(EmployeeVacationOpeningBalance);
            if (this.EmployeeVacationOpeningBalanceID != 0)
            {
                result.Entity = this;
                result.EnumType = typeof(EmployeesVacationsOpeningBalancesValidationEnum);
                result.EnumMember = EmployeesVacationsOpeningBalancesValidationEnum.Done.ToString();
            }
            return result;
        }

        public virtual Result Update()
        {
            try
            {
                Result result = new Result();
                EmployeesVacationsOpeningBalances EmployeesVacationsOpeningBalances = new EmployeesVacationsOpeningBalancesDAL().GetByEmployeeVacationOpeningBalanceID(this.EmployeeVacationOpeningBalanceID);
                EmployeesVacationsOpeningBalances.OpeningBalance = this.OpeningBalance;
                this.EmployeeVacationOpeningBalanceID = new EmployeesVacationsOpeningBalancesDAL().Update(EmployeesVacationsOpeningBalances);
                if (this.EmployeeVacationOpeningBalanceID != 0)
                {
                    result.Entity = this;
                    result.EnumType = typeof(EmployeesVacationsOpeningBalancesValidationEnum);
                    result.EnumMember = EmployeesVacationsOpeningBalancesValidationEnum.Done.ToString();
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public EmployeesVacationsOpeningBalancesBLL GetByEmployeeVacationOpeningBalanceID(int EmployeeVacationOpeningBalanceID)
        {
            EmployeesVacationsOpeningBalancesBLL EmployeesVacationsOpeningBalancesBLL = null;
            EmployeesVacationsOpeningBalances EmployeesVacationsOpeningBalances = new EmployeesVacationsOpeningBalancesDAL().GetByEmployeeVacationOpeningBalanceID(EmployeeVacationOpeningBalanceID);
            if (EmployeesVacationsOpeningBalances != null)
            {
                EmployeesVacationsOpeningBalancesBLL = new EmployeesVacationsOpeningBalancesBLL().MapEmployeesVacationsOpeningBalances(EmployeesVacationsOpeningBalances);
            }
            return EmployeesVacationsOpeningBalancesBLL;
        }

        public EmployeesVacationsOpeningBalancesBLL GetByEmployeeCodeID(int EmployeeCodeID)
        {
            EmployeesVacationsOpeningBalancesBLL EmployeesVacationsOpeningBalancesBLL = null;
            EmployeesVacationsOpeningBalances EmployeesVacationsOpeningBalances = new EmployeesVacationsOpeningBalancesDAL().GetByEmployeeCodeID(EmployeeCodeID);
            if (EmployeesVacationsOpeningBalances != null)
            {
                EmployeesVacationsOpeningBalancesBLL = new EmployeesVacationsOpeningBalancesBLL().MapEmployeesVacationsOpeningBalances(EmployeesVacationsOpeningBalances);
            }
            return EmployeesVacationsOpeningBalancesBLL;
        }

        internal EmployeesVacationsOpeningBalancesBLL MapEmployeesVacationsOpeningBalances(EmployeesVacationsOpeningBalances item)
        {
            return item != null ?
                new EmployeesVacationsOpeningBalancesBLL()
                {
                    EmployeeVacationOpeningBalanceID = item.EmployeeVacationOpeningBalanceID,
                    OpeningBalance = item.OpeningBalance,
                    EmployeeCode = new EmployeesCodesBLL().MapEmployeeCode(item.EmployeesCodes),
                    VacationType = new VacationsTypesBLL().MapVacationsTypes(item.VacationsTypes)
                }
                : null;
        }
    }
}
