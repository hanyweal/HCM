using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class TransfersDAL
    {
        public int Insert(Transfers Transfer)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Transfers.Add(Transfer);
                    db.SaveChanges();
                    return Transfer.TransferID;
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

        public int Update(Transfers Transfer)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Transfers TransferObj = db.Transfers.FirstOrDefault(x => x.TransferID.Equals(Transfer.TransferID));
                    TransferObj.TransferTypeID = Transfer.TransferTypeID;
                    TransferObj.CareerDegreeName = Transfer.CareerDegreeName;
                    TransferObj.Destination = Transfer.Destination;
                    TransferObj.EmployeeCareerHistoryID = Transfer.EmployeeCareerHistoryID;
                    TransferObj.JobCode = Transfer.JobCode;
                    TransferObj.JobName = Transfer.JobName;
                    TransferObj.KSACityID = Transfer.KSACityID;
                    TransferObj.OrganizationName = Transfer.OrganizationName;
                    TransferObj.RankName = Transfer.RankName;
                    TransferObj.TransferDate = Transfer.TransferDate;
                    TransferObj.LastUpdatedDate = Transfer.LastUpdatedDate;
                    TransferObj.LastUpdatedBy = Transfer.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateIsProcess(Transfers Transfer)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Transfers TransferObj = db.Transfers.FirstOrDefault(x => x.TransferID.Equals(Transfer.TransferID));
                    TransferObj.IsProcessed = Transfer.IsProcessed;
                    TransferObj.LastUpdatedDate = Transfer.LastUpdatedDate;
                    TransferObj.LastUpdatedBy = Transfer.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(Transfers Transfer, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Transfers TransferObj = db.Transfers.FirstOrDefault(x => x.TransferID.Equals(Transfer.TransferID));
                    db.Transfers.Remove(TransferObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Transfers> GetTransfers()
        {
            try
            {
                var db = new HCMEntities();
                var query = db.Transfers
                                       .Include("TransfersTypes")
                                       .Include("EmployeesCareersHistory");

                query = query.Include("EmployeesCareersHistory.EmployeesCodes")
                                  .Include("EmployeesCareersHistory.EmployeesCodes.Employees");

                query = query.Include("EmployeesCareersHistory.OrganizationsJobs")
                                   .Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                                   .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks");


                return query.ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<Transfers> GetTransfersNotProccessed()
        {
            try
            {
                var db = new HCMEntities();
                var query = db.Transfers.Include("TransfersTypes");                

                query = query.Include("EmployeesCareersHistory.EmployeesCodes.Employees");

                query = query.Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                             .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks");

                return query.Where(x => x.IsProcessed == false).ToList();
            }
            catch
            {
                throw;
            }
        }

        public Transfers GetByTransferID(int TransferID)
        {
            try
            {
                var db = new HCMEntities();
                var query = db.Transfers
                                     .Include("TransfersTypes")
                                     .Include("EmployeesCareersHistory");

                query = query.Include("EmployeesCareersHistory.EmployeesCodes")
                                  .Include("EmployeesCareersHistory.EmployeesCodes.Employees");

                query = query.Include("EmployeesCareersHistory.OrganizationsJobs")
                                   .Include("EmployeesCareersHistory.OrganizationsJobs.Jobs")
                                   .Include("EmployeesCareersHistory.OrganizationsJobs.Ranks");

                return query.FirstOrDefault(x => x.TransferID.Equals(TransferID));
            }
            catch
            {
                throw;
            }
        }
    }
}
