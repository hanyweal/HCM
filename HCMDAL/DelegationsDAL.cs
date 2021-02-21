using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class DelegationsDAL : CommonEntityDAL
    {
        public int Insert(Delegations Delegation)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Delegations.Add(Delegation);
                    db.SaveChanges();
                    return Delegation.DelegationID;
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

        public int Update(Delegations Delegation)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Delegations DelegationObj = db.Delegations.SingleOrDefault(x => x.DelegationID.Equals(Delegation.DelegationID));
                    DelegationObj.DelegationStartDate = Delegation.DelegationStartDate;
                    DelegationObj.DelegationEndDate = Delegation.DelegationEndDate;
                    DelegationObj.DelegationReason = Delegation.DelegationReason;
                    DelegationObj.Notes = Delegation.Notes;
                    DelegationObj.CountryID = Delegation.CountryID;
                    DelegationObj.KSACityID = Delegation.KSACityID;
                    DelegationObj.DelegationDistancePeriod = Delegation.DelegationDistancePeriod;
                    DelegationObj.DelegationKindID = Delegation.DelegationKindID;
                    DelegationObj.DelegationTypeID = Delegation.DelegationTypeID;
                    DelegationObj.LastUpdatedDate = Delegation.LastUpdatedDate;
                    DelegationObj.LastUpdatedBy = Delegation.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int DelegationID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Delegations DelegationObj = db.Delegations.SingleOrDefault(x => x.DelegationID.Equals(DelegationID));
                    DelegationObj.DelegationsDetails.ToList().ForEach(d => db.DelegationsDetails.Remove(d));
                    db.Delegations.Remove(DelegationObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Delegations> GetDelegations(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                var db = new HCMEntities();
                var odData = db.Delegations.Include("DelegationsKinds")
                                     .Include("DelegationsTypes")
                                     .Include("KSACities")
                                     .Include("KSACities.KSARegions")
                                     .Include("Countries")
                                     .Include("CreatedByNav.Employees")
                                     .Include("LastUpdatedByNav.Employees");
                // Total record count.
                totalRecordsOut = odData.Count();

                IQueryable<Delegations> iq = odData.Where(p => p.DelegationID != null);
                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    iq = iq.Where(p =>
                                       p.DelegationID.ToString().ToLower().Contains(search.ToLower()) ||
                                       p.DelegationsTypes.DelegationTypeName.ToLower().Contains(search.ToLower()) ||
                                       p.DelegationsKinds.DelegationKindName.ToLower().Contains(search.ToLower()) ||
                                       p.DelegationStartDate.ToString().ToLower().Contains(search.ToLower()) ||
                                       //p.DelegationPeriod.ToLower().Contains(search.ToLower()) ||
                                       p.DelegationReason.ToLower().Contains(search.ToLower())
                                       );
                }
                iq = this.SortExpression(iq);
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

        private IQueryable<Delegations> SortExpression(IQueryable<Delegations> iq)
        {
            this.orderDir = string.IsNullOrEmpty(this.orderDir) ? "asc" : this.orderDir;
            this.orderByColumnName = string.IsNullOrEmpty(this.orderByColumnName) ? "" : this.orderByColumnName;
            switch (this.orderByColumnName)
            {
                case "DelegationID":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.DelegationID) : iq.OrderBy(p => p.DelegationID);
                    break;
                case "DelegationTypeName":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.DelegationsTypes.DelegationTypeName) : iq.OrderBy(p => p.DelegationsTypes.DelegationTypeName);
                    break;
                case "DelegationKindName":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.DelegationsKinds.DelegationKindName) : iq.OrderBy(p => p.DelegationsKinds.DelegationKindName);
                    break;
                case "DelegationStartDate":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.DelegationStartDate) : iq.OrderBy(p => p.DelegationStartDate);
                    break;
                //case "DelegationPeriod":
                //    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.DelegationEndDate.Subtract(p.DelegationStartDate).Days + 1) : iq.OrderBy(p => p.DelegationEndDate.Subtract(p.DelegationStartDate).Days + 1);
                //    break;
                case "DelegationReason":
                    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.DelegationReason) : iq.OrderBy(p => p.DelegationReason);
                    break;
                //case "IsApproved":
                //    iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.ApprovedBy) : iq.OrderBy(p => p.ApprovedBy);
                //    //iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => (p.ApprovedBy.HasValue ? p.ApprovedBy.Value: 0) ) : iq.OrderBy(p => (p.ApprovedBy.HasValue ? p.ApprovedBy.Value : 0));
                //    break;
                default:
                    //iq = this.orderDir.ToLower().Equals("desc") ? iq.OrderByDescending(p => p.DelegationID) : iq.OrderBy(p => p.DelegationID);
                    break;
            }

            return iq;
        }

        public List<Delegations> GetDelegations()
        {
            try
            {
                var db = new HCMEntities();

                var odData = db.Delegations.Include("DelegationsKinds")
                                     .Include("DelegationsTypes")
                                     .Include("KSACities")
                                     .Include("KSACities.KSARegions")
                                     .Include("Countries")
                                     .Include("CreatedByNav.Employees")
                                     .Include("LastUpdatedByNav.Employees");

                var data = odData.ToList();

                return data;
            }
            catch
            {
                throw;
            }
        }

        public Delegations GetByDelegationID(int DelegationID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Delegations.Include("DelegationsKinds")
                                     .Include("DelegationsTypes")
                                     .Include("KSACities")
                                     .Include("KSACities.KSARegions")
                                     .Include("Countries")
                                     .Include("CreatedByNav.Employees")
                                     .SingleOrDefault(x => x.DelegationID.Equals(DelegationID));
            }
            catch
            {
                throw;
            }
        }

        public int Approve(Delegations Delegation)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Delegations DelegationObj = db.Delegations.SingleOrDefault(x => x.DelegationID.Equals(Delegation.DelegationID));
                    DelegationObj.ApprovedBy = Delegation.ApprovedBy;
                    DelegationObj.ApprovedDate = Delegation.ApprovedDate;
                    DelegationObj.IsApproved = Delegation.IsApproved;
                    DelegationObj.LastUpdatedBy = Delegation.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public bool IsApproved(int DelegationID)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    return db.Delegations.Find(DelegationID).IsApproved;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}