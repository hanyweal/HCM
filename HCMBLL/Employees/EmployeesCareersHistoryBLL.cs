using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCMBLL
{
    public class EmployeesCareersHistoryBLL : CommonEntity, IEntity
    {
        public int EmployeeCareerHistoryID { get; set; }

        public virtual EmployeesCodesBLL EmployeeCode { get; set; }

        public CareersHistoryTypesBLL CareerHistoryType { get; set; }

        public OrganizationsJobsBLL OrganizationJob { get; set; }

        public CareersDegreesBLL CareerDegree { get; set; }

        public DateTime JoinDate { get; set; }

        public DateTime ProbationEndDate
        {
            get
            {
                if (JoinDate == DateTime.MinValue)
                    return new DateTime();
                return Globals.Calendar.AddYearMonthDayInUmAlQuraDate(this.JoinDate, 1, 0, 0);
            }
        }

        public DateTime TransactionStartDate { get; set; }

        public DateTime? TransactionEndDate { get; set; }

        public bool IsActive { get; set; }

        public Result Add()
        {
            try
            {
                Result result = new Result();
                EmployeesCareersHistoryBLL HiringRecord = this.GetHiringRecordByEmployeeCodeID(this.EmployeeCode.EmployeeCodeID);

                #region Check about is hiring exists before or not
                if (this.CareerHistoryType.CareerHistoryTypeID.Equals((int)CareersHistoryTypesEnum.Hiring))
                {
                    if (HiringRecord != null)
                    {
                        result.Entity = HiringRecord;
                        result.EnumType = typeof(CareersHistoryValidationEnum);
                        result.EnumMember = CareersHistoryValidationEnum.RejectedBecauseOfAlreadyHiringBefore.ToString();
                        return result;
                    }
                }
                #endregion

                #region TASK 260 : check new employee career history record date must be less than 'Join Date' of hiring record 
                if (this.CareerHistoryType.CareerHistoryTypeID != (int)CareersHistoryTypesEnum.Hiring)
                {
                    if (HiringRecord != null && HiringRecord.JoinDate.Date >= this.JoinDate.Date)
                    {
                        result.Entity = HiringRecord;
                        result.EnumType = typeof(CareersHistoryValidationEnum);
                        result.EnumMember = CareersHistoryValidationEnum.RejectedBecauseOfJoinDateMustBeLessThanHiringRecordJoinDate.ToString();
                        return result;
                    }
                }
                #endregion

                #region Create new record
                EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistory()
                {
                    EmployeeCodeID = this.EmployeeCode.EmployeeCodeID,
                    CareerHistoryTypeID = this.CareerHistoryType.CareerHistoryTypeID,
                    CareerDegreeID = this.CareerDegree.CareerDegreeID,
                    OrganizationJobID = this.OrganizationJob.OrganizationJobID,
                    JoinDate = this.JoinDate.Date,
                    TransactionStartDate = this.JoinDate.Date,
                    IsActive = false,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID,
                    CreatedDate = DateTime.Now
                };
                EmployeeCareerHistory.EmployeeCareerHistoryID = new EmployeesCareersHistoryDAL().Insert(EmployeeCareerHistory);
                #endregion

                #region Change current job
                ChangeCurrentJobByEmployeeCodeID(this.EmployeeCode.EmployeeCodeID);
                #endregion

                #region TASK 310: Break last assigning
                //EmployeesCareersHistoryBLL LastActiveCareerHistory = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(this.EmployeeCode.EmployeeCodeID);
                //if (LastActiveCareerHistory != null && LastActiveCareerHistory.EmployeeCareerHistoryID > 0)
                //{
                new BaseAssigningsBLL() { LoginIdentity = this.LoginIdentity }
                        .BreakLastAssigning(this.EmployeeCode.EmployeeCodeID, EmployeeCareerHistory.JoinDate,
                                                AssigningsReasonsEnum.CreatedNewCareerHistory);
                //}
                #endregion

                result.Entity = this;
                result.EnumType = typeof(CareersHistoryValidationEnum);
                result.EnumMember = CareersHistoryValidationEnum.Done.ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result Update()
        {
            try
            {
                Result result = new Result();

                #region Validation to check if EmployeeCareerHistory Exist in PromotionsRecordsEmployees as NewEmployeeCareerHistory or CurrentEmployeeCareerHistory
                //check if OrganizationJob Exist in PromotionsRecordsJobsVacancies as PromotionRecordJobVacancyID
                // Then Can't update OrganizationID,RankID,JobID
                List<PromotionsRecordsEmployeesBLL> PromotionsRecordsEmployeesList = new PromotionsRecordsEmployeesBLL().GetByEmployeeCareerHistoryID(this.EmployeeCareerHistoryID);
                if (PromotionsRecordsEmployeesList.Count != 0)
                {
                    result.EnumType = typeof(CareersHistoryValidationEnum);
                    result.EnumMember = CareersHistoryValidationEnum.RejectedBecauseOfExistsInPromotionsRecordsEmployees.ToString();
                    return result;
                }
                #endregion

                #region Check about is hiring exists before or not
                if (this.CareerHistoryType.CareerHistoryTypeID.Equals((int)CareersHistoryTypesEnum.Hiring))
                {
                    EmployeesCareersHistoryBLL Hr = this.GetHiringRecordByEmployeeCodeID(this.EmployeeCode.EmployeeCodeID);
                    if (Hr != null)
                    {
                        if (Hr.EmployeeCareerHistoryID != this.EmployeeCareerHistoryID)
                        {
                            result.Entity = Hr;
                            result.EnumType = typeof(CareersHistoryValidationEnum);
                            result.EnumMember = CareersHistoryValidationEnum.RejectedBecauseOfAlreadyHiringBefore.ToString();
                            return result;
                        }
                    }
                }
                #endregion

                #region TASK 260 : check new employee career history record date must be less than 'Join Date' of hiring record
                if (this.CareerHistoryType.CareerHistoryTypeID != (int)CareersHistoryTypesEnum.Hiring)
                {
                    EmployeesCareersHistoryBLL oo = this.GetHiringRecordByEmployeeCodeID(this.EmployeeCode.EmployeeCodeID);
                    if (oo != null && oo.JoinDate.Date >= this.JoinDate.Date)
                    {
                        result.Entity = oo;
                        result.EnumType = typeof(CareersHistoryValidationEnum);
                        result.EnumMember = CareersHistoryValidationEnum.RejectedBecauseOfJoinDateMustBeLessThanHiringRecordJoinDate.ToString();
                        return result;
                    }
                }
                #endregion

                #region if the record is hiring ... Check about there is vacation transactions before new hiring date or not
                if (this.CareerHistoryType.CareerHistoryTypeID.Equals((int)CareersHistoryTypesEnum.Hiring))
                {
                    bool IsVacationExists = new BaseVacationsBLL().IsEmployeeVacationExistsBeforeDate(this.EmployeeCode.EmployeeCodeID, this.JoinDate.Date);
                    if (IsVacationExists)
                    {
                        //result.Entity = Hr;
                        result.EnumType = typeof(CareersHistoryValidationEnum);
                        result.EnumMember = CareersHistoryValidationEnum.RejectedHiringDateUpdatingBecauseOfAlreadyActionsBefore.ToString();
                        return result;
                    }
                }
                #endregion

                #region Update record
                EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistory()
                {
                    EmployeeCareerHistoryID = this.EmployeeCareerHistoryID,
                    CareerHistoryTypeID = this.CareerHistoryType.CareerHistoryTypeID,
                    CareerDegreeID = this.CareerDegree.CareerDegreeID,
                    OrganizationJobID = this.OrganizationJob.OrganizationJobID,
                    JoinDate = this.JoinDate.Date,
                    TransactionStartDate = this.JoinDate.Date,
                    LastUpdatedBy = this.LoginIdentity != null ? this.LoginIdentity.EmployeeCodeID : (int?)null,
                    LastUpdatedDate = DateTime.Now,
                    IsActive = this.IsActive,
                };
                new EmployeesCareersHistoryDAL().Update(EmployeeCareerHistory);
                #endregion

                #region Change current job
                ChangeCurrentJobByEmployeeCodeID(this.EmployeeCode.EmployeeCodeID);
                #endregion

                result.Entity = this;
                result.EnumType = typeof(CareersHistoryValidationEnum);
                result.EnumMember = CareersHistoryValidationEnum.Done.ToString();
                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// TASK 220: 
        /// </summary>
        /// <returns></returns>
        public Result UpdateCareerDegree()
        {
            try
            {
                Result result = new Result();

                #region Update record
                EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistory()
                {
                    EmployeeCareerHistoryID = this.EmployeeCareerHistoryID,
                    CareerDegreeID = this.CareerDegree.CareerDegreeID,
                    LastUpdatedBy = this.LoginIdentity != null ? this.LoginIdentity.EmployeeCodeID : (int?)null,
                    LastUpdatedDate = DateTime.Now,
                };
                new EmployeesCareersHistoryDAL().UpdateCareerDegree(EmployeeCareerHistory);
                #endregion

                result.Entity = this;
                result.EnumType = typeof(CareersHistoryValidationEnum);
                result.EnumMember = CareersHistoryValidationEnum.Done.ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result UpdateTransactionStartDate(int EmployeeCareerHistoryID, DateTime TransactionStartDate, int LastUpdatedBy)
        {
            try
            {
                Result result = new Result();

                EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistory()
                {
                    EmployeeCareerHistoryID = EmployeeCareerHistoryID,
                    JoinDate = TransactionStartDate,
                    TransactionStartDate = TransactionStartDate,
                    LastUpdatedBy = LastUpdatedBy,
                    LastUpdatedDate = DateTime.Now
                };
                new EmployeesCareersHistoryDAL().UpdateTransactionStartDate(EmployeeCareerHistory);

                result.Entity = this;
                result.EnumType = typeof(CareersHistoryValidationEnum);
                result.EnumMember = CareersHistoryValidationEnum.Done.ToString();
                return result;
            }
            catch
            {
                throw;
            }
        }

        public Result IncreaseCareerDegree(EmployeesCareersHistoryBLL EmpCareerHistory)
        {
            try
            {
                Result result = new Result();
                int newCareerDegree = EmpCareerHistory.CareerDegree.CareerDegreeID + 1;
                #region Check newCareerDegree Exist In CareerDegree table.
                if (!new CareersDegreesBLL().GetCareersDegrees().Exists(c => c.CareerDegreeID == newCareerDegree))
                {
                    result.Entity = EmpCareerHistory;
                    result.EnumType = typeof(CareersHistoryValidationEnum);
                    result.EnumMember = CareersHistoryValidationEnum.RejectedBecauseOfCareerDegreeOutOfRange.ToString();
                    return result;
                }
                #endregion

                // checking if new career degree & rank has 'basic salary' exists or not, if not exists skip
                if (EmpCareerHistory.OrganizationJob != null && EmpCareerHistory.OrganizationJob.Rank != null)
                {
                    BasicSalariesBLL BasicSalary = new BasicSalariesBLL().GetBasicSalary(EmpCareerHistory.OrganizationJob.Rank.RankID, newCareerDegree);
                    if (BasicSalary.BasicSalary <= 0)
                    {
                        result.Entity = EmpCareerHistory;
                        result.EnumType = typeof(CareersHistoryValidationEnum);
                        result.EnumMember = CareersHistoryValidationEnum.RejectedBecauseOfCareerDegreeOutOfRange.ToString();
                        return result;
                    }
                }

                #region Update record
                EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistory()
                {
                    EmployeeCareerHistoryID = EmpCareerHistory.EmployeeCareerHistoryID,
                    CareerDegreeID = newCareerDegree,
                    LastUpdatedBy = EmpCareerHistory.LoginIdentity != null ? EmpCareerHistory.LoginIdentity.EmployeeCodeID : (int?)null,
                    LastUpdatedDate = DateTime.Now,
                };
                new EmployeesCareersHistoryDAL().UpdateCareerDegree(EmployeeCareerHistory);
                #endregion
                result.Entity = EmpCareerHistory;
                result.EnumType = typeof(CareersHistoryValidationEnum);
                result.EnumMember = CareersHistoryValidationEnum.Done.ToString();
                return result;
            }
            catch
            {
                throw;
            }
        }

        public Result IncreaseCareerDegree()
        {
            try
            {
                int newCareerDegree;
                Result result = new Result();
                List<EmployeesCareersHistory> Employees = new EmployeesCareersHistoryDAL().GetActiveEmployeesCareersHistory();
                List<EmployeesCareersHistory> EmployeesToUpdate = new List<EmployeesCareersHistory>();

                foreach (var EmpCareerHistory in Employees)
                {
                    newCareerDegree = EmpCareerHistory.CareerDegreeID + 1;

                    // checking if new career degree exists or not, if not exists skip
                    if (!new CareersDegreesBLL().GetCareersDegrees().Exists(c => c.CareerDegreeID == newCareerDegree))
                        continue;

                    // checking if new career degree & rank has 'basic salary' exists or not, if not exists skip
                    BasicSalariesBLL BasicSalary = new BasicSalariesBLL().GetBasicSalary(EmpCareerHistory.OrganizationsJobs.RankID, newCareerDegree);
                    if(BasicSalary.BasicSalary <= 0)
                        continue;

                    // Update record
                    EmpCareerHistory.CareerDegreeID = newCareerDegree;
                    EmpCareerHistory.LastUpdatedBy = this.LoginIdentity != null ? this.LoginIdentity.EmployeeCodeID : (int?)null;
                    EmpCareerHistory.LastUpdatedDate = DateTime.Now;

                    EmployeesToUpdate.Add(EmpCareerHistory);
                }

                new EmployeesCareersHistoryDAL().UpdateCareerDegree(EmployeesToUpdate);
                //#endregion
                result.Entity = this;
                result.EnumType = typeof(CareersHistoryValidationEnum);
                result.EnumMember = CareersHistoryValidationEnum.Done.ToString();
                return result;
            }
            catch
            {
                throw;
            }
        }

        public Result DecreaseCareerDegree()
        {
            try
            {
                Result result = new Result();
                #region Update record
                EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistory()
                {
                    EmployeeCareerHistoryID = this.EmployeeCareerHistoryID,
                    CareerDegreeID = this.CareerDegree.CareerDegreeID - 1,
                    LastUpdatedBy = this.LoginIdentity != null ? this.LoginIdentity.EmployeeCodeID : (int?)null,
                    LastUpdatedDate = DateTime.Now,
                };
                new EmployeesCareersHistoryDAL().UpdateCareerDegree(EmployeeCareerHistory);
                #endregion
                result.Entity = this;
                result.EnumType = typeof(CareersHistoryValidationEnum);
                result.EnumMember = CareersHistoryValidationEnum.Done.ToString();
                return result;
            }
            catch
            {
                throw;
            }
        }

        //public Result Remove(int EmployeeCareerHistoryID)
        //{
        //    try
        //    {
        //        Result result = null;
        //        new EmployeesCareersHistoryDAL().Delete(EmployeeCareerHistoryID, this.LoginIdentity.EmployeeCodeID);
        //        return result = new Result()
        //        {
        //            EnumType = typeof(CareersHistoryValidationEnum),
        //            EnumMember = CareersHistoryValidationEnum.Done.ToString()
        //        };
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public Result UpdatePreviousCareerHistory()
        //{
        //    try
        //    {
        //        Result result = new Result();

        //        #region Update record
        //        EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistory()
        //        {
        //            EmployeeCareerHistoryID = this.EmployeeCareerHistoryID,
        //            CareerHistoryTypeID = this.CareerHistoryType.CareerHistoryTypeID,
        //            CareerDegreeID = this.CareerDegree.CareerDegreeID,
        //            OrganizationJobID = this.OrganizationJob.OrganizationJobID,
        //            JoinDate = this.JoinDate.Date,
        //            TransactionStartDate = this.JoinDate.Date,
        //            LastUpdatedBy = this.LoginIdentity != null ? this.LoginIdentity.EmployeeCodeID : (int?)null,//this.LoginIdentity.EmployeeCodeID?null:(int)?null,
        //            LastUpdatedDate = DateTime.Now,
        //            IsActive = this.IsActive,
        //        };
        //        new EmployeesCareersHistoryDAL().Update(EmployeeCareerHistory);
        //        #endregion

        //        result.Entity = this;
        //        result.EnumType = typeof(CareersHistoryValidationEnum);
        //        result.EnumMember = CareersHistoryValidationEnum.Done.ToString();
        //        return result;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public Result Remove(int EmployeeCareerHistoryID)
        {
            Result result = new Result();
            new EmployeesCareersHistoryDAL().Delete(EmployeeCareerHistoryID, this.LoginIdentity.EmployeeCodeID);

            #region Change current job
            ChangeCurrentJobByEmployeeCodeID(this.EmployeeCode.EmployeeCodeID);
            #endregion

            result.Entity = this;
            result.EnumType = typeof(CareersHistoryValidationEnum);
            result.EnumMember = CareersHistoryValidationEnum.Done.ToString();
            return result;

            //if (EmployeeCareerHistoryOld.EmployeeCode != null)
            //{
            //    EmployeeCareerHistoryNew = new EmployeesCodesBLL().GetEmployeeCareerHistoryByEmployeeCodeID(EmployeeCareerHistoryOld.EmployeeCode.EmployeeCodeID)
            //        .Where(x => x.EmployeeCode.EmployeeCodeID.Equals(EmployeeCareerHistoryOld.EmployeeCode.EmployeeCodeID) && x.EmployeeCareerHistoryID != EmployeeCareerHistoryID)
            //        .OrderByDescending(s => s.JoinDate).FirstOrDefault();//.OrderByDescending(x => x.JoinDate).FirstOrDefault(x => x.EmployeeCode.EmployeeCodeID.Equals(EmployeeCareerHistoryOld.EmployeeCode.EmployeeCodeID));
            //}
            //if (EmployeeCareerHistoryNew != null)
            //{
            //    new EmployeesCareersHistoryDAL().Delete(EmployeeCareerHistoryID, this.LoginIdentity.EmployeeCodeID);
            //    new EmployeesCareersHistoryBLL()
            //    {
            //        EmployeeCareerHistoryID = EmployeeCareerHistoryNew.EmployeeCareerHistoryID,
            //        CareerHistoryType = new CareersHistoryTypesBLL() { CareerHistoryTypeID = EmployeeCareerHistoryNew.CareerHistoryType.CareerHistoryTypeID },
            //        CareerDegree = new CareersDegreesBLL() { CareerDegreeID = EmployeeCareerHistoryNew.CareerDegree.CareerDegreeID },
            //        OrganizationJob = new OrganizationsJobsBLL() { OrganizationJobID = EmployeeCareerHistoryNew.OrganizationJob.OrganizationJobID },
            //        JoinDate = EmployeeCareerHistoryNew.JoinDate.Date,
            //        TransactionStartDate = EmployeeCareerHistoryNew.JoinDate.Date,
            //        LoginIdentity = this.LoginIdentity,
            //        LastUpdatedDate = DateTime.Now,
            //        IsActive = true,
            //    }.UpdatePreviousCareerHistory();

            //result = new Result();
            //result.EnumType = typeof(CareersHistoryValidationEnum);
            //result.EnumMember = CareersHistoryValidationEnum.Done.ToString();
            //return result;
            //}
            //else
            //{
            //    new EmployeesCareersHistoryDAL().Delete(EmployeeCareerHistoryID, this.LoginIdentity.EmployeeCodeID);
            //    result = new Result();
            //    result.EnumType = typeof(CareersHistoryValidationEnum);
            //    result.EnumMember = CareersHistoryValidationEnum.Done.ToString();
            //    return result;
            //}
        }

        public EmployeesCareersHistoryBLL GetActiveByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        {
            EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistoryDAL().GetEmployeesCareersHistoryByEmployeeCareerHistoryID(EmployeeCareerHistoryID);
            EmployeesCareersHistoryBLL EmployeeCareerHistoryBLL = new EmployeesCareersHistoryBLL();
            if (EmployeeCareerHistory != null && EmployeeCareerHistory.IsActive)
                EmployeeCareerHistoryBLL = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(EmployeeCareerHistory);

            return EmployeeCareerHistoryBLL;
        }

        public EmployeesCareersHistoryBLL GetByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        {
            EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistoryDAL().GetEmployeesCareersHistoryByEmployeeCareerHistoryID(EmployeeCareerHistoryID);
            EmployeesCareersHistoryBLL EmployeeCareerHistoryBLL = new EmployeesCareersHistoryBLL();
            if (EmployeeCareerHistory != null)
                EmployeeCareerHistoryBLL = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(EmployeeCareerHistory);

            return EmployeeCareerHistoryBLL;
        }

        public EmployeesCareersHistoryBLL GetHiringRecordByEmployeeCodeID(int EmployeeCodeID)
        {
            EmployeesCareersHistoryBLL Obj = new EmployeesCodesBLL().GetCareerHistoryByEmployeeCodeID(EmployeeCodeID).FirstOrDefault(x => x.EmployeeCode.EmployeeCodeID.Equals(EmployeeCodeID)
                                                                                                                                                    && x.CareerHistoryType.CareerHistoryTypeID.Equals((int)CareersHistoryTypesEnum.Hiring));
            return Obj;
        }

        public EmployeesCareersHistoryBLL GetHiringRecordByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        {
            int EmployeeCodeID = new EmployeesCareersHistoryBLL().GetByEmployeeCareerHistoryID(EmployeeCareerHistoryID).EmployeeCode.EmployeeCodeID;
            EmployeesCareersHistoryBLL Obj = new EmployeesCodesBLL().GetCareerHistoryByEmployeeCodeID(EmployeeCodeID).FirstOrDefault(x => x.EmployeeCode.EmployeeCodeID.Equals(EmployeeCodeID)
                                                                                                                                                    && x.CareerHistoryType.CareerHistoryTypeID.Equals((int)CareersHistoryTypesEnum.Hiring));
            return Obj;
        }

        private int ChangeCurrentJobByEmployeeCodeID(int EmployeeCodeID)
        {
            List<EmployeesCareersHistoryBLL> ObjList = new EmployeesCodesBLL().GetCareerHistoryByEmployeeCodeID(EmployeeCodeID).OrderByDescending(x => x.JoinDate).ToList();
            foreach (var item in ObjList)
            {
                EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistory()
                {
                    EmployeeCareerHistoryID = item.EmployeeCareerHistoryID,
                    LastUpdatedBy = this.LoginIdentity != null ? this.LoginIdentity.EmployeeCodeID : (int?)null,
                    LastUpdatedDate = DateTime.Now,
                };

                if (ObjList.IndexOf(item) == 0)
                    EmployeeCareerHistory.IsActive = true;
                else
                    EmployeeCareerHistory.IsActive = false;

                new EmployeesCareersHistoryDAL().UpdateIsActive(EmployeeCareerHistory);
            }
            return 0;
        }

        public EmployeesCareersHistoryBLL GetEmployeeCurrentJob(int EmployeeCodeID)
        {
            EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistoryDAL().GetActiveJobByEmployeeCodeID(EmployeeCodeID);
            EmployeesCareersHistoryBLL EmployeeCareerHistoryBLL = new EmployeesCareersHistoryBLL();
            if (EmployeeCareerHistory != null)
                EmployeeCareerHistoryBLL = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(EmployeeCareerHistory);

            return EmployeeCareerHistoryBLL;
        }

        public EmployeesCareersHistoryBLL GetEmployeePreviousJob(int EmployeeCodeID)
        {
            EmployeesCareersHistoryBLL CurrentJob = this.GetEmployeeCurrentJob(EmployeeCodeID);
            RanksBLL PreviousRank = new RanksBLL().GetPreviousRankByRankID(CurrentJob.OrganizationJob.Rank.RankID);

            EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistoryDAL().GetByEmployeeCodeIDRankID(EmployeeCodeID, PreviousRank.RankID);
            EmployeesCareersHistoryBLL EmployeeCareerHistoryBLL = new EmployeesCareersHistoryBLL();
            if (EmployeeCareerHistory != null)
            {
                EmployeeCareerHistoryBLL = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(EmployeeCareerHistory);
            }

            return EmployeeCareerHistoryBLL;
        }

        public List<EmployeesCareersHistoryBLL> GetActiveEmployeesByRankID(int RankID)
        {
            List<EmployeesCareersHistoryBLL> DeservedEmployeesBLLList = new List<EmployeesCareersHistoryBLL>();
            List<EmployeesCareersHistory> DeservedEmployeesList = new EmployeesCareersHistoryDAL().GetActiveEmployeesByRankID(RankID).ToList();
            foreach (var item in DeservedEmployeesList)
            {
                DeservedEmployeesBLLList.Add(new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item));
            }
            return DeservedEmployeesBLLList;
        }

        public List<EmployeesCareersHistoryBLL> GetNotActiveJobsByRankID(int RankID, List<int> EmployeeCodeIDs)
        {
            List<EmployeesCareersHistoryBLL> EmployeesBLLList = new List<EmployeesCareersHistoryBLL>();
            List<EmployeesCareersHistory> EmployeesList = new EmployeesCareersHistoryDAL().GetNotActiveJobsByRankID(RankID, EmployeeCodeIDs).ToList();
            foreach (var item in EmployeesList)
            {
                EmployeesBLLList.Add(new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item));
            }
            return EmployeesBLLList;
        }

        public List<EmployeesCareersHistoryBLL> GetEmployeesByOrganizationJobID(int OrganizationJobID)
        {
            List<EmployeesCareersHistoryBLL> DeservedEmployeesBLLList = new List<EmployeesCareersHistoryBLL>();
            List<EmployeesCareersHistory> DeservedEmployeesList = new EmployeesCareersHistoryDAL().GetEmployeesByOrganizationJobID(OrganizationJobID).ToList();
            foreach (var item in DeservedEmployeesList)
            {
                DeservedEmployeesBLLList.Add(new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item));
            }
            return DeservedEmployeesBLLList;
        }

        public int GetOnGoingExperienceDays(EmployeesCareersHistory EmployeeHiringRecord, DateTime TillDate)
        {
            int experience = 0;

            if (EmployeeHiringRecord != null && EmployeeHiringRecord.EmployeeCareerHistoryID > 0)
                experience = Convert.ToInt32((TillDate.Date - EmployeeHiringRecord.JoinDate.Date).TotalDays);

            return experience;
        }

        public DateTime GetHiringDate(int EmployeeCodeID)
        {
            DateTime HiringDate = DateTime.Now;
            EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistoryDAL().GetEmployeesCareersHistoryByEmployeeCodeID(EmployeeCodeID)
                                                            .FirstOrDefault(x => x.EmployeeCodeID.Equals(EmployeeCodeID)
                                                                 && x.CareerHistoryTypeID.Equals((int)CareersHistoryTypesEnum.Hiring));

            if (EmployeeCareerHistory != null && EmployeeCareerHistory.EmployeeCareerHistoryID > 0)
                HiringDate = EmployeeCareerHistory.JoinDate;

            return HiringDate;
        }

        //public List<EmployeesCareersHistoryBLL> GetDeservedEmployeesInPromotion(int RankID, DateTime PromotionEndDate, int DaysCount)
        //{
        //    List<EmployeesCareersHistoryBLL> DeservedEmployeesBLLList = new List<EmployeesCareersHistoryBLL>();
        //    List<EmployeesCareersHistory> DeservedEmployeesList = new EmployeesCareersHistoryDAL().GetActiveEmployeesByRankID(RankID).Where(x => PromotionEndDate >= x.JoinDate.AddDays(DaysCount)).ToList();
        //    foreach (var item in DeservedEmployeesList)
        //    {
        //        DeservedEmployeesBLLList.Add(new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item));
        //    }
        //    return DeservedEmployeesBLLList;
        //}

        //       public EmployeesCareersHistoryBLL GetDeservedEmployeeInPromotionByEmployeeCodeID(int EmployeeCodeID, DateTime PromotionEndDate, int DaysCount)
        //       {
        //reutthis.GetEmployeeCurrentJob(EmployeeCodeID)
        //       }

        public EmployeesCareersHistoryBLL Get(OrganizationsJobsBLL OrganizationJob, CareersHistoryTypesEnum CareerHistoryType, EmployeesCodesBLL EmployeeCode)
        {
            EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistoryDAL().GetByOrganizationJobIDEmployeeCodeIDCareerHistoryTypeID(OrganizationJob.OrganizationJobID, (int)CareerHistoryType, EmployeeCode.EmployeeCodeID);
            EmployeesCareersHistoryBLL EmployeeCareerHistoryBLL = null;
            if (EmployeeCareerHistory != null)
                EmployeeCareerHistoryBLL = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(EmployeeCareerHistory);

            return EmployeeCareerHistoryBLL;
        }

        public void DeactivateAllCareerHistoryOfEmployee(int EmployeeCodeID)
        {
            try
            {
                List<EmployeesCareersHistory> EmployeesCareersHistoryList = new EmployeesCareersHistoryDAL().GetEmployeesCareersHistoryByEmployeeCodeID(EmployeeCodeID);
                EmployeesCareersHistory EmployeeCareerHistory = new EmployeesCareersHistory();
                foreach (var item in EmployeesCareersHistoryList)
                {
                    EmployeeCareerHistory = item;
                    EmployeeCareerHistory.IsActive = false;
                    EmployeeCareerHistory.LastUpdatedDate = DateTime.Now;
                    EmployeeCareerHistory.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;

                    new EmployeesCareersHistoryDAL().UpdateIsActive(EmployeeCareerHistory);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EmployeesCareersHistoryBLL> GetAllPreviousEmployeeCarrersHistoryByEmployeeCodeID(int EmployeeCodeID)
        {
            List<EmployeesCareersHistoryBLL> PreviousEmployeesBLLList = new List<EmployeesCareersHistoryBLL>();
            //List<EmployeesCareersHistory> PreviousEmployeesList = new EmployeesCareersHistoryDAL().GetAllPreviousEmployeeCarrersHistoryByEmployeeCodeID(EmployeeCodeID).ToList();
            //foreach (var item in PreviousEmployeesList)
            //{
            //    EmployeesCareersHistory _ech = new EmployeesCareersHistoryDAL().GetEmployeesCareersHistoryByEmployeeCareerHistoryID(item.EmployeeCareerHistoryID);
            //    PreviousEmployeesBLLList.Add(new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(_ech));
            //}
            return PreviousEmployeesBLLList;
        }
        public EmployeesCareersHistoryBLL MapEmployeeCareerHistory(EmployeesCareersHistory EmployeeCareerHistory)
        {
            try
            {
                EmployeesCareersHistoryBLL EmployeeCareerHistoryBLL = null;
                if (EmployeeCareerHistory != null)
                {
                    EmployeeCareerHistoryBLL = new EmployeesCareersHistoryBLL()
                    {
                        EmployeeCareerHistoryID = EmployeeCareerHistory.EmployeeCareerHistoryID,
                        CareerHistoryType = new CareersHistoryTypesBLL().MapCareerHistoryType(EmployeeCareerHistory.CareersHistoryTypes),
                        CareerDegree = new CareersDegreesBLL().MapCareerDegree(EmployeeCareerHistory.CareersDegrees),
                        OrganizationJob = new OrganizationsJobsBLL().MapOrganizationJob(EmployeeCareerHistory.OrganizationsJobs),
                        EmployeeCode = new EmployeesCodesBLL()
                        {
                            EmployeeCodeID = EmployeeCareerHistory.EmployeeCodeID,
                            EmployeeCodeNo = EmployeeCareerHistory.EmployeesCodes.EmployeeCodeNo,
                            Employee = new EmployeesBLL().MapEmployee(EmployeeCareerHistory.EmployeesCodes.Employees)
                        },
                        JoinDate = EmployeeCareerHistory.JoinDate,
                        TransactionStartDate = EmployeeCareerHistory.TransactionStartDate,
                        TransactionEndDate = EmployeeCareerHistory.TransactionEndDate,
                        IsActive = EmployeeCareerHistory.IsActive,
                        CreatedDate = EmployeeCareerHistory.CreatedDate,
                    };
                }
                return EmployeeCareerHistoryBLL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
