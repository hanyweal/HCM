using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace HCMDAL
{
    public class InternshipScholarshipsDAL
    {
        public int Insert(InternshipScholarships InternshipScholarship)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.InternshipScholarships.Add(InternshipScholarship);
                    db.SaveChanges();
                    return InternshipScholarship.InternshipScholarshipID;
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

        public int Update(InternshipScholarships InternshipScholarship)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    InternshipScholarships InternshipScholarshipObj = db.InternshipScholarships.SingleOrDefault(x => x.InternshipScholarshipID.Equals(InternshipScholarship.InternshipScholarshipID));
                    InternshipScholarshipObj.InternshipScholarshipStartDate = InternshipScholarship.InternshipScholarshipStartDate;
                    InternshipScholarshipObj.InternshipScholarshipEndDate = InternshipScholarship.InternshipScholarshipEndDate;
                    InternshipScholarshipObj.InternshipScholarshipReason = InternshipScholarship.InternshipScholarshipReason;
                    InternshipScholarshipObj.CountryID = InternshipScholarship.CountryID;
                    InternshipScholarshipObj.KSACityID = InternshipScholarship.KSACityID;
                    InternshipScholarshipObj.InternshipScholarshipLocation = InternshipScholarship.InternshipScholarshipLocation;
                    InternshipScholarshipObj.InternshipScholarshipTypeID = InternshipScholarship.InternshipScholarshipTypeID;
                    InternshipScholarshipObj.LastUpdatedDate = InternshipScholarship.LastUpdatedDate;
                    InternshipScholarshipObj.LastUpdatedBy = InternshipScholarship.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int InternshipScholarshipID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    InternshipScholarships InternshipScholarshipObj = db.InternshipScholarships.SingleOrDefault(x => x.InternshipScholarshipID.Equals(InternshipScholarshipID));
                    InternshipScholarshipObj.InternshipScholarshipsDetails.ToList().ForEach(d => db.InternshipScholarshipsDetails.Remove(d));
                    db.InternshipScholarships.Remove(InternshipScholarshipObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public List<InternshipScholarships> GetInternshipScholarships()
        {
            try
            {
                var db = new HCMEntities();
                return db.InternshipScholarships
                                     .Include("InternshipScholarshipsTypes")
                                     .Include("KSACities")
                                     .Include("KSACities.KSARegions")
                                     .Include("Countries")
                                     .Include("CreatedByNav.Employees")
                                     .Include("LastUpdatedByNav.Employees")
                                     .ToList();

            }
            catch
            {
                throw;
            }
        }

        public InternshipScholarships GetByInternshipScholarshipID(int InternshipScholarshipID)
        {
            try
            {
                var db = new HCMEntities();
                return db.InternshipScholarships
                                     .Include("InternshipScholarshipsTypes")
                                     .Include("KSACities")
                                     .Include("KSACities.KSARegions")
                                     .Include("Countries")
                                     .Include("CreatedByNav.Employees")
                                     .SingleOrDefault(x => x.InternshipScholarshipID.Equals(InternshipScholarshipID));

            }
            catch
            {
                throw;
            }
        }
    }
}