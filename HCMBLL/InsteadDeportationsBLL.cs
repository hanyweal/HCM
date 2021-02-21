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

namespace HCMBLL
{
    public class InsteadDeportationsBLL : CommonEntity, IEntity
    {
        public int InsteadDeportationID { get; set; }

        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public DateTime DeportationDate { get; set; }

        public string Note { get; set; }

        public double? Amount { get; set; }

        public virtual Result Add()
        {
            Result result = new Result();
            result.EnumType = typeof(InsteadDeportationValidationEnum);
            EmployeesCareersHistoryBLL EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetActiveByEmployeeCareerHistoryID(this.EmployeeCareerHistory.EmployeeCareerHistoryID);
            InsteadDeportations InsteadDeportation = new InsteadDeportations();
            InsteadDeportation.EmployeeCareerHistoryID = this.EmployeeCareerHistory.EmployeeCareerHistoryID;
            InsteadDeportation.DeportationDate = this.DeportationDate;
            InsteadDeportation.Amount = this.Amount;
            InsteadDeportation.Note = this.Note;
            InsteadDeportation.CreatedDate = DateTime.Now;
            InsteadDeportation.CreatedBy = this.LoginIdentity.EmployeeCodeID;
            this.InsteadDeportationID = new InsteadDeportationsDAL().Insert(InsteadDeportation);
            if (this.InsteadDeportationID != 0)
            {
                result.Entity = this;
                result.EnumMember = AllowanceValidationEnum.Done.ToString();
            }
            return result;
        }

        public virtual Result Update()
        {
            Result result = new Result();
            result.EnumType = typeof(AllowanceValidationEnum);
            InsteadDeportations InsteadDeportation = new InsteadDeportations();
            EmployeesCareersHistoryBLL EmployeeCareerHistory = new EmployeesCareersHistoryBLL().GetActiveByEmployeeCareerHistoryID(this.EmployeeCareerHistory.EmployeeCareerHistoryID);
            InsteadDeportation.InsteadDeportationID = this.InsteadDeportationID;
            InsteadDeportation.EmployeeCareerHistoryID = this.EmployeeCareerHistory.EmployeeCareerHistoryID;
            InsteadDeportation.DeportationDate = this.DeportationDate;
            InsteadDeportation.Note = this.Note;
            InsteadDeportation.Amount = this.Amount;
            InsteadDeportation.LastUpdatedDate = DateTime.Now;
            InsteadDeportation.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
            this.InsteadDeportationID = new InsteadDeportationsDAL().Update(InsteadDeportation);
            if (this.InsteadDeportationID != 0)
            {
                result.Entity = this;
                result.EnumMember = AllowanceValidationEnum.Done.ToString();
            }

            return result;
        }

        public Result Remove(int InsteadDeportationID)
        {
            try
            {
                Result result = null;
                new InsteadDeportationsDAL().Delete(InsteadDeportationID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(AllowanceValidationEnum),
                    EnumMember = AllowanceValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        public virtual List<InsteadDeportationsBLL> GetInsteadDeportations(out int totalRecordsOut, out int recFilterOut)
        {
            List<InsteadDeportationsBLL> InsteadDeportationsBLL = new List<InsteadDeportationsBLL>();
            List<InsteadDeportations> InsteadDeportations = new InsteadDeportationsDAL()
            {
                search = Search,
                order = Order,
                orderDir = OrderDir,
                startRec = StartRec,
                pageSize = PageSize

            }.GetInsteadDeportations(out totalRecordsOut, out recFilterOut);
            foreach (var item in InsteadDeportations)
            {
                InsteadDeportationsBLL.Add(MapInsteadDeportation(item));
            }
            return InsteadDeportationsBLL;
        }

        public virtual List<InsteadDeportationsBLL> GetInsteadDeportations()
        {
            List<InsteadDeportationsBLL> InsteadDeportationsBLL = new List<InsteadDeportationsBLL>();
            List<InsteadDeportations> InsteadDeportations = new InsteadDeportationsDAL().GetInsteadDeportations();
            foreach (var item in InsteadDeportations)
            {
                InsteadDeportationsBLL.Add(MapInsteadDeportation(item));
            }
            return InsteadDeportationsBLL;
        }

        internal InsteadDeportationsBLL MapInsteadDeportation(InsteadDeportations InsteadDeportation)
        {
            try
            {
                InsteadDeportationsBLL InsteadDeportationBLL = null;
                if (InsteadDeportation != null)
                {
                    InsteadDeportationBLL = new InsteadDeportationsBLL()
                    {
                        InsteadDeportationID = InsteadDeportation.InsteadDeportationID,
                        EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(InsteadDeportation.EmployeesCareersHistory),
                        DeportationDate = InsteadDeportation.DeportationDate.Date,
                        Note = InsteadDeportation.Note,
                        Amount = InsteadDeportation.Amount,
                        CreatedDate = InsteadDeportation.CreatedDate,
                        CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(InsteadDeportation.CreatedByNav),
                        LastUpdatedBy = new EmployeesCodesBLL().MapEmployeeCode(InsteadDeportation.LastUpdatedByNav)
                    };
                }
                return InsteadDeportationBLL;
            }
            catch
            {
                throw;
            }
        }

        public InsteadDeportationsBLL GetByInsteadDeportationID(int id)
        {
            InsteadDeportationsBLL InsteadDeportationsBLL = null;
            InsteadDeportations InsteadDeportation = new InsteadDeportationsDAL().GetInsteadDeportationByInsteadDeportationID(id);
            if (InsteadDeportation != null)
            {
                InsteadDeportationsBLL = MapInsteadDeportation(InsteadDeportation);
            }
            return InsteadDeportationsBLL;
        }
    }
}