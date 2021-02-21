using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class BaseAssigningsBLL : CommonEntity, IEntity
    {
        public virtual int AssigningID
        {
            get;
            set;
        }

        public virtual DateTime AssigningStartDate
        {
            get;
            set;
        }

        public virtual AssigningsReasonsBLL AssigningReason
        {
            get;
            set;
        }

        public virtual AssigningsTypesBLL AssigningType
        {
            get;
            set;
        }

        public string AssigningReasonOther { get; set; }

        public string Notes { get; set; }

        public virtual bool IsFinished
        {
            get;
            set;
        }

        public virtual EmployeesCareersHistoryBLL EmployeeCareerHistory
        {
            get;
            set;
        }

        public virtual DateTime? AssigningEndDate
        {
            get;
            set;
        }

        public virtual AssigningsReasonsBLL AssigningEndReason
        {
            get;
            set;
        }

        internal virtual Assignings DALInstance
        {
            get
            {
                //if (this.AssigningEndDate != null && this.AssigningEndDate <= DateTime.Now.Date)
                //    this.IsFinished = true;
                //else
                //    this.IsFinished = false;

                Assignings Assigning = new Assignings();
                Assigning.AssigningTypID = this.AssigningType.AssigningTypeID;
                Assigning.AssigningStartDate = this.AssigningStartDate;
                Assigning.AssigningEndDate = this.AssigningEndDate;
                Assigning.EmployeeCareerHistoryID = this.EmployeeCareerHistory.EmployeeCareerHistoryID;
                Assigning.IsFinished = this.IsFinished;

                if (this.AssigningReason != null && this.AssigningReason.AssigningReasonID > 0)
                {
                    Assigning.AssigningReasonID = this.AssigningReason.AssigningReasonID;
                    Assigning.AssigningReasonOther = this.AssigningReasonOther;
                }

                if (this.AssigningID <= 0)
                {
                    Assigning.CreatedDate = DateTime.Now;
                    Assigning.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                }
                else
                {
                    Assigning.AssigningID = this.AssigningID;
                    Assigning.LastUpdatedDate = DateTime.Now;
                    Assigning.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                }
                return Assigning;
            }
        }

        public virtual Result Add()
        {
            Result result = IsValid();       //null;
            if (result == null)
            {
                BaseAssigningsBLL AssigningBLL = this.GetEmployeeActiveAssigning(this.EmployeeCareerHistory.EmployeeCareerHistoryID);
                if (AssigningBLL != null)
                {
                    result = new Result();
                    result.Entity = AssigningBLL;
                    result.EnumType = typeof(AssigningsValidationEnum);
                    result.EnumMember = AssigningsValidationEnum.RejectedBecauseOfActivePreviousAssigning.ToString();
                    return result;
                }
            }
            return result;
        }

        public virtual Result Update()
        {
            Result result = IsValid();       //null;

            if (result == null)
            {
                //BaseAssigningsBLL AssigningBLL = this.GetEmployeeActiveAssigning(this.EmployeeCareerHistory.EmployeeCareerHistoryID);
                //if (AssigningBLL != null && AssigningBLL.AssigningID != this.AssigningID)
                //{
                //    result = new Result();
                //    result.Entity = AssigningBLL;
                //    result.EnumType = typeof(AssigningsValidationEnum);
                //    result.EnumMember = AssigningsValidationEnum.RejectedBecauseOfActivePreviousAssigning.ToString();
                //    return result;
                //}

                #region Validation if the assigning is finished or not
                BaseAssigningsBLL AssigningBLL = GetByAssigningID(this.AssigningID);
                if (AssigningBLL.IsFinished)
                {
                    result = new Result();
                    result.Entity = AssigningBLL;
                    result.EnumType = typeof(AssigningsValidationEnum);
                    result.EnumMember = AssigningsValidationEnum.RejectedBecauseOfAlreadyFinished.ToString();
                    return result;
                }
                #endregion
            }
            return result;
        }

        public Result Remove(int AssigningID)
        {
            try
            {
                Result result = null;

                #region Validation if the assigning is finished or not
                BaseAssigningsBLL AssigningBLL = GetByAssigningID(AssigningID);
                if (AssigningBLL.IsFinished)
                {
                    result = new Result();
                    result.Entity = AssigningBLL;
                    result.EnumType = typeof(AssigningsValidationEnum);
                    result.EnumMember = AssigningsValidationEnum.RejectedBecauseOfAlreadyFinished.ToString();
                    return result;
                }
                #endregion

                new AssigningsDAL().Delete(AssigningID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(AssigningsValidationEnum),
                    EnumMember = AssigningsValidationEnum.Done.ToString()
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public BaseAssigningsBLL GetEmployeeActiveAssigning(int EmployeeCareerHistoryID)
        {
            try
            {
                return new BaseAssigningsBLL().MapAssigning(new AssigningsDAL().GetActiveAssigningByEmployeeCareerHistoryID(EmployeeCareerHistoryID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BaseAssigningsBLL GetEmployeeActiveAssigning(string EmployeeCodeNo)
        {
            try
            {
                return new BaseAssigningsBLL().MapAssigning(new AssigningsDAL().GetActiveAssigningByEmployeeCodeNo(EmployeeCodeNo));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<BaseAssigningsBLL> GetAssignings(out int totalRecordsOut, out int recFilterOut)
        {
            List<BaseAssigningsBLL> AssigningBLLList = new List<BaseAssigningsBLL>();
            List<Assignings> Assignings = new AssigningsDAL()
            {
                search = Search,
                order = Order,
                orderDir = OrderDir,
                startRec = StartRec,
                pageSize = PageSize
            }.GetAssignings(out totalRecordsOut, out recFilterOut);
            foreach (var item in Assignings)
                AssigningBLLList.Add(new BaseAssigningsBLL().MapAssigning(item));

            return AssigningBLLList;
        }

        public virtual List<BaseAssigningsBLL> GetAssignings()
        {
            List<BaseAssigningsBLL> AssigningBLLList = new List<BaseAssigningsBLL>();
            List<Assignings> Assignings = new AssigningsDAL().GetAssignings().Take(30).ToList();
            foreach (var item in Assignings)            
                AssigningBLLList.Add(new BaseAssigningsBLL().MapAssigning(item));
            
            return AssigningBLLList;
        }

        public virtual List<BaseAssigningsBLL> GetAssigningsWillExpire(DateTime FromDate, int MonthPeriod)
        {
            List<BaseAssigningsBLL> AssigningBLLList = new List<BaseAssigningsBLL>();
            List<Assignings> Assignings = new AssigningsDAL().GetAssigningsWillExpire(FromDate, MonthPeriod);
            foreach (var item in Assignings)
                AssigningBLLList.Add(new BaseAssigningsBLL().MapAssigning(item));

            return AssigningBLLList;
        }

        public virtual List<BaseAssigningsBLL> GetAssigningsAlreadyExpiredNotFinished()
        {
            List<BaseAssigningsBLL> AssigningBLLList = new List<BaseAssigningsBLL>();
            List<Assignings> Assignings = new AssigningsDAL().GetAssigningsAlreadyExpiredNotFinished();
            foreach (var item in Assignings)
                AssigningBLLList.Add(new BaseAssigningsBLL().MapAssigning(item));

            return AssigningBLLList;
        }

        public static BaseAssigningsBLL GetByAssigningID(int AssigningID)
        {
            BaseAssigningsBLL assigningBLL = null;
            Assignings assigning = new AssigningsDAL().GetByAssigningID(AssigningID);
            if (assigning != null)
                assigningBLL = new BaseAssigningsBLL().MapAssigning(assigning);

            return assigningBLL;
        }

        //public BaseAssigningsBLL GetActiveAssigningByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        //{
        //    BaseAssigningsBLL assigningBLL = new BaseAssigningsBLL();
        //    Assignings assigning = new AssigningsDAL().GetActiveAssigningByEmployeeCareerHistoryID(EmployeeCareerHistoryID);
        //    if (assigning != null)            
        //        assigningBLL = new BaseAssigningsBLL().MapAssigning(assigning);

        //    return assigningBLL;
        //}

        /// <summary>
        /// Task # 226 : 
        /// Finish last assigning, based on passing parameters
        /// this function called by different modules like Promotion, Delegation, or StopWork etc
        /// Task # 310 : change CareerHistoryID to EmployeeCodeID because of task 310
        /// Task 318: Service to Cancel EServicesProxies By System
        /// </summary>
        /// <param name="EmployeeCodeID"></param>
        /// <param name="AssigningEndDate"></param>
        /// <param name="EndAssigningReason"></param>
        /// <returns></returns>
        public Result BreakLastAssigning(int EmployeeCodeID, DateTime AssigningEndDate, AssigningsReasonsEnum EndAssigningReason, string EndAssigningReasonNotes = "")
        {
            Result result = new Result();
            BaseAssigningsBLL assigningBLL = new BaseAssigningsBLL();
            Assignings assigning = new AssigningsDAL().GetActiveAssigningByEmployeeCodeID(EmployeeCodeID);
            
            if (assigning != null)
            {
                if (assigning.IsFinished)
                {
                    result.Entity = this;
                    result.EnumType = typeof(AssigningsValidationEnum);
                    result.EnumMember = AssigningsValidationEnum.Done.ToString();
                    return result;
                }

                if (AssigningEndDate < assigning.AssigningStartDate)
                {
                    result = new Result();
                    result.EnumType = typeof(AssigningsValidationEnum);
                    result.EnumMember = AssigningsValidationEnum.RejectedBecauseOfEndDateIsLessThanCreationDate.ToString();

                    return result;
                }

                assigning.AssigningEndDate = AssigningEndDate;
                assigning.EndAssigningReasonID = (int)EndAssigningReason;
                assigning.IsFinished = true;
                assigning.Notes = EndAssigningReasonNotes;
                assigning.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                assigning.LastUpdatedDate = DateTime.Now;
                this.AssigningID = new AssigningsDAL().BreakAssigning(assigning);
                if (this.AssigningID != 0)
                {
                    #region Cancellation of all pending e vacations requests of employee after breaking last assigning of him
                    List<EVacationsRequests> PendingEVacationRequestsList = new EVacationsRequestsDAL().GetByEmployeeCodeID(EmployeeCodeID, (int)EVacationRequestStatusEnum.Pending);
                    foreach (var item in PendingEVacationRequestsList)
                        result = new EVacationsRequestsBLL().CancelEVacationRequest(item.EVacationRequestID, EVacationRequestStatusEnum.CancelledBySystem, Globalization.EVacationRequestCancelledBySystemBecauseOfBreakingLastAssigningText);
                    #endregion

                    #region Task 318: Service to Cancel EServicesProxies By System
                    try
                    {
                        result = new EServicesProxiesBLL() { LoginIdentity = this.LoginIdentity }
                                .RevokeEServiceProxyByEmployeeCodeID(EmployeeCodeID, EServicesProxiesStatusEnum.CancelledBySystem, Globalization.EServiceProxyCancelledBySystemBecauseOfBreakingLastAssigningText);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    #endregion

                    result.Entity = this;
                    result.EnumType = typeof(AssigningsValidationEnum);
                    result.EnumMember = AssigningsValidationEnum.Done.ToString();
                }
            }
            else
            {
                result.EnumType = typeof(AssigningsValidationEnum);
                result.EnumMember = AssigningsValidationEnum.RejectedBecauseAssigningNotFound.ToString();
            }

            return result;
        }

        internal BaseAssigningsBLL MapAssigning(Assignings Assigning)
        {
            try
            {
                BaseAssigningsBLL AssigningBLL = null;
                if (Assigning != null)
                {
                    if (Assigning.AssigningTypID == (Int32)AssigningsTypesEnum.Internal)
                    {
                        AssigningBLL = new InternalAssigningBLL()
                        {
                            Job = new JobsBLL().MapJob(Assigning.Jobs),
                            Organization = new OrganizationsStructuresBLL().MapOrganization(Assigning.OrganizationsStructures),
                            Manager = new EmployeesCodesBLL().MapEmployeeCode(Assigning.ManagerCode)
                        };

                        ((InternalAssigningBLL)AssigningBLL).Organization.FullOrganizationName = new OrganizationsStructuresBLL().GetOrganizationNameTillLastParentExceptPresident(((InternalAssigningBLL)AssigningBLL).Organization.OrganizationID);
                    }
                    else if (Assigning.AssigningTypID == (Int32)AssigningsTypesEnum.External)
                    {
                        AssigningBLL = new ExternalAssigningBLL()
                        {
                            ExternalKSACity = new KSACitiesBLL().MapKSACity(Assigning.KSACities),
                            ExternalOrganization = Assigning.ExternalOrganization,
                        };
                    }
                    AssigningBLL.AssigningID = Assigning.AssigningID;
                    AssigningBLL.AssigningStartDate = Assigning.AssigningStartDate;
                    AssigningBLL.AssigningEndDate = Assigning.AssigningEndDate;
                    AssigningBLL.AssigningType = new AssigningsTypesBLL().MapAssigningType(Assigning.AssigningsTypes);
                    AssigningBLL.EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(Assigning.EmployeesCareersHistory);
                    AssigningBLL.IsFinished = Assigning.IsFinished;
                    AssigningBLL.AssigningReason = new AssigningsReasonsBLL().MapAssigningReason(Assigning.AssigningsReasons);
                    AssigningBLL.AssigningReasonOther = "" + Assigning.AssigningReasonOther;
                    AssigningBLL.CreatedDate = Assigning.CreatedDate;
                    AssigningBLL.CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(Assigning.CreatedByNav);
                }
                return AssigningBLL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Result IsValid()
        {
            Result result = null;

            #region Validation on End date, end date must be greater than current date: "TASK#136 : the user is not able to create new assign record if the end date of this record before creation date."
            if (this.AssigningEndDate < DateTime.Now.Date)
            {
                result = new Result();
                result.EnumType = typeof(AssigningsValidationEnum);
                result.EnumMember = AssigningsValidationEnum.RejectedBecauseOfEndDateIsLessThanCreationDate.ToString();

                return result;
            }
            #endregion

            return result;
        }

        public virtual AssigningsReasonsBLL GetLastAssigningEndReasonsByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                List<Assignings> AssigningsList = new AssigningsDAL().GetAssigningsByEmployeeCodeID(EmployeeCodeID).ToList();
                if (AssigningsList.Count == 0) // thats mean the employee is new ... and the reason of assigning will be (Beacuse of hiring)
                    return new AssigningsReasonsBLL().GetByAssigningReasonID((int)AssigningsReasonsEnum.Hiring);
                else
                {
                    if (AssigningsList.Any(x => x.IsFinished == false)) // thats mean the employee already has active assigning and no Last Assignings End Reasons in this case
                        return null;

                    Assignings LastBrokenAssigning = new AssigningsDAL().GetAssigningsByEmployeeCodeID(EmployeeCodeID).OrderByDescending(x => x.AssigningEndDate)
                                                                                                                      .OrderByDescending(x => x.AssigningID)
                                                                                                                      .FirstOrDefault(x => x.IsFinished == true && x.AssigningTypID == (int)AssigningsTypesEnum.Internal);
                    if (LastBrokenAssigning != null && LastBrokenAssigning.EndAssigningReasonID.HasValue)
                        return new AssigningsReasonsBLL().MapAssigningReason(LastBrokenAssigning.EndAssigningsReasonsNav);

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

