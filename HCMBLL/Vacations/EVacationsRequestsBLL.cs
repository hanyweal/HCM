using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCMBLL
{
    public class EVacationsRequestsBLL : CommonEntity, IEntity
    {
        public int EVacationRequestID { get; set; }

        public EmployeesCareersHistoryBLL EmployeeCareerHistory { get; set; }

        public OrganizationsStructuresBLL ActualEmployeeOrganization { get; set; }

        public JobsBLL ActualEmployeeJob { get; set; }

        public VacationsTypesBLL VacationType { get; set; }

        public int EVacationRequestNo { get; set; }

        internal VacationsTypesEnum VacationTypeEnum { get; set; }

        public DateTime VacationStartDate { get; set; }

        public DateTime VacationEndDate { get; set; }

        public virtual int VacationPeriod
        {
            get
            {
                return this.VacationEndDate.Subtract(this.VacationStartDate).Days + 1;
            }
        }

        public virtual DateTime WorkDate
        {
            get
            {
                return this.VacationEndDate.AddDays(1);
            }
        }

        public int OldBalanceConsumed { get; set; }

        public string CreatorNotes { get; set; }

        public EVacationRequestStatusBLL EVacationRequestStatus { get; set; }

        public DateTime? ApprovalDateTime { get; set; }

        public EmployeesCodesBLL ApprovedBy { get; set; }

        public string ApproverNotes { get; set; }

        public string CancellationReasonByHR { get; set; }


        public List<EVacationsRequestsBLL> GetAllEVacationsRequests()
        {
            try
            {
                List<EVacationsRequests> EVacationsRequestsList = new EVacationsRequestsDAL().Get();
                List<EVacationsRequestsBLL> EVacationsRequestsBLLList = new List<EVacationsRequestsBLL>();
                EVacationsRequestsList.ForEach(x => EVacationsRequestsBLLList.Add(MapEVacationRequest(x)));

                return EVacationsRequestsBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal Result AddEVacationRequest()
        {
            try
            {
                Result result;
                PlacementBLL CurrentActualOrgAndActualJob = new PlacementBLL().GetCurrentActualOrgAndActualJob(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo);
                EVacationsRequests EVacationRequest = new EVacationsRequests()
                {
                    EmployeeCareerHistoryID = this.EmployeeCareerHistory.EmployeeCareerHistoryID,
                    //ActualOrganizationID = CurrentActualOrgAndActualJob != null ? CurrentActualOrgAndActualJob?.Organization?.OrganizationID : (int?)null,
                    //ActualJobID = CurrentActualOrgAndActualJob != null ? CurrentActualOrgAndActualJob?.Job?.JobID != 0 ? CurrentActualOrgAndActualJob?.Job?.JobID : (int?)null : null,
                    ActualOrganizationID = CurrentActualOrgAndActualJob?.Organization?.OrganizationID,
                    ActualJobID = CurrentActualOrgAndActualJob?.Job?.JobID == 0 ? null : CurrentActualOrgAndActualJob?.Job?.JobID,
                    VacationStartDate = this.VacationStartDate,
                    VacationEndDate = this.VacationEndDate,
                    CreatorNotes = this.CreatorNotes,
                    VacationTypeID = (int)this.VacationTypeEnum,
                    EVacationRequestNo = new EVacationsRequestsDAL().GetMaxEVacationRequestNoByYear(DateTime.Now.Year) + 1,
                    EVacationRequestStatusID = (int)EVacationRequestStatusEnum.Pending,
                    CreatedDate = DateTime.Now,
                    CreatedBy = this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID
                };

                #region Validation to check there is any e vacation request is pending or not 
                // pending means that no action by his manager till now or this e vacation is not cancelled by creator
                result = IsEVacationRequestPendingExist(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo);
                if (result != null) return result;
                #endregion

                #region Checking proxies
                #region Validation to check if the requester is authorized person in e services authorizations module ... if the requester has e services authorizations :
                // 1 - he can not create e vacation request if the vacation start date will be in the future
                // 2 - he can create e vacation request if the vacation period will be in the past
                if (this.VacationStartDate > DateTime.Now || this.VacationEndDate > DateTime.Now) // no need to check about of old vacations
                {
                    EServicesAuthorizationsBLL EServiceAuthorization = new EServicesAuthorizationsBLL().GetOrganizationAuthorizedPerson(CurrentActualOrgAndActualJob.Organization.OrganizationID, EServicesTypesEnum.Vacation);
                    if (EServiceAuthorization.AuthorizedPerson.EmployeeCodeNo == this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeNo) // thats mean the manager has authorization
                    {
                        // check the manager created valid proxy to e vacation service to other person or not
                        EServicesProxiesBLL ActiveProxy = new EServicesProxiesBLL().GetActiveByFromEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID, EServicesTypesEnum.Vacation);
                        if (ActiveProxy == null) // thats mean he did not create valid proxy to e vacation service to other person
                        {
                            result = new Result();
                            //result.Entity = this;
                            result.EnumMember = VacationsValidationEnum.RejectedBecauseOfNoActiveProxyCreatedToOtherPerson.ToString();
                            return result;
                        }
                    }

                    #region check the employee has proxy by other person or not
                    EServicesProxiesBLL ActiveEVacationServiceProxy = new EServicesProxiesBLL().GetActiveByToEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID).FirstOrDefault(x => x.EServiceType.EServiceTypeID == (int)EServicesTypesEnum.Vacation);
                    if (ActiveEVacationServiceProxy != null) // thats mean he is proxied by other person
                    {
                        result = new Result();
                        result.Entity = ActiveEVacationServiceProxy;
                        result.EnumMember = VacationsValidationEnum.RejectedBecauseOfEmployeeHasProxyByOtherPerson.ToString();
                        return result;
                    }
                    #endregion
                }

                #endregion
                #endregion

                result = new Result();
                new EVacationsRequestsDAL().Insert(EVacationRequest);
                this.EVacationRequestNo = EVacationRequest.EVacationRequestNo;
                result.Entity = this;
                result.EnumMember = VacationsValidationEnum.Done.ToString();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result CancelEVacationRequest(int EVacationRequestID, EVacationRequestStatusEnum CancelledBy, string CancellationReason = "")
        {
            try
            {
                Result result;
                EVacationsRequestsBLL EVacationRequestObj = new EVacationsRequestsBLL().GetEVacationsRequestsByEVacationRequestID(EVacationRequestID);

                #region Validation if Authorized person approved or rejected, no chance to cancel vacation after decision from Authorized person
                if (EVacationRequestObj.EVacationRequestStatus?.EVacationRequestStatusID != (int)EVacationRequestStatusEnum.Pending)
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfEVacationRequestStatusNotPending.ToString();
                    return result;
                }
                #endregion

                #region Changing status of eservice request
                EVacationsRequests EVacationRequest = new EVacationsRequests()
                {
                    EVacationRequestID = EVacationRequestID,
                    EVacationRequestStatusID = (int)CancelledBy,
                    CancellationReasonByHR = CancellationReason,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = CancelledBy == EVacationRequestStatusEnum.CancelledByHR ? this.LoginIdentity.EmployeeCodeID : EVacationRequestObj.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID
                };

                result = new Result();

                new EVacationsRequestsDAL().UpdateStatus(EVacationRequest);
                result.Entity = this;
                result.EnumMember = VacationsValidationEnum.Done.ToString();
                #endregion

                #region If cancellation by HR, Sending sms to employee to notify him
                if (CancelledBy == EVacationRequestStatusEnum.CancelledByHR || CancelledBy == EVacationRequestStatusEnum.CancelledBySystem)
                {
                    SMSBLL sMSBLL = new SMSBLL();
                    sMSBLL.SendSMS(new SMSLogsBLL()
                    {
                        BusinssSubCategory = BusinessSubCategoriesEnum.AuthorizedPersonDecisionForEVacationRequest,
                        DetailID = EVacationRequestObj.EVacationRequestID,
                        MobileNo = EVacationRequestObj.EmployeeCareerHistory.EmployeeCode.Employee.EmployeeMobileNo,
                        Message = string.Format(Globalization.SMSEVacationRequestCancelledByHrText),
                        CreatedBy = EVacationRequestObj.ApprovedBy,
                    });
                }
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update data in EVacation Requests table , then Send the vacation details to Vacations module to be added
        /// </summary>
        /// <returns></returns>
        private Result ApproveEVacationRequest(EVacationsRequestsBLL EVacationRequestObj, EVacationRequestStatusEnum EVacationRequestStatus)
        {
            try
            {
                Result result;

                EVacationsRequests EVacationRequest = new EVacationsRequests()
                {
                    EVacationRequestID = EVacationRequestID,
                    EVacationRequestStatusID = (int)EVacationRequestStatus,
                    ApprovalDateTime = DateTime.Now,
                    ApproverNotes = EVacationRequestObj.ApproverNotes,
                    ApprovedBy = EVacationRequestObj.ApprovedBy.EmployeeCodeID,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = EVacationRequestObj.ApprovedBy.EmployeeCodeID
                };

                result = new Result();

                new EVacationsRequestsDAL().Update(EVacationRequest);
                result.Entity = this;
                result.EnumMember = VacationsValidationEnum.Done.ToString();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<EVacationsRequestsBLL> GetEVacationsRequestsBasedOnStatus(string EmployeeCodeNo, EVacationRequestStatusEnum EVacationRequestStatus)
        {
            return GetEVacationsRequestsByEmployeeCodeNo(EmployeeCodeNo).Where(x => x.EVacationRequestStatus.EVacationRequestStatusID == (int)EVacationRequestStatus).ToList();
        }

        public List<EVacationsRequestsBLL> GetEVacationsRequestsByEmployeeCodeNo(string EmployeeCodeNo)
        {
            try
            {
                List<EVacationsRequests> EVacationsRequestsList = new EVacationsRequestsDAL().GetByEmployeeCodeNo(EmployeeCodeNo);
                List<EVacationsRequestsBLL> EVacationsRequestsBLLList = new List<EVacationsRequestsBLL>();
                EVacationsRequestsList.ForEach(x => EVacationsRequestsBLLList.Add(MapEVacationRequest(x)));

                return EVacationsRequestsBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EVacationsRequestsBLL GetEVacationsRequestsByEVacationRequestID(int EVacationRequestID)
        {
            try
            {
                EVacationsRequests EVacationRequest = new EVacationsRequestsDAL().GetByEVacationRequestID(EVacationRequestID);
                EVacationsRequestsBLL EVacationRequestBLL = null;
                if (EVacationRequest != null)
                    EVacationRequestBLL = MapEVacationRequest(EVacationRequest);

                // if the request still pending , we will show who the person that authorize to take decision for this request
                if (EVacationRequest.EVacationRequestStatusID == (int)EVacationRequestStatusEnum.Pending)
                    EVacationRequestBLL.ApprovedBy = new EmployeesCodesBLL().GetEVacationAuthorizedPersonOfEmployee(EVacationRequestBLL.CreatedBy.EmployeeCodeNo, EServicesTypesEnum.Vacation);

                return EVacationRequestBLL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal Result IsEVacationRequestPendingExist(string EmployeeCodeNo)
        {
            Result result = null;
            EVacationsRequestsBLL PendingEVacationsRequests = GetEVacationsRequestsBasedOnStatus(EmployeeCodeNo, EVacationRequestStatusEnum.Pending).FirstOrDefault();

            if (PendingEVacationsRequests != null)
            {
                result = new Result();
                result.EnumMember = VacationsValidationEnum.RejectedBecauseOfEVacationRequestPendingExist.ToString();
                this.EVacationRequestNo = PendingEVacationsRequests.EVacationRequestNo;
                result.Entity = this;
            }
            return result;
        }

        /// <summary>
        /// Get e vacations requests that approved by authorized perso
        /// </summary>
        /// <param name="ApproverCodeNo"></param>
        /// <returns></returns>
        public List<EVacationsRequestsBLL> GetDoneEVacationsRequestsByApproverCodeNo(string ApproverCodeNo)
        {
            try
            {
                List<EVacationsRequests> EVacationsRequestsList = new EVacationsRequestsDAL().GetByApproverCodeNo(ApproverCodeNo).Where(x => x.EVacationRequestStatusID != (int)EVacationRequestStatusEnum.Pending).ToList();
                List<EVacationsRequestsBLL> EVacationsRequestsBLLList = new List<EVacationsRequestsBLL>();
                if (EVacationsRequestsList.Count > 0)
                {
                    foreach (var item in EVacationsRequestsList)
                        EVacationsRequestsBLLList.Add(MapEVacationRequest(item));
                }

                return EVacationsRequestsBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EVacationsRequestsBLL> GetPendingEVacationsRequestsByAuthorizedPerson(string AuthorizedPersonCodeNo)
        {
            try
            {
                List<EServicesAuthorizationsBLL> OrganizationsList = new EServicesAuthorizationsBLL().GetBasedOnAuthorizedPerson(AuthorizedPersonCodeNo, EServicesTypesEnum.Vacation);
                List<int> OrganizationsIDs = new List<int>();
                OrganizationsList.ForEach(x => OrganizationsIDs.Add(x.Organization.OrganizationID));

                #region Get evacations requests of normal employees
                List<EVacationsRequests> EVacationsRequestsList = new EVacationsRequestsDAL().GetEVacationsRequestsByOrganizations(OrganizationsIDs).Where(x => x.EVacationRequestStatusID == (int)EVacationRequestStatusEnum.Pending).ToList();
                List<EVacationsRequestsBLL> EVacationsRequestsBLLList = new List<EVacationsRequestsBLL>();
                EVacationsRequestsList.ForEach(x => EVacationsRequestsBLLList.Add(MapEVacationRequest(x)));
                #endregion

                #region Get evacations requests of managers under him
                List<int> OrganizationIDList = new List<int>();
                new OrganizationsStructuresBLL().GetAllOrganizationsForManagerByManagerCodeNo(AuthorizedPersonCodeNo).ForEach(x => OrganizationIDList.Add(x.OrganizationID));
                //int? OrganizationID = new OrganizationsStructuresBLL().GetAllOrganizationsForManagerByManagerCodeNo(AuthorizedPersonCodeNo).FirstOrDefault()?.OrganizationID;
                foreach (var OrganizationID in OrganizationIDList)
                {
                    List<int> ChildOrganizationIDs = new OrganizationsStructuresBLL().GetOrganizationFirstLevelByID(OrganizationID);
                    List<EVacationsRequests> ChildOrganizationIDEVacationsRequestsList = new EVacationsRequestsDAL().GetEVacationsRequestsByOrganizations(ChildOrganizationIDs).Where(x => x.EVacationRequestStatusID == (int)EVacationRequestStatusEnum.Pending
                                                                                                                                                                                    && !x.ActualJobID.HasValue) // thats mean is manager
                                                                                                                                                                                    .ToList();
                    ChildOrganizationIDEVacationsRequestsList.ForEach(x => EVacationsRequestsBLLList.Add(MapEVacationRequest(x)));
                }
                #endregion

                return EVacationsRequestsBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result MakeDecisionOfEVacationRequest(EVacationsRequestsBLL EVacationRequestObj, EVacationRequestStatusEnum EVacationRequestStatus)
        {
            try
            {
                Result result = null;
                EVacationRequestObj.ApprovedBy = new EmployeesCodesBLL().GetByEmployeeCodeNo(EVacationRequestObj.ApprovedBy.EmployeeCodeNo);
                EVacationsRequestsBLL EVacationsRequestsData = new EVacationsRequestsBLL().GetEVacationsRequestsByEVacationRequestID(EVacationRequestObj.EVacationRequestID);

                #region Validate there is a decision of this e vacation request or not
                if (EVacationsRequestsData.EVacationRequestStatus.EVacationRequestStatusID != (int)EVacationRequestStatusEnum.Pending)
                {
                    result = new Result();
                    result.Entity = EVacationsRequestsData;
                    result.EnumMember = VacationsValidationEnum.RejectedBecauseOfEVacationRequestStatusNotPending.ToString();
                    return result;
                }
                #endregion

                #region Validate the approver person is authorized to employee manager or not
                EmployeesCodesBLL ActualAuthorizedPerson = new EServicesAuthorizationsBLL().GetOrganizationAuthorizedPerson(EVacationsRequestsData.ActualEmployeeOrganization.OrganizationID, EServicesTypesEnum.Vacation).AuthorizedPerson;
                if (ActualAuthorizedPerson.EmployeeCodeNo != EVacationRequestObj.ApprovedBy.EmployeeCodeNo)
                {
                    result = new Result();
                    result.EnumMember = VacationsValidationEnum.RejectedBeacuseOfApproverIsNotAuthorizedPerson.ToString();
                    result.Entity = ActualAuthorizedPerson;
                    return result;
                }
                #endregion

                string SMSMessage = string.Empty;
                // in case of approval, send the vacation data to vacations module to be added, after that change the status in e vacation requests module
                if (EVacationRequestStatus == EVacationRequestStatusEnum.Approved)
                {
                    #region Send vacation to vacations module
                    BaseVacationsBLL Vacation = new BaseVacationsBLL()
                    {
                        IsApprovedFromManager = true,
                        EVacationsRequest = EVacationsRequestsData,
                        EmployeeCareerHistory = EVacationsRequestsData.EmployeeCareerHistory,
                        VacationType = VacationsTypesEnum.Normal,
                        VacationStartDate = EVacationsRequestsData.VacationStartDate,
                        VacationEndDate = EVacationsRequestsData.VacationEndDate,
                        Notes = EVacationsRequestsData.CreatorNotes,
                        ApprovedBy = EVacationRequestObj.ApprovedBy,
                        LoginIdentity = EVacationsRequestsData.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsCanceled = false,
                    };
                    result = Vacation.Add();
                    #endregion

                    #region Update IsApproved in vacations module
                    result = Vacation.Approve();
                    #endregion

                    if (result.EnumMember == VacationsValidationEnum.Done.ToString())
                        result = ApproveEVacationRequest(EVacationRequestObj, EVacationRequestStatus);

                    SMSMessage = string.Format(Globalization.SMSEVacationRequestApprovedText, EVacationsRequestsData.VacationType.VacationTypeName, EVacationsRequestsData.VacationStartDate, EVacationsRequestsData.VacationPeriod);
                }
                else  // in case of refuse, change the status in e vacation requests module only
                {
                    result = ApproveEVacationRequest(EVacationRequestObj, EVacationRequestStatus);
                    SMSMessage = Globalization.SMSEVacationRequestRefusedText;
                }

                #region Sending sms to employee to notify him the authorized person decision
                SMSBLL sMSBLL = new SMSBLL();
                sMSBLL.SendSMS(new SMSLogsBLL()
                {
                    BusinssSubCategory = BusinessSubCategoriesEnum.AuthorizedPersonDecisionForEVacationRequest,
                    DetailID = EVacationsRequestsData.EVacationRequestID,
                    MobileNo = EVacationsRequestsData.EmployeeCareerHistory?.EmployeeCode?.Employee?.EmployeeMobileNo,
                    Message = SMSMessage,
                    CreatedBy = EVacationRequestObj.ApprovedBy,
                });
                #endregion

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal EVacationsRequestsBLL MapEVacationRequest(EVacationsRequests EVacationRequest)
        {
            try
            {
                EVacationsRequestsBLL EVacationRequestBLL = null;
                if (EVacationRequest != null)
                {
                    EVacationRequestBLL = new EVacationsRequestsBLL()
                    {
                        EVacationRequestID = EVacationRequest.EVacationRequestID,
                        EVacationRequestNo = EVacationRequest.EVacationRequestNo,
                        VacationType = new VacationsTypesBLL().MapVacationsTypes(EVacationRequest.VacationsTypes),
                        VacationStartDate = EVacationRequest.VacationStartDate,
                        VacationEndDate = EVacationRequest.VacationEndDate,
                        EmployeeCareerHistory = new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(EVacationRequest.EmployeesCareersHistory),
                        ActualEmployeeOrganization = new OrganizationsStructuresBLL().MapOrganizationWithoutManager(EVacationRequest.OrganizationsStructures),
                        ActualEmployeeJob = new JobsBLL().MapJob(EVacationRequest.Jobs),
                        EVacationRequestStatus = new EVacationRequestStatusBLL() { EVacationRequestStatusID = EVacationRequest.EVacationRequestStatusID.Value, EVacationRequestStatusName = EVacationRequest.EVacationRequestsStatus.EVacationRequestStatusName },
                        CreatorNotes = EVacationRequest.CreatorNotes,
                        ApprovedBy = new EmployeesCodesBLL().MapEmployeeCode(EVacationRequest.ApprovedByNav),
                        ApprovalDateTime = EVacationRequest.ApprovalDateTime,
                        ApproverNotes = EVacationRequest.ApproverNotes,
                        CancellationReasonByHR = EVacationRequest.CancellationReasonByHR,
                        CreatedBy = new EmployeesCodesBLL().MapEmployeeCode(EVacationRequest.CreatedByNav),
                        CreatedDate = EVacationRequest.CreatedDate,
                        LastUpdatedBy = new EmployeesCodesBLL().MapEmployeeCode(EVacationRequest.LastUpdatedByNav),
                        LastUpdatedDate = EVacationRequest.LastUpdatedDate
                    };
                }
                return EVacationRequestBLL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
