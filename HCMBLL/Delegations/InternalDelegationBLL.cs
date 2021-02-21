﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HCMDAL;
using HCMBLL.Enums;

namespace HCMBLL
{
    public class InternalDelegationBLL : BaseDelegationsBLL
    {
        public virtual KSACitiesBLL KSACity
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
                Delegations Delegation = DALInstance;
                Delegation.KSACityID = this.KSACity.KSACityID;
                Delegation.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                this.DelegationID = new DelegationsDAL().Insert(Delegation);
                if (this.DelegationID != 0)
                {
                    result.Entity = this;
                    result.EnumType = typeof(DelegationsValidationEnum);
                    result.EnumMember = DelegationsValidationEnum.Done.ToString();
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
                Delegations Delegation = DALInstance;
                Delegation.KSACityID = this.KSACity.KSACityID;
                Delegation.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                this.DelegationID = new DelegationsDAL().Update(Delegation);
                if (this.DelegationID != 0)
                {
                    result.Entity = this;
                    result.EnumType = typeof(DelegationsValidationEnum);
                    result.EnumMember = DelegationsValidationEnum.Done.ToString();
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

