using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class GovernmentFundsDeactiveReasonsBLL
    {
        public virtual int GovernmentFundDeactiveReasonID
        {
            get;
            set;
        }
        public virtual string GovernmentFundDeactiveReasonName
        {
            get;
            set;
        }
        public virtual List<GovernmentFundsDeactiveReasonsBLL> GetGovernmentFundsDeactiveReasons()
        {
            List<GovernmentFundsDeactiveReasons> GovernmentFundsDeactiveReasonsList = new GovernmentFundsDeactiveReasonsDAL().GetGovernmentFundsDeactiveReasons();
            List<GovernmentFundsDeactiveReasonsBLL> GovernmentFundsDeactiveReasonsBLLList = new List<GovernmentFundsDeactiveReasonsBLL>();
            if (GovernmentFundsDeactiveReasonsList.Count > 0)
            {
                foreach (var item in GovernmentFundsDeactiveReasonsList)
                {
                    GovernmentFundsDeactiveReasonsBLLList.Add(new GovernmentFundsDeactiveReasonsBLL()
                    {
                        GovernmentFundDeactiveReasonID = item.GovernmentFundDeactiveReasonID,
                        GovernmentFundDeactiveReasonName = item.GovernmentFundDeactiveReasonName,
                    });
                }
            }

            return GovernmentFundsDeactiveReasonsBLLList;
        }
        public GovernmentFundsDeactiveReasonsBLL GetByGovernmentFundDeactiveReasonID(int GovernmentFundDeactiveReasonID)
        {
            return GetGovernmentFundsDeactiveReasons().SingleOrDefault(x => x.GovernmentFundDeactiveReasonID.Equals(GovernmentFundDeactiveReasonID));
        }

        internal GovernmentFundsDeactiveReasonsBLL MapGovernmentFundsDeactiveReasonsBLL(GovernmentFundsDeactiveReasons GovernmentFundsDeactiveReason)
        {
            GovernmentFundsDeactiveReasonsBLL GovernmentFundsDeactiveReasonsBLL = null;
            if (GovernmentFundsDeactiveReason != null)
            {
                GovernmentFundsDeactiveReasonsBLL = new GovernmentFundsDeactiveReasonsBLL()
                {


                    GovernmentFundDeactiveReasonID = GovernmentFundsDeactiveReason.GovernmentFundDeactiveReasonID,
                    GovernmentFundDeactiveReasonName = GovernmentFundsDeactiveReason.GovernmentFundDeactiveReasonName,


                };
            }
            return GovernmentFundsDeactiveReasonsBLL;
        }

    }
}