using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class StopWorksCategoriesBLL
    {
        public int StopWorkCategoryID { get; set; }

        public string StopWorkCategoryName { get; set; }

        public List<StopWorksCategoriesBLL> GetStopWorksCategories()
        {
            try
            {
                List<StopWorksCategories> StopWorksCategoriesList = new StopWorksCategoriesDAL().GetStopWorksCategories();
                List<StopWorksCategoriesBLL> StopWorksCategoriesBLLList = new List<StopWorksCategoriesBLL>();
                if (StopWorksCategoriesList.Count > 0)
                {
                    foreach (var item in StopWorksCategoriesList)
                    {
                        StopWorksCategoriesBLLList.Add(new StopWorksCategoriesBLL()
                        {
                            StopWorkCategoryID = item.StopWorkCategoryID,
                            StopWorkCategoryName = item.StopWorkCategoryName
                        });
                    }
                }

                return StopWorksCategoriesBLLList;
            }
            catch
            {
                throw;
            }
        }

        public StopWorksCategoriesBLL GetByStopWorkCategoryID(int StopWorkCategoryID)
        {
            try
            {
                StopWorksCategories StopWorkCategory = new StopWorksCategoriesDAL().GetByStopWorkCategoryID(StopWorkCategoryID);
                return new StopWorksCategoriesBLL()
                {
                    StopWorkCategoryID = StopWorkCategory.StopWorkCategoryID,
                    StopWorkCategoryName = StopWorkCategory.StopWorkCategoryName
                };
            }
            catch
            {
                throw;
            }
        }

        internal StopWorksCategoriesBLL MapInternshipScholarshipType(StopWorksCategories StopWorkCategory)
        {
            try
            {
                StopWorksCategoriesBLL StopWorksCategoriesBLL = null;
                if (StopWorkCategory != null)
                {
                    StopWorksCategoriesBLL = new StopWorksCategoriesBLL()
                    {
                        StopWorkCategoryID = StopWorkCategory.StopWorkCategoryID,
                        StopWorkCategoryName = StopWorkCategory.StopWorkCategoryName
                    };
                }
                return StopWorksCategoriesBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
