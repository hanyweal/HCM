using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class VacationsTypesBLL
    {
        public int VacationTypeID { get; set; }

        public string VacationTypeName { get; set; }

        public bool? IsPossibleToBeCreatedFromEVacationRequest { get; set; }

        public List<VacationsTypesBLL> GetVacationsTypes()
        {
            try
            {
                List<VacationsTypes> VacationsTypesList = new VacationsTypesDAL().GetVacationsTypes();
                List<VacationsTypesBLL> VacationsTypesBLLList = new List<VacationsTypesBLL>();
                foreach (var item in VacationsTypesList)
                    VacationsTypesBLLList.Add(MapVacationsTypes(item));

                return VacationsTypesBLLList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VacationsTypesBLL> GetVacationsTypes(int GenderID, bool? IsPossibleTobeCreatedFromEVacationRequest = null)
        {
            try
            {
                List<VacationsTypes> VacationsTypesList = null; 
                
                if (GenderID == (Int16)GendersEnum.Male)               
                    VacationsTypesList = new VacationsTypesDAL().GetVacationsTypes().Where(x => x.IsForFemaleOnly == false).ToList();               
                else               
                    VacationsTypesList = new VacationsTypesDAL().GetVacationsTypes();  
                
                List<VacationsTypesBLL> VacationsTypesBLLList = new List<VacationsTypesBLL>();

                foreach (var item in VacationsTypesList)
                    VacationsTypesBLLList.Add(MapVacationsTypes(item));

                return VacationsTypesBLLList.Where(x => IsPossibleTobeCreatedFromEVacationRequest.HasValue ? x.IsPossibleToBeCreatedFromEVacationRequest == IsPossibleTobeCreatedFromEVacationRequest : x.IsPossibleToBeCreatedFromEVacationRequest == x.IsPossibleToBeCreatedFromEVacationRequest).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal VacationsTypesBLL MapVacationsTypes(VacationsTypes item)
        {
            return item != null ?
                new VacationsTypesBLL()
                {
                    VacationTypeID = item.VacationTypeID,
                    VacationTypeName = item.VacationTypeName,
                    IsPossibleToBeCreatedFromEVacationRequest = item.IsPossibleTobeCreatedFromEVacationRequest
                }
                : null;
        }

    }
}
