﻿using HCMDAL;
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Configuration;
using System.IO;
using HCMBLL.Enums;

namespace HCMBLL
{
    public class EmployeesBLL : CommonEntity
    {
        //private string TimeAttendanceServiceURL = ConfigurationManager.AppSettings["TimeAttendanceServiceURL"].ToString();
        //private WebClient client = new WebClient();

        public virtual int EmployeeID
        {
            get;
            set;
        }

        public virtual string FirstNameAr
        {
            get;
            set;
        }

        public virtual string MiddleNameAr
        {
            get;
            set;
        }

        public virtual string GrandFatherNameAr
        {
            get;
            set;
        }

        public virtual string FifthNameAr
        {
            get;
            set;
        }

        public virtual string LastNameAr
        {
            get;
            set;
        }

        public virtual string FirstNameEn
        {
            get;
            set;
        }

        public virtual string MiddleNameEn
        {
            get;
            set;
        }

        public virtual string GrandFatherNameEn
        {
            get;
            set;
        }

        public virtual string FifthNameEn
        {
            get;
            set;
        }

        public virtual string LastNameEn
        {
            get;
            set;
        }

        public virtual string EmployeeNameAr
        {
            get
            {
                return FirstNameAr + " " + MiddleNameAr + " " + GrandFatherNameAr + " " + FifthNameAr + " " + LastNameAr;
            }
        }

        public virtual string EmployeeNameEn
        {
            get
            {
                return FirstNameEn + " " + MiddleNameEn + " " + GrandFatherNameEn + " " + FifthNameEn + " " + LastNameEn;
            }
        }

        public virtual string EmployeeIDNo
        {
            get;
            set;
        }

        public virtual string EmployeeBirthPlace
        {
            get;
            set;
        }

        public virtual DateTime? EmployeePassportEndDate
        {
            get;
            set;
        }

        public virtual string EmployeeEMail
        {
            get;
            set;
        }

        public virtual string EmployeeMobileNo
        {
            get;
            set;
        }

        public virtual DateTime? EmployeePassportIssueDate
        {
            get;
            set;
        }

        public virtual DateTime? EmployeeBirthDate
        {
            get;
            set;
        }

        public virtual DateTime? EmployeeIDIssueDate
        {
            get;
            set;
        }

        public virtual string EmployeePicture
        {
            get;
            set;
        }

        public virtual string EmployeePassportNo
        {
            get;
            set;
        }

        public virtual string EmployeePassportSource
        {
            get;
            set;
        }

        public virtual EmployeesCodesBLL ActiveEmployeeCode
        {
            get
            {
                return new EmployeesCodesBLL().GetActiveEmployeeCodeByEmployeeID(EmployeeID);
            }
        }

        public virtual IEnumerable<EmployeesCodesBLL> EmployeeCodes
        {
            get;
            set;
        }

        public CountriesBLL Nationality { get; set; }

        public DateTime? EmployeeIDExpiryDate { get; set; }

        public int? EmployeeIDCopyNo { get; set; }

        public string EmployeeIDIssuePlace { get; set; }

        public int? DependentCount { get; set; }

        public MaritalStatusBLL MaritalStatus { get; set; }

        public GendersBLL Gender { get; set; }


        public virtual int Add()
        {
            try
            {
                EmployeesDAL employeeDal = new EmployeesDAL();
                Employees employee = new Employees()
                {
                    EmployeeIDNo = this.EmployeeIDNo,
                    FirstNameAr = this.FirstNameAr,
                    MiddleNameAr = this.MiddleNameAr,
                    GrandFatherNameAr = this.GrandFatherNameAr,
                    FifthNameAr = this.FifthNameAr,
                    LastNameAr = this.LastNameAr,
                    FirstNameEn = this.FirstNameEn,
                    MiddleNameEn = this.MiddleNameEn,
                    GrandFatherNameEn = this.GrandFatherNameEn,
                    FifthNameEn = this.FifthNameEn,
                    LastNameEn = this.LastNameEn,
                    EmployeeBirthDate = (DateTime)this.EmployeeBirthDate.Value.Date,
                    EmployeeBirthPlace = this.EmployeeBirthPlace,
                    EmployeeMobileNo = this.EmployeeMobileNo,
                    EmployeePassportNo = this.EmployeePassportNo,
                    EmployeeEMail = this.EmployeeEMail,
                    EmployeeIDIssueDate = this.EmployeeIDIssueDate != null ? (DateTime)this.EmployeeIDIssueDate.Value.Date : (DateTime?)null,
                    EmployeePassportSource = this.EmployeePassportSource,
                    EmployeePassportIssueDate = this.EmployeePassportIssueDate != null ? (DateTime)this.EmployeePassportIssueDate.Value.Date : (DateTime?)null,
                    EmployeePassportEndDate = this.EmployeePassportEndDate != null ? (DateTime)this.EmployeePassportEndDate.Value.Date : (DateTime?)null,
                    EmployeeIDExpiryDate = this.EmployeeIDExpiryDate,
                    EmployeeIDCopyNo = this.EmployeeIDCopyNo,
                    EmployeeIDIssuePlace = this.EmployeeIDIssuePlace,
                    DependentCount = this.DependentCount,
                    MaritalStatusID = this.MaritalStatus.MaritalStatusID,
                    GenderID = this.Gender.GenderID,
                    NationalityID = this.Nationality.CountryID,
                    CreatedDate = DateTime.Now,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID,
                };
                employeeDal.Insert(employee);
                return this.EmployeeID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual int Remove()
        {
            try
            {
                return new EmployeesDAL().Delete(new Employees() { EmployeeID = EmployeeID }, this.LoginIdentity.EmployeeCodeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual int Update()
        {
            try
            {
                Employees Employee = new Employees()
                {
                    EmployeeID = this.EmployeeID,
                    EmployeeIDNo = this.EmployeeIDNo,
                    EmployeeBirthDate = (DateTime)this.EmployeeBirthDate,
                    EmployeeBirthPlace = this.EmployeeBirthPlace,
                    EmployeeEMail = this.EmployeeEMail,
                    EmployeeIDIssueDate = this.EmployeeIDIssueDate,
                    EmployeePassportEndDate = this.EmployeePassportEndDate,
                    EmployeePassportIssueDate = this.EmployeePassportIssueDate,
                    EmployeePassportNo = this.EmployeePassportNo,
                    EmployeePassportSource = this.EmployeePassportSource,
                    EmployeeIDExpiryDate = this.EmployeeIDExpiryDate,
                    EmployeeIDCopyNo = this.EmployeeIDCopyNo,
                    EmployeeIDIssuePlace = this.EmployeeIDIssuePlace,
                    DependentCount = this.DependentCount,
                    MaritalStatusID = this.MaritalStatus.MaritalStatusID,
                    GenderID = this.Gender.GenderID,
                    NationalityID = this.Nationality.CountryID,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
                };
                return new EmployeesDAL().Update(Employee);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual int UpdateMobileNo()
        {
            try
            {
                Employees _employee = new Employees()
                {
                    EmployeeID = this.GetByEmployeeIDNo(this.EmployeeIDNo).EmployeeID,
                    EmployeeMobileNo = this.EmployeeMobileNo,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
                };
                return new EmployeesDAL().UpdateMobileNo(_employee);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual int UpdateEmployeeName()
        {
            Employees _employee = new Employees()
            {
                EmployeeID = this.EmployeeID,
                FirstNameAr = this.FirstNameAr,
                MiddleNameAr = this.MiddleNameAr,
                GrandFatherNameAr = this.GrandFatherNameAr,
                FifthNameAr = this.FifthNameAr,
                LastNameAr = this.LastNameAr,
                FirstNameEn = this.FirstNameEn,
                MiddleNameEn = this.MiddleNameEn,
                GrandFatherNameEn = this.GrandFatherNameEn,
                FifthNameEn = this.FifthNameEn,
                LastNameEn = this.LastNameEn,
                LastUpdatedDate = DateTime.Now,
                LastUpdatedBy = this.LoginIdentity.EmployeeCodeID
            };
            return new EmployeesDAL().UpdateEmployeeName(_employee);
        }

        public virtual List<EmployeesBLL> GetEmployees()
        {
            try
            {
                List<Employees> EmployeesList = new EmployeesDAL().GetEmployees();
                List<EmployeesBLL> EmployeesBLLList = new List<EmployeesBLL>();
                foreach (var item in EmployeesList)
                    EmployeesBLLList.Add(new EmployeesBLL().MapEmployee(item));

                return EmployeesBLLList.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual EmployeesBLL GetByEmployeeID(int EmployeeID)
        {
            try
            {
                EmployeesBLL EmployeeBLL = null;
                Employees Employee = new EmployeesDAL().GetByEmployeeID(EmployeeID);
                if (Employee != null)
                    EmployeeBLL = this.MapEmployee(Employee);

                return EmployeeBLL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual EmployeesBLL GetByEmployeeIDWithEmpPic(int EmployeeID)
        {
            try
            {
                Employees Employee = new EmployeesDAL().GetByEmployeeID(EmployeeID);
                if (Employee != null)
                {
                    EmployeesBLL EmployeeBLL = MapEmployee(Employee);
                    EmployeeBLL.EmployeePicture = EmployeeBLL.GetEmployeePicture();
                    return EmployeeBLL;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual EmployeesBLL GetByEmployeeIDNo(string EmployeeIDNo)
        {
            try
            {
                Employees Employee = new EmployeesDAL().GetByEmployeeIDNo(EmployeeIDNo);

                if (Employee != null)
                {
                    EmployeesBLL EmployeeBLL = MapEmployee(Employee);
                    EmployeeBLL.EmployeePicture = EmployeeBLL.GetEmployeePicture();
                    return EmployeeBLL;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmployeesBLL GetByEmployeeNameAr(string EmployeeNameAr)
        {
            try
            {
                Employees Employee = new EmployeesDAL().GetByEmployeeNameAr(EmployeeNameAr);
                if (Employee != null)
                {
                    EmployeesBLL EmployeeBLL = MapEmployee(Employee);
                    EmployeeBLL.EmployeePicture = EmployeeBLL.GetEmployeePicture();
                    return EmployeeBLL;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal string GetEmployeePicture()
        {
            if (this.ActiveEmployeeCode != null)
                //return Convert.ToBase64String(client.DownloadData(TimeAttendanceServiceURL + "GetEmployeeImageByEmployeeCode/" + this.ActiveEmployeeCode.EmployeeCodeNo));
                return Convert.ToBase64String(GetEmployeePictureBytes(this.ActiveEmployeeCode.EmployeeCodeNo));
            else
                return null;
        }

        public byte[] GetEmployeePictureBytes(string EmployeeCodeNo)
        {
            try
            {
                string EmployeesPicsPath = System.Configuration.ConfigurationManager.AppSettings["EmployeesPicsPath"].ToString();
                string ImagePath = @EmployeesPicsPath + EmployeeCodeNo + ".jpg";

                if (!File.Exists(ImagePath))
                    ImagePath = @EmployeesPicsPath + "anonymous.png";

                return File.ReadAllBytes(ImagePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual int AddHiringNewEmployee(EmployeesBLL EmployeesBLL, EmployeesCodesBLL EmployeesCodesBLL, EmployeesCareersHistoryBLL EmployeesCareersHistoryBLL, EmployeesQualificationsBLL EmployeesQualificationsBLL, ContractorsBasicSalariesBLL ContractorsBasicSalariesBLL, List<EmployeesAllowancesBLL> EmployeesAllowancesBLLLst)
        {
            try
            {
                EmployeesDAL employeeDal = new EmployeesDAL();
                Employees employee = new Employees()
                {
                    EmployeeIDNo = this.EmployeeIDNo,
                    FirstNameAr = this.FirstNameAr,
                    MiddleNameAr = this.MiddleNameAr,
                    GrandFatherNameAr = this.GrandFatherNameAr,
                    FifthNameAr = this.FifthNameAr,
                    LastNameAr = this.LastNameAr,
                    FirstNameEn = this.FirstNameEn,
                    MiddleNameEn = this.MiddleNameEn,
                    GrandFatherNameEn = this.GrandFatherNameEn,
                    FifthNameEn = this.FifthNameEn,
                    LastNameEn = this.LastNameEn,
                    EmployeeBirthDate = (DateTime)this.EmployeeBirthDate.Value.Date,
                    EmployeeBirthPlace = this.EmployeeBirthPlace,
                    EmployeeMobileNo = this.EmployeeMobileNo,
                    EmployeePassportNo = this.EmployeePassportNo,
                    EmployeeEMail = this.EmployeeEMail,
                    EmployeeIDIssueDate = this.EmployeeIDIssueDate != null ? (DateTime)this.EmployeeIDIssueDate.Value.Date : (DateTime?)null,
                    EmployeePassportSource = this.EmployeePassportSource,
                    EmployeePassportIssueDate = this.EmployeePassportIssueDate != null ? (DateTime)this.EmployeePassportIssueDate.Value.Date : (DateTime?)null,
                    EmployeePassportEndDate = this.EmployeePassportEndDate != null ? (DateTime)this.EmployeePassportEndDate.Value.Date : (DateTime?)null,
                    EmployeeIDExpiryDate = this.EmployeeIDExpiryDate,
                    EmployeeIDCopyNo = this.EmployeeIDCopyNo,
                    EmployeeIDIssuePlace = this.EmployeeIDIssuePlace,
                    DependentCount = this.DependentCount,
                    MaritalStatusID = this.MaritalStatus.MaritalStatusID,
                    GenderID = this.Gender.GenderID,
                    NationalityID = this.Nationality.CountryID,
                    CreatedDate = DateTime.Now,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID,
                };
                EmployeesCodes employeesCode = new EmployeesCodes()
                { 
                    EmployeeCodeNo = EmployeesCodesBLL.EmployeeCodeNo,
                    EmployeeTypeID = EmployeesCodesBLL.EmployeeType.EmployeeTypeID,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID
                };

                EmployeesCareersHistory employeeCareerHistory = new EmployeesCareersHistory()
                {
                    CareerHistoryTypeID = EmployeesCareersHistoryBLL.CareerHistoryType.CareerHistoryTypeID,
                    CareerDegreeID = EmployeesCareersHistoryBLL.CareerDegree.CareerDegreeID,
                    OrganizationJobID = EmployeesCareersHistoryBLL.OrganizationJob.OrganizationJobID,
                    JoinDate = EmployeesCareersHistoryBLL.JoinDate.Date,
                    TransactionStartDate = EmployeesCareersHistoryBLL.JoinDate.Date,
                    IsActive = true,
                    CreatedBy = this.LoginIdentity.EmployeeCodeID,
                    CreatedDate = DateTime.Now
                };
                EmployeesQualifications employeeQualification = new EmployeesQualifications();
                employeeQualification.QualificationDegreeID = EmployeesQualificationsBLL.QualificationDegree.QualificationDegreeID;
                employeeQualification.QualificationID = EmployeesQualificationsBLL.Qualification.QualificationID;
                employeeQualification.GeneralSpecializationID = EmployeesQualificationsBLL.GeneralSpecialization.GeneralSpecializationID;
                employeeQualification.ExactSpecializationID = EmployeesQualificationsBLL.ExactSpecialization.ExactSpecializationID == 0 ? (int?)null : EmployeesQualificationsBLL.ExactSpecialization.ExactSpecializationID;
                employeeQualification.UniSchName = EmployeesQualificationsBLL.UniSchName;
                employeeQualification.Department = EmployeesQualificationsBLL.Department;
                employeeQualification.FullGPA = EmployeesQualificationsBLL.FullGPA;
                employeeQualification.GPA = EmployeesQualificationsBLL.GPA;
                employeeQualification.StudyPlace = EmployeesQualificationsBLL.StudyPlace;
                employeeQualification.GraduationDate = EmployeesQualificationsBLL.GraduationDate;
                employeeQualification.GraduationYear = EmployeesQualificationsBLL.GraduationYear;
                employeeQualification.Percentage = EmployeesQualificationsBLL.Percentage;
                employeeQualification.QualificationTypeID = EmployeesQualificationsBLL.QualificationType.QualificationTypeID == 0 ? (int?)null : EmployeesQualificationsBLL.QualificationType.QualificationTypeID;
                employeeQualification.CreatedDate = DateTime.Now;
                employeeQualification.CreatedBy = this.LoginIdentity.EmployeeCodeID;

                ContractorsBasicSalaries contractorBasicSalary = new ContractorsBasicSalaries();
                contractorBasicSalary.BasicSalary = ContractorsBasicSalariesBLL.BasicSalary;
                contractorBasicSalary.TransfareAllowance = ContractorsBasicSalariesBLL.TransfareAllowance;
                contractorBasicSalary.CreatedDate = DateTime.Now;
                contractorBasicSalary.CreatedBy = this.LoginIdentity.EmployeeCodeID;

                List<EmployeesAllowances> employeesAllowancesList = new List<EmployeesAllowances>();
                foreach (var EmployeeAllowanceBLL in EmployeesAllowancesBLLLst)
                {
                    employeesAllowancesList.Add(new EmployeesAllowances()
                    {
                        AllowanceID = EmployeeAllowanceBLL.Allowance.AllowanceID,
                        AllowanceStartDate= EmployeeAllowanceBLL.AllowanceStartDate,
                        IsActive= EmployeeAllowanceBLL.IsActive,
                        CreatedBy=this.LoginIdentity.EmployeeCodeID,
                        CreatedDate = DateTime.Now
                });
                }


                employee.EmployeesCodes = new List<EmployeesCodes>();
                employee.EmployeesCodes.Add(employeesCode);
                employeesCode.EmployeesCareersHistory = new List<EmployeesCareersHistory>();
                employeesCode.EmployeesCareersHistory.Add(employeeCareerHistory);
                employeesCode.EmployeesQualifications = new List<EmployeesQualifications>();
                employeesCode.EmployeesQualifications.Add(employeeQualification);
                #region check if the new employee is contractor then add financial advantages to him
                OrganizationsJobsBLL OrganizationsJobsBLL=new OrganizationsJobsBLL().GetByOrganizationJobID(employeeCareerHistory.OrganizationJobID);
                if (OrganizationsJobsBLL.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.ContractualExpats || OrganizationsJobsBLL.Rank.RankCategory.RankCategoryID == (int)RanksCategoriesEnum.ContractualSaudis)
                {
                    employeesCode.ContractorsBasicSalaries = new List<ContractorsBasicSalaries>();
                    employeesCode.ContractorsBasicSalaries.Add(contractorBasicSalary);
                    employeeCareerHistory.EmployeesAllowances = employeesAllowancesList;
                }
                #endregion
                employeeDal.Insert(employee);
                return this.EmployeeID;
            }
            catch
            {
                throw;
            }
        }

        internal EmployeesBLL MapEmployee(Employees Employee)
        {
            try
            {
                EmployeesBLL EmployeeBLL = null;
                if (Employee != null)
                {
                    EmployeeBLL = new EmployeesBLL()
                    {
                        EmployeeID = Employee.EmployeeID,
                        EmployeeIDNo = Employee.EmployeeIDNo,
                        FirstNameAr = Employee.FirstNameAr,
                        MiddleNameAr = Employee.MiddleNameAr,
                        GrandFatherNameAr = Employee.GrandFatherNameAr,
                        FifthNameAr = Employee.FifthNameAr,
                        LastNameAr = Employee.LastNameAr,
                        FirstNameEn = Employee.FirstNameEn,
                        MiddleNameEn = Employee.MiddleNameEn,
                        GrandFatherNameEn = Employee.GrandFatherNameEn,
                        FifthNameEn = Employee.FifthNameEn,
                        LastNameEn = Employee.LastNameEn,
                        EmployeeBirthDate = Employee.EmployeeBirthDate,
                        EmployeeBirthPlace = Employee.EmployeeBirthPlace,
                        EmployeeEMail = Employee.EmployeeEMail,
                        EmployeeIDIssueDate = Employee.EmployeeIDIssueDate,
                        EmployeeMobileNo = Employee.EmployeeMobileNo,
                        EmployeePassportNo = Employee.EmployeePassportNo,
                        EmployeePassportSource = Employee.EmployeePassportSource,
                        EmployeePassportIssueDate = Employee.EmployeePassportIssueDate,
                        EmployeePassportEndDate = Employee.EmployeePassportEndDate,
                        EmployeeIDExpiryDate = Employee.EmployeeIDExpiryDate,
                        EmployeeIDCopyNo = Employee.EmployeeIDCopyNo,
                        EmployeeIDIssuePlace = Employee.EmployeeIDIssuePlace,
                        DependentCount = Employee.DependentCount,
                        MaritalStatus = new MaritalStatusBLL().MapMaritalStatus(Employee.MaritalStatus),
                        Gender = new GendersBLL().MapGenders(Employee.Genders),
                        Nationality = new CountriesBLL().MapCountry(Employee.Nationalities)

                        //EmployeePicture = new EmployeesBLL() { EmployeeID = Employee.EmployeeID }.GetEmployeePicture(),                        
                    };
                }
                return EmployeeBLL;
            }
            catch
            {
                throw;
            }
        }
    }
}
