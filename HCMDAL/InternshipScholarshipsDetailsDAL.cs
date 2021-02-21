using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class InternshipScholarshipsDetailsDAL
    {
        public List<InternshipScholarshipsDetails> GetInternshipScholarshipsDetails()
        {
            try
            {
                var db = new HCMEntities();
                return db.InternshipScholarshipsDetails.Include("InternshipScholarships")
                                            .Include("EmployeesCareersHistory.EmployeesCodes.Employees").ToList();
            }
            catch
            {
                throw;
            }
        }

        public List<InternshipScholarshipsDetails> GetInternshipScholarshipsDetailsByInternshipScholarshipID(int InternshipScholarshipID)
        {
            try
            {
                var db = new HCMEntities();
                return db.InternshipScholarshipsDetails.Include("InternshipScholarships")
                                            .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                            .Where(x => x.InternshipScholarshipID == InternshipScholarshipID).ToList();

            }
            catch
            {
                throw;
            }
        }

        public InternshipScholarshipsDetails GetInternshipScholarshipsDetailsByInternshipScholarshipDetailID(int InternshipScholarshipDetailID)
        {
            try
            {
                var db = new HCMEntities();
                return db.InternshipScholarshipsDetails.Include("InternshipScholarships")
                                                .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                                .FirstOrDefault(x => x.InternshipScholarshipDetailID == InternshipScholarshipDetailID);

            }
            catch
            {
                throw;
            }
        }

        public int Delete(int InternshipScholarshipDetailID, int UserIdentity)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    InternshipScholarshipsDetails InternshipScholarshipDetailObj = db.InternshipScholarshipsDetails.SingleOrDefault(x => x.InternshipScholarshipDetailID.Equals(InternshipScholarshipDetailID));
                    db.InternshipScholarshipsDetails.Remove(InternshipScholarshipDetailObj);
                    return db.SaveChanges(UserIdentity);
                }
            }
            catch
            {
                throw;
            }
        }

        public void Delete(InternshipScholarshipsDetails InternshipScholarshipDetail)
        {
            throw new NotImplementedException();
        }

        public int Insert(InternshipScholarshipsDetails InternshipScholarshipDetail)
        {
            try
            {
                using (var db = new HCMEntities())
                {
                    db.InternshipScholarshipsDetails.Add(InternshipScholarshipDetail);
                    db.SaveChanges();
                    return InternshipScholarshipDetail.InternshipScholarshipDetailID;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<InternshipScholarshipsDetails> GetInternshipScholarshipsDetailsByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.InternshipScholarshipsDetails.Include("InternshipScholarships")
                                            .Include("EmployeesCareersHistory.EmployeesCodes.Employees")
                                            .Where(c => c.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeID == EmployeeCodeID).ToList();

            }
            catch
            {
                throw;
            }
        }
    }
}
