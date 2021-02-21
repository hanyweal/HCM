using HCMDAL;
using System;
using System.Collections.Generic;

namespace HCMBLL
{
    public class NormalVacationsTypesBLL
    {
        public int NormalVacationTypeID { get; set; }

        public string NormalVacationTypeName { get; set; }

        public List<NormalVacationsTypesBLL> GetNormalVacationsTypes()
        {
            try
            {
                List<NormalVacationsTypes> NormalVacationsTypesList = new NormalVacationsTypesDAL().GetNormalVacationsTypes();
                List<NormalVacationsTypesBLL> NormalVacationsTypesBLLList = new List<NormalVacationsTypesBLL>();
                foreach (var item in NormalVacationsTypesList)
                {
                    NormalVacationsTypesBLLList.Add(new NormalVacationsTypesBLL() { NormalVacationTypeID = item.NormalVacationTypeID, NormalVacationTypeName = item.NormalVacationTypeName });
                }
                return NormalVacationsTypesBLLList;
            }
            catch (Exception)
            {

                throw;
            }
        }


        internal NormalVacationsTypesBLL MapNormalVacationsTypes(NormalVacationsTypes item)
        {
            return item != null ?
                new NormalVacationsTypesBLL()
                {
                    NormalVacationTypeID = item.NormalVacationTypeID,
                    NormalVacationTypeName = item.NormalVacationTypeName
                }
                : null;
        }

    }
}
