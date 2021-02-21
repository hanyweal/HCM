//using HCM.Classes.CustomAttributes;
//using HCMBLL;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Configuration;
//using System.Linq;
//using System.Web;

//namespace HCM.Models
//{
//    public class JobsCategoriesQualificationsViewModel : BaseViewModel
//    {
//        public int? JobCategoryQualificationID { get; set; }

//        #region JobsCategories
//        [CustomDisplay("JobGroupNameText")]
//        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
//        public virtual JobsCategoriesBLL JobCategory { get; set; }
//        #endregion

//        #region GeneralSpecializations
//        [CustomDisplay("GeneralSpecializationNameText")]
//        public GeneralSpecializationsBLL GeneralSpecialization { get; set; }
//        #endregion

//        #region Qualifications
//        [CustomDisplay("QualificationNameText")]
//        public QualificationsBLL Qualification { get; set; }
//        #endregion

//        #region QualificationsDegrees
//        [CustomDisplay("QualificationDegreeNameText")]
//        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
//        public QualificationsDegreesBLL QualificationDegree { get; set; }
//        #endregion
//    }
//}