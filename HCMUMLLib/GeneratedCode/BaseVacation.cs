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

public abstract class BaseVacation
{
	public virtual EmployeesCareersHistoryBLL EmployeeCareerHistory
	{
		get;
		set;
	}

	public virtual DateTime EndDate
	{
		get;
		set;
	}

	public virtual DateTime StartDate
	{
		get;
		set;
	}

	public virtual int VacationPeriod
	{
		get;
		set;
	}

	public virtual DateTime WorkDate
	{
		get;
		set;
	}

}

