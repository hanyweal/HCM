using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class StopWorksTypesBLL
    {
        public int StopWorkTypeID { get; set; }

        public string StopWorkTypeName { get; set; }

        public StopWorksCategoriesBLL StopWorkCategory { get; set; }

        public List<StopWorksTypesBLL> GetStopWorksTypes()
        {
            try
            {
                List<StopWorksTypes> StopWorksTypesList = new StopWorksTypesDAL().GetStopWorksTypes();
                List<StopWorksTypesBLL> StopWorksTypesBLLList = new List<StopWorksTypesBLL>();
                if (StopWorksTypesList.Count > 0)
                {
                    foreach (var item in StopWorksTypesList)
                    {
                        StopWorksTypesBLLList.Add(new StopWorksTypesBLL()
                        {
                            StopWorkTypeID = item.StopWorkTypeID,
                            StopWorkTypeName = item.StopWorkTypeName
                        });
                    }
                }

                return StopWorksTypesBLLList;
            }
            catch
            {
                throw;
            }
        }

        public List<StopWorksTypesBLL> GetByStopWorkCategoryID(int StopWorkCategoryID)
        {
            try
            {
                List<StopWorksTypes> StopWorksTypesList = new StopWorksTypesDAL().GetByStopWorkCategoryID(StopWorkCategoryID);
                List<StopWorksTypesBLL> StopWorksTypesBLLList = new List<StopWorksTypesBLL>();
                if (StopWorksTypesList.Count > 0)
                {
                    foreach (var item in StopWorksTypesList)
                    {
                        StopWorksTypesBLLList.Add(new StopWorksTypesBLL()
                        {
                            StopWorkTypeID = item.StopWorkTypeID,
                            StopWorkTypeName = item.StopWorkTypeName,
                            StopWorkCategory = new StopWorksCategoriesBLL().MapInternshipScholarshipType(item.StopWorksCategories)
                        });
                    }
                }

                return StopWorksTypesBLLList;
            }
            catch
            {
                throw;
            }
        }

        public StopWorksTypesBLL GetByStopWorkTypeID(int StopWorkTypeID)
        {
            try
            {
                StopWorksTypes StopWorkType = new StopWorksTypesDAL().GetByStopWorkTypeID(StopWorkTypeID);
                return new StopWorksTypesBLL()
                {
                    StopWorkTypeID = StopWorkType.StopWorkTypeID,
                    StopWorkTypeName = StopWorkType.StopWorkTypeName,
                    StopWorkCategory=new StopWorksCategoriesBLL().MapInternshipScholarshipType(StopWorkType.StopWorksCategories)
                };
            }
            catch
            {
                throw;
            }
        }

        internal StopWorksTypesBLL MapStopWorkType(StopWorksTypes StopWorkType)
        {
            try
            {
                StopWorksTypesBLL StopWorkTypeBLL = null;
                if (StopWorkType != null)
                {
                    StopWorkTypeBLL = new StopWorksTypesBLL()
                    {
                        StopWorkTypeID = StopWorkType.StopWorkTypeID,
                        StopWorkTypeName = StopWorkType.StopWorkTypeName,
                        StopWorkCategory=new StopWorksCategoriesBLL().MapInternshipScholarshipType(StopWorkType.StopWorksCategories)
                    };
                }
                return StopWorkTypeBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
