using HCMBLL;
using System;

namespace HCM.Models
{
    public class PromotionsRecordsQualificationPointsViewModel : BaseViewModel
    {
        public int PromotionRecordQualificationPointID { get; set; }
        public PromotionsRecordsBLL PromotionRecord { get; set; }
        public QualificationsDegreesBLL QualificationDegree { get; set; }
        public QualificationsBLL Qualification { get; set; }
        public GeneralSpecializationsBLL GeneralSpecialization { get; set; }
        public Nullable<decimal> Points { get; set; }
        public PromotionsRecordsQualificationsKindsBLL PromotionRecordQualificationKind { get; set; }
        //public List<PromotionsRecordsQualificationsKindsBLL> PromotionRecordQualificationKindList { get; set; }
    }
}