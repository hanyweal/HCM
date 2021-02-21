using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCMDAL;
using HCMBLL.Enums;


namespace HCMBLL
{
    public class EmployeeExperiencesWithDetailsBLL : CommonEntity, IEntity
    {
        public virtual int EmployeeExperienceWithDetailID
        {
            get;
            set;
        }

        public virtual DateTime FromDate
        {
            get;
            set;
        }

        public virtual DateTime ToDate
        {
            get;
            set;
        }

        public virtual string SectorName
        {
            get;
            set;
        }

        public virtual string JobName
        {
            get;
            set;
        }

        public virtual EmployeesCodesBLL EmployeesCodes
        {
            get;
            set;
        }

        public virtual SectorsTypesBLL SectorsTypes
        {
            get;
            set;
        }

        public int ExperienceYears
        {
            get
            {
                decimal year = Convert.ToDecimal(this.ToDate.Subtract(this.FromDate).TotalDays) / this.DaysCountInGregYear;
                return Convert.ToInt32(Math.Floor(year));
            }
        }
        public int ExperienceMonths
        {
            get
            {
                decimal months = (Convert.ToDecimal(this.ToDate.Subtract(this.FromDate).TotalDays) % this.DaysCountInGregYear) / this.DaysCountInGregMonth;
                return Convert.ToInt32(Math.Floor(months));
            }
        }
        public int ExperienceDays
        {
            get
            {
                decimal days = (Convert.ToDecimal(this.ToDate.Subtract(this.FromDate).TotalDays) % this.DaysCountInGregYear) % this.DaysCountInGregMonth;
                return Convert.ToInt32(Math.Floor(days));
            }
        }
        public virtual Result Add()
        {
            try
            {
                Result result = null;

                EmployeeExperiencesWithDetails EmployeeExperiencesWithDetail = new EmployeeExperiencesWithDetails()
                {
                    EmployeeCodeID = this.EmployeesCodes.EmployeeCodeID,
                    FromDate = this.FromDate,
                    ToDate = this.ToDate,
                    JobName = this.JobName,
                    SectorName = this.SectorName,
                    SectorTypeID = this.SectorsTypes.SectorTypeID,
                    CreatedDate = DateTime.Now,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID
                };
                this.EmployeeExperienceWithDetailID = new EmployeeExperiencesWithDetailsDAL().Insert(EmployeeExperiencesWithDetail);
                result = new Result()
                {
                    Entity = this,
                    EnumType = typeof(EmployeesExperiencesValidationEnum),
                    EnumMember = EmployeesExperiencesValidationEnum.Done.ToString(),
                };
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

                EmployeeExperiencesWithDetails EmployeeExperiencesWithDetail = new EmployeeExperiencesWithDetails()
                {
                    EmployeeExperienceWithDetailID = this.EmployeeExperienceWithDetailID,
                    EmployeeCodeID = this.EmployeesCodes.EmployeeCodeID,
                    FromDate = this.FromDate,
                    ToDate = this.ToDate,
                    JobName = this.JobName,
                    SectorName = this.SectorName,
                    SectorTypeID = this.SectorsTypes.SectorTypeID,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
                };
                this.EmployeeExperienceWithDetailID = new EmployeeExperiencesWithDetailsDAL().Update(EmployeeExperiencesWithDetail);
                result = new Result()
                {
                    Entity = this,
                    EnumType = typeof(EmployeesExperiencesValidationEnum),
                    EnumMember = EmployeesExperiencesValidationEnum.Done.ToString(),
                };
                return result;
            }
            catch
            {
                throw;
            }
        }

        public virtual Result Remove(int EmployeeExperienceID)
        {
            try
            {
                new EmployeeExperiencesWithDetailsDAL().Delete(EmployeeExperienceID, this.LoginIdentity.EmployeeCodeID);
                Result result = new Result()
                {
                    //Entity = this,
                    EnumType = typeof(EmployeesExperiencesValidationEnum),
                    EnumMember = EmployeesExperiencesValidationEnum.Done.ToString(),
                };
                return result;
            }
            catch
            {
                throw;
            }
        }

        public virtual List<EmployeeExperiencesWithDetailsBLL> GetEmployeesExperiences()
        {
            try
            {
                List<EmployeeExperiencesWithDetails> EmployeesExperiencesList = new EmployeeExperiencesWithDetailsDAL().GetEmployeeExperienceWithDetails();
                List<EmployeeExperiencesWithDetailsBLL> EmployeesExperiencesBLLList = new List<EmployeeExperiencesWithDetailsBLL>();
                foreach (var item in EmployeesExperiencesList)
                {
                    EmployeesExperiencesBLLList.Add(new EmployeeExperiencesWithDetailsBLL().MapEmployeeExperienceWithDetail(item));
                }
                return EmployeesExperiencesBLLList;
            }
            catch
            {
                throw;
            }
        }

        public virtual List<EmployeeExperiencesWithDetailsBLL> GetEmployeesExperiencesWithDetailByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                List<EmployeeExperiencesWithDetails> EmployeesExperiencesWithDetailsList = new EmployeeExperiencesWithDetailsDAL().GetEmployeeExperiencesByEmployeeCodeID(EmployeeCodeID);
                List<EmployeeExperiencesWithDetailsBLL> EmployeesExperiencesWithDetailsListBLLList = new List<EmployeeExperiencesWithDetailsBLL>();
                foreach (var item in EmployeesExperiencesWithDetailsList)
                {
                    EmployeesExperiencesWithDetailsListBLLList.Add(new EmployeeExperiencesWithDetailsBLL().MapEmployeeExperienceWithDetail(item));
                }
                return EmployeesExperiencesWithDetailsListBLLList;
            }
            catch
            {
                throw;
            }
        }

        //public int GetTotalDaysByEmployeeExperience(EmployeeExperiencesWithDetailsBLL EmployeesExperience)
        //{
        //    int TotalExperience = 0;
        //    decimal totExpWithDecimals = 0;
        //    //EmployeesExperiences EmployeesExperience = new EmployeesExperiencesDAL().GetEmployeeExperiencesByEmployeeCodeID(EmployeeCodeID).FirstOrDefault();

        //    if (EmployeesExperience != null && EmployeesExperience.EmployeeExperienceID > 0)
        //    {
        //        totExpWithDecimals = Convert.ToDecimal((EmployeesExperience.TotalYears * DaysCountInUmAlquraYear)) + (EmployeesExperience.TotalMonths * DaysCountInUmAlquraMonth) + Convert.ToDecimal(EmployeesExperience.TotalDays);
        //        TotalExperience = Convert.ToInt32(Math.Floor(totExpWithDecimals));
        //    }

        //    return TotalExperience;
        //}

        public virtual EmployeeExperiencesWithDetailsBLL GetByEmployeesExperiencesWithDetailID(int EmployeesExperiencesWithDetailID)
        {
            try
            {
                EmployeeExperiencesWithDetails EmployeeExperience = new EmployeeExperiencesWithDetailsDAL().GetByEmployeeExperienceWithDetailID(EmployeesExperiencesWithDetailID);
                return new EmployeeExperiencesWithDetailsBLL().MapEmployeeExperienceWithDetail(EmployeeExperience);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Task # 252 Calculate total Experience days
        /// Details: calculate total experience days day by day because there is possibility that 
        ///         employee has experience in same days so we need to count single day.
        /// Examples:
        ///         04-01-2020      15-01-2020      
        ///         01-02-2020      23-02-2020      
        ///         01-01-2020      03-03-2020      
        /// Total Experience days must be :        90 
        /// </summary>
        /// <param name="EmployeeExperienceDetailList"></param>
        /// <returns></returns>
        public int GetTotalDaysByEmployeeExperience(List<EmployeeExperiencesWithDetails> EmployeeExperienceDetailList)
        {
            List<EmployeeExperiencesWithDetails> ExperienceCalculatedList = new List<EmployeeExperiencesWithDetails>();
            //List<EmployeeExperiencesWithDetails> EmployeeExperienceDetailList = new EmployeeExperiencesWithDetailsDAL().GetEmployeeExperiencesByEmployeeCodeID(EmployeeCodeID);
            //int TotalExperience = 0;
            //decimal totExpWithDecimals = 0;
            DateTime Date;
            int TotalDays = 0;
            bool isAlreadyCal = false;

            foreach (var EmpExp in EmployeeExperienceDetailList)
            {
                for (DateTime i = EmpExp.FromDate.Value; i <= EmpExp.ToDate.Value;)
                {
                    Date = i;

                    if (ExperienceCalculatedList.Count > 0)
                    {
                        isAlreadyCal = false;
                        foreach (var item in ExperienceCalculatedList)
                        {
                            if (Date >= item.FromDate.Value && Date <= item.ToDate.Value)
                            {
                                // skip this date because already included
                                isAlreadyCal = true;
                            }
                        }

                        if (isAlreadyCal == false)
                            TotalDays++;
                    }
                    else
                    {
                        TotalDays = Convert.ToInt32(EmpExp.ToDate.Value.Subtract(EmpExp.FromDate.Value).TotalDays);
                        break;
                    }

                    i = i.AddDays(1);
                }

                ExperienceCalculatedList.Add(EmpExp);
                //totExpWithDecimals = Convert.ToDecimal(item.ToDate.Value.Subtract(item.FromDate.Value).TotalDays);
                //TotalExperience = TotalExperience + Convert.ToInt32(Math.Floor(totExpWithDecimals));
            }

            return TotalDays;
        }

        /// <summary>
        /// TASK # 268
        /// This function calculate the total experience in Years, Months and Days
        /// </summary>
        /// <param name="EmployeeCodeID"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="days"></param>
        public void GetTotalEmployeeExperienceInYMD(int EmployeeCodeID, out int year, out int month, out int days)
        {
            List<EmployeeExperiencesWithDetails> EmployeeExperienceDetailList = new EmployeeExperiencesWithDetailsDAL().GetEmployeeExperiencesByEmployeeCodeID(EmployeeCodeID);
            int TotalDays = this.GetTotalDaysByEmployeeExperience(EmployeeExperienceDetailList);
            year = month = days = 0;

            year = Convert.ToInt32(Math.Floor(Convert.ToDecimal(TotalDays) / this.DaysCountInGregYear));
            month = Convert.ToInt32(Math.Floor((Convert.ToDecimal(TotalDays) % this.DaysCountInGregYear) / this.DaysCountInGregMonth));
            days = Convert.ToInt32(Math.Floor((Convert.ToDecimal(TotalDays) % this.DaysCountInGregYear) % this.DaysCountInGregMonth));
        }

        public double GetTotalExperienceDaysBasedOnSectorType(int EmployeeCodeID, SectorsTypesEnum SectorType)
        {
            List<EmployeeExperiencesWithDetails> EmployeeExperienceDetailList = new EmployeeExperiencesWithDetailsDAL().GetEmployeeExperiencesByEmployeeCodeID(EmployeeCodeID, (int)SectorType);
            int TotalDays = GetTotalDaysByEmployeeExperience(EmployeeExperienceDetailList);
            return TotalDays;
        }

        internal EmployeeExperiencesWithDetailsBLL MapEmployeeExperienceWithDetail(EmployeeExperiencesWithDetails EmployeeExperiencesWithDetail)
        {
            try
            {
                EmployeeExperiencesWithDetailsBLL EmployeeExperiencesWithDetailBLL = null;
                if (EmployeeExperiencesWithDetail != null)
                {
                    EmployeeExperiencesWithDetailBLL = new EmployeeExperiencesWithDetailsBLL()
                    {
                        EmployeeExperienceWithDetailID = EmployeeExperiencesWithDetail.EmployeeExperienceWithDetailID,
                        FromDate = EmployeeExperiencesWithDetail.FromDate.Value,
                        ToDate = EmployeeExperiencesWithDetail.ToDate.Value,
                        JobName = EmployeeExperiencesWithDetail.JobName,
                        SectorName = EmployeeExperiencesWithDetail.SectorName,
                        EmployeesCodes = new EmployeesCodesBLL().MapEmployeeCode(EmployeeExperiencesWithDetail.EmployeesCodes),
                        CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(EmployeeExperiencesWithDetail.EmployeesCodes1),
                        CreatedDate = EmployeeExperiencesWithDetail.CreatedDate.Value,
                        SectorsTypes = new SectorsTypesBLL().MapSectorType(EmployeeExperiencesWithDetail.SectorsTypes),
                    };
                }
                return EmployeeExperiencesWithDetailBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
