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

public class GovernmentFundsBLL : CommonEntity, IEntity
{
	public virtual int GovernmentFundID
	{
		get;
		set;
	}

	public virtual float MonthlyDeductionAmount
	{
		get;
		set;
	}

	public virtual string LoanNo
	{
		get;
		set;
	}

	public virtual Nullable<DateTime> LoanDate
	{
		get;
		set;
	}

	public virtual DateTime DeductionDate
	{
		get;
		set;
	}

	public virtual float TotalDeductionAmount
	{
		get;
		set;
	}

	public virtual EmployeesCodesBLL EmployeeCode
	{
		get;
		set;
	}

	public virtual GovernmentDeductionsTypesBLL GovernmentDeductionType
	{
		get;
		set;
	}

	public virtual GovernmentFundsTypesBLL GovernmentFundType
	{
		get;
		set;
	}

	public virtual List<GovermentFundsBLL> GetGovernmentFunds()
	{
		throw new System.NotImplementedException();
	}

	public virtual Result Add()
	{
		throw new System.NotImplementedException();
	}

	public virtual Result Remove()
	{
		throw new System.NotImplementedException();
	}

	public virtual Result Update()
	{
		throw new System.NotImplementedException();
	}

	public virtual GovernmentFundsBLL GetByGovernmentFundID(object int GovernmentFundID)
	{
		throw new System.NotImplementedException();
	}

}

