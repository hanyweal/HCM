using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PassengerOrdersEscortsDAL
    {
        public List<PassengerOrdersEscorts> GetPassengerOrdersEscortsByPassengerOrderID(int PassengerOrderID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PassengerOrdersEscorts.Where(dd => dd.PassengerOrderID == PassengerOrderID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int PassengerOrderEscortID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PassengerOrdersEscorts Obj = db.PassengerOrdersEscorts.SingleOrDefault(x => x.PassengerOrderEscortID.Equals(PassengerOrderEscortID));
                    db.PassengerOrdersEscorts.Remove(Obj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public int Insert(PassengerOrdersEscorts PassengerOrderEscort, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PassengerOrdersEscorts.Add(PassengerOrderEscort);
                    db.SaveChanges(UserIdentity);
                    return PassengerOrderEscort.PassengerOrderEscortID;
                }
            }
            catch
            {

                throw;
            }
        }
    }
}
