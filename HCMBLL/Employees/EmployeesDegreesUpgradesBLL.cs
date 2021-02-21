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
    public class EmployeesDegreesUpgradesBLL : CommonEntity, IEntity
    {
        public int EmployeeDegreeUpgradeID
        {
            get;
            set;
        }

        public int EmployeeDegreeUpgradeYear
        {
            get;
            set;
        }
        
        public virtual Result Add()
        {
            Result result = null;
            result = new Result();
            EmployeesDegreesUpgrades EmployeeDegreeUpgrade = new EmployeesDegreesUpgrades();

            if (new EmployeesDegreesUpgradesDAL().GetEmployeesDegreesUpgrades().Exists(c => c.EmployeeDegreeUpgradeYear == this.EmployeeDegreeUpgradeYear))
            {
                result.Entity = this;
                result.EnumType = typeof(EmployeesDegreesUpgradesValidationEnum);
                result.EnumMember = EmployeesDegreesUpgradesValidationEnum.RejectedAlreadyExists.ToString();
                return result;
            }

            new EmployeesCareersHistoryBLL() { LoginIdentity = this.LoginIdentity }.IncreaseCareerDegree();

            EmployeeDegreeUpgrade.EmployeeDegreeUpgradeYear = this.EmployeeDegreeUpgradeYear;            
            EmployeeDegreeUpgrade.CreatedDate = DateTime.Now;
            EmployeeDegreeUpgrade.CreatedBy = this.LoginIdentity.EmployeeCodeID;
            this.EmployeeDegreeUpgradeID = new EmployeesDegreesUpgradesDAL().Insert(EmployeeDegreeUpgrade);
            if (this.EmployeeDegreeUpgradeID != 0)
            {
                result.Entity = this;
                result.EnumType = typeof(EmployeesDegreesUpgradesValidationEnum);
                result.EnumMember = EmployeesDegreesUpgradesValidationEnum.Done.ToString();
            }

            return result;
        }          

        public List<EmployeesDegreesUpgradesBLL> GetEmployeesDegreesUpgrades()
        {
            List<EmployeesDegreesUpgrades> EmployeesDegreesUpgradesList = new EmployeesDegreesUpgradesDAL().GetEmployeesDegreesUpgrades();
            List<EmployeesDegreesUpgradesBLL> EmployeesDegreesUpgradesBLLList = new List<EmployeesDegreesUpgradesBLL>();
            if (EmployeesDegreesUpgradesList.Count > 0)
            {
                foreach (var item in EmployeesDegreesUpgradesList)
                {
                    EmployeesDegreesUpgradesBLLList.Add(MapEmployeeDegreeUpgrade(item));
                }
            }

            return EmployeesDegreesUpgradesBLLList;
        } 

        internal EmployeesDegreesUpgradesBLL MapEmployeeDegreeUpgrade(EmployeesDegreesUpgrades item)
        {
            return item != null ?
                new EmployeesDegreesUpgradesBLL()
                {
                    EmployeeDegreeUpgradeID = item.EmployeeDegreeUpgradeID,
                    EmployeeDegreeUpgradeYear = item.EmployeeDegreeUpgradeYear,
                    CreatedDate =item.CreatedDate,
                    CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(item.EmployeesCodes),
        }
                : null;
        }
    }
}