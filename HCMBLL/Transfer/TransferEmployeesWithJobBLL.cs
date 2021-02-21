using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class TransferEmployeesWithJobBLL : BaseTransfersBLL
    {
        public override Result Add()
        {
            try
            {
                Result result = base.Add();
                if (result != null)
                    return result;

                result = new Result();
                Transfers EmployeeTransfer = DALInstance;
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
                // EmployeeTransfer.TransferID = this.TransferID;
                
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
            catch (Exception ex)
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
