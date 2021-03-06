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
using System.Globalization;
using System.Linq;
using System.Text;

namespace HCMBLL
{
    public class BaseInternshipScholarshipsBLL : CommonEntity, IEntity
    {
        public virtual int InternshipScholarshipID
        {
            get;
            set;
        }

        public virtual DateTime InternshipScholarshipStartDate
        {
            get;
            set;
        }

        public virtual int InternshipScholarshipPeriod
        {
            get
            {
                return this.InternshipScholarshipEndDate.Subtract(this.InternshipScholarshipStartDate).Days + 1;
            }
        }

        public virtual DateTime InternshipScholarshipEndDate
        {
            get;
            set;
        }

        public virtual InternshipScholarshipsTypesBLL InternshipScholarshipType
        {
            get;
            set;
        }

        public virtual string InternshipScholarshipLocation
        {
            get;
            set;
        }

        public virtual string InternshipScholarshipReason
        {
            get;
            set;
        }

        internal virtual InternshipScholarships DALInstance
        {
            get
            {
                InternshipScholarships InternshipScholarship = new InternshipScholarships();
                InternshipScholarship.InternshipScholarshipTypeID = this.InternshipScholarshipType.InternshipScholarshipTypeID;
                InternshipScholarship.InternshipScholarshipStartDate = this.InternshipScholarshipStartDate;
                InternshipScholarship.InternshipScholarshipEndDate = this.InternshipScholarshipEndDate;
                InternshipScholarship.InternshipScholarshipReason = this.InternshipScholarshipReason;
                InternshipScholarship.InternshipScholarshipLocation = this.InternshipScholarshipLocation;
                InternshipScholarship.IsActive = true;
                if (this.InternshipScholarshipID <= 0)
                {
                    InternshipScholarship.CreatedDate = DateTime.Now;
                    InternshipScholarship.CreatedBy = LoginIdentity.EmployeeCodeID;
                    foreach (InternshipScholarshipsDetailsBLL item in this.InternshipScholarshipsDetails)
                    {
                        InternshipScholarship.InternshipScholarshipsDetails.Add(new InternshipScholarshipsDetails()
                        {
                            EmployeeCareerHistoryID = item.EmployeeCareerHistory.EmployeeCareerHistoryID,
                            CreatedDate = DateTime.Now,
                            CreatedBy = this.LoginIdentity.EmployeeCodeID,
                            IsPassengerOrderCompensation=item.IsPassengerOrderCompensation
                        });
                    }
                }
                else
                {
                    InternshipScholarship.InternshipScholarshipID = this.InternshipScholarshipID;
                    InternshipScholarship.LastUpdatedDate = DateTime.Now;
                    InternshipScholarship.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                }
                return InternshipScholarship;
            }
        }

        public virtual List<InternshipScholarshipsDetailsBLL> InternshipScholarshipsDetails
        {
            get;
            set;
        }

        public virtual List<BaseInternshipScholarshipsBLL> GetInternshipScholarships()
        {
            List<BaseInternshipScholarshipsBLL> InternshipScholarshipBLLList = new List<BaseInternshipScholarshipsBLL>();
            List<InternshipScholarships> InternshipScholarships = new InternshipScholarshipsDAL().GetInternshipScholarships();
            foreach (var item in InternshipScholarships)
                InternshipScholarshipBLLList.Add(MapInternshipScholarship(item));

            return InternshipScholarshipBLLList;
        }

        public BaseInternshipScholarshipsBLL GetByInternshipScholarshipID(int InternshipScholarshipID)
        {
            BaseInternshipScholarshipsBLL InternshipScholarshipBLL = null;
            InternshipScholarships InternshipScholarship = new InternshipScholarshipsDAL().GetByInternshipScholarshipID(InternshipScholarshipID);
            if (InternshipScholarship != null)
                InternshipScholarshipBLL = MapInternshipScholarship(InternshipScholarship);

            return InternshipScholarshipBLL;
        }

        public virtual Result Add()
        {
            Result result = new Result() { Entity = null, EnumType = typeof(InternshipScholarshipsValidationEnum), EnumMember = InternshipScholarshipsValidationEnum.Done.ToString() };

            // validate employees
            if (DALInstance.InternshipScholarshipsDetails == null || DALInstance.InternshipScholarshipsDetails.Count <= 0)
            {
                result = new Result();
                result.Entity = null;
                result.EnumType = typeof(InternshipScholarshipsValidationEnum);
                result.EnumMember = InternshipScholarshipsValidationEnum.RejectedBecauseEmployeeRequired.ToString();

                return result;
            }

            // validate 
            foreach (InternshipScholarshipsDetailsBLL dd in this.InternshipScholarshipsDetails)
            {
                result = CommonHelper.IsNoConflictWithOtherProcess(dd.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID,this.InternshipScholarshipStartDate, this.InternshipScholarshipEndDate);
                //if (result.EnumMember != NoConflictWithOtherProcessValidationEnum.Done.ToString())
                //    return result;
                if (result != null)
                    return result;
                else
                    result = new Result();
            }

            // setting result is null in case of "DONE"
            if (result.EnumMember == InternshipScholarshipsValidationEnum.Done.ToString())
                result = null;

            return result;
        }

        public virtual Result Update()
        {
            Result result = null;
            return result;
        }

        public Result Remove(int InternshipScholarshipID)
        {
            try
            {
                Result result = null;
                new InternshipScholarshipsDAL().Delete(InternshipScholarshipID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(InternshipScholarshipsValidationEnum),
                    EnumMember = InternshipScholarshipsValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        internal BaseInternshipScholarshipsBLL MapInternshipScholarship(InternshipScholarships InternshipScholarship)
        {
            try
            {
                BaseInternshipScholarshipsBLL InternshipScholarshipBLL = null;
                if (InternshipScholarship != null)
                {
                    KSACitiesBLL KSACity1 = InternshipScholarship.KSACities != null ? new KSACitiesBLL().MapKSACity(InternshipScholarship.KSACities) : null;
                    CountriesBLL Country1 = InternshipScholarship.Countries != null ? new CountriesBLL().MapCountry(InternshipScholarship.Countries) : null;
                    if (InternshipScholarship.InternshipScholarshipTypeID == (Int32)InternshipScholarshipsTypesEnum.Internal)
                    {
                        InternshipScholarshipBLL = new InternalInternshipScholarshipsBLL()
                        {
                            KSACity = KSACity1,
                        };

                    }
                    else if (InternshipScholarship.InternshipScholarshipTypeID == (int)InternshipScholarshipsTypesEnum.External)
                    {
                        InternshipScholarshipBLL = new ExternalInternshipScholarshipsBLL()
                        {
                            Country = Country1,
                        };
                    }

                    InternshipScholarshipBLL.InternshipScholarshipID = InternshipScholarship.InternshipScholarshipID;
                    InternshipScholarshipBLL.InternshipScholarshipType = new InternshipScholarshipsTypesBLL().MapInternshipScholarshipType(InternshipScholarship.InternshipScholarshipsTypes);
                    InternshipScholarshipBLL.InternshipScholarshipStartDate = InternshipScholarship.InternshipScholarshipStartDate;
                    InternshipScholarshipBLL.InternshipScholarshipEndDate = InternshipScholarship.InternshipScholarshipEndDate;
                    InternshipScholarshipBLL.InternshipScholarshipReason = InternshipScholarship.InternshipScholarshipReason;
                    InternshipScholarshipBLL.InternshipScholarshipLocation = InternshipScholarship.InternshipScholarshipLocation;
                    InternshipScholarshipBLL.CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(InternshipScholarship.CreatedByNav);
                    InternshipScholarshipBLL.CreatedDate = InternshipScholarship.CreatedDate;
                    //InternshipScholarshipBLL.LastUpdatedBy = new EmployeesCodesBLL().MapEmployeeCode(InternshipScholarship.EmployeesCodes1);
                    //InternshipScholarshipBLL.LastUpdatedDate = InternshipScholarship.LastUpdatedDate;
                }
                return InternshipScholarshipBLL;
            }
            catch
            {
                throw;
            }
        }

        internal int GetDaysWithTwoDates(int EmployeeCodeID, DateTime StartDate, DateTime EndDate)
        {
            DateTime InternshipStartDate, InternshipEndDate, Date;
            int DaysCounter = 0;            
            List<InternshipScholarshipsDetails> InternshipScholarships = 
                new InternshipScholarshipsDetailsDAL().GetInternshipScholarshipsDetailsByEmployeeCodeID(EmployeeCodeID).Where(o => o.InternshipScholarships.IsActive).ToList();

            for (DateTime i = StartDate; i <= EndDate; )
            {
                Date = i;// Convert.ToDateTime(Globals.Calendar.UmAlquraToGreg(string.Format("{0}/{1}/{2}", i.Day, i.Month, i.Year)), new CultureInfo("en-US"));

                foreach (InternshipScholarshipsDetails item in InternshipScholarships)
                {
                    InternshipStartDate = item.InternshipScholarships.InternshipScholarshipStartDate.Date;
                    InternshipEndDate = item.InternshipScholarships.InternshipScholarshipEndDate.Date;

                    if (Date >= InternshipStartDate && Date <= InternshipEndDate)
                        DaysCounter++;
                }

                i = i.AddDays(1);
            }

            return DaysCounter;
        }

        public List<BaseInternshipScholarshipsBLL> GetByEmployeeCodeID(int EmployeeCodeID, DateTime StartDate, DateTime EndDate)
        {
            return new EmployeesCodesBLL().GetInternshipScholarshipsByEmployeeCodeID(EmployeeCodeID)
                .Where(
                     x =>
                        (StartDate >= x.InternshipScholarshipStartDate && StartDate <= x.InternshipScholarshipEndDate) ||
                        (EndDate >= x.InternshipScholarshipStartDate && EndDate <= x.InternshipScholarshipEndDate) ||
                        (StartDate >= x.InternshipScholarshipStartDate && EndDate <= x.InternshipScholarshipEndDate) ||
                        (StartDate <= x.InternshipScholarshipStartDate && EndDate >= x.InternshipScholarshipEndDate)
                      )
                .ToList();
        }
    }
}