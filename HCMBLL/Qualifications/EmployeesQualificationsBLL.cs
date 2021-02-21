using HCMBLL.DTO;
using HCMBLL.Enums;
using HCMDAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HCMBLL
{
    public class EmployeesQualificationsBLL : CommonEntity, IEntity
    {
        public int EmployeeQualificationID { get; set; }

        public QualificationsDegreesBLL QualificationDegree { get; set; }

        public QualificationsBLL Qualification { get; set; }

        public GeneralSpecializationsBLL GeneralSpecialization { get; set; }

        public ExactSpecializationsBLL ExactSpecialization { get; set; }

        public EmployeesCodesBLL EmployeeCode { get; set; }

        public string UniSchName { get; set; }

        public string Department { get; set; }

        public decimal? FullGPA { get; set; }

        public decimal? GPA { get; set; }

        public string StudyPlace { get; set; }

        public DateTime? GraduationDate { get; set; }

        public string GraduationYear { get; set; }

        public decimal? Percentage { get; set; }

        public QualificationsTypesBLL QualificationType { get; set; }

        public int Weight { get; set; }

        public decimal? Points { get; set; }

        public bool IsIncluded { get; set; }

        //public EmployeesQualificationsBLL GetEmployeeCurrentQualificationByEmployeeCodeID(int EmployeeCodeID)
        //{
        //    EmployeesQualifications EmployeesQualifications = new EmployeesQualificationsDAL().GetEmployeeCurrentQualificationByEmployeeCodeID(EmployeeCodeID).FirstOrDefault();
        //    EmployeesQualificationsBLL EmployeesQualificationsBLL = new EmployeesQualificationsBLL();
        //    if (EmployeesQualifications != null)
        //    {

        //        EmployeesQualificationsBLL = new EmployeesQualificationsBLL()
        //        {
        //            EmployeeQualificationID = EmployeesQualifications.EmployeeQualificationID,
        //            QualificationName = string.Format("{0} - {1}",
        //                                    EmployeesQualifications.QualificationsDegrees != null ? EmployeesQualifications.QualificationsDegrees.QualificationDegreeName : "",
        //                                    EmployeesQualifications.Qualifications != null ? EmployeesQualifications.Qualifications.QualificationName : "")

        //        };
        //    }
        //}

        public List<EmployeesQualificationsBLL> GetEmployeesQualifications()
        {
            try
            {
                List<EmployeesQualifications> EmployeeQualificationList = new EmployeesQualificationsDAL().GetEmployeesQualifications();
                List<EmployeesQualificationsBLL> EmployeesQualificationsBLL = new List<EmployeesQualificationsBLL>();

                foreach (var EmployeeQualification in EmployeeQualificationList)
                    EmployeesQualificationsBLL.Add(this.MapEmployeeQualification(EmployeeQualification));

                return EmployeesQualificationsBLL;
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesQualificationsBLL> GetEmployeesQualifications(List<int> EmployeesCodesIDs)
        {
            try
            {
                List<EmployeesQualifications> EmployeeQualificationList = new EmployeesQualificationsDAL().GetEmployeesQualifications(EmployeesCodesIDs);
                List<EmployeesQualificationsBLL> EmployeesQualificationsBLL = new List<EmployeesQualificationsBLL>();

                foreach (var EmployeeQualification in EmployeeQualificationList)
                    EmployeesQualificationsBLL.Add(this.MapEmployeeQualification(EmployeeQualification));

                return EmployeesQualificationsBLL;
            }
            catch
            {
                throw;
            }
        }

        public List<EmployeesQualificationsBLL> GetEmployeeQualificationByEmployeeCodeID(int EmployeeCodeID)
        {
            List<EmployeesQualifications> EmployeeQualificationList = new EmployeesQualificationsDAL().GetEmployeeQualificationByEmployeeCodeID(EmployeeCodeID);
            List<EmployeesQualificationsBLL> EmployeesQualificationsBLL = new List<EmployeesQualificationsBLL>();

            foreach (var EmployeeQualification in EmployeeQualificationList)
            {
                EmployeesQualificationsBLL.Add(this.MapEmployeeQualification(EmployeeQualification));
            }
            return EmployeesQualificationsBLL;
        }

        public virtual EmployeesQualificationsBLL GetByEmployeeQualificationID(int EmployeeQualificationID)
        {
            try
            {
                EmployeesQualifications EmployeeQualification = new EmployeesQualificationsDAL().GetByEmployeeQualificationID(EmployeeQualificationID);
                return new EmployeesQualificationsBLL().MapEmployeeQualification(EmployeeQualification);
            }
            catch
            {
                throw;
            }
        }

        public virtual Result Add()
        {
            try
            {
                Result result = null;
                EmployeesQualifications EmployeeQualification = new EmployeesQualifications();
                EmployeeQualification.QualificationDegreeID = this.QualificationDegree.QualificationDegreeID;
                EmployeeQualification.QualificationID = this.Qualification.QualificationID;
                EmployeeQualification.GeneralSpecializationID = this.GeneralSpecialization.GeneralSpecializationID;
                EmployeeQualification.ExactSpecializationID = this.ExactSpecialization.ExactSpecializationID == 0 ? (int?)null : this.ExactSpecialization.ExactSpecializationID;
                EmployeeQualification.EmployeeCodeID = this.EmployeeCode.EmployeeCodeID;
                EmployeeQualification.UniSchName = this.UniSchName;
                EmployeeQualification.Department = this.Department;
                EmployeeQualification.FullGPA = this.FullGPA;
                EmployeeQualification.GPA = this.GPA;
                EmployeeQualification.StudyPlace = this.StudyPlace;
                EmployeeQualification.GraduationDate = this.GraduationDate;
                EmployeeQualification.GraduationYear = this.GraduationYear;
                //EmployeeQualification.Percentage = this.Percentage;
                EmployeeQualification.QualificationTypeID = this.QualificationType.QualificationTypeID == 0 ? (int?)null : this.QualificationType.QualificationTypeID;
                EmployeeQualification.CreatedDate = DateTime.Now;
                EmployeeQualification.CreatedBy = this.LoginIdentity.EmployeeCodeID;

                this.EmployeeQualificationID = new EmployeesQualificationsDAL().Insert(EmployeeQualification);
                result = new Result();
                result.Entity = this;
                result.EnumType = typeof(EmployeesQualificationsValidationEnum);
                result.EnumMember = EmployeesQualificationsValidationEnum.Done.ToString();
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
                EmployeesQualifications EmployeeQualification = new EmployeesQualifications();
                EmployeeQualification.EmployeeQualificationID = this.EmployeeQualificationID;
                EmployeeQualification.QualificationDegreeID = this.QualificationDegree.QualificationDegreeID;
                EmployeeQualification.QualificationID = this.Qualification.QualificationID;
                EmployeeQualification.GeneralSpecializationID = this.GeneralSpecialization.GeneralSpecializationID;
                EmployeeQualification.ExactSpecializationID = this.ExactSpecialization.ExactSpecializationID == 0 ? (int?)null : this.ExactSpecialization.ExactSpecializationID;
                //EmployeeQualification.EmployeeCodeID = this.EmployeeCode.EmployeeCodeID;
                EmployeeQualification.UniSchName = this.UniSchName;
                EmployeeQualification.Department = this.Department;
                EmployeeQualification.FullGPA = this.FullGPA;
                EmployeeQualification.GPA = this.GPA;
                EmployeeQualification.StudyPlace = this.StudyPlace;
                EmployeeQualification.GraduationDate = this.GraduationDate;
                EmployeeQualification.GraduationYear = this.GraduationYear;
                //EmployeeQualification.Percentage = this.Percentage;
                EmployeeQualification.QualificationTypeID = this.QualificationType.QualificationTypeID == 0 ? (int?)null : this.QualificationType.QualificationTypeID; //this.QualificationType.QualificationTypeID;
                EmployeeQualification.LastUpdatedDate = DateTime.Now;
                EmployeeQualification.LastUpdatedBy = this.LoginIdentity.EmployeeCodeID;

                int UpdateResult = new EmployeesQualificationsDAL().Update(EmployeeQualification);
                if (UpdateResult != 0)
                {
                    result = new Result();
                    result.Entity = this;
                    result.EnumType = typeof(EmployeesQualificationsValidationEnum);
                    result.EnumMember = EmployeesQualificationsValidationEnum.Done.ToString();
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        public Result Remove(int EmployeeQualificationID)
        {
            try
            {
                Result result = null;
                new EmployeesQualificationsDAL().Delete(EmployeeQualificationID, this.LoginIdentity.EmployeeCodeID);
                return result = new Result()
                {
                    EnumType = typeof(EmployeesQualificationsValidationEnum),
                    EnumMember = EmployeesQualificationsValidationEnum.Done.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        internal EmployeesQualificationsBLL GetLastEmployeeQualification(int EmployeeCodeID)
        {
            EmployeesQualificationsBLL LastEmployeeQualification = null;
            LastEmployeeQualification = this.GetEmployeeQualificationByEmployeeCodeID(EmployeeCodeID).OrderByDescending(x => x.QualificationDegree.Weight)
                                                                                //.OrderByDescending(x => x.GraduationDate.HasValue ? x.GraduationDate.Value)
                                                                                .Take(1).FirstOrDefault();
            if (LastEmployeeQualification == null)
            {
                LastEmployeeQualification = new EmployeesQualificationsBLL()
                {
                    QualificationDegree = new QualificationsDegreesBLL(),
                    Qualification = new QualificationsBLL(),
                    GeneralSpecialization = new GeneralSpecializationsBLL()
                };
            }

            return LastEmployeeQualification;
        }

        internal EmployeesQualificationsBLL GetLastEmployeeQualification(List<EmployeesQualificationsBLL> EmployeesQualificationsList, int EmployeeCodeID)
        {
            EmployeesQualificationsBLL LastEmployeeQualification = null;
            EmployeesQualificationsList.ForEach(x => x.GraduationDate = x.GraduationDate.HasValue ? x.GraduationDate.Value : Convert.ToDateTime(Globals.Calendar.UmAlquraToGreg(string.Format("{0}/{1}/{2}", "1", "1", x.GraduationYear)), new CultureInfo("en-US")));
            EmployeesQualificationsList.ForEach(x => x.GraduationYear = !string.IsNullOrEmpty(x.GraduationYear) ? x.GraduationYear : x.GraduationDate.HasValue ? x.GraduationDate.Value.Year.ToString() : string.Empty);

            LastEmployeeQualification = EmployeesQualificationsList.Where(x => x.EmployeeCode.EmployeeCodeID == EmployeeCodeID)
                                                                    .OrderByDescending(x => x.QualificationDegree.Weight)
                                                                    .OrderByDescending(x=> x.GraduationDate)
                                                                    .OrderByDescending(x => x.GraduationYear)
                                                                    //.OrderByDescending(x => x.GraduationDate.HasValue ? x.GraduationDate.Value : Convert.ToDateTime(Globals.Calendar.UmAlquraToGreg(string.Format("{0}/{1}/{2}", "1", "1", x.GraduationYear)), new CultureInfo("en-US")))
                                                                    //.OrderByDescending(x => !string.IsNullOrEmpty(x.GraduationYear) ? x.GraduationYear : x.GraduationDate.Value.Year.ToString())
                                                                    .Take(1).FirstOrDefault();

            if (LastEmployeeQualification == null)
            {
                LastEmployeeQualification = new EmployeesQualificationsBLL()
                {
                    QualificationDegree = new QualificationsDegreesBLL(),
                    Qualification = new QualificationsBLL(),
                    GeneralSpecialization = new GeneralSpecializationsBLL()
                };
            }

            return LastEmployeeQualification;
        }

        public EmployeesQualificationsBLL GetByQualificationID(int QualificationID)
        {
            EmployeesQualifications EmployeeQualification = new EmployeesQualificationsDAL().GetByQualificationID(QualificationID);
            return new EmployeesQualificationsBLL().MapEmployeeQualification(EmployeeQualification);
        }

        public EmployeesQualificationsBLL GetByGeneralSpecializationID(int GeneralSpecializationID)
        {
            EmployeesQualifications EmployeeQualification = new EmployeesQualificationsDAL().GetByGeneralSpecializationID(GeneralSpecializationID);
            return new EmployeesQualificationsBLL().MapEmployeeQualification(EmployeeQualification);
        }

        public EmployeesQualificationsBLL GetByExactSpecializationID(int ExactSpecializationID)
        {
            EmployeesQualifications EmployeeQualification = new EmployeesQualificationsDAL().GetByExactSpecializationID(ExactSpecializationID);
            return new EmployeesQualificationsBLL().MapEmployeeQualification(EmployeeQualification);
        }

        internal EmployeesQualificationsBLL MapEmployeeQualification(EmployeesQualifications item)
        {
            return item != null ?
                new EmployeesQualificationsBLL()
                {
                    EmployeeQualificationID = item.EmployeeQualificationID,
                    QualificationDegree = item.QualificationDegreeID.HasValue ? new QualificationsDegreesBLL().MapQualificationDegree(item.QualificationsDegrees) : null,
                    Qualification = item.QualificationID.HasValue ? new QualificationsBLL().MapQualification(item.Qualifications) : null,
                    GeneralSpecialization = item.GeneralSpecializationID.HasValue ? new GeneralSpecializationsBLL().MapGeneralSpecialization(item.GeneralSpecializations) : null,
                    ExactSpecialization = item.ExactSpecializationID.HasValue ? new ExactSpecializationsBLL().MapExactSpecialization(item.ExactSpecializations) : null,
                    EmployeeCode = item.EmployeeCodeID.HasValue ? new EmployeesCodesBLL().MapEmployeeCode(item.EmployeesCodes) : null,
                    UniSchName = item.UniSchName,
                    FullGPA = item.FullGPA.HasValue ? item.FullGPA.Value : (decimal?)null,
                    Department = item.Department,
                    GPA = item.GPA.HasValue ? item.GPA.Value : (decimal?)null,
                    StudyPlace = item.StudyPlace,
                    GraduationDate = item.GraduationDate,
                    GraduationYear = item.GraduationYear,
                    Percentage = item.Percentage.HasValue ? item.Percentage.Value : (decimal?)null,
                    QualificationType = item.QualificationTypeID.HasValue ? new QualificationsTypesBLL().GetByQualificationTypeID(item.QualificationTypeID.Value) : null,
                    //CreatedBy = item.CreatedBy.HasValue ? new EmployeesCodesBLL().MapEmployeeCode(item.EmployeesCodes) : null,
                    CreatedBy = item.CreatedBy.HasValue ? new EmployeesCodesBLL().MapEmployeeCode(item.CreatedByNav) : null,
                    CreatedDate = item.CreatedDate.Value
                }
                : null;
        }


        //public List<ChartsAxis> GetQualificationsBasedOnAssigningsAsRanksCategories(int QualificationDegreeID, int QualificationID, int GeneralSpecializationID, int OrganizationID)
        //{
        //    try
        //    {
        //        List<int> OrganizationIDsList = new OrganizationsStructuresBLL().GetByOrganizationIDsWithhAllChilds(OrganizationID);

        //        // Get actual employees Based On Assignings by date
        //        List<vwActualEmployeesBasedOnAssignings> ActualEmployeesBasedOnAssignings = new AssigningsDAL().GetActualEmployeeBasedOnAssignings().Where(x => OrganizationIDsList.Contains(x.OrganizationID.Value)).ToList();

        //        List<int> EmployeesCodesIDs = new List<int>();
        //        ActualEmployeesBasedOnAssignings.ForEach(x => EmployeesCodesIDs.Add(x.EmployeeCodeID));

        //        List<EmployeesQualificationsBLL> EmployeesQualificationsList = new EmployeesQualificationsBLL().GetEmployeesQualifications(EmployeesCodesIDs);

        //        List<EmployeesQualificationsBLL> EmployeesQualificationsOfActualEmployeesList = new List<EmployeesQualificationsBLL>();

        //        ActualEmployeesBasedOnAssignings.ForEach(x => EmployeesQualificationsOfActualEmployeesList.Add(new EmployeesQualificationsBLL().GetLastEmployeeQualification(EmployeesQualificationsList, x.EmployeeCodeID)));

        //        EmployeesQualificationsOfActualEmployeesList = EmployeesQualificationsOfActualEmployeesList.Where(x => x.QualificationDegree.QualificationDegreeID == QualificationDegreeID
        //                                                                                                            && (QualificationID != 0 ? x.Qualification.QualificationID == QualificationID : x.Qualification.QualificationID == x.Qualification.QualificationID)
        //                                                                                                            && (GeneralSpecializationID != 0 ? x.GeneralSpecialization.GeneralSpecializationID == GeneralSpecializationID : x.GeneralSpecialization.GeneralSpecializationID == x.GeneralSpecialization.GeneralSpecializationID)).ToList();

        //        ActualEmployeesBasedOnAssignings.RemoveAll(x => !EmployeesQualificationsOfActualEmployeesList.Any(y => y.EmployeeCode.EmployeeCodeID == x.EmployeeCodeID));

        //        var query = ActualEmployeesBasedOnAssignings.GroupBy(x => x.RankCategoryName)
        //                                                    .Select(y => new
        //                                                    {
        //                                                        RankCategoryName = y.Key,
        //                                                        RecordCount = y.Count()
        //                                                    }).ToList();

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

        public IQueryable<EmployeesQualificationBasedOnAssigngingsDTO> GetQualificationsBasedOnAssigningsAsRanksCategoriesDetails(int QualificationDegreeID, int QualificationID, int GeneralSpecializationID, int OrganizationID)
        {
            try
            {
                List<int> OrganizationIDsList = new OrganizationsStructuresBLL().GetByOrganizationIDsWithhAllChilds(OrganizationID);

                // Get actual employees Based On Assignings by date
                List<vwActualEmployeesBasedOnAssignings> ActualEmployeesBasedOnAssignings = new AssigningsDAL().GetActualEmployeeBasedOnAssignings().Where(x => OrganizationIDsList.Contains(x.OrganizationID.Value)).ToList();

                List<int> EmployeesCodesIDs = new List<int>();
                ActualEmployeesBasedOnAssignings.ForEach(x => EmployeesCodesIDs.Add(x.EmployeeCodeID));

                List<EmployeesQualificationsBLL> EmployeesQualificationsList = new EmployeesQualificationsBLL().GetEmployeesQualifications(EmployeesCodesIDs);

                List<EmployeesQualificationsBLL> EmployeesQualificationsOfActualEmployeesList = new List<EmployeesQualificationsBLL>();

                ActualEmployeesBasedOnAssignings.ForEach(x => EmployeesQualificationsOfActualEmployeesList.Add(new EmployeesQualificationsBLL().GetLastEmployeeQualification(EmployeesQualificationsList, x.EmployeeCodeID)));

                EmployeesQualificationsOfActualEmployeesList = EmployeesQualificationsOfActualEmployeesList.Where(x => x.QualificationDegree.QualificationDegreeID == QualificationDegreeID
                                                                                                                    && (QualificationID != 0 ? x.Qualification.QualificationID == QualificationID : x.Qualification.QualificationID == x.Qualification.QualificationID)
                                                                                                                    && (GeneralSpecializationID != 0 ? x.GeneralSpecialization.GeneralSpecializationID == GeneralSpecializationID : x.GeneralSpecialization.GeneralSpecializationID == x.GeneralSpecialization.GeneralSpecializationID)).ToList();

                ActualEmployeesBasedOnAssignings.RemoveAll(x => !EmployeesQualificationsOfActualEmployeesList.Any(y => y.EmployeeCode.EmployeeCodeID == x.EmployeeCodeID));

                var query = ActualEmployeesBasedOnAssignings.Select(y => new EmployeesQualificationBasedOnAssigngingsDTO(y.EmployeeCodeNo,
                                                                            y.EmployeeNameAr,
                                                                            y.OrganizationName,
                                                                            y.JobName,
                                                                            y.RankCategoryName,
                                                                            y.RankName,
                                                                            EmployeesQualificationsOfActualEmployeesList.FirstOrDefault(x => x.EmployeeCode.EmployeeCodeID == y.EmployeeCodeID).QualificationDegree.QualificationDegreeName,
                                                                            EmployeesQualificationsOfActualEmployeesList.FirstOrDefault(x => x.EmployeeCode.EmployeeCodeID == y.EmployeeCodeID).Qualification.QualificationName,
                                                                            EmployeesQualificationsOfActualEmployeesList.FirstOrDefault(x => x.EmployeeCode.EmployeeCodeID == y.EmployeeCodeID).GeneralSpecialization.GeneralSpecializationName,
                                                                            y.Sorting
                                                                            ));

                return query.AsQueryable();
            }
            catch
            {
                throw;
            }
        }
    }
}