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
    public class QualificationsDegreesBLL : CommonEntity, IEntity
    {
        public virtual int QualificationDegreeID
        {
            get;
            set;
        }

        public virtual string QualificationDegreeName
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

        public int Weight { get; set; }
    
        public virtual IEnumerable<OrganizationsJobsBLL> OrganizationsJobsBLL
        {
            get;
            set;
        }

        public List<QualificationsDegreesBLL> GetQualificationsDegrees()
        {
            List<QualificationsDegreesBLL> QualificationsDegreesBLLList = new List<QualificationsDegreesBLL>();
            List<QualificationsDegrees> QualificationsDegreesList = new QualificationsDegreesDAL().GetQualificationsDegrees();
            foreach (var QualificationDegree in QualificationsDegreesList)
            {
                QualificationsDegreesBLLList.Add(this.MapQualificationDegree(QualificationDegree));
            }
            return QualificationsDegreesBLLList;
        }

        public QualificationsDegreesBLL GetByQualificationDegreeID(int QualificationDegreeID)
        {
            //return GetQualificationsDegrees().SingleOrDefault(x => x.QualificationDegreeID.Equals(QualificationDegreeID));
            return this.MapQualificationDegree(new QualificationsDegreesDAL().GetByQualificationDegreeID(QualificationDegreeID));
        }

        public virtual Result Add()
        {
            try
            {
                Result result = null;
                QualificationsDegrees QualificationsDegree = new QualificationsDegrees();
                QualificationsDegree.QualificationDegreeName = this.QualificationDegreeName;
                //QualificationsDegree.DirectPoints = this.DirectPoints;
                //QualificationsDegree.IndirectPoints = this.IndirectPoints;
                QualificationsDegree.CreatedDate = DateTime.Now;
                QualificationsDegree.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                this.QualificationDegreeID = new QualificationsDegreesDAL().Insert(QualificationsDegree);

                result = new Result();
                result.Entity = this;
                result.EnumType = typeof(QualificationsDegreesValidationEnum);
                result.EnumMember = QualificationsDegreesValidationEnum.Done.ToString();
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
                QualificationsDegrees QualificationsDegree = new QualificationsDegrees();
                QualificationsDegree.QualificationDegreeID = this.QualificationDegreeID;
                QualificationsDegree.QualificationDegreeName = this.QualificationDegreeName;
                //QualificationsDegree.DirectPoints = this.DirectPoints;
                //QualificationsDegree.IndirectPoints = this.IndirectPoints;
                QualificationsDegree.LastUpdatedDate = DateTime.Now;
                QualificationsDegree.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;

                int UpdateResult = new QualificationsDegreesDAL().Update(QualificationsDegree);
                if (UpdateResult != 0)
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(QualificationsDegreesValidationEnum);
                    result.EnumMember = QualificationsDegreesValidationEnum.Done.ToString();
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public Result Remove(int QualificationDegreeID)
        {
            try
            {
                Result result = null;
                new QualificationsDegreesDAL().Delete(QualificationDegreeID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(QualificationsDegreesValidationEnum),
                    EnumMember = QualificationsDegreesValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        internal QualificationsDegreesBLL MapQualificationDegree(QualificationsDegrees item)
        {
            return item != null ?
                new QualificationsDegreesBLL()
                {
                    QualificationDegreeID = item.QualificationDegreeID,
                    QualificationDegreeName = item.QualificationDegreeName,
                    DirectPoints = item.DirectPoints,
                    IndirectPoints = item.IndirectPoints,
                    Weight = item.Weight.HasValue ? item.Weight.Value : 0,
                    CreatedBy = item.CreatedBy.HasValue ? new EmployeesCodesBLL().MapEmployeeCode(item.EmployeesCodes) : null,
                    CreatedDate = item.CreatedDate.Value
                }
                : null;
        }
    }
}

