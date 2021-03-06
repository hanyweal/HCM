﻿using HCMDAL;
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCMBLL
{
    public class HolidaysTypesBLL : CommonEntity
    {
        public int HolidayTypeID
        {
            get;
            set;
        }

        public string HolidayTypeName
        {
            get;
            set;
        }

        public int Add()
        {
            throw new System.NotImplementedException();
        }

        public int Update()
        {
            throw new System.NotImplementedException();
        }

        public int Remove()
        {
            throw new System.NotImplementedException();
        }

        public List<HolidaysTypesBLL> GetHolidaysTypes()
        {
            List<HolidaysTypes> HolidaysTypesList = new HolidaysTypesDAL().GetHolidaysTypes();
            List<HolidaysTypesBLL> HolidaysTypesBLLList = new List<HolidaysTypesBLL>();
            if (HolidaysTypesList.Count > 0)
            {
                foreach (var item in HolidaysTypesList)
                {
                    HolidaysTypesBLLList.Add(new HolidaysTypesBLL()
                    {
                        HolidayTypeID = item.HolidayTypeID,
                        HolidayTypeName = item.HolidayTypeName
                    });
                }
            }

            return HolidaysTypesBLLList;
        }

        public HolidaysTypesBLL GetByHolidayTypeID(int HolidayTypeID)
        {
            return GetHolidaysTypes().SingleOrDefault(x => x.HolidayTypeID.Equals(HolidayTypeID));
        }

        internal HolidaysTypesBLL MapHolidayType(HolidaysTypes item)
        {
            return item != null ? 
                new HolidaysTypesBLL()
                {
                    HolidayTypeID = item.HolidayTypeID,
                    HolidayTypeName = item.HolidayTypeName
                } 
                : null;
        }
    }
}