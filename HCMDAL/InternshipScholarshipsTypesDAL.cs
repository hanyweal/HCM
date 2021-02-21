using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class InternshipScholarshipsTypesDAL
    {
        public List<InternshipScholarshipsTypes> GetInternshipScholarshipsTypes()
        {
            try
            {
                var db = new HCMEntities();
                return db.InternshipScholarshipsTypes.ToList();
            }
            catch
            {
                throw;
            }
        }

        public InternshipScholarshipsTypes GetByInternshipScholarshipTypeID(int InternshipScholarshipTypeID)
        {
            try
            {
                var db = new HCMEntities();
                return db.InternshipScholarshipsTypes.SingleOrDefault(x => x.InternshipScholarshipTypeID.Equals(InternshipScholarshipTypeID));
            }
            catch
            {
                throw;
            }
        }
    }
}
