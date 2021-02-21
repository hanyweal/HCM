using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class ScholarshipsDAL
    {
        public int Insert(Scholarships EmployeeScholarship)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.Scholarships.Add(EmployeeScholarship);
                    db.SaveChanges();
                    return EmployeeScholarship.ScholarshipID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(Scholarships EmployeeScholarship)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Scholarships EmployeeScholarshipObj = db.Scholarships.FirstOrDefault(x => x.ScholarshipID.Equals(EmployeeScholarship.ScholarshipID));
                    EmployeeScholarshipObj.ScholarshipStartDate = EmployeeScholarship.ScholarshipStartDate;
                    EmployeeScholarshipObj.ScholarshipEndDate = EmployeeScholarship.ScholarshipEndDate;
                    EmployeeScholarshipObj.Location = EmployeeScholarship.Location;
                    EmployeeScholarshipObj.ScholarshipReason = EmployeeScholarship.ScholarshipReason;
                    EmployeeScholarshipObj.LastUpdatedDate = EmployeeScholarship.LastUpdatedDate;
                    EmployeeScholarshipObj.LastUpdatedBy = EmployeeScholarship.LastUpdatedBy;

                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateScholarshipStatus(Scholarships EmployeeScholarship)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Scholarships EmployeeScholarshipObj = db.Scholarships.FirstOrDefault(x => x.ScholarshipID.Equals(EmployeeScholarship.ScholarshipID));
                    EmployeeScholarshipObj.ScholarshipStartDate = EmployeeScholarship.ScholarshipStartDate;
                    EmployeeScholarshipObj.ScholarshipEndDate = EmployeeScholarship.ScholarshipEndDate;
                    EmployeeScholarshipObj.ScholarshipJoinDate = EmployeeScholarship.ScholarshipJoinDate;
                    EmployeeScholarshipObj.IsCanceled = EmployeeScholarship.IsCanceled;
                    EmployeeScholarshipObj.IsPassed = EmployeeScholarship.IsPassed;
                    EmployeeScholarshipObj.LastUpdatedDate = EmployeeScholarship.LastUpdatedDate;
                    EmployeeScholarshipObj.LastUpdatedBy = EmployeeScholarship.LastUpdatedBy;

                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateJoinScholarshipStatus(Scholarships EmployeeScholarship)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Scholarships EmployeeScholarshipObj = db.Scholarships.FirstOrDefault(x => x.ScholarshipID.Equals(EmployeeScholarship.ScholarshipID));
                    EmployeeScholarshipObj.ScholarshipStartDate = EmployeeScholarship.ScholarshipStartDate;
                    EmployeeScholarshipObj.ScholarshipEndDate = EmployeeScholarship.ScholarshipEndDate;
                    EmployeeScholarshipObj.ScholarshipJoinDate = EmployeeScholarship.ScholarshipJoinDate;
                    EmployeeScholarshipObj.IsPassed = EmployeeScholarship.IsPassed;
                    EmployeeScholarshipObj.LastUpdatedDate = EmployeeScholarship.LastUpdatedDate;
                    EmployeeScholarshipObj.LastUpdatedBy = EmployeeScholarship.LastUpdatedBy;
                    return db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(Scholarships EmployeeScholarship, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    Scholarships EmployeeScholarshipObj = db.Scholarships.FirstOrDefault(x => x.ScholarshipID.Equals(EmployeeScholarship.ScholarshipID));
                    db.Scholarships.Remove(EmployeeScholarshipObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Scholarships> GetScholarships()
        {
            try
            {
                var db = new HCMEntities();
                var query = db.Scholarships.Include("ScholarshipsTypes");
                query = query.Include("Qualifications").Include("Qualifications.QualificationsDegrees");
                query = query.Include("EmployeesCodes").Include("EmployeesCodes.Employees");

                query = query.Include("ScholarshipsDetails");
                return query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Scholarships GetByScholarshipID(int ScholarshipID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Scholarships.Include("ScholarshipsDetails")
                                      .Include("ScholarshipsTypes")
                                      .Include("EmployeesCodes.Employees")
                                      .Include("Qualifications.QualificationsDegrees")
                                      .FirstOrDefault(x => x.ScholarshipID.Equals(ScholarshipID));
            }
            catch
            {
                throw;
            }
        }

        public List<Scholarships> GetScholarshipsByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                var query = db.Scholarships.Include("ScholarshipsTypes");

                query = query.Include("Qualifications").Include("Qualifications.QualificationsDegrees");

                query = query.Include("EmployeesCodes").Include("EmployeesCodes.Employees");

                query = query.Include("ScholarshipsDetails");

                return query.Where(c => c.EmployeesCodes.EmployeeCodeID == EmployeeCodeID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<Scholarships> GetValidByEmployeeCodeIDScholarshipTypeID(int EmployeeCodeID, int ScholarshipTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Scholarships
                                    .Include("ScholarshipsTypes")
                                    .Where(x => x.EmployeesCodes.EmployeeCodeID == EmployeeCodeID
                                            && x.ScholarshipEndDate >= DateTime.Now
                                            && x.ScholarshipTypeID == ScholarshipTypeID
                                            && x.IsCanceled != true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<Scholarships> GetCanceledByEmployeeCodeIDScholarshipTypeID(int EmployeeCodeID, int ScholarshipTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Scholarships
                                    .Include("ScholarshipsTypes")
                                    .Where(x => x.EmployeesCodes.EmployeeCodeID == EmployeeCodeID
                                            && x.ScholarshipTypeID == ScholarshipTypeID
                                            && x.IsCanceled == true

                               ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<Scholarships> GetFinishedByEmployeeCodeIDScholarshipTypeID(int EmployeeCodeID, int ScholarshipTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.Scholarships
                                    .Include("ScholarshipsTypes")
                                    .Where(x => x.EmployeesCodes.EmployeeCodeID == EmployeeCodeID
                                            && x.ScholarshipEndDate < DateTime.Now
                                             && x.ScholarshipTypeID == ScholarshipTypeID
                                            && x.IsCanceled != true).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetCountByEmployeeCodeIDAndDateDuration(int EmployeeCodeID, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var db = new HCMEntities();
                return db.Scholarships.Where(x => x.EmployeeCodeID == EmployeeCodeID &&
                                                 ((StartDate >= x.ScholarshipStartDate && StartDate <= x.ScholarshipEndDate) ||
                                                 (EndDate >= x.ScholarshipStartDate && EndDate <= x.ScholarshipEndDate) ||
                                                 (StartDate >= x.ScholarshipStartDate && EndDate <= x.ScholarshipEndDate) ||
                                                 (StartDate <= x.ScholarshipStartDate && EndDate >= x.ScholarshipEndDate))
                                               ).Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}