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
    public class EndOfServicesVacationsBLL : CommonEntity, IEntity
    {
        public virtual int EndOfServiceVacationID
        {
            get;
            set;
        }

        public virtual EndOfServicesBLL EndOfService { get; set; }

        public virtual VacationsTypesBLL VacationType { get; set; }

        public int MaxNormalCompensation { get { return 180; } }

        public int VacationPeriod
        {
            get
            {
                return this.VacationEndDate.Subtract(this.VacationStartDate).Days + 1;
            }
        }

        public virtual DateTime VacationStartDate
        {
            get;
            set;
        }

        public virtual DateTime VacationEndDate
        {
            get;
            set;
        }


        public virtual Result Add()
        {
            Result result = null;
            result = new Result();

            #region Check if vacation after his endOfService date.
            EndOfServicesBLL EndOfServicesBLL = new EndOfServicesBLL().GetByEndOfServiceID(this.EndOfService.EndOfServiceID);
            if (EndOfServicesBLL.EndOfServiceDate < this.VacationEndDate)
            {
                result.Entity = this;
                result.EnumType = typeof(EndOfServicesVacationsValidationEnum);
                result.EnumMember = EndOfServicesVacationsValidationEnum.RejectedBecauseOfVacationEndDateBiggerThanEndOfServiceDate.ToString();
                return result;
            }
            #endregion



            EndOfServicesVacations EndOfServiceVacation = new EndOfServicesVacations();
            EndOfServiceVacation.VacationStartDate = this.VacationStartDate;
            EndOfServiceVacation.VacationEndDate = this.VacationEndDate;
            EndOfServiceVacation.EndOfServiceID = this.EndOfService.EndOfServiceID;
            EndOfServiceVacation.VacationTypeID = this.VacationType.VacationTypeID;
            EndOfServiceVacation.CreatedDate = DateTime.Now;
            EndOfServiceVacation.CreatedBy = this.LoginIdentity.EmployeeCodeID;

            this.EndOfServiceVacationID = new EndOfServicesVacationsDAL().Insert(EndOfServiceVacation);
            if (this.EndOfServiceVacationID != 0)
            {
                result.Entity = this;
                result.EnumType = typeof(LookupsValidationEnum);
                result.EnumMember = LookupsValidationEnum.Done.ToString();
            }

            return result;
        }

        public virtual Result Remove(int EndOfServiceVacationID)
        {
            try
            {
                Result result = null;
                new EndOfServicesVacationsDAL().Delete(EndOfServiceVacationID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(LookupsValidationEnum),
                    EnumMember = LookupsValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        public virtual Result Update()
        {
            Result result = new Result();
            EndOfServicesVacations EndOfServiceVacation = new EndOfServicesVacations();
            EndOfServiceVacation.EndOfServiceVacationID = this.EndOfServiceVacationID;
            EndOfServiceVacation.VacationStartDate = this.VacationStartDate;
            EndOfServiceVacation.VacationEndDate = this.VacationEndDate;
            EndOfServiceVacation.EndOfServiceID = this.EndOfService.EndOfServiceID;
            EndOfServiceVacation.VacationTypeID = this.VacationType.VacationTypeID;
            EndOfServiceVacation.LastUpdatedDate = DateTime.Now;
            EndOfServiceVacation.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
            this.EndOfServiceVacationID = new EndOfServicesVacationsDAL().Update(EndOfServiceVacation);
            if (this.EndOfServiceVacationID != 0)
            {
                result.Entity = this;
                result.EnumType = typeof(LookupsValidationEnum);
                result.EnumMember = LookupsValidationEnum.Done.ToString();
            }
            return result;
        }

        public virtual List<EndOfServicesVacationsBLL> GetEndOfServicesVacations()
        {
            List<EndOfServicesVacations> EndOfServicesVacationsList = new EndOfServicesVacationsDAL().GetEndOfServicesVacations();
            List<EndOfServicesVacationsBLL> EndOfServicesVacationsBLLList = new List<EndOfServicesVacationsBLL>();
            if (EndOfServicesVacationsList.Count > 0)
            {
                foreach (var item in EndOfServicesVacationsList)
                {
                    EndOfServicesVacationsBLLList.Add(new EndOfServicesVacationsBLL().MapEndOfServiceVacation(item));
                }
            }

            return EndOfServicesVacationsBLLList;
        }

        public virtual EndOfServicesVacationsBLL GetByEndOfServiceVacationID(int EndOfServiceVacationID)
        {
            EndOfServicesVacations EndOfServiceVacation = new EndOfServicesVacationsDAL().GetByEndOfServiceVacationID(EndOfServiceVacationID);
            EndOfServicesVacationsBLL EndOfServicesVacationsBLL = new EndOfServicesVacationsBLL();
            if (EndOfServiceVacation != null)
            {
                if (EndOfServiceVacation.EndOfServiceVacationID > 0)
                {
                    return new EndOfServicesVacationsBLL().MapEndOfServiceVacation(EndOfServiceVacation);
                }
            }
            return null;
        }

        public virtual List<EndOfServicesVacationsBLL> GetByEndOfServiceID(int EndOfServiceID)
        {
            List<EndOfServicesVacations> EndOfServicesVacationsList = new EndOfServicesVacationsDAL().GetByEndOfServiceID(EndOfServiceID);
            List<EndOfServicesVacationsBLL> EndOfServicesVacationsBLLList = new List<EndOfServicesVacationsBLL>();
            if (EndOfServicesVacationsList.Count > 0)
            {
                foreach (var item in EndOfServicesVacationsList)
                {
                    EndOfServicesVacationsBLLList.Add(new EndOfServicesVacationsBLL().MapEndOfServiceVacation(item));
                }
            }

            return EndOfServicesVacationsBLLList;
        }

        public EndOfServicesVacationsBLL MapEndOfServiceVacation(EndOfServicesVacations EndOfServiceVacation)
        {
            try
            {
                EndOfServicesVacationsBLL EndOfServiceVacationBLL = null;
                if (EndOfServiceVacation != null)
                {
                    EndOfServiceVacationBLL = new EndOfServicesVacationsBLL()
                    {
                        EndOfServiceVacationID = EndOfServiceVacation.EndOfServiceVacationID,
                        VacationStartDate = EndOfServiceVacation.VacationStartDate,
                        VacationEndDate = EndOfServiceVacation.VacationEndDate
                    };
                    if (EndOfServiceVacation.VacationsTypes != null)
                    {
                        EndOfServiceVacationBLL.VacationType = new VacationsTypesBLL().MapVacationsTypes(EndOfServiceVacation.VacationsTypes);
                    }
                    if (EndOfServiceVacation.EndOfServices != null)
                    {
                        EndOfServiceVacationBLL.EndOfService = new EndOfServicesBLL().MapEndOfService(EndOfServiceVacation.EndOfServices);
                    }
                }
                return EndOfServiceVacationBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}

