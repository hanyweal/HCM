using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class PassengerOrdersDAL : CommonEntityDAL
    {
        public int Insert(PassengerOrders PassengerOrder)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.PassengerOrders.Add(PassengerOrder);
                    db.SaveChanges();
                    return PassengerOrder.PassengerOrderID;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                }
                throw;
            }
        }

        public int Update(PassengerOrders PassengerOrder)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PassengerOrders PassengerOrderObj = db.PassengerOrders.SingleOrDefault(x => x.PassengerOrderID.Equals(PassengerOrder.PassengerOrderID));
                    PassengerOrderObj.EmployeeCareerHistoryID = PassengerOrder.EmployeeCareerHistoryID;
                    PassengerOrderObj.TravellingDate = PassengerOrder.TravellingDate;
                    PassengerOrderObj.RankID = PassengerOrder.RankID;
                    PassengerOrderObj.TicketClassID = PassengerOrder.TicketClassID;
                    PassengerOrderObj.Going = PassengerOrder.Going;
                    PassengerOrderObj.Returning = PassengerOrder.Returning;
                    PassengerOrderObj.Reason = PassengerOrder.Reason;
                    PassengerOrderObj.LastUpdatedDate = PassengerOrder.LastUpdatedDate;
                    PassengerOrderObj.LastUpdatedBy = PassengerOrder.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(PassengerOrders PassengerOrder, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    PassengerOrders PassengerOrderObj = db.PassengerOrders.SingleOrDefault(x => x.PassengerOrderID.Equals(PassengerOrder.PassengerOrderID));
                    PassengerOrderObj.PassengerOrdersItineraries.ToList().ForEach(d => db.PassengerOrdersItineraries.Remove(d));
                    db.PassengerOrders.Remove(PassengerOrderObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<PassengerOrders> GetPassengerOrders(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();

                //var query = db.PassengerOrders
                //                              .Include("CreatedByNav.Employees")
                //                              .Include("LastUpdatedByNav.Employees")
                //                              .Include("PassengerOrdersItineraries");
                ////.Include("DelegationsDetails")
                ////.Include("DelegationsDetails.Delegations")
                ////.Include("DelegationsDetails.EmployeesCareersHistory.EmployeesCodes")
                ////.Include("DelegationsDetails.EmployeesCareersHistory.EmployeesCodes.Employees");

                //return query.Include("TicketsClasses").Include("Ranks").ToList();

                var odData = db.PassengerOrders
                            .Include("CreatedByNav.Employees")
                            .Include("LastUpdatedByNav.Employees")
                            .Include("PassengerOrdersItineraries")
                            .Include("TicketsClasses")
                            .Include("Ranks");
                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<PassengerOrders> iq = odData.Where(p => p.PassengerOrderID != null);
                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                                       p.PassengerOrderID.ToString().ToLower().Contains(search.ToLower())
                                       );
                }
                // Sorting.
                //iqOD = this.SortByColumnWithOrder2(order, orderDir, iqOD);
                iq = iq.OrderBy(p => p.PassengerOrderID);
                // Filter record count.
                recFilterOut = iq.Count();

                // Apply pagination.
                iq = iq.Skip(startRec).Take(pageSize);
                //Get list of data
                var data = iq.ToList();

                return data;

            }
            catch
            {
                throw;
            }
        }

        public List<PassengerOrders> GetPassengerOrders()
        {
            try
            {
                var db = new HCMEntities();

                //var query = db.PassengerOrders
                //                              .Include("CreatedByNav.Employees")
                //                              .Include("LastUpdatedByNav.Employees")
                //                              .Include("PassengerOrdersItineraries");
                ////.Include("DelegationsDetails")
                ////.Include("DelegationsDetails.Delegations")
                ////.Include("DelegationsDetails.EmployeesCareersHistory.EmployeesCodes")
                ////.Include("DelegationsDetails.EmployeesCareersHistory.EmployeesCodes.Employees");

                //return query.Include("TicketsClasses").Include("Ranks").ToList();

                var odData = db.PassengerOrders
                            .Include("CreatedByNav.Employees")
                            .Include("LastUpdatedByNav.Employees")
                            .Include("PassengerOrdersItineraries")
                            .Include("TicketsClasses")
                            .Include("Ranks");

                var data = odData.ToList();

                return data;

            }
            catch
            {
                throw;
            }
        }

        public PassengerOrders GetByPassengerOrderID(int PassengerOrderID)
        {
            try
            {
                var db = new HCMEntities();

                var query = db.PassengerOrders.Include("CreatedByNav.Employees")
                                              .Include("LastUpdatedByNav.Employees")
                                              .Include("PassengerOrdersItineraries");

                return query.Include("TicketsClasses").Include("Ranks").SingleOrDefault(x => x.PassengerOrderID.Equals(PassengerOrderID));
            }
            catch
            {
                throw;
            }
        }

        //public List<PassengerOrders> GetPassengerOrdersByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        //{
        //    try
        //    {
        //        var db = new HCMEntities();
        //        /*return db.PassengerOrders
        //            //.Include("DelegationsDetails.Delegations")
        //            //.Include("DelegationsDetails.EmployeesCodes")
        //                                  //.Include("DelegationsDetails.EmployeesCareersHistory.EmployeesCodes.Employees")
        //                                  //.Where(x => x.DelegationsDetails.EmployeesCareersHistory.EmployeeCareerHistoryID.Equals(EmployeeCareerHistoryID))
        //                                  .ToList();

        //        var query = db.PassengerOrders;
        //       var query2 = query.Where(p => p.vwPassengerOrdersDetails.ID == 1);        //.Include("vwPassengerOrdersDetails").ToList();
        //       List<PassengerOrders> a = query2.ToList();
        //        */

        //        var query = (from p in db.PassengerOrders
        //                     //join pd in db.vwPassengerOrdersDetails on p.PassengerOrderTypeID equals pd.Type
        //                     //join ech in db.EmployeesCareersHistory on pd.EmployeeCareerHistoryID equals ech.EmployeeCareerHistoryID
        //                     where p.EmployeeCareerHistoryID == p.EmployeeCareerHistoryID //&& ech.EmployeeCareerHistoryID == EmployeeCareerHistoryID                             
        //                     select p);

        //        return query.ToList();  //db.PassengerOrders.ToList();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public PassengerOrders IsPassengerOrderAlreadyTook(int EmployeeCareerHistoryID)
        //{
        //    try
        //    {
        //        var db = new HCMEntities();
        //        return db.PassengerOrders.SingleOrDefault(x => x.EmployeeCareerHistoryID == EmployeeCareerHistoryID);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public List<PassengerOrders> GetPassengerOrdersByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();

                var query = db.PassengerOrders.Include("CreatedByNav.Employees")
                                              .Include("LastUpdatedByNav.Employees")
                                              .Include("EmployeesCareersHistory");

                return query.Include("TicketsClasses").Include("Ranks").Where(x => x.EmployeesCareersHistory.EmployeeCodeID.Equals(EmployeeCodeID)).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}