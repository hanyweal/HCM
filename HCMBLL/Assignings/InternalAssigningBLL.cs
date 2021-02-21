using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HCMDAL;
using HCMBLL.Enums;
using HCMBLL.DTO;

namespace HCMBLL
{
    public class InternalAssigningBLL : BaseAssigningsBLL
    {
        public virtual JobsBLL Job
        {
            get;
            set;
        }

        public virtual OrganizationsStructuresBLL Organization
        {
            get;
            set;
        }

        public EmployeesCodesBLL Manager { get; set; }

        public virtual List<InternalAssigningBLL> GetByEmployeeCodeID(int EmployeeCodeID)
        {
            throw new System.NotImplementedException();
        }

        public override Result Add()
        {
            try
            {
                Result result = base.Add();
                if (result != null)
                    return result;

                int EmployeeCodeID = this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID;
                DateTime StartDate = this.AssigningStartDate;
                DateTime EndDate = this.AssigningEndDate ?? DateTime.Now.Date;

                #region Validaion for conflict with lender
                EmployeesCareersHistoryBLL EmployeesCareersHistory = new EmployeesCareersHistoryBLL().GetActiveByEmployeeCareerHistoryID(this.EmployeeCareerHistory.EmployeeCareerHistoryID);
                //if (EmployeesCareersHistory != null && EmployeesCareersHistory.EmployeeCareerHistoryID > 0)
                //    EmployeeCodeID = EmployeesCareersHistory.EmployeeCode.EmployeeCodeID;

                List<LendersBLL> LendersBLLList = new LendersBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                if (LendersBLLList.Count() != 0)
                {
                    result = new Result();
                    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithLender.ToString();
                    return result;
                }
                #endregion

                #region Validaion for conflict with StopWorks
                List<StopWorksBLL> StopWorksBLLList = new StopWorksBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                if (StopWorksBLLList.Count() != 0)
                {
                    result = new Result();
                    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithStopWork.ToString();
                    return result;
                }
                #endregion

                #region Validaion for conflict with scholarship
                List<BaseScholarshipsBLL> BaseScholarshipsBLLList = new BaseScholarshipsBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                if (BaseScholarshipsBLLList.Count() != 0)
                {
                    result = new Result();
                    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithScholarship.ToString();
                    return result;
                }
                #endregion

                #region Validaion for conflict with External Assigning
                List<ExternalAssigningBLL> ExternalAssigningBLLList = new ExternalAssigningBLL().GetByEmployeeCodeID(EmployeeCodeID, StartDate, EndDate);
                if (ExternalAssigningBLLList.Count() != 0)
                {
                    result = new Result();
                    result.EnumType = typeof(NoConflictWithOtherProcessValidationEnum);
                    result.EnumMember = NoConflictWithOtherProcessValidationEnum.RejectedBecauseOfConflictWithExternalAssigning.ToString();
                    return result;
                }
                #endregion

                result = new Result();
                Assignings assigning = DALInstance;
                assigning.JobID = this.Job.JobID;
                assigning.ManagerCodeID = this.Manager != null ? this.Manager.EmployeeCodeID : (int?)null;
                assigning.OrganizationID = this.Organization.OrganizationID;
                assigning.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                this.AssigningID = new AssigningsDAL().Insert(assigning);
                if (this.AssigningID != 0)
                {
                    result.Entity = this;
                    result.EnumType = typeof(AssigningsValidationEnum);
                    result.EnumMember = AssigningsValidationEnum.Done.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override Result Update()
        {
            try
            {
                Result result = base.Update();
                if (result != null)
                    return result;

                result = new Result();
                Assignings assigning = DALInstance;
                assigning.JobID = this.Job.JobID;
                assigning.OrganizationID = this.Organization.OrganizationID;
                assigning.ManagerCodeID = this.Manager != null ? this.Manager.EmployeeCodeID : (int?)null;
                assigning.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;
                this.AssigningID = new AssigningsDAL().Update(assigning);
                if (this.AssigningID != 0)
                {
                    result.Entity = this;
                    result.EnumType = typeof(AssigningsValidationEnum);
                    result.EnumMember = AssigningsValidationEnum.Done.ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result AssignEmployeeUnderManager()
        {
            try
            {
                Result result;
                //Result result = base.Add();
                //if (result != null)
                //    return result;

                //int EmployeeCodeID = this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID;
                //DateTime StartDate = this.AssigningStartDate;
                //DateTime EndDate = this.AssigningEndDate ?? DateTime.Now.Date;

                #region Validate if the placement period finished or not
                result = CommonHelper.IsValidToCompleteEmployeesPlacement();
                if (result != null)
                    return result;
                #endregion

                #region Validate if the employee has active Assigning or not
                BaseAssigningsBLL ActiveEmployeeAssigning = new EmployeesCodesBLL().GetAssigningsByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID).FirstOrDefault(x => x.IsFinished == false);
                if (ActiveEmployeeAssigning != null)
                {
                    result = new Result();
                    result.Entity = ActiveEmployeeAssigning;
                    result.EnumType = typeof(AssigningsValidationEnum);
                    result.EnumMember = AssigningsValidationEnum.RejectedBecauseOfActivePreviousAssigning.ToString();
                    return result;
                }
                #endregion


                AssigningsReasonsBLL AssigningReason = GetLastAssigningEndReasonsByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID);
                Assignings assigning = new Assignings();
                assigning.AssigningTypID = (int)AssigningsTypesEnum.Internal;
                assigning.AssigningReasonID = AssigningReason != null ? AssigningReason.AssigningReasonID : (int)AssigningsReasonsEnum.BasedOnWorkNeeds;
                assigning.AssigningStartDate = DateTime.Now.Date;
                assigning.ManagerCodeID = this.Manager.EmployeeCodeID;
                assigning.OrganizationID = this.Organization.OrganizationID;
                assigning.JobID = this.Job.JobID;
                assigning.EmployeeCareerHistoryID = this.EmployeeCareerHistory?.EmployeeCareerHistoryID;
                assigning.CreatedBy = this.LoginIdentity.EmployeeCodeID;
                assigning.CreatedDate = DateTime.Now;
                this.AssigningID = new AssigningsDAL().Insert(assigning);

                if (this.AssigningID != 0)
                {
                    // this to get info of employee
                    EmployeesCodesBLL EmployeeCodeBLL = new EmployeesCodesBLL().GetByEmployeeCodeID(this.EmployeeCareerHistory.EmployeeCode != null ? this.EmployeeCareerHistory.EmployeeCode.EmployeeCodeID : 0);

                    // this to get info of manager
                    EmployeesCodesBLL ManagerCodeBLL = new EmployeesCodesBLL().GetByEmployeeCodeID(this.Manager.EmployeeCodeID);

                    if (EmployeeCodeBLL != null)
                    {
                        SMSLogsBLL SMSLogBLL = new SMSLogsBLL()
                        {
                            BusinssSubCategory = BusinessSubCategoriesEnum.OrganizationStructure,
                            MobileNo = EmployeeCodeBLL.Employee.EmployeeMobileNo,
                            DetailID = 0,
                            Message = string.Format(Globalization.SMSEmployeeAlreadyAssignedUnderManagerMessageText, EmployeeCodeBLL.Employee.FirstNameAr + " " + EmployeeCodeBLL.Employee.LastNameAr, new OrganizationsStructuresBLL().GetOrganizationNameTillLastParentExceptPresident(this.Organization.OrganizationID), ManagerCodeBLL.Employee.EmployeeNameAr),
                            CreatedBy = this.LoginIdentity,
                            CreatedDate = DateTime.Now,
                        };
                        new SMSBLL().SendSMS(SMSLogBLL);
                    }

                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(AssigningsValidationEnum);
                    result.EnumMember = AssigningsValidationEnum.Done.ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result RemoveEmployeeFromManager()
        {
            try
            {
                Result result = null;

                #region Validate if the Placement period finished or not
                result = CommonHelper.IsValidToCompleteEmployeesPlacement();
                if (result != null)
                    return result;
                #endregion

                Assignings Assigning = new AssigningsDAL().GetByAssigningID(this.AssigningID);
                new AssigningsDAL().Delete(this.AssigningID, this.LoginIdentity.EmployeeCodeID);

                if (Assigning != null)
                {
                    SMSLogsBLL SMSLogBLL = new SMSLogsBLL()
                    {
                        BusinssSubCategory = BusinessSubCategoriesEnum.OrganizationStructure,
                        MobileNo = Assigning.EmployeesCareersHistory.EmployeesCodes.Employees.EmployeeMobileNo,
                        DetailID = 0,
                        Message = string.Format(Globalization.SMSEmployeeAlreadyRemovedFromOrganizationManagerMessageText, Assigning.EmployeesCareersHistory.EmployeesCodes.Employees.FirstNameAr + " " + Assigning.EmployeesCareersHistory.EmployeesCodes.Employees.LastNameAr, new OrganizationsStructuresBLL().GetOrganizationNameTillLastParentExceptPresident(Assigning.OrganizationID.Value)),
                        CreatedBy = this.LoginIdentity,
                        CreatedDate = DateTime.Now,
                    };
                    new SMSBLL().SendSMS(SMSLogBLL);
                }

                result = new Result();
                result.EnumType = typeof(AssigningsValidationEnum);
                result.EnumMember = AssigningsValidationEnum.Done.ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InternalAssigningBLL> GetEmployeesUnderManagerByOrganization()
        {
            try
            {
                List<Assignings> AssigningsList = new AssigningsDAL().GetInternalAssigningsByManagerCodeIDOrganizationID(this.Manager.EmployeeCodeID, this.Organization.OrganizationID);
                List<InternalAssigningBLL> InternalAssigningBLLList = new List<InternalAssigningBLL>();
                foreach (var item in AssigningsList)
                    InternalAssigningBLLList.Add((InternalAssigningBLL)MapAssigning(item));

                return InternalAssigningBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InternalAssigningBLL> GetActiveEmployeesAssigingByOrganization(int OrganizationID)
        {
            try
            {
                List<Assignings> AssigningsList = new AssigningsDAL().GetActiveInternalAssigningsByOrganizationID(OrganizationID).ToList();
                List<InternalAssigningBLL> InternalAssigningBLLList = new List<InternalAssigningBLL>();
                foreach (var item in AssigningsList)
                    InternalAssigningBLLList.Add((InternalAssigningBLL)MapAssigning(item));

                return InternalAssigningBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<InternalAssigningBLL> Get()
        {
            try
            {
                List<Assignings> AssigningsList = new AssigningsDAL().GetAssignings().Where(x => x.AssigningsTypes.AssigningTypID == (int)AssigningsTypesEnum.Internal).ToList();
                List<InternalAssigningBLL> InternalAssigningBLLList = new List<InternalAssigningBLL>();
                foreach (var item in AssigningsList)
                    InternalAssigningBLLList.Add((InternalAssigningBLL)MapAssigning(item));

                return InternalAssigningBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual OrganizationsStructuresBLL GetCurrentManagerOfEmployee(int EmployeeCareerHistoryID)
        {
            try
            {
                BaseAssigningsBLL ActiveAssigning = new BaseAssigningsBLL().GetEmployeeActiveAssigning(EmployeeCareerHistoryID);
                if (ActiveAssigning != null)
                    return ((InternalAssigningBLL)ActiveAssigning).Organization;
                else
                {
                    string ManagerCodeNo = new EmployeesCareersHistoryBLL().GetByEmployeeCareerHistoryID(EmployeeCareerHistoryID).EmployeeCode.EmployeeCodeNo;
                    List<OrganizationsStructuresBLL> AllOrganizationsForManager = new OrganizationsStructuresBLL().GetAllOrganizationsForManagerByManagerCodeNo(ManagerCodeNo);
                    return AllOrganizationsForManager.FirstOrDefault().ParentOrganization;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<AssigngingsDTO> GetActualEmployeesBasedOnAssigningsAsRanksCategories(int OrganizationID)
        {
            try
            {
                List<int> OrganizationIDsList = new OrganizationsStructuresBLL().GetByOrganizationIDsWithhAllChilds(OrganizationID);

                // Get actual employees Based On Assignings by date
                List<vwActualEmployeesBasedOnAssignings> ActualEmployeesBasedOnAssignings = new AssigningsDAL().GetActualEmployeeBasedOnAssignings().Where(x => OrganizationIDsList.Contains(x.OrganizationID.Value)).ToList();

                var query = ActualEmployeesBasedOnAssignings.Select(y => new AssigngingsDTO(y.EmployeeCodeNo,
                                                                            y.EmployeeNameAr,
                                                                            y.OrganizationName,
                                                                            y.JobName,
                                                                            y.RankCategoryName,
                                                                            y.RankName,
                                                                            y.Sorting
                                                                            ));


                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<ChartsAxis> GetActualEmployeesBasedOnAssigningsAsRanks(string RankCategoryName, int OrganizationID)
        //{
        //    try
        //    {
        //        List<int> OrganizationIDsList = new OrganizationsStructuresBLL().GetByOrganizationIDsWithhAllChilds(OrganizationID);
        //        var query = new AssigningsDAL().GetActualEmployeeBasedOnAssignings().Where(x => x.IsFinished == false && x.RankCategoryName == RankCategoryName && OrganizationIDsList.Contains(x.OrganizationID.Value))
        //                                                                            .GroupBy(x => x.RankName)
        //                                                                            .Select(y => new
        //                                                                            {
        //                                                                                RankCategoryName = y.Key,
        //                                                                                RecordCount = y.Count()
        //                                                                            }).ToList();
        //        List<ChartsAxis> ChartAxisList = new List<ChartsAxis>();
        //        foreach (var item in query)
        //            ChartAxisList.Add(new ChartsAxis { KeyName = item.RankCategoryName, Value = item.RecordCount });

        //        return ChartAxisList;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public IEnumerable<Object> GetActualEmployeesBasedOnAssigningsAsDetails(string RankName, int OrganizationID)
        //{
        //    try
        //    {
        //        List<int> OrganizationIDsList = new OrganizationsStructuresBLL().GetByOrganizationIDsWithhAllChilds(OrganizationID);
        //        var query = new AssigningsDAL().GetActualEmployeeBasedOnAssignings().Where(x => x.IsFinished == false
        //                                                                                    && x.RankName == RankName
        //                                                                                    && OrganizationIDsList.Contains(x.OrganizationID.Value))
        //                                                                            .Select(y => new
        //                                                                            {
        //                                                                                EmployeeNameAr = y.EmployeeNameAr,
        //                                                                                EmployeeCodeNo = y.EmployeeCodeNo,
        //                                                                                OrganizationName = y.OrganizationName,
        //                                                                                JobName = y.JobName,
        //                                                                                RankName = y.RankName
        //                                                                            }).ToList();
        //        return query;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
    }
}

