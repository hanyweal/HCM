using HCM.Classes.CustomAttributes;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HCM.Models
{
    public class EVacationsRequestsViewModel
    {
        public int EVacationRequestID { get; set; }

        public int EmployeeCodeID { get; set; }

        [CustomDisplay("EmployeeCodeNoText")]
        public string EmployeeCodeNo { get; set; }

        [CustomDisplay("EmployeeNameArText")]
        public string EmployeeNameAr { get; set; }

        public int OrganizationID { get; set; }

        [CustomDisplay("OrganizationNameText")]
        public string OrganizationName { get; set; }

        [CustomDisplay("RequestNoText")]
        public int EVacationRequestNo { get; set; }

        [CustomDisplay("VacationTypeText")]
        public string VacationType { get; set; }

        //[DataType(DataType.Date)]
        [CustomDisplay("VacationStartDateText")]
        public string VacationStartDate { get; set; }

        [CustomDisplay("VacationPeriodText")]
        public int VacationPeriod { get; set; }

        //[DataType(DataType.Date)]
        [CustomDisplay("VacationEndDateText")]
        public string VacationEndDate { get; set; }

        //[DataType(DataType.Date)]
        [CustomDisplay("WorkDateText")]
        public string WorkDate { get; set; }

        [CustomDisplay("NotesText")]
        public string CreatorNotes { get; set; }

        [CustomDisplay("RequestCreatedDateText")]
        public DateTime CreatedDate { get; set; }

        [CustomDisplay("AuthorizedPersonNameArText")]
        public string AuthorizedPersonNameAr { get; set; }

        [CustomDisplay("AuthorizedPersonCodeNoText")]
        public string AuthorizedPersonCodeNo { get; set; }

        [CustomDisplay("AuthorizedPersonDecisionText")]
        public string AuthorizedPersonDecision { get; set; }

        [CustomDisplay("AuthorizedPersonDecisionDateTimeText")]
        public DateTime? AuthorizedPersonDecisionDateTime { get; set; }

        [CustomDisplay("AuthorizedPersonNotesText")]
        public string AuthorizedPersonNotes { get; set; }

        [CustomDisplay("RequestStatusText")]
        public string EVacationRequestStatusName { get; set; }

        public EVacationRequestStatusEnum EVacationRequestStatusEnum { get; set; }

        //public EVacationRequestStatusBLL EVacationRequestStatus { get; set; }

        [CustomDisplay("LastUpdatedDateText")]
        public DateTime? LastUpdatedDate { get; set; }

        [CustomDisplay("LastUpdatedByText")]
        public string LastUpdatedBy { get; set; }

        [CustomDisplay("EVacationRequestCancellationReasonByHRText")]
        public string CancellationReasonByHR { get; set; }

    }
}