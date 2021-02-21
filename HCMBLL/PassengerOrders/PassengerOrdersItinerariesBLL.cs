using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class PassengerOrdersItinerariesBLL:CommonEntity
    {
        public int PassengerOrdersItineraryID { get; set; }
        public PassengerOrdersBLL PassengerOrder { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }

        public List<PassengerOrdersItinerariesBLL> GetPassengerOrdersItinerariesByPassengerOrderID(int PassengerOrderID)
        {
            List<PassengerOrdersItineraries> PassengerOrdersItinerariesList = new PassengerOrdersItinerariesDAL().GetPassengerOrdersItinerariesByPassengerOrderID(PassengerOrderID);
            List<PassengerOrdersItinerariesBLL> PassengerOrdersItinerariesBLLList = new List<PassengerOrdersItinerariesBLL>();
            if (PassengerOrdersItinerariesList.Count > 0)
            {
                foreach (var item in PassengerOrdersItinerariesList)
                {
                    PassengerOrdersItinerariesBLLList.Add(new PassengerOrdersItinerariesBLL()
                    {
                        PassengerOrdersItineraryID = item.PassengerOrdersItineraryID,
                        PassengerOrder = new PassengerOrdersBLL().GetByPassengerOrderID(item.PassengerOrderID),
                        FromCity = item.FromCity,
                        ToCity = item.ToCity
                    });
                }
            }

            return PassengerOrdersItinerariesBLLList;
        }

        public Result Add(PassengerOrdersItinerariesBLL ItineraryBLL)
        {
            Result result = new Result();

            //// validate location already exists
            PassengerOrdersItineraries Itinerary = new PassengerOrdersItinerariesDAL()
                .GetPassengerOrdersItinerariesByPassengerOrderID(ItineraryBLL.PassengerOrder.PassengerOrderID)
                .Where(e => e.FromCity.ToUpper().Equals(ItineraryBLL.FromCity.ToUpper())
                            && e.ToCity.ToUpper().Equals(ItineraryBLL.ToCity.ToUpper())).FirstOrDefault();
            if (Itinerary != null)
            { 
                result.Entity = null;
                result.EnumType = typeof(PassengerOrdersValidationEnum);
                result.EnumMember = PassengerOrdersValidationEnum.RejectedBecausePassengerOrderItineraryCityAlreadyExist.ToString();

                return result;
            }

            result = new Result();
            PassengerOrdersItineraries Obj = new PassengerOrdersItineraries()
            {
                PassengerOrderID = ItineraryBLL.PassengerOrder.PassengerOrderID,
                FromCity = ItineraryBLL.FromCity,
                ToCity = ItineraryBLL.ToCity,
                CreatedDate = DateTime.Now,
            };

            this.PassengerOrdersItineraryID = new PassengerOrdersItinerariesDAL().Insert(Obj, ItineraryBLL.LoginIdentity.EmployeeCodeID);
            if (this.PassengerOrdersItineraryID != 0)
            {
                result.Entity = null;
                result.EnumType = typeof(PassengerOrdersValidationEnum);
                result.EnumMember = PassengerOrdersValidationEnum.Done.ToString();
            }
            return result;
        }

        public Result Remove(int PassengerOrdersItineraryID)
        {
            try
            {
                Result result = null;
                new PassengerOrdersItinerariesDAL().Delete(PassengerOrdersItineraryID,this.LoginIdentity.EmployeeCodeID);
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