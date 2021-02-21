using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMDAL;
using HCMBLL.Enums; 

namespace HCMBLL
{
    public class BusinessRulesBLL
    {
        public int BusinessRuleID { get; set; }

        public string BusinessRuleAr { get; set; }

        public string BusinessRuleEn { get; set; }

        public bool IsActive { get; set; }

        public BusinessSubCategoriesEnum BusinessSubCategory { get; set; }

        public List<BusinessRulesBLL> GetByBusinessSubCategory(int BusinessSubCategoryID)
        {
            try
            {
                List<BusinessRules> BusinessRulesList = new BusinessRulesDAL().GetByBusinessSubCategory(BusinessSubCategoryID);
                List<BusinessRulesBLL> BusinessRulesBLLList = new List<BusinessRulesBLL>();
                foreach (var item in BusinessRulesList)
                {
                    BusinessRulesBLLList.Add(new BusinessRulesBLL() 
                    { 
                        BusinessRuleAr = item.BusinessRuleAr, 
                        BusinessRuleEn = item.BusinessRuleEn, 
                        BusinessRuleID = item.BusinessRuleID,
                        IsActive = item.IsActive
                    });
                }
                return BusinessRulesBLLList;
            }
            catch
            {
                throw;
            }
        }  
    }
}
