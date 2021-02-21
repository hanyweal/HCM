using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCMBLL
{
    public class EndOfServicesBLL : CommonEntity, IEntity
    {
        public int EndOfServiceID { get; set; }

        public DateTime EndOfServiceDate { get; set; }

        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public string EndOfServiceDecisionNo { get; set; }

        public DateTime EndOfServiceDecisionDate { get; set; }

        public EndOfServicesReasonsBLL EndOfServiceReason { get; set; }        

        public virtual Result Add()
        {
            Result result = new Result();
            EndOfServices EndOfService = new EndOfServices();

            if (new StopWorksBLL().IsStopWorkExistsByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCareerHistoryID, this.EndOfServiceDate))
            {
                result = new Result();
                result.Entity = this;
                result.EnumMember = EndOfServicesValidationEnum.RejectedBecauseOfConflictWithStopWorks.ToString();
                result.EnumType = typeof(EndOfServicesValidationEnum);
                return result;
            }

            EndOfService.EndOfServiceDate = this.EndOfServiceDate;
            EndOfService.EmployeeCareerHistoryID = this.EmployeeCareerHistory.EmployeeCareerHistoryID;
            EndOfService.EndOfServiceDecisionNo = this.EndOfServiceDecisionNo;
            EndOfService.EndOfServiceDecisionDate = this.EndOfServiceDecisionDate;
            EndOfService.EndOfServiceReasonID = this.EndOfServiceReason.EndOfServiceReasonID;
            EndOfService.CreatedDate = DateTime.Now;
            EndOfService.CreatedBy = this.LoginIdentity.EmployeeCodeID;
            this.EndOfServiceID = new EndOfServicesDAL().Insert(EndOfService);
            if (this.EndOfServiceID != 0)
            {
                result.Entity = this;
                result.EnumType = typeof(EndOfServicesValidationEnum);
                result.EnumMember = EndOfServicesValidationEnum.Done.ToString();
            }

            return result;
        }

        public virtual Result Update()
        {
            Result result = new Result();
            EndOfServices EndOfService = new EndOfServices();

            if (this.EndOfServiceID > 0)
                this.EmployeeCareerHistory = new EmployeesCareersHistoryBLL() { EmployeeCareerHistoryID = GetByEndOfServiceID(this.EndOfServiceID).EmployeeCareerHistory.EmployeeCareerHistoryID };

            if (new StopWorksBLL().IsStopWorkExistsByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCareerHistoryID, this.EndOfServiceDate))
            {
                result = new Result();
                result.Entity = this;
                result.EnumMember = EndOfServicesValidationEnum.RejectedBecauseOfConflictWithStopWorks.ToString();
                result.EnumType = typeof(EndOfServicesValidationEnum);
                return result;
            }
            else if(this.EndOfServiceDate <= DateTime.Now.Date)
            {
                result = new Result();
                result.Entity = this;
                result.EnumMember = EndOfServicesValidationEnum.RejectedBecauseOfEndOfServicesDateIsPassedAway.ToString();
                result.EnumType = typeof(EndOfServicesValidationEnum);
                return result;
            }

            EndOfService.EndOfServiceID = this.EndOfServiceID;
            EndOfService.EndOfServiceDate = this.EndOfServiceDate;
            EndOfService.EmployeeCareerHistoryID = this.EmployeeCareerHistory.EmployeeCareerHistoryID;
            EndOfService.EndOfServiceDecisionNo = this.EndOfServiceDecisionNo;
            EndOfService.EndOfServiceDecisionDate = this.EndOfServiceDecisionDate;
            EndOfService.EndOfServiceReasonID = this.EndOfServiceReason.EndOfServiceReasonID;
            EndOfService.LastUpdatedDate = DateTime.Now;
            EndOfService.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
            this.EndOfServiceID = new EndOfServicesDAL().Update(EndOfService);
            if (this.EndOfServiceID != 0)
            {
                result.Entity = this;
                result.EnumType = typeof(LookupsValidationEnum);
                result.EnumMember = LookupsValidationEnum.Done.ToString();
            }
            return result;
        }

        public Result Remove(int EndOfServiceID)
        {
            try
            {
                Result result = null;

                EndOfServicesBLL EndOfService = GetByEndOfServiceID(EndOfServiceID);
                if (EndOfService.EndOfServiceDate <= DateTime.Now.Date)
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumMember = EndOfServicesValidationEnum.RejectedBecauseOfEndOfServicesDateIsPassedAway.ToString();
                    result.EnumType = typeof(EndOfServicesValidationEnum);
                    return result;
                }

                new EndOfServicesDAL().Delete(EndOfServiceID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(EndOfServicesValidationEnum),
                    EnumMember = EndOfServicesValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        public List<EndOfServicesBLL> GetEndOfServices(out int totalRecordsOut, out int recFilterOut)
        {
            List<EndOfServices> EndOfServicesList = new EndOfServicesDAL(){
                    search = Search,
                    order = Order,
                    orderDir = OrderDir,
                    startRec = StartRec,
                    pageSize = PageSize
            }.GetEndOfServices(out totalRecordsOut, out recFilterOut);
            List<EndOfServicesBLL> EndOfServicesBLLList = new List<EndOfServicesBLL>();
            if (EndOfServicesList.Count > 0)
            {
                foreach (var item in EndOfServicesList)
                    EndOfServicesBLLList.Add(MapEndOfService(item));
            }

            return EndOfServicesBLLList;
        }

        public List<EndOfServicesBLL> GetEndOfServices()
        {
            List<EndOfServices> EndOfServicesList = new EndOfServicesDAL().GetEndOfServices();
            List<EndOfServicesBLL> EndOfServicesBLLList = new List<EndOfServicesBLL>();
            if (EndOfServicesList.Count > 0)
            {
                foreach (var item in EndOfServicesList)
                    EndOfServicesBLLList.Add(MapEndOfService(item));
            }

            return EndOfServicesBLLList;
        }

        public EndOfServicesBLL GetByEndOfServiceID(int EndOfServiceID)
        {
            return GetEndOfServices().SingleOrDefault(x => x.EndOfServiceID.Equals(EndOfServiceID));
        }

        public List<EndOfServicesBLL> GetByEmployeeCareerHistoryID(int EmployeeCareerHistoryID)
        {
            return GetEndOfServices().Where(x => x.EmployeeCareerHistory.EmployeeCareerHistoryID.Equals(EmployeeCareerHistoryID)).ToList();
        }

        /// <summary>
        /// This function called by Windows services and schedule to run on daily basis
        /// This function to get all EOS records from EOStable with isprocessed null/0 and EOSDate is <= Current Date
        /// Following action is performed:
        /// 1 - Create Employee Career History record for EOS type
        /// 2 - Deactivate all Employee Career History records of each EOS record
        /// 3 - Deactivate employee
        /// 4 - Set Job as Vacant 
        /// 5 - Mark isProcessed = 1 after successful of above 4 steps
        /// </summary>
        /// <returns></returns>
        public virtual Result StartProcess()
        {
            try
            {
                Result result = new Result();
                List<EndOfServicesBLL> EndOfServicesNotProcessed = this.GetEndOfServicesNotProcessed();
                foreach (EndOfServicesBLL item in EndOfServicesNotProcessed)
                {
                    if (DateTime.Now.Date >= item.EndOfServiceDate.Date)
                    {
                        #region Adding new record with new career history type End of service                    
                        EmployeesCareersHistoryBLL EmployeeCareerHistory = new EmployeesCareersHistoryBLL().Get(item.EmployeeCareerHistory.OrganizationJob, CareersHistoryTypesEnum.EndOfService, item.EmployeeCareerHistory.EmployeeCode);
                        if (EmployeeCareerHistory == null)
                        {
                            result = new EmployeesCareersHistoryBLL()
                            {
                                EmployeeCode = item.EmployeeCareerHistory.EmployeeCode,
                                OrganizationJob = item.EmployeeCareerHistory.OrganizationJob,
                                CareerHistoryType = new CareersHistoryTypesBLL() { CareerHistoryTypeID = (int)CareersHistoryTypesEnum.EndOfService },
                                CareerDegree = item.EmployeeCareerHistory.CareerDegree,
                                JoinDate = item.EndOfServiceDate,
                                TransactionStartDate = item.EndOfServiceDate,
                                IsActive = false,
                                LoginIdentity = item.CreatedBy,
                            }.Add();

                            if (result.EnumMember != CareersHistoryValidationEnum.Done.ToString())
                                return result;
                            else
                            {
                                #region Deactivate all career history of employee
                                new EmployeesCareersHistoryBLL() { LoginIdentity = item.CreatedBy }.DeactivateAllCareerHistoryOfEmployee(item.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);
                                #endregion
                            }
                        }
                        #endregion

                        #region Deactivate employee
                        result = new EmployeesCodesBLL() { LoginIdentity = item.CreatedBy }.DeactivateEmployee(item.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);
                        if (result.EnumMember != CareersHistoryValidationEnum.Done.ToString())
                            return result;
                        #endregion

                        #region Set his job as job vacant
                        result = new OrganizationsJobsBLL() { LoginIdentity = item.CreatedBy }.SetJobAsVacant(item.EmployeeCareerHistory.OrganizationJob.OrganizationJobID);
                        #endregion

                        #region Update is process to true
                        result = new EndOfServicesBLL() { LoginIdentity = item.CreatedBy }.MarkIsProcessToDone(item.EndOfServiceID);
                        if (result.EnumMember != CareersHistoryValidationEnum.Done.ToString())
                            return result;
                        #endregion
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal virtual List<EndOfServicesBLL> GetEndOfServicesNotProcessed()
        {
            try
            {
                List<EndOfServicesBLL> BLLList = new List<EndOfServicesBLL>();
                List<EndOfServices> EOSs = new EndOfServicesDAL().GetEndOfServicesNotProccessed();
                EOSs.ForEach(x => BLLList.Add(MapEndOfService(x)));
                return BLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal virtual Result MarkIsProcessToDone(int EndOfServiceID)
        {
            try
            {
                Result result = null;
                new EndOfServicesDAL().UpdateIsProcess(new EndOfServices()
                {
                    EndOfServiceID = EndOfServiceID,
                    isProcessed = true,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
                });
                return result = new Result()
                {
                    EnumType = typeof(EndOfServicesValidationEnum),
                    EnumMember = EndOfServicesValidationEnum.Done.ToString()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal EndOfServicesBLL MapEndOfService(EndOfServices item)
        {
            return item != null ?
                new EndOfServicesBLL()
                    {
                        EndOfServiceID = item.EndOfServiceID,
                        EndOfServiceDate = item.EndOfServiceDate,
                        EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item.EmployeesCareersHistory),
                        EndOfServiceDecisionNo = item.EndOfServiceDecisionNo,
                        EndOfServiceDecisionDate = item.EndOfServiceDecisionDate,
                        EndOfServiceReason = new EndOfServicesReasonsBLL().MapEndOfServiceReason(item.EndOfServicesReasons),
                        CreatedDate = item.CreatedDate,
                        CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(item.CreatedByNav)
                    }
                : null;
        }
    
    }
}