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
    public class GeneralSpecializationsBLL : CommonEntity, IEntity
    {
        public virtual int GeneralSpecializationID
        {
            get;
            set;
        }
        public virtual string GeneralSpecializationName
        {
            get;
            set;
        }
        public virtual QualificationsBLL Qualification
        {
            get;
            set;
        }
        public virtual decimal? DirectPoints
        {
            get;
            set;
        }
        public virtual decimal? IndirectPoints
        {
            get;
            set;
        }
        public List<GeneralSpecializationsBLL> GetGeneralSpecializations()
        {
            List<GeneralSpecializations> GeneralSpecializationList = new GeneralSpecializationsDAL().GetGeneralSpecializations();
            List<GeneralSpecializationsBLL> GeneralSpecializationsBLLList = new List<GeneralSpecializationsBLL>();
            foreach (var GeneralSpecialization in GeneralSpecializationList)
            {
                GeneralSpecializationsBLLList.Add(this.MapGeneralSpecialization(GeneralSpecialization));
            }
            return GeneralSpecializationsBLLList;

        }
        public virtual GeneralSpecializationsBLL GetByGeneralSpecializationID(int GeneralSpecializationID)
        {
            return this.MapGeneralSpecialization(new GeneralSpecializationsDAL().GetByGeneralSpecializationID(GeneralSpecializationID));
        }
        //public List<GeneralSpecializationsBLL> GetByQualificationID(int QualificationID)
        //{
        //    return GetGeneralSpecializations().Where(x => x.Qualification.QualificationID.Equals(QualificationID)).ToList();
        //}
        public List<GeneralSpecializationsBLL> GetByQualificationID(int QualificationID)
        {
            List<GeneralSpecializations> GeneralSpecializationList = new GeneralSpecializationsDAL().GetByQualificationID(QualificationID);
            List<GeneralSpecializationsBLL> GeneralSpecializationsBLLList = new List<GeneralSpecializationsBLL>();
            foreach (var GeneralSpecialization in GeneralSpecializationList)
            {
                GeneralSpecializationsBLLList.Add(this.MapGeneralSpecialization(GeneralSpecialization));
            }
            return GeneralSpecializationsBLLList;
        }

        public virtual Result Add()
        {
            try
            {
                Result result = null;
                GeneralSpecializations GeneralSpecialization = new GeneralSpecializations();
                GeneralSpecialization.GeneralSpecializationName = this.GeneralSpecializationName;
                GeneralSpecialization.QualificationID = this.Qualification.QualificationID;
                GeneralSpecialization.DirectPoints = this.DirectPoints;
                GeneralSpecialization.IndirectPoints = this.IndirectPoints;
                GeneralSpecialization.CreatedDate = DateTime.Now;
                GeneralSpecialization.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                this.GeneralSpecializationID = new GeneralSpecializationsDAL().Insert(GeneralSpecialization);

                result = new Result();
                result.Entity = this;
                result.EnumType = typeof(GeneralSpecializationsValidationEnum);
                result.EnumMember = GeneralSpecializationsValidationEnum.Done.ToString();
                return result;
            }
            catch
            {
                throw;
            }
        }

        public virtual Result Update()
        {
            try
            {
                Result result = null;

                #region Validation for GeneralSpecializationID EmployeeQualifications
                EmployeesQualificationsBLL EmployeeQualificationBLL = new EmployeesQualificationsBLL().GetByGeneralSpecializationID(this.GeneralSpecializationID);
                if (EmployeeQualificationBLL != null)
                {
                    if (EmployeeQualificationBLL.Qualification.QualificationID != this.Qualification.QualificationID)
                    {
                        result = new Result();
                        result.Entity = null;
                        result.EnumType = typeof(GeneralSpecializationsValidationEnum);
                        result.EnumMember = GeneralSpecializationsValidationEnum.RejectedBecauseOfThisGeneralSpecializationExistsInEmployeesQualifications.ToString();
                        return result;
                    }
                }
                #endregion

                #region Validation for GeneralSpecializationID JobsCategoriesQualifications
                JobsCategoriesQualificationsBLL JobsCategoriesQualificationsBLL = new JobsCategoriesQualificationsBLL().GetByGeneralSpecializationID(this.GeneralSpecializationID);
                if (JobsCategoriesQualificationsBLL != null)
                {
                    if (JobsCategoriesQualificationsBLL.Qualification.QualificationID != this.Qualification.QualificationID)
                    {
                        result = new Result();
                        result.Entity = null;
                        result.EnumType = typeof(GeneralSpecializationsValidationEnum);
                        result.EnumMember = GeneralSpecializationsValidationEnum.RejectedBecauseOfThisGeneralSpecializationExistsInJobsCategoriesQualifications.ToString();
                        return result;
                    }
                }
                #endregion

                GeneralSpecializations GeneralSpecialization = new GeneralSpecializations();
                GeneralSpecialization.GeneralSpecializationID = this.GeneralSpecializationID;
                GeneralSpecialization.GeneralSpecializationName = this.GeneralSpecializationName;
                GeneralSpecialization.QualificationID = this.Qualification.QualificationID;
                GeneralSpecialization.DirectPoints = this.DirectPoints;
                GeneralSpecialization.IndirectPoints = this.IndirectPoints;
                GeneralSpecialization.LastUpdatedDate = DateTime.Now;
                GeneralSpecialization.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;

                int UpdateResult = new GeneralSpecializationsDAL().Update(GeneralSpecialization);
                if (UpdateResult != 0)
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(GeneralSpecializationsValidationEnum);
                    result.EnumMember = GeneralSpecializationsValidationEnum.Done.ToString();
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public Result Remove(int GeneralSpecializationID)
        {
            try
            {
                Result result = null;
                new GeneralSpecializationsDAL().Delete(GeneralSpecializationID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(GeneralSpecializationsValidationEnum),
                    EnumMember = GeneralSpecializationsValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        internal GeneralSpecializationsBLL MapGeneralSpecialization(GeneralSpecializations item)
        {
            return item != null ?
                new GeneralSpecializationsBLL()
                {
                    GeneralSpecializationID = item.GeneralSpecializationID,
                    GeneralSpecializationName = item.GeneralSpecializationName,
                    Qualification = item.QualificationID.HasValue ? new QualificationsBLL().MapQualification(item.Qualifications) : null,
                    DirectPoints = item.DirectPoints,
                    IndirectPoints = item.IndirectPoints,
                    CreatedBy = item.CreatedBy.HasValue ? new EmployeesCodesBLL().MapEmployeeCode(item.EmployeesCodes) : null,
                    CreatedDate = item.CreatedDate.Value
                }
                : null;
        }
    }
}
