using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class PassengerOrdersTypesBLL
    {
        public int PassengerOrderTypeID { get; set; }

        public string PassengerOrderTypeName { get; set; }

        public List<PassengerOrdersTypesBLL> GetPassengerOrdersTypes()
        {
            try
            {
                List<PassengerOrdersTypes> PassengerOrderTypeList = new PassengerOrdersTypesDAL().GetPassengerOrdersTypes();
                List<PassengerOrdersTypesBLL> PassengerOrdersTypesBLLList = new List<PassengerOrdersTypesBLL>();
                foreach (var item in PassengerOrderTypeList)
                {
                    PassengerOrdersTypesBLLList.Add(new PassengerOrdersTypesBLL().MapPassengerOrderType(item));
                }
                return PassengerOrdersTypesBLLList;
            }
            catch
            {
                throw;
            }
        }

        internal PassengerOrdersTypesBLL MapPassengerOrderType(PassengerOrdersTypes PassengerOrderType)
        {
            try
            {
                PassengerOrdersTypesBLL PassengerOrderTypeBLL = null;
                if (PassengerOrderType != null)
                {
                    PassengerOrderTypeBLL = new PassengerOrdersTypesBLL()
                    {
                        PassengerOrderTypeID = PassengerOrderType.PassengerOrderTypeID,
                        PassengerOrderTypeName = PassengerOrderType.PassengerOrderTypeName
                    };
                }
                return PassengerOrderTypeBLL;
            }
            catch
            {
                throw;
            }
        }
    }

}
