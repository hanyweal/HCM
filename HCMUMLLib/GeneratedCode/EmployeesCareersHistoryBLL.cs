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

public class EmployeesCareersHistoryBLL : CommonEntity, IEntity
{
	public virtual int EmployeeCareerHistoryID
	{
		get;
		set;
	}

	public virtual int CareerHistoryTypeID
	{
		get;
		set;
	}

	public virtual OrganizationsJobsBLL OrganizationJob
	{
		get;
		set;
	}

	public virtual int CareerDegreeID
	{
		get;
		set;
	}

	public virtual DateTime JoinDate
	{
		get;
		set;
	}

	public virtual DateTime TransactionStartDate
	{
		get;
		set;
	}

	public virtual DateTime TransactionEndDate
	{
		get;
		set;
	}

	public virtual bool IsActive
	{
		get;
		set;
	}

	public virtual EmployeesCodesBLL EmployeeCode
	{
		get;
		set;
	}

	public virtual IEnumerable<BaseEmployeesTransfersBLL> BaseEmployeesTransfersBLL
	{
		get;
		set;
	}

	public virtual IEnumerable<EmployeesAllowancesBLL> EmployeesAllowancesBLL
	{
		get;
		set;
	}

	public virtual IEnumerable<DelegationsDetailsBLL> DelegationsDetailsBLL
	{
		get;
		set;
	}

	public virtual InternshipScholarshipsDetailsBLL InternshipScholarshipsDetailsBLL
	{
		get;
		set;
	}

	public virtual EmployeesCareersHistoryBLL GetEmployeeCareerHistoryByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
	{
		throw new System.NotImplementedException();
	}

	public virtual EmployeesCareersHistoryBLL GetEmployeeCareerHistoryByEmployeeCodeID(int EmployeeCodeID)
	{
		throw new System.NotImplementedException();
	}

	public virtual Result Add()
	{
		throw new System.NotImplementedException();
	}

	public virtual Result Update()
	{
		throw new System.NotImplementedException();
	}

	private EmployeesCareersHistoryBLL GetHiringRecordByEmployeeCodeID(object int EmployeeCodeID)
	{
		throw new System.NotImplementedException();
	}

	private void ChangeCurrentJobByEmployeeCodeID(object int EmployeeCodeID)
	{
		throw new System.NotImplementedException();
	}

}

