using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class PassengerOrdersEscortsBLL:CommonEntity
    {
        public int PassengerOrdersEscortID { get; set; }
        public PassengerOrdersBLL PassengerOrder { get; set; }
        public string EscortName { get; set; }
        public string EscortIDNo { get; set; }
        public string EscortAge { get; set; }
        public string EscortRelativeRelation { get; set; }

        public List<PassengerOrdersEscortsBLL> GetPassengerOrdersEscortsByPassengerOrderID(int PassengerOrderID)
        {
            List<PassengerOrdersEscorts> PassengerOrdersEscortsList = new PassengerOrdersEscortsDAL().GetPassengerOrdersEscortsByPassengerOrderID(PassengerOrderID);
            List<PassengerOrdersEscortsBLL> PassengerOrdersEscortsBLLList = new List<PassengerOrdersEscortsBLL>();
            if (PassengerOrdersEscortsList.Count > 0)
            {
                foreach (var item in PassengerOrdersEscortsList)
                {
                    PassengerOrdersEscortsBLLList.Add(new PassengerOrdersEscortsBLL()
                    {
                        PassengerOrdersEscortID = item.PassengerOrderEscortID,
                        PassengerOrder = new PassengerOrdersBLL().GetByPassengerOrderID(item.PassengerOrderID),
                        EscortName = item.EscortName,
                        EscortIDNo = item.EscortIDNo,
                        EscortAge = item.EscortAge,
                        EscortRelativeRelation = item.EscortRelativeRelation
                    });
                }
            }

            return PassengerOrdersEscortsBLLList;
        }

        public Result Add(PassengerOrdersEscortsBLL EscortBLL)
        {
            Result result = new Result();

            //// validate location already exists
            PassengerOrdersEscorts Escort = new PassengerOrdersEscortsDAL()
                .GetPassengerOrdersEscortsByPassengerOrderID(EscortBLL.PassengerOrder.PassengerOrderID)
                .Where(e => e.EscortName.ToUpper().Equals(EscortBLL.EscortName.ToUpper())
                            && e.EscortIDNo.ToUpper().Equals(EscortBLL.EscortIDNo.ToUpper())
                            && e.EscortAge.ToUpper().Equals(EscortBLL.EscortAge.ToUpper())
                            && e.EscortRelativeRelation.ToUpper().Equals(EscortBLL.EscortRelativeRelation.ToUpper())).FirstOrDefault();
            if (Escort != null)
            { 
                result.Entity = null;
                result.EnumType = typeof(PassengerOrdersValidationEnum);
                result.EnumMember = PassengerOrdersValidationEnum.RejectedBecausePassengerOrderEscortAlreadyExist.ToString();

                return result;
            }

            result = new Result();
            PassengerOrdersEscorts Obj = new PassengerOrdersEscorts()
            {
                PassengerOrderID = EscortBLL.PassengerOrder.PassengerOrderID,
                EscortName = EscortBLL.EscortName,
                EscortIDNo = EscortBLL.EscortIDNo,
                EscortAge = EscortBLL.EscortAge,
                EscortRelativeRelation = EscortBLL.EscortRelativeRelation,
                CreatedDate = DateTime.Now,
            };

            this.PassengerOrdersEscortID = new PassengerOrdersEscortsDAL().Insert(Obj, EscortBLL.LoginIdentity.EmployeeCodeID);
            if (this.PassengerOrdersEscortID != 0)
            {
                result.Entity = null;
                result.EnumType = typeof(PassengerOrdersValidationEnum);
                result.EnumMember = PassengerOrdersValidationEnum.Done.ToString();
            }
            return result;
        }

        public Result Remove(int PassengerOrdersEscortID)
        {
            try
            {
                Result result = null;
                new PassengerOrdersEscortsDAL().Delete(PassengerOrdersEscortID,this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(PassengerOrdersValidationEnum),
                    EnumMember = PassengerOrdersValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }
    }
}