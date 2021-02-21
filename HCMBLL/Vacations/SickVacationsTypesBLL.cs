using HCMDAL;
using System;
using System.Collections.Generic;

namespace HCMBLL
{

    public class SickVacationsTypesBLL
    {
        public int SickVacationTypeID { get; set; }

        public string SickVacationTypeName { get; set; }

        public List<SickVacationsTypesBLL> GetSickVacationsTypes()
        {
            try
            {
                List<SickVacationsTypes> SickVacationsTypesList = new SickVacationsTypesDAL().GetSickVacationsTypes();
                List<SickVacationsTypesBLL> SickVacationsTypesBLLList = new List<SickVacationsTypesBLL>();
                foreach (var item in SickVacationsTypesList)
                {
                    SickVacationsTypesBLLList.Add(new SickVacationsTypesBLL() { SickVacationTypeID = item.SickVacationTypeID, SickVacationTypeName = item.SickVacationTypeName });
                }
                return SickVacationsTypesBLLList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal SickVacationsTypesBLL MapSickVacationsTypes(SickVacationsTypes item)
        {
            return item != null ?
                new SickVacationsTypesBLL()
                {
                    SickVacationTypeID = item.SickVacationTypeID,
                    SickVacationTypeName = item.SickVacationTypeName
                }
                : null;
        }

    }
}

