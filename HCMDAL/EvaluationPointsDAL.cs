using System.Collections.Generic;
using System.Linq;

namespace HCMDAL
{
    public class EvaluationPointsDAL
    {
        public List<EvaluationPoints> GetEvaluationPoints()
        {
            try
            {
                var db = new HCMEntities();
                return db.EvaluationPoints.ToList();
            }
            catch
            {
                throw;
            }
        }

        public EvaluationPoints GetByEvaluationPointID(int EvaluationPointID)
        {
            try
            {
                var db = new HCMEntities();
                return db.EvaluationPoints.FirstOrDefault(e => e.EvaluationPointID == EvaluationPointID);
            }
            catch
            {
                throw;
            }
        }
    }
}

