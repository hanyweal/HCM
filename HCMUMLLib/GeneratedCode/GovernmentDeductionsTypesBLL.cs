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

public class GovernmentDeductionsTypesBLL : CommonEntity
{
	public virtual int GovernmentDeductionTypeID
	{
		get;
		set;
	}

	public virtual string + GovernmentDeductionTypeName
	{
		get;
		set;
	}

	public virtual IEnumerable<GovernmentFundsBLL> GovernmentFundsBLL
	{
		get;
		set;
	}

	public virtual void GetGovernmentDeductionsTypes : List<GovernmentDeductionsTypesBLL>()
	{
		throw new System.NotImplementedException();
	}

}
