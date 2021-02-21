using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class RanksTicketsClassesDAL
    {
        public int Insert(RanksTicketsClasses RankTicketClass)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.RanksTicketsClasses.Add(RankTicketClass);
                    db.SaveChanges();
                    return RankTicketClass.RankTicketClassID;
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

        public int Update(RanksTicketsClasses RankTicketClass)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    RanksTicketsClasses RankTicketClassObj = db.RanksTicketsClasses.SingleOrDefault(x => x.RankTicketClassID.Equals(RankTicketClass.RankTicketClassID));
                    RankTicketClassObj.RankID = RankTicketClass.RankID;
                    RankTicketClassObj.TicketClassID = RankTicketClass.TicketClassID;
                    RankTicketClassObj.LastUpdatedDate = DateTime.Now;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int RankTicketClassID)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    RanksTicketsClasses RankTicketClassObj = db.RanksTicketsClasses.SingleOrDefault(x => x.RankTicketClassID.Equals(RankTicketClassID));
                    db.RanksTicketsClasses.Remove(RankTicketClassObj);
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<RanksTicketsClasses> GetRanksTicketsClasses()
        {
            try
            {
                var db = new HCMEntities();
                return db.RanksTicketsClasses.Include("Ranks").Include("TicketsClasses").ToList();
            }
            catch
            {
                throw;
            }
        }

        public RanksTicketsClasses GetByRankTicketClassID(int RankTicketClassID)
        {
            try
            {
                var db = new HCMEntities();
                return db.RanksTicketsClasses.Include("Ranks").Include("TicketsClasses").SingleOrDefault(x => x.RankTicketClassID.Equals(RankTicketClassID));
            }
            catch
            {
                throw;
            }
        }
    }
}