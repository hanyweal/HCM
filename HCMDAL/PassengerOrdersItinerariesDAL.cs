using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class PassengerOrdersItinerariesDAL
    {
        public List<PassengerOrdersItineraries> GetPassengerOrdersItinerariesByPassengerOrderID(int PassengerOrderID)
        {
            try
            {
                var db = new HCMEntities();
                return db.PassengerOrdersItineraries.Where(dd => dd.PassengerOrderID == PassengerOrderID).ToList();
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int PassengerOrderItineraryID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PassengerOrdersItineraries Obj = db.PassengerOrdersItineraries.SingleOrDefault(x => x.PassengerOrdersItineraryID.Equals(PassengerOrderItineraryID));
                    db.PassengerOrdersItineraries.Remove(Obj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public int Insert(PassengerOrdersItineraries PassengerOrderItinerary, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PassengerOrdersItineraries.Add(PassengerOrderItinerary);
                    db.SaveChanges(UserIdentity);
                    return PassengerOrderItinerary.PassengerOrdersItineraryID;
                }
            }
            catch
            {

                throw;
            }
        }
    }
}
