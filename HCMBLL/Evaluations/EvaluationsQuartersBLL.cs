using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class EvaluationsQuartersBLL
    {
        public int EvaluationQuarterID { get; set; }

        public string EvaluationQuarterName { get; set; }

        public virtual List<EvaluationsQuartersBLL> GetEvaluationsQuarters()
        {
            List<EvaluationsQuarters> EvaluationsQuartersList = new EvaluationsQuartersDAL().GetEvaluationsQuarters();
            List<EvaluationsQuartersBLL> EvaluationsQuartersBLLList = new List<EvaluationsQuartersBLL>();
            if (EvaluationsQuartersList.Count > 0)
            {
                foreach (var item in EvaluationsQuartersList)
                    EvaluationsQuartersBLLList.Add(MapEvaluationQuarter(item));
            }

            return EvaluationsQuartersBLLList;
        }

        internal EvaluationsQuartersBLL MapEvaluationQuarter(EvaluationsQuarters EvaluationQuarter)
        {
            try
            {
                EvaluationsQuartersBLL EvaluationQuarterBLL = null;
                if (EvaluationQuarter != null)
                {
                    EvaluationQuarterBLL = new EvaluationsQuartersBLL()
                    {
                        EvaluationQuarterID = EvaluationQuarter.EvaluationQuarterID,
                        EvaluationQuarterName = EvaluationQuarter.EvaluationQuarterName
                    };
                }
                return EvaluationQuarterBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}