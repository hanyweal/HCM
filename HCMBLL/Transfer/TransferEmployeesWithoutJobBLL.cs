using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class TransferEmployeesWithoutJobBLL : BaseTransfersBLL
    {
        public virtual string JobName { get; set; }

        public virtual string RankName { get; set; }

        public virtual string JobCode { get; set; }

        public virtual string OrganizationName { get; set; }

        public virtual string CareerDegreeName { get; set; }

        public override Result Add()
        {
            try
            {
                Result result = base.Add();
                if (result != null)
                    return result;

                result = new Result();
                Transfers EmployeeTransfer = DALInstance;

                EmployeeTransfer.CareerDegreeName = this.CareerDegreeName;
                EmployeeTransfer.JobCode = this.JobCode;
                EmployeeTransfer.JobName = this.JobName;
                EmployeeTransfer.RankName = this.RankName;
                EmployeeTransfer.OrganizationName = this.OrganizationName;

                this.TransferID = new TransfersDAL().Insert(EmployeeTransfer);

                if (this.TransferID != 0)
                {
                    result.Entity = this;
                    result.EnumType = typeof(TransfersValidationEnum);
                    result.EnumMember = TransfersValidationEnum.Done.ToString();
                }
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public override Result Update()
        {
            try
            {
                Result result = base.Update();
                if (result != null)
                    return result;

                result = new Result();
                Transfers EmployeeTransfer = DALInstance;
             
                EmployeeTransfer.CareerDegreeName = this.CareerDegreeName;
                EmployeeTransfer.JobCode = this.JobCode;
                EmployeeTransfer.JobName = this.JobName;
                EmployeeTransfer.OrganizationName = this.OrganizationName;
                EmployeeTransfer.RankName = this.RankName;
                EmployeeTransfer.LastUpdatedDate = DateTime.Now;
                EmployeeTransfer.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;

                this.TransferID = new TransfersDAL().Update(EmployeeTransfer);
                if (this.TransferID != 0)
                {
                    result.Entity = this;
                    result.EnumType = typeof(TransfersValidationEnum);
                    result.EnumMember = TransfersValidationEnum.Done.ToString();
                }
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public override Result Remove(int TransferID)
        {
            return base.Remove(TransferID);
        }
    }
}
