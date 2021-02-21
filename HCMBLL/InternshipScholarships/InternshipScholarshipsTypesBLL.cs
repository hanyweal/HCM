using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class InternshipScholarshipsTypesBLL
    {
        public int InternshipScholarshipTypeID { get; set; }

        public string InternshipScholarshipTypeName { get; set; }

        public List<InternshipScholarshipsTypesBLL> GetInternshipScholarshipsTypes()
        {
            try
            {
                List<InternshipScholarshipsTypes> InternshipScholarshipsTypesList = new InternshipScholarshipsTypesDAL().GetInternshipScholarshipsTypes();
                List<InternshipScholarshipsTypesBLL> InternshipScholarshipsTypesBLLList = new List<InternshipScholarshipsTypesBLL>();
                if (InternshipScholarshipsTypesList.Count > 0)
                {
                    foreach (var item in InternshipScholarshipsTypesList)
                    {
                        InternshipScholarshipsTypesBLLList.Add(new InternshipScholarshipsTypesBLL()
                        {
                            InternshipScholarshipTypeID = item.InternshipScholarshipTypeID,
                            InternshipScholarshipTypeName = item.InternshipScholarshipTypeName
                        });
                    }
                }

                return InternshipScholarshipsTypesBLLList;
            }
            catch
            {
                throw;
            }
        }

        public InternshipScholarshipsTypesBLL GetByInternshipScholarshipTypeID(int InternshipScholarshipTypeID)
        {
            try
            {
                InternshipScholarshipsTypes InternshipScholarshipType = new InternshipScholarshipsTypesDAL().GetByInternshipScholarshipTypeID(InternshipScholarshipTypeID);
                return new InternshipScholarshipsTypesBLL()
                {
                    InternshipScholarshipTypeID = InternshipScholarshipType.InternshipScholarshipTypeID,
                    InternshipScholarshipTypeName = InternshipScholarshipType.InternshipScholarshipTypeName
                };
            }
            catch
            {
                throw;
            }
        }

        internal InternshipScholarshipsTypesBLL MapInternshipScholarshipType(InternshipScholarshipsTypes InternshipScholarshipType)
        {
            try
            {
                InternshipScholarshipsTypesBLL InternshipScholarshipTypeBLL = null;
                if (InternshipScholarshipType != null)
                {
                    InternshipScholarshipTypeBLL = new InternshipScholarshipsTypesBLL()
                    {
                        InternshipScholarshipTypeID = InternshipScholarshipType.InternshipScholarshipTypeID,
                        InternshipScholarshipTypeName = InternshipScholarshipType.InternshipScholarshipTypeName
                    };
                }
                return InternshipScholarshipTypeBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
