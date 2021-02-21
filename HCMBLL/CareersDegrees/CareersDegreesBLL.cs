using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMDAL;

namespace HCMBLL
{
    public class CareersDegreesBLL
    {
        public int CareerDegreeID { get; set; }

        public string CareerDegreeName { get; set; }

        public List<CareersDegreesBLL> GetCareersDegrees()
        {
            List<CareersDegrees> CareersDegreesList = new CareersDegreesDAL().GetCareersDegrees();
            List<CareersDegreesBLL> CareersDegreesBLLList = new List<CareersDegreesBLL>();
            foreach (var item in CareersDegreesList)
            {
                CareersDegreesBLLList.Add(new CareersDegreesBLL().MapCareerDegree(item));
            }
            return CareersDegreesBLLList.OrderBy(x => Convert.ToInt16(x.CareerDegreeName)).ToList();
        }

        internal CareersDegreesBLL MapCareerDegree(CareersDegrees CareerDegree)
        {
            try
            {
                CareersDegreesBLL CareerDegreeBLL = null;
                if (CareerDegree != null)
                {
                    CareerDegreeBLL = new CareersDegreesBLL()
                    {
                        CareerDegreeID = CareerDegree.CareerDegreeID,
                        CareerDegreeName = CareerDegree.CareerDegreeName
                    };
                }
                return CareerDegreeBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
