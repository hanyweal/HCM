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
    public class GendersBLL : CommonEntity
    {
        public int GenderID
        {
            get;
            set;
        }

        public string GenderName
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

        public List<GendersBLL> GetGenders()
        {
            List<Genders> GendersList = new GendersDAL().GetGenders();
            List<GendersBLL> GendersBLLList = new List<GendersBLL>();
            if (GendersList.Count > 0)
            {
                foreach (var item in GendersList)
                {
                    GendersBLLList.Add(new GendersBLL()
                    {
                        GenderID = item.GenderID,
                        GenderName = item.GenderName
                    });
                }
            }

            return GendersBLLList;
        }

        public GendersBLL GetByGenderID(int GenderID)
        {
            return GetGenders().SingleOrDefault(x => x.GenderID.Equals(GenderID));
        }

        internal GendersBLL MapGenders(Genders Gender)
        {
            try
            {
                GendersBLL GendersBLL = null;
                if (Gender != null)
                {
                    GendersBLL = new GendersBLL()
                    {
                        GenderID = Gender.GenderID,
                        GenderName = Gender.GenderName
                    };
                }
                return GendersBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}