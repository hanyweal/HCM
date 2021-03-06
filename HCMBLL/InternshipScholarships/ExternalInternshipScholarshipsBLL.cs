﻿using HCMBLL.Enums;
using HCMDAL;
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
    public class ExternalInternshipScholarshipsBLL : BaseInternshipScholarshipsBLL
    {
        public virtual CountriesBLL Country
        {
            get;
            set;
        }

        public override Result Add()
        {
            try
            {
                Result result = base.Add();
                if (result != null)
                    return result;

                result = new Result();
                InternshipScholarships InternshipScholarship = DALInstance;
                InternshipScholarship.CountryID = this.Country.CountryID;
                InternshipScholarship.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                this.InternshipScholarshipID = new InternshipScholarshipsDAL().Insert(InternshipScholarship);
                if (this.InternshipScholarshipID != 0)
                {
                    result.Entity = this;
                    result.EnumType = typeof(InternshipScholarshipsValidationEnum);
                    result.EnumMember = InternshipScholarshipsValidationEnum.Done.ToString();
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public override Result Update()
        {
            try
            {
                Result result = base.Update();
                if (result != null)
                    return result;

                result = new Result();
                InternshipScholarships InternshipScholarship = DALInstance;
                InternshipScholarship.CountryID = this.Country.CountryID;
                InternshipScholarship.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                this.InternshipScholarshipID = new InternshipScholarshipsDAL().Update(InternshipScholarship);
                if (this.InternshipScholarshipID != 0)
                {
                    result.Entity = this;
                    result.EnumType = typeof(InternshipScholarshipsValidationEnum);
                    result.EnumMember = InternshipScholarshipsValidationEnum.Done.ToString();
                }
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}