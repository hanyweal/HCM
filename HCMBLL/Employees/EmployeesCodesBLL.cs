using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using HCMBLL.DTO;
using HCMBLL.Enums;
using HCMDAL;

namespace HCMBLL
{
    public class EmployeesCodesBLL : CommonEntity , IEntity
    {
        public virtual int EmployeeCodeID
        {
            get;
            set;
        }

        public virtual string EmployeeCodeNo
        {
            get;
            set;
        }

        public virtual EmployeesBLL Employee
        {
            get;
            set;
        }

        public virtual EmployeeDelegationBLL EmployeeDelegation { get; set; }

        public virtual EmployeesCareersHistoryBLL EmployeeCurrentJob { get; set; }

        public virtual EmployeesCareersHistoryBLL HiringRecord { get; set; }

        public virtual bool IsActive
        {
            get;
            set;
        }

        public virtual EmployeesTypesBLL EmployeeType
        {
            get;
            set;
        }

        public virtual string EmployeeCurrentQualification
        {
            get;
            set;
        }

        public virtual List<EmployeesCodesBLL> GetEmployeesCodes(out int totalRecordsOut, out int recFilterOut)
        {
            try
            {
                List<EmployeesCodes> EmployeesCodesList = new EmployeesCodesDAL()
                {
                    search = Search,
                    order = Order,
                    orderDir = OrderDir,
                    startRec = StartRec,
                    pageSize = PageSize
                }.GetEmployeesCodes(out totalRecordsOut, out recFilterOut).ToList();
                List<EmployeesCodesBLL> EmployeesCodesBLLList = new List<EmployeesCodesBLL>();
                foreach (var item in EmployeesCodesList)
                {
                    EmployeesCodesBLLList.Add(new EmployeesCodesBLL()
                    {
                        EmployeeCodeID = item.EmployeeCodeID,
                        EmployeeCodeNo = item.EmployeeCodeNo,
                        IsActive = true,
                        Employee = new EmployeesBLL().MapEmployee(item.Employees)
                    }); ;
                }

                return EmployeesCodesBLLList.ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public virtual EmployeesCodesBLL GetByEmployeeCodeNo(string EmployeeCodeNo)
        {
            try
            {
                EmployeesCodes EmployeeCode = new EmployeesCodesDAL().GetByEmployeeCodeNo(new EmployeesCodes() { EmployeeCodeNo = EmployeeCodeNo });
                EmployeesCodesBLL EmployeeCodeBLL = null;
                if (EmployeeCode != null)
                {
                    EmployeeCodeBLL = new EmployeesCodesBLL().MapEmployeeCode(EmployeeCode);
                    EmployeeCodeBLL.Employee.EmployeePicture = EmployeeCodeBLL.Employee.GetEmployeePicture();
                }
                return EmployeeCodeBLL;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public virtual EmployeesCodesBLL GetByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                EmployeesCodes EmployeeCode = new EmployeesCodesDAL().GetByEmployeeCodeID(EmployeeCodeID);
                EmployeesCodesBLL EmployeeCodeBLL = null;
                if (EmployeeCode != null)
                {
                    EmployeeCodeBLL = new EmployeesCodesBLL().MapEmployeeCode(EmployeeCode);
                    EmployeeCodeBLL.HiringRecord = new EmployeesCareersHistoryBLL().GetHiringRecordByEmployeeCodeID(EmployeeCodeID);
                    EmployeeCodeBLL.Employee.EmployeePicture = EmployeeCodeBLL.Employee.GetEmployeePicture();
                }
                return EmployeeCodeBLL;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public virtual EmployeesCodesBLL GetDelegationBalanceByEmployeeCodeID(int EmployeeCodeID, DateTime DelegationStartDate, DateTime DelegationEndDate)
        {
            try
            {
                return new EmployeesCodesBLL()
                {
                    EmployeeCodeID = EmployeeCodeID,
                    EmployeeDelegation = new EmployeeDelegationBLL(DelegationStartDate, DelegationEndDate, (int)DelegationsKindsEnum.Tasks, new EmployeesCodesBLL() { EmployeeCodeID = EmployeeCodeID })
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual EmployeesCodesBLL GetActiveEmployeeCodeByEmployeeID(int EmployeeID)
        {
            try
            {
                EmployeesCodes _employeeCode = new EmployeesCodesDAL().GetIsActiveByEmployeeID(EmployeeID);
                EmployeesCodesBLL _employeeCodeBLL = null;
                if (_employeeCode != null)
                {
                    _employeeCodeBLL = new EmployeesCodesBLL();
                    _employeeCodeBLL.EmployeeCodeID = _employeeCode.EmployeeCodeID;
                    _employeeCodeBLL.EmployeeCodeNo = _employeeCode.EmployeeCodeNo;
                    _employeeCodeBLL.IsActive = _employeeCode.IsActive;
                    //  _employeeCodeBLL.EmployeeCurrentJob = new EmployeesCareersHistoryBLL().GetEmployeeCurrentJob(_employeeCode.EmployeeCodeID);
                    _employeeCodeBLL.EmployeeType = new EmployeesTypesBLL()
                    {
                        EmployeeTypeID = _employeeCode.EmployeesTypes.EmployeeTypeID,
                        EmployeeTypeName = _employeeCode.EmployeesTypes.EmployeeTypeName
                    };
                }

                return _employeeCodeBLL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EmployeesCareersHistoryBLL> GetCareerHistoryByEmployeeCodeID(int EmployeeCodeID)
        {
            List<EmployeesCareersHistory> EmployeeCareerHistoryList = new EmployeesCareersHistoryDAL().GetEmployeesCareersHistoryByEmployeeCodeID(EmployeeCodeID);
            List<EmployeesCareersHistoryBLL> EmployeeCareerHistoryBLLList = new List<EmployeesCareersHistoryBLL>();
            if (EmployeeCareerHistoryList != null)
            {
                foreach (var item in EmployeeCareerHistoryList)
                    EmployeeCareerHistoryBLLList.Add(new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item));
            }

            return EmployeeCareerHistoryBLLList.OrderByDescending(x => x.JoinDate).ToList();
        }

        public virtual List<BaseAssigningsBLL> GetAssigningsByEmployeeCodeID(int EmployeeCodeID)
        {
            // get assignings from assigmoing module
            List<BaseAssigningsBLL> AssigningBLLList = new List<BaseAssigningsBLL>();
            List<Assignings> Assignings = new AssigningsDAL().GetAssigningsByEmployeeCodeID(EmployeeCodeID);
            foreach (var item in Assignings)
                AssigningBLLList.Add(new BaseAssigningsBLL().MapAssigning(item));

            return AssigningBLLList;
        }

        public virtual List<BaseDelegationsBLL> GetDelegationsByEmployeeCodeID(int EmployeeCodeID)
        {
            List<BaseDelegationsBLL> DelegationBLLList = new List<BaseDelegationsBLL>();
            List<DelegationsDetails> delegationDetailsList = new DelegationsDetailsDAL().GetDelegationsDetailsByEmployeeCodeID(EmployeeCodeID);
            foreach (DelegationsDetails DelegationsDetail in delegationDetailsList)
                DelegationBLLList.Add(new BaseDelegationsBLL().MapDelegation(DelegationsDetail.Delegations));

            return DelegationBLLList;
        }

        public List<OverTimesBLL> GetOverTimesByEmployeeCodeID(int EmployeeCodeID)
        {
            List<OverTimesBLL> OverTimeBLLList = new List<OverTimesBLL>();
            List<OverTimesDetails> overTimesDetailsList = new OverTimesDetailsDAL().GetOverTimesDetailsByEmployeeCodeID(EmployeeCodeID);
            foreach (OverTimesDetails item in overTimesDetailsList)
                OverTimeBLLList.Add(new OverTimesBLL().MapOverTime(item.OverTimes));

            return OverTimeBLLList;
        }

        public List<VacationsDetailsBLL> GetVacationsDetailsByEmployeeCodeID(int EmployeeCodeID)
        {
            List<VacationsDetailsBLL> VacationsDetailBLLList = new List<VacationsDetailsBLL>();
            List<VacationsDetails> VacationsDetailsList = new VacationsDetailsDAL().GetByEmployeeCodeID(EmployeeCodeID);
            foreach (VacationsDetails item in VacationsDetailsList)
                VacationsDetailBLLList.Add(new VacationsDetailsBLL().MapVacationDetail(item));

            return VacationsDetailBLLList;
        }

        public List<BaseVacationsBLL> GetVacationsByEmployeeCodeID(int EmployeeCodeID)
        {
            return new BaseVacationsBLL().GetByEmployeeCodeID(EmployeeCodeID);
        }

        public List<StopWorksBLL> GetStopWorksByEmployeeCodeID(int EmployeeCodeID)
        {
            List<StopWorksBLL> StopWorkBLLList = new List<StopWorksBLL>();
            List<StopWorks> StopWorkList = new StopWorksDAL().GetStopWorksByEmployeeCodeID(EmployeeCodeID);
            foreach (StopWorks item in StopWorkList)
                StopWorkBLLList.Add(new StopWorksBLL().MapStopWork(item));

            return StopWorkBLLList;
        }

        public virtual List<EmployeesEvaluationsBLL> GetEmployeeEvaluationByEmployeeCodeID(int EmployeeCodeID)
        {
            List<EmployeesEvaluations> EmployeesEvaluationsList = new EmployeesEvaluationsDAL().GetEmployeeEvaluationsByEmployeeCodeID(EmployeeCodeID);
            List<EmployeesEvaluationsBLL> EmployeesEvaluationsBLLList = new List<EmployeesEvaluationsBLL>();
            foreach (var item in EmployeesEvaluationsList)
                EmployeesEvaluationsBLLList.Add(new EmployeesEvaluationsBLL().MapEmployeeEvaluation(item));

            return EmployeesEvaluationsBLLList;
        }

        public List<BaseScholarshipsBLL> GetScholarshipsByEmployeeCodeID(int EmployeeCodeID)
        {
            List<BaseScholarshipsBLL> BaseScholarshipsBLLList = new List<BaseScholarshipsBLL>();
            List<Scholarships> ScholarshipsList = new ScholarshipsDAL().GetScholarshipsByEmployeeCodeID(EmployeeCodeID);
            foreach (Scholarships item in ScholarshipsList)
                BaseScholarshipsBLLList.Add(new BaseScholarshipsBLL().MapScholarship(item));

            return BaseScholarshipsBLLList;
        }

        public List<EmployeesExperiencesBLL> GetExperiencesByEmployeeCodeID(int EmployeeCodeID)
        {
            List<EmployeesExperiencesBLL> EmployeesExperiencesBLLList = new List<EmployeesExperiencesBLL>();
            List<EmployeesExperiences> EmployeesExperiencesList = new EmployeesExperiencesDAL().GetEmployeeExperiencesByEmployeeCodeID(EmployeeCodeID);
            foreach (EmployeesExperiences item in EmployeesExperiencesList)
                EmployeesExperiencesBLLList.Add(new EmployeesExperiencesBLL().MapEmployeeExperience(item));

            return EmployeesExperiencesBLLList;
        }

        public List<EmployeesEvaluationsBLL> GetEvaluationsByEmployeeCodeID(int EmployeeCodeID, int PromotionRecordEmployeeID)
        {
            List<EmployeesEvaluationsBLL> EmployeeEvaluationsBLLList = new List<EmployeesEvaluationsBLL>();
            EmployeesEvaluationsBLL EmployeeEvaluationBLL = new EmployeesEvaluationsBLL();
            List<EmployeesEvaluations> EmployeeEvaluationsList = new EmployeesEvaluationsDAL().GetEmployeeEvaluationsByEmployeeCodeID(EmployeeCodeID);
            List<PromotionsRecordsEmployeesEvaluationsDetailsBLL> List = new List<PromotionsRecordsEmployeesEvaluationsDetailsBLL>();
            PromotionsRecordsEmployeesEvaluationsDetailsBLL Eval;

            if (PromotionRecordEmployeeID > 0)
                List = new PromotionsRecordsEmployeesEvaluationsDetailsBLL().GetByPromotionRecordEmployeeID(PromotionRecordEmployeeID);

            foreach (EmployeesEvaluations item in EmployeeEvaluationsList)
            {
                EmployeeEvaluationBLL = new EmployeesEvaluationsBLL().MapEmployeeEvaluation(item);

                if (PromotionRecordEmployeeID > 0)
                {
                    Eval = List.FirstOrDefault(e => e.EvaluationYear == item.MaturityYearsBalances.MaturityYear && e.Evaluation == item.EvaluationPoints.Evaluation);
                    if (Eval != null)
                    {
                        EmployeeEvaluationBLL.Points = Eval.Points.HasValue ? Eval.Points.Value : 0;
                        EmployeeEvaluationsBLLList.Add(EmployeeEvaluationBLL);
                    }
                }
                else
                {
                    EmployeeEvaluationsBLLList.Add(EmployeeEvaluationBLL);
                }

            }
            return EmployeeEvaluationsBLLList;
        }

        public List<EmployeesQualificationsBLL> GetQualificationsByEmployeeCodeID(int EmployeeCodeID, int PromotionRecordEmployeeID)
        {
            List<EmployeesQualificationsBLL> EmployeesQualificationsBLLList = new List<EmployeesQualificationsBLL>();
            EmployeesQualificationsBLL EmployeeQualificationBLL = new EmployeesQualificationsBLL();
            List<EmployeesQualifications> EmployeesQualificationsList = new EmployeesQualificationsDAL().GetEmployeeQualificationByEmployeeCodeID(EmployeeCodeID);
            List<PromotionsRecordsEmployeesEducationsDetailsBLL> List = new List<PromotionsRecordsEmployeesEducationsDetailsBLL>();
            PromotionsRecordsEmployeesEducationsDetailsBLL Edu;

            if (PromotionRecordEmployeeID > 0)
                List = new PromotionsRecordsEmployeesEducationsDetailsBLL().GetByPromotionRecordEmployeeID(PromotionRecordEmployeeID);

            foreach (EmployeesQualifications item in EmployeesQualificationsList)
            {
                EmployeeQualificationBLL = new EmployeesQualificationsBLL().MapEmployeeQualification(item);

                if (PromotionRecordEmployeeID > 0)
                {
                    Edu = List.FirstOrDefault(e => e.QualificationDegree.QualificationDegreeID == item.QualificationDegreeID
                                                && e.Qualification.QualificationID == item.QualificationID
                                                && e.GeneralSpecialization.GeneralSpecializationID == item.GeneralSpecializationID);
                    if (Edu != null)
                    {
                        EmployeeQualificationBLL.Points = Edu.Points.HasValue ? Edu.Points.Value : 0;
                        EmployeeQualificationBLL.Weight = Edu.Weight;
                        EmployeeQualificationBLL.IsIncluded = Edu.IsIncluded;

                        EmployeesQualificationsBLLList.Add(EmployeeQualificationBLL);
                    }
                }
                else
                {
                    EmployeesQualificationsBLLList.Add(EmployeeQualificationBLL);
                }

            }
            return EmployeesQualificationsBLLList;
        }

        public List<LendersBLL> GetLendersByEmployeeCodeID(int EmployeeCodeID)
        {
            List<LendersBLL> LendersBLLList = new List<LendersBLL>();
            List<Lenders> LendersList = new LendersDAL().GetLendersByEmployeeCodeID(EmployeeCodeID);
            foreach (Lenders item in LendersList)
                LendersBLLList.Add(new LendersBLL().MapLender(item));

            return LendersBLLList;
        }

        public List<TeachersBLL> GetTeachersByEmployeeCodeID(int EmployeeCodeID)
        {
            List<TeachersBLL> TeachersBLLList = new List<TeachersBLL>();
            List<Teachers> TeachersList = new TeachersDAL().GetTeachersByEmployeeCodeID(EmployeeCodeID);
            foreach (Teachers item in TeachersList)
                TeachersBLLList.Add(new TeachersBLL().MapTeacher(item));

            return TeachersBLLList;
        }

        public List<ExternalAssigningBLL> GetExternalAssigningByEmployeeCodeID(int EmployeeCodeID)
        {
            List<ExternalAssigningBLL> ExternalAssigningBLLList = new List<ExternalAssigningBLL>();
            List<Assignings> AssigningList = new AssigningsDAL().GetAssigningsByEmployeeCodeID(EmployeeCodeID);
            foreach (Assignings item in AssigningList)
            {
                BaseAssigningsBLL Assigning = (BaseAssigningsBLL)new BaseAssigningsBLL().MapAssigning(item);
                if (Assigning.AssigningType.AssigningTypeID == (int)AssigningsTypesEnum.External)
                    ExternalAssigningBLLList.Add((ExternalAssigningBLL)Assigning);

            }
            return ExternalAssigningBLLList;
        }

        public virtual List<PassengerOrdersBLL> GetPassengerOrdersByEmployeeCodeID(int EmployeeCodeID)
        {
            List<PassengerOrdersBLL> PassengerOrderBLLList = new List<PassengerOrdersBLL>();
            List<PassengerOrders> PassengerOrdersList = new PassengerOrdersDAL().GetPassengerOrdersByEmployeeCodeID(EmployeeCodeID);
            foreach (PassengerOrders item in PassengerOrdersList)
            {
                PassengerOrdersBLL PassengerOrderBLL = new PassengerOrdersBLL().MapPassengerOrder(item);
                PassengerOrderBLLList.Add(PassengerOrderBLL);
            }
            return PassengerOrderBLLList;
        }

        public List<GovernmentFundsBLL> GetGovernmentFundsByEmployeeCodeID(int EmployeeCodeID)
        {
            List<GovernmentFundsBLL> GovernmentFundsBLLList = new List<GovernmentFundsBLL>();
            List<GovernmentFunds> GovernmentFundsList = new GovernmentFundsDAL().GetByEmployeeCodeID(EmployeeCodeID);
            foreach (var item in GovernmentFundsList)
                GovernmentFundsBLLList.Add(new GovernmentFundsBLL().MapGovernmentFund(item));

            return GovernmentFundsBLLList;
        }

        public List<EmployeesAllowancesBLL> GetActiveAllownacessByEmployeeCodeID(int EmployeeCodeID)
        {
            List<EmployeesAllowancesBLL> EmployeesAllowancesBLLList = new List<EmployeesAllowancesBLL>();
            List<EmployeesAllowances> EmployeesAllowancesList = new EmployeesAllowancesDAL().GetByEmployeeCodeID(EmployeeCodeID).Where(x => x.IsActive == true).ToList();
            foreach (var item in EmployeesAllowancesList)
                EmployeesAllowancesBLLList.Add(new EmployeesAllowancesBLL().MapEmployeeAllowance(item));

            return EmployeesAllowancesBLLList;
        }

        public List<TimeAttendanceBLL> GetAbsenceByEmployeeCodeID(int EmployeeCodeID, DateTime FromDate, DateTime ToDate)
        {
            return new TimeAttendanceBLL().GetAbsenceDaysByEmployeeCodeNo(this.GetByEmployeeCodeID(EmployeeCodeID).EmployeeCodeNo, FromDate, ToDate);
        }

        public virtual List<BaseInternshipScholarshipsBLL> GetInternshipScholarshipsByEmployeeCodeID(int EmployeeCodeID)
        {
            List<BaseInternshipScholarshipsBLL> InternshipScholarshipBLLList = new List<BaseInternshipScholarshipsBLL>();
            List<InternshipScholarshipsDetails> InternshipScholarshipDetailsList = new InternshipScholarshipsDetailsDAL().GetInternshipScholarshipsDetailsByEmployeeCodeID(EmployeeCodeID);
            foreach (InternshipScholarshipsDetails InternshipScholarshipsDetail in InternshipScholarshipDetailsList)
                InternshipScholarshipBLLList.Add(new BaseInternshipScholarshipsBLL().MapInternshipScholarship(InternshipScholarshipsDetail.InternshipScholarships));

            return InternshipScholarshipBLLList;
        }

        public EmployeesCodesBLL GetEmployeeCurrentQualificationByEmployeeCodeID(int EmployeeCodeID)
        {
            try
            {
                EmployeesQualificationsBLL EmployeeQualification = new EmployeesQualificationsBLL().GetEmployeeQualificationByEmployeeCodeID(EmployeeCodeID).FirstOrDefault();
                EmployeesCodesBLL EmployeeCodeBLL = null;
                if (EmployeeQualification != null)
                {
                    EmployeeCodeBLL = new EmployeesCodesBLL()
                    {
                        EmployeeCurrentQualification = EmployeeQualification.Qualification.QualificationName
                    };
                }
                return EmployeeCodeBLL;
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCodesBLL> GetBasedOnAssigningsByOrganizationID(int OrganizationID)
        {
            try
            {
                List<EmployeesCodes> EmployeeCodeList = new EmployeesCodesDAL().GetBasedOnAssigningsByOrganizationID(OrganizationID);
                List<EmployeesCodesBLL> EmployeeCodeBLLList = new List<EmployeesCodesBLL>();
                EmployeesCodesBLL EmployeeCodeBLL = null;
                if (EmployeeCodeList != null)
                {
                    foreach (var item in EmployeeCodeList)
                    {
                        EmployeeCodeBLL = new EmployeesCodesBLL().MapEmployeeCode(item);
                        EmployeeCodeBLLList.Add(EmployeeCodeBLL);
                    }
                }

                return EmployeeCodeBLLList;
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesCodesBLL> GetByOrganizationID(int OrganizationID)
        {
            try
            {
                List<EmployeesCodes> EmployeeCodeList = new EmployeesCodesDAL().GetByOrganizationID(OrganizationID);
                List<EmployeesCodesBLL> EmployeeCodeBLLList = new List<EmployeesCodesBLL>();
                EmployeesCodesBLL EmployeeCodeBLL = null;
                if (EmployeeCodeList != null)
                {
                    foreach (var item in EmployeeCodeList)
                    {
                        EmployeeCodeBLL = new EmployeesCodesBLL().MapEmployeeCode(item);
                        EmployeeCodeBLLList.Add(EmployeeCodeBLL);
                    }
                }

                return EmployeeCodeBLLList;
            }
            catch
            {
                throw;
            }
        }

        public virtual int Add()
        {
            try
            {
                EmployeesCodesDAL employeesCodeDal = new EmployeesCodesDAL();
                EmployeesCodes employeesCode = new EmployeesCodes()
                {
                    EmployeeID = this.Employee.EmployeeID,
                    EmployeeCodeNo = this.EmployeeCodeNo,
                    EmployeeTypeID = this.EmployeeType.EmployeeTypeID,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID
                };
                employeesCodeDal.Insert(employeesCode);
                return this.EmployeeCodeID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual int Edit()
        {
            try
            {
                EmployeesCodes EmployeesCode = new EmployeesCodes()
                {
                    EmployeeID = this.Employee.EmployeeID,
                    EmployeeCodeNo = this.EmployeeCodeNo,
                    EmployeeTypeID = this.EmployeeType.EmployeeTypeID,
                    IsActive = this.Employee.ActiveEmployeeCode.IsActive,
                    LastUpdatedDate = DateTime.Now,
                    EmployeeCodeID = this.Employee.ActiveEmployeeCode.EmployeeCodeID,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
                };
                return new EmployeesCodesDAL().Update(EmployeesCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal Result DeactivateEmployee(int EmployeeCodeID)
        {
            try
            {
                Result result = new Result();
                EmployeesCodes EmployeesCode = new EmployeesCodes()
                {
                    IsActive = false,
                    EmployeeCodeID = EmployeeCodeID,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID,
                    LastUpdatedDate = DateTime.Now
                };
                new EmployeesCodesDAL().UpdateIsActive(EmployeesCode);
                result.EnumMember = OrganizationJobValidationEnum.Done.ToString();
                result.Entity = this;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual int AddOrEdit()
        {
            if (this.Employee.ActiveEmployeeCode != null)
                Edit();
            else
                Add();

            return 1;
        }

        public List<EmployeesCodesBLL> GetEmployees()
        {
            try
            {
                List<EmployeesCodes> EmployeeCodeList = new EmployeesCodesDAL().GetEmployees();
                List<EmployeesCodesBLL> EmployeeCodeBLLList = new List<EmployeesCodesBLL>();
                EmployeesCodesBLL EmployeeCodeBLL = null;
                if (EmployeeCodeList != null)
                {
                    foreach (var item in EmployeeCodeList)
                    {
                        EmployeeCodeBLL = new EmployeesCodesBLL();
                        EmployeeCodeBLL.EmployeeCodeID = item.EmployeeCodeID;
                        EmployeeCodeBLL.EmployeeCodeNo = item.EmployeeCodeNo;
                        EmployeeCodeBLL.Employee = new EmployeesBLL()
                        {
                            EmployeeID = item.EmployeeID,
                            FirstNameAr = item.Employees.FirstNameAr,
                            MiddleNameAr = item.Employees.MiddleNameAr,
                            GrandFatherNameAr = item.Employees.GrandFatherNameAr,
                            LastNameAr = item.Employees.LastNameAr,
                            EmployeeMobileNo = item.Employees.EmployeeMobileNo
                        };
                        EmployeeCodeBLLList.Add(EmployeeCodeBLL);
                    }
                }

                return EmployeeCodeBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EmployeesCodesBLL> GetEmployeesWithDetails()
        {
            try
            {
                List<EmployeesCodes> EmployeeCodeList = new EmployeesCodesDAL().GetEmployeesWithDetails();
                List<EmployeesCodesBLL> EmployeeCodeBLLList = new List<EmployeesCodesBLL>();
                EmployeesCodesBLL EmployeeCodeBLL = null;
                if (EmployeeCodeList != null)
                {
                    foreach (var item in EmployeeCodeList)
                    {
                        EmployeeCodeBLL = new EmployeesCodesBLL();
                        EmployeeCodeBLL.EmployeeCodeID = item.EmployeeCodeID;
                        EmployeeCodeBLL.EmployeeCodeNo = item.EmployeeCodeNo;
                        EmployeeCodeBLL.Employee = new EmployeesBLL()
                        {
                            EmployeeID = item.EmployeeID,
                            EmployeeIDNo = item.Employees.EmployeeIDNo,
                            FirstNameAr = item.Employees.FirstNameAr,
                            MiddleNameAr = item.Employees.MiddleNameAr,
                            GrandFatherNameAr = item.Employees.GrandFatherNameAr,
                            LastNameAr = item.Employees.LastNameAr,
                            EmployeeMobileNo = item.Employees.EmployeeMobileNo
                        };
                        EmployeeCodeBLL.EmployeeCurrentJob = item.EmployeesCareersHistory.Count() > 0 ? new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item.EmployeesCareersHistory.FirstOrDefault(x => x.IsActive == true)) : new EmployeesCareersHistoryBLL() { OrganizationJob = new OrganizationsJobsBLL() { Job = new JobsBLL() { JobName = "" }, OrganizationStructure = new OrganizationsStructuresBLL() { OrganizationName = "" } } };
                        IsActive = true;
                        EmployeeCodeBLLList.Add(EmployeeCodeBLL);
                    }
                }

                return EmployeeCodeBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EmployeesCodesBLL> GetAllEmployeesWithDetails()
        {
            try
            {
                List<EmployeesCodes> EmployeeCodeList = new EmployeesCodesDAL().GetAllEmployeesWithDetails();
                List<EmployeesCodesBLL> EmployeeCodeBLLList = new List<EmployeesCodesBLL>();
                EmployeesCodesBLL EmployeeCodeBLL = null;
                if (EmployeeCodeList != null)
                {
                    foreach (var item in EmployeeCodeList)
                    {
                        EmployeeCodeBLL = new EmployeesCodesBLL();
                        EmployeeCodeBLL.EmployeeCodeID = item.EmployeeCodeID;
                        EmployeeCodeBLL.EmployeeCodeNo = item.EmployeeCodeNo;
                        EmployeeCodeBLL.Employee = new EmployeesBLL()
                        {
                            EmployeeID = item.EmployeeID,
                            EmployeeIDNo = item.Employees.EmployeeIDNo,
                            FirstNameAr = item.Employees.FirstNameAr,
                            MiddleNameAr = item.Employees.MiddleNameAr,
                            GrandFatherNameAr = item.Employees.GrandFatherNameAr,
                            LastNameAr = item.Employees.LastNameAr,
                            EmployeeMobileNo = item.Employees.EmployeeMobileNo
                        };
                        EmployeeCodeBLL.EmployeeCurrentJob = item.EmployeesCareersHistory.Count() > 0 ? new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(item.EmployeesCareersHistory.FirstOrDefault(x => x.IsActive == true)) : new EmployeesCareersHistoryBLL() { OrganizationJob = new OrganizationsJobsBLL() { Job = new JobsBLL() { JobName = "" }, OrganizationStructure = new OrganizationsStructuresBLL() { OrganizationName = "" } } };
                        IsActive = true;
                        EmployeeCodeBLLList.Add(EmployeeCodeBLL);
                    }
                }

                return EmployeeCodeBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PromotionsRecordsEmployeesBLL> GetPromotionRecordsByEmployeeCodeID(int EmployeeCodeID)
        {
            List<PromotionsRecordsEmployeesBLL> PromotionRecordEmployeeBLLList = new List<PromotionsRecordsEmployeesBLL>();
            List<PromotionsRecordsEmployees> PromotionRecordEmployeesList = new PromotionsRecordsEmployeesDAL().GetByEmployeeCodeID(EmployeeCodeID);
            foreach (PromotionsRecordsEmployees item in PromotionRecordEmployeesList)
                PromotionRecordEmployeeBLLList.Add(new PromotionsRecordsEmployeesBLL().MapPromotionRecordEmployee(item));

            return PromotionRecordEmployeeBLLList;
        }

        public List<ActiveEmployeesWithFullDataDTO> GetActiveEmployeesWithFullData()
        {
            try
            {
                List<ActiveEmployeesWithFullDataDTO> ActiveEmployeesWithFullDataDTOList = new List<ActiveEmployeesWithFullDataDTO>();
                ActiveEmployeesWithFullDataDTO ActiveEmployeesWithFullDataDTO;
                List<EmployeesCodes> EmployeeCodeList = new EmployeesCodesDAL().GetEmployeesCodesWithMoreDetails();
                List<SalaryDetailsBLL> SalaryDetailsList = new SalaryDetailsBLL().GetSalaryDetails(null);
                List<OrganizationsStructures> OrganizationsStructuresList = new OrganizationsStructuresDAL().GetOrganizationStructure();
                foreach (EmployeesCodes item in EmployeeCodeList)           //.Where(x=>x.EmployeeCodeNo == "90034017")
                {
                    ActiveEmployeesWithFullDataDTO = new ActiveEmployeesWithFullDataDTO();
                    ActiveEmployeesWithFullDataDTO.EmployeeCodeNo = item.EmployeeCodeNo;
                    ActiveEmployeesWithFullDataDTO.EmployeeIDNo = item.Employees.EmployeeIDNo;
                    ActiveEmployeesWithFullDataDTO.EmployeeNameAr = item.Employees.FirstNameAr + " " + item.Employees.MiddleNameAr + " " + item.Employees.GrandFatherNameAr + " " + item.Employees.LastNameAr;
                    ActiveEmployeesWithFullDataDTO.EmployeeBirthDate = item.Employees.EmployeeBirthDate;
                    ActiveEmployeesWithFullDataDTO.GenderName = item.Employees.Genders != null ? item.Employees.Genders.GenderName : "";
                    ActiveEmployeesWithFullDataDTO.NationalityName = item.Employees.Nationalities != null ? item.Employees.Nationalities.CountryName : "";
                    ActiveEmployeesWithFullDataDTO.MobileNo = item.Employees.EmployeeMobileNo;

                    EmployeesCareersHistory employeesCareersHistory = item.EmployeesCareersHistory.FirstOrDefault(ech => ech.IsActive == true);
                    List<EmployeesCareersHistory> employeesCareersHistoryPrevious = item.EmployeesCareersHistory.Where(ech => ech.IsActive == false).ToList();

                    SalaryDetailsBLL SalaryDetails = null;
                    Assignings assignings = null;

                    EmployeesCareersHistory careerHistoryHiring = new EmployeesCareersHistoryDAL().GetEmployeesCareersHistoryByEmployeeCodeID(item.EmployeeCodeID)
                                                                        .FirstOrDefault(x => x.CareerHistoryTypeID == (int)CareersHistoryTypesEnum.Hiring);

                    if (careerHistoryHiring != null && careerHistoryHiring.EmployeeCareerHistoryID > 0)
                    {
                        ActiveEmployeesWithFullDataDTO.HiringDate = careerHistoryHiring.JoinDate.ToString(ConfigurationManager.AppSettings["DateFormat"]);  //.ToShortDateString();   //careerHistoryHiring.JoinDate;
                    }

                    if (employeesCareersHistory != null)
                    {
                        ActiveEmployeesWithFullDataDTO.OrganizationName = employeesCareersHistory.OrganizationsJobs.OrganizationsStructures.OrganizationName;
                        ActiveEmployeesWithFullDataDTO.JoinDate = employeesCareersHistory.JoinDate.ToString(ConfigurationManager.AppSettings["DateFormat"]);  //.ToShortDateString(); // employeesCareersHistory.JoinDate;
                        ActiveEmployeesWithFullDataDTO.RankName = employeesCareersHistory.OrganizationsJobs.Ranks.RankName;
                        ActiveEmployeesWithFullDataDTO.CareerDegreeName = employeesCareersHistory.CareersDegrees.CareerDegreeName;
                        ActiveEmployeesWithFullDataDTO.JobName = employeesCareersHistory.OrganizationsJobs.Jobs.JobName;
                        ActiveEmployeesWithFullDataDTO.JobNo = employeesCareersHistory.OrganizationsJobs.JobNo;
                        SalaryDetails = SalaryDetailsList.Find(X => X.Employee.EmployeeCareerHistoryID == employeesCareersHistory.EmployeeCareerHistoryID);
                        assignings = employeesCareersHistory.Assignings.FirstOrDefault(a => a.IsFinished == false && a.AssigningTypID == (int)AssigningsTypesEnum.Internal);
                    }

                    if (assignings != null)
                    {
                        ActiveEmployeesWithFullDataDTO.ActualJobName = assignings.Jobs.JobName;
                        //ActiveEmployeesWithFullDataDTO.ActualOrganizationName = assignings.OrganizationsStructures.OrganizationName;
                        ActiveEmployeesWithFullDataDTO.ActualOrganizationName = new OrganizationsStructuresBLL().GetOrganizationNameTillLastParentExceptPresident(OrganizationsStructuresList, assignings.OrganizationsStructures);
                    }
                    else
                    {
                        if (item.OrganizationsStructures.Count != 0)
                        {
                            ActiveEmployeesWithFullDataDTO.ActualJobName = "مدير " + item.OrganizationsStructures.FirstOrDefault(x => x.ManagerCodeID == item.EmployeeCodeID).OrganizationName;
                            ActiveEmployeesWithFullDataDTO.ActualOrganizationName = item.OrganizationsStructures.FirstOrDefault(x => x.ManagerCodeID == item.EmployeeCodeID).OrganizationName;
                        }
                    }
                    if (employeesCareersHistoryPrevious != null && employeesCareersHistoryPrevious.Count() > 0)
                    {
                        employeesCareersHistoryPrevious.ForEach(p => ActiveEmployeesWithFullDataDTO.PreviosJobName += p.OrganizationsJobs.Jobs.JobName + ',');
                        ActiveEmployeesWithFullDataDTO.PreviosJobName = ActiveEmployeesWithFullDataDTO.PreviosJobName != null ? ActiveEmployeesWithFullDataDTO.PreviosJobName.TrimEnd(',') : "";
                    }

                    List<EmployeesQualificationsBLL> employeesQualificationsList = new List<EmployeesQualificationsBLL>();
                    item.EmployeesQualifications.ToList().ForEach(eq => employeesQualificationsList.Add(new EmployeesQualificationsBLL().MapEmployeeQualification(eq)));
                    EmployeesQualificationsBLL employeesQualifications = new EmployeesQualificationsBLL().GetLastEmployeeQualification(employeesQualificationsList, item.EmployeeCodeID);
                    if (employeesQualifications != null)
                    {
                        ActiveEmployeesWithFullDataDTO.QualificationDegreeName = employeesQualifications.QualificationDegree.QualificationDegreeName;
                        ActiveEmployeesWithFullDataDTO.QualificationName = employeesQualifications.Qualification.QualificationName;
                        ActiveEmployeesWithFullDataDTO.GeneralSpecializationName = employeesQualifications.GeneralSpecialization.GeneralSpecializationName;
                    }

                    if (SalaryDetails != null)
                    {
                        ActiveEmployeesWithFullDataDTO.TransfareAllowance = SalaryDetails.Benefits.TransfareAllowance;
                        ActiveEmployeesWithFullDataDTO.TotalAllowances = SalaryDetails.Benefits.TotalAllowances;
                        ActiveEmployeesWithFullDataDTO.TotalDeductions = SalaryDetails.Deductions.TotalDeductions;
                        ActiveEmployeesWithFullDataDTO.NetSalary = SalaryDetails.NetSalary;
                        ActiveEmployeesWithFullDataDTO.BasicSalary = SalaryDetails.Benefits.BasicSalary;
                    }
                    ActiveEmployeesWithFullDataDTOList.Add(ActiveEmployeesWithFullDataDTO);
                }
                return ActiveEmployeesWithFullDataDTOList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// if he is a manager of an organization, we get his manager of his department from organization structure module ... otherwise, we get manager of last active assigning from internal assigning .. 
        /// </summary>
        /// <param name="EmployeeCodeNo"></param>
        /// <param name="EServiceType"></param>
        /// <returns></returns>
        public EmployeesCodesBLL GetEVacationAuthorizedPersonOfEmployee(string EmployeeCodeNo, EServicesTypesEnum EServiceType)
        {
            try
            {
                int OrganizationID = 0;
                EmployeesCodesBLL AuthorizedPerson = null;
                // first check the employee is manager or not ... the priority is manager of his department if he is manager in organization structure
                OrganizationsStructuresBLL ParentOrganization = new OrganizationsStructuresBLL().GetAllOrganizationsForManagerByManagerCodeNo(EmployeeCodeNo)?.FirstOrDefault()?.ParentOrganization;
                if (ParentOrganization != null)
                    OrganizationID = ParentOrganization.OrganizationID;
                else
                {
                    #region get last active assigned to employee to get his organization
                    BaseAssigningsBLL LastAssigning = new InternalAssigningBLL().GetEmployeeActiveAssigning(EmployeeCodeNo);
                    if (LastAssigning != null)
                    {
                        InternalAssigningBLL LastActiveAssigning = (InternalAssigningBLL)LastAssigning;
                        OrganizationID = LastActiveAssigning.Organization.OrganizationID;
                    }
                    #endregion
                }

                AuthorizedPerson = new EServicesAuthorizationsBLL().GetOrganizationAuthorizedPerson(OrganizationID, EServiceType)?.AuthorizedPerson;

                //else
                //{
                //    OrganizationsStructuresBLL ParentOrganization = new OrganizationsStructuresBLL().GetAllOrganizationsForManagerByManagerCodeNo(EmployeeCodeNo).FirstOrDefault().ParentOrganization;
                //    if (ParentOrganization != null)
                //        OrganizationID = ParentOrganization.OrganizationID;
                //}

                return AuthorizedPerson;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Related to Task 316: Index Page of EServicesProxies 
        /// Summary: Get All Employees, Managers & Siblings From Organization ID & EServiceType
        /// step 1: Get All child organization from Organization ID
        /// step 2: First get all employees from assigning based on all Child Organization ID
        /// step 3: Then get all managers from organization table based on all Child Organization ID
        /// step 4: Get all siblings based on Organization ID 
        /// </summary>
        /// <param name="OrganizationID"></param>
        /// <param name="EServiceType"></param>
        /// <param name="EmployeeCodeID"></param>
        /// <returns></returns>
        public List<EmployeesCodesBLL> GetAllEmployeesManagersByOrganizationID(int OrganizationID, EServicesTypesEnum EServiceType, int EmployeeCodeID = 0)
        {
            try
            {
                if (OrganizationID == 0)
                    return new List<EmployeesCodesBLL>();

                List<int> ChildOrganizationIDs = new OrganizationsStructuresBLL().GetAllChildernByOrganizationID(OrganizationID);
                //ChildOrganizationIDs.Remove(OrganizationID);    // remove logged-In organization
                List<Assignings> AssigningList = new AssigningsDAL().GetByOrganizationIDs(ChildOrganizationIDs, (int)AssigningsTypesEnum.Internal, false);
                List<OrganizationsStructures> OrganizationList = new OrganizationsStructuresDAL().GetByOrganizationIDs(ChildOrganizationIDs);

                OrganizationsStructuresBLL OrganizationStructureBLL = new OrganizationsStructuresBLL().GetByOrganizationID(OrganizationID);
                EmployeesCodesBLL EmployeeCode;
                PlacementBLL PlacementBLL;
                List<EmployeesCodesBLL> EmployeeCodeBLLList = new List<EmployeesCodesBLL>();
                bool IsExists;

                #region Get Siblings
                List<int> SiblingOrganizationIDs = new OrganizationsStructuresBLL().GetOrganizationFirstLevelByID(OrganizationStructureBLL.ParentOrganization.OrganizationID);
                SiblingOrganizationIDs.Remove(OrganizationStructureBLL.ParentOrganization.OrganizationID);
                SiblingOrganizationIDs.Remove(OrganizationID);
                List<OrganizationsStructures> SiblingOrganizationList = new OrganizationsStructuresDAL().GetByOrganizationIDs(SiblingOrganizationIDs);
                if (EmployeeCodeID > 0)// this condition to get one record                
                {
                    SiblingOrganizationList = SiblingOrganizationList.Where(x => x.ManagerCodeID.HasValue && x.ManagerCodeID.Value == EmployeeCodeID).ToList();
                }
                foreach (OrganizationsStructures item in SiblingOrganizationList)
                {
                    if (item.ManagerCodeID.HasValue)
                    {
                        IsExists = EmployeeCodeBLLList.Any(x => x.EmployeeCodeID == item.ManagerCodeID.Value);
                        if (IsExists == false)
                        {
                            OrganizationStructureBLL = new OrganizationsStructuresBLL().GetByOrganizationID(item.OrganizationID);
                            PlacementBLL = new PlacementBLL().GetCurrentActualOrgAndActualJob(item.EmployeesCodes.EmployeeCodeNo);
                            EmployeeCode = new EmployeesCodesBLL();
                            EmployeeCode.EmployeeCodeID = item.ManagerCodeID.Value;
                            EmployeeCode.EmployeeCodeNo = item.EmployeesCodes.EmployeeCodeNo;
                            EmployeeCode.Employee = new EmployeesBLL().GetByEmployeeID(item.EmployeesCodes.EmployeeID);
                            OrganizationStructureBLL.OrganizationManager =
                                new EmployeesCodesBLL().GetByEmployeeCodeID(OrganizationStructureBLL.ParentOrganization.OrganizationManager.EmployeeCodeID);

                            EmployeeCode.EmployeeCurrentJob = new EmployeesCareersHistoryBLL()
                            {
                                OrganizationJob = new OrganizationsJobsBLL()
                                {
                                    OrganizationStructure = OrganizationStructureBLL,
                                    Job = PlacementBLL.Job
                                }
                            };
                            EmployeeCodeBLLList.Add(EmployeeCode);
                        }
                    }
                }
                #endregion

                #region Managers from OrganizationStucture table
                if (EmployeeCodeID > 0)// this condition to get one record                
                {
                    OrganizationList = OrganizationList.Where(x => x.ManagerCodeID.HasValue && x.ManagerCodeID.Value == EmployeeCodeID).ToList();
                }
                foreach (OrganizationsStructures item in OrganizationList)
                {
                    if (item.ManagerCodeID.HasValue)
                    {
                        IsExists = EmployeeCodeBLLList.Any(x => x.EmployeeCodeID == item.ManagerCodeID.Value);
                        if (IsExists == false)
                        {
                            OrganizationStructureBLL = new OrganizationsStructuresBLL().GetByOrganizationID(item.OrganizationID);
                            PlacementBLL = new PlacementBLL().GetCurrentActualOrgAndActualJob(item.EmployeesCodes.EmployeeCodeNo);

                            EmployeeCode = new EmployeesCodesBLL();
                            EmployeeCode.EmployeeCodeID = item.ManagerCodeID.Value;
                            EmployeeCode.EmployeeCodeNo = item.EmployeesCodes.EmployeeCodeNo;
                            EmployeeCode.Employee = new EmployeesBLL().GetByEmployeeID(item.EmployeesCodes.EmployeeID);
                            OrganizationStructureBLL.OrganizationManager =
                                new EmployeesCodesBLL().GetByEmployeeCodeID(OrganizationStructureBLL.ParentOrganization.OrganizationManager.EmployeeCodeID);

                            EmployeeCode.EmployeeCurrentJob = new EmployeesCareersHistoryBLL()
                            {
                                OrganizationJob = new OrganizationsJobsBLL()
                                {
                                    OrganizationStructure = OrganizationStructureBLL,
                                    Job = PlacementBLL.Job
                                }
                            };
                            EmployeeCodeBLLList.Add(EmployeeCode);
                        }
                    }
                }
                #endregion

                #region Employees from Assigning table
                if (EmployeeCodeID > 0)// this condition to get one record                
                {
                    AssigningList = AssigningList.Where(x => x.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeID == EmployeeCodeID).ToList();
                }
                foreach (Assignings item in AssigningList)
                {
                    if (item.EmployeeCareerHistoryID.HasValue)
                    {
                        IsExists = EmployeeCodeBLLList.Any(x => x.EmployeeCodeID == item.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeID);
                        if (IsExists == false)
                        {
                            PlacementBLL = new PlacementBLL().GetCurrentActualOrgAndActualJob(item.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo);

                            EmployeeCode = new EmployeesCodesBLL();
                            EmployeeCode.EmployeeCodeID = item.EmployeesCareersHistory.EmployeeCodeID;
                            EmployeeCode.EmployeeCodeNo = item.EmployeesCareersHistory.EmployeesCodes.EmployeeCodeNo;
                            EmployeeCode.Employee = new EmployeesBLL().GetByEmployeeID(item.EmployeesCareersHistory.EmployeesCodes.EmployeeID);
                            EmployeeCode.EmployeeCurrentJob = new EmployeesCareersHistoryBLL()
                            {
                                OrganizationJob = new OrganizationsJobsBLL()
                                {
                                    OrganizationStructure = new OrganizationsStructuresBLL().GetByOrganizationID(item.OrganizationID.Value),
                                    Job = PlacementBLL.Job
                                }
                            };
                            EmployeeCodeBLLList.Add(EmployeeCode);
                        }
                    }
                }
                #endregion

                return EmployeeCodeBLLList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal EmployeesCodesBLL MapEmployeeCode(EmployeesCodes EmployeeCode)
        {
            try
            {
                EmployeesCodesBLL EmployeeCodeBLL = null;
                if (EmployeeCode != null)
                {
                    EmployeeCodeBLL = new EmployeesCodesBLL()
                    {
                        Employee = new EmployeesBLL().MapEmployee(EmployeeCode.Employees),
                        EmployeeCodeID = EmployeeCode.EmployeeCodeID,
                        EmployeeCodeNo = EmployeeCode.EmployeeCodeNo,
                        EmployeeCurrentJob = EmployeeCode.EmployeesCareersHistory.Count() > 0 ? new EmployeesCareersHistoryBLL().MapEmployeeCareerHistory(EmployeeCode.EmployeesCareersHistory.FirstOrDefault(x => x.IsActive == true)) : null,
                        IsActive = true,
                    };
                }
                return EmployeeCodeBLL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}