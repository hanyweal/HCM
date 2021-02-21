﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HCMDAL;

namespace HCMBLL
{
    public class GovernmentDeductionsTypesBLL : CommonEntity
    {
        public virtual int GovernmentDeductionTypeID
        {
            get;
            set;
        }

        public virtual string GovernmentDeductionTypeName
        {
            get;
            set;
        }

        public virtual IEnumerable<GovernmentFundsBLL> GovernmentFundsBLL
        {
            get;
            set;
        }

        public virtual List<GovernmentDeductionsTypesBLL> GetGovernmentDeductionsTypes()
        {
            try
            {
                List<GovernmentDeductionsTypes> GovernmentDeductionsTypesList = new GovernmentDeductionsTypesDAL().GetGovernmentDeductionTypes();
                List<GovernmentDeductionsTypesBLL> GovernmentDeductionsTypesBLList = new List<GovernmentDeductionsTypesBLL>();
                if (GovernmentDeductionsTypesList.Count > 0)
                {
                    foreach (var item in GovernmentDeductionsTypesList)
                    {
                        GovernmentDeductionsTypesBLList.Add(new GovernmentDeductionsTypesBLL()
                        {
                            GovernmentDeductionTypeID = item.GovernmentDeductionTypeID,
                            GovernmentDeductionTypeName = item.GovernmentDeductionTypeName
                        });
                    }
                }

                return GovernmentDeductionsTypesBLList;
            }
            catch
            {
                throw;
            }
        }

        internal GovernmentDeductionsTypesBLL MapGovernmentDeductionsTypesBLL(GovernmentDeductionsTypes GovernmentDeductionsType)
        {
            GovernmentDeductionsTypesBLL GovernmentDeductionsTypeBLL = null;
            if (GovernmentDeductionsType != null)
            {
                GovernmentDeductionsTypeBLL = new GovernmentDeductionsTypesBLL()
                {
                    GovernmentDeductionTypeID = GovernmentDeductionsType.GovernmentDeductionTypeID,
                    GovernmentDeductionTypeName = GovernmentDeductionsType.GovernmentDeductionTypeName
                };
            }
            return GovernmentDeductionsTypeBLL;
        }

    }
}

