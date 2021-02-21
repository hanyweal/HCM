using HCM.Classes.CustomAttributes;
using HCMBLL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class JobsCategoriesViewModel : BaseViewModel
    {
        public int? JobCategoryID { get; set; }

        [CustomDisplay("JobCategoryNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string JobCategoryName { get; set; }

        [CustomDisplay("JobCategoryNoText")]
        [Range(1, 99999, ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "ValidationRangeText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public string JobCategoryNo { get; set; }

        #region JobsGroups
        [CustomDisplay("JobGroupNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int JobGroupID { get; set; }

        public string JobGroupName { get; set; }

        public virtual JobsGroupsBLL JobGroup { get; set; }
        #endregion

        #region GeneralSpecializations
        [CustomDisplay("GeneralSpecializationNameText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        //public int GeneralSpecializationID { get; set; }

        //public string GeneralSpecializationName { get; set; }

        public GeneralSpecializationsBLL GeneralSpecialization { get; set; }
        #endregion

        #region Qualifications
        [CustomDisplay("QualificationNameText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        //public int QualificationID { get; set; }

        //public string QualificationName { get; set; }

        public QualificationsBLL Qualification { get; set; }
        #endregion

        #region QualificationsDegrees
        [CustomDisplay("QualificationDegreeNameText")]
        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        //public int QualificationDegreeID { get; set; }

        //public string QualificationDegreeName { get; set; }

        public QualificationsDegreesBLL QualificationDegree { get; set; }
        #endregion

        #region JobGeneralGroup
        [CustomDisplay("JobGeneralGroupNameText")]
        public JobsGeneralGroupsBLL JobGeneralGroup { get; set; }
        #endregion

        #region JobCategoryQualification
        public JobsCategoriesQualificationsBLL JobCategoryQualification { get; set; }

        public List<JobsCategoriesQualificationsBLL> JobsCategoriesQualifications { get; set; }
        #endregion

        #region PromotionJobCategory
        [CustomDisplay("PromotionRecordPeriodNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int? PromotionPeriodID { get; set; }
        [CustomDisplay("JobCategoryPromotionJobCategoryText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int? AssignedJobCategoryJobCategoryID { get; set; }
        public PromotionsJobsCategoriesBLL PromotionJobCategory { get; set; }

        public List<PromotionsJobsCategoriesBLL> PromotionsJobsCategories { get; set; }
        #endregion

        public string PartialName { get; set; }
    }
}