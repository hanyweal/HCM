using HCM.Classes.CustomAttributes;
using HCMBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class PassengerOrdersViewModel : BaseViewModel
    {
        public int PassengerOrderID { get; set; }

        [DataType(DataType.Date)]
        [CustomDisplay("TravellingDateText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public DateTime TravellingDate { get; set; }

        [CustomDisplay("RankNameText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int RankID { get; set; }

        [CustomDisplay("RankNameText")]
        public string RankName { get; set; }

        [CustomDisplay("TicketClassText")]
        public List<TicketsClassesBLL> TicketsClasses { get; set; }

        [CustomDisplay("TicketClassText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int TicketClassID { get; set; }

        public string TicketClassName { get; set; }

        [CustomDisplay("GoingText")]
        public bool Going { get; set; }

        [CustomDisplay("ReturningText")]
        public bool Returning { get; set; }

        [CustomDisplay("ReasonText")]
        public string Reason { get; set; }

        [CustomDisplay("EmployeeCareerHistoryIDText")]
        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public int EmployeeCareerHistoryID { get; set; }

        public EmployeesViewModel Employee { get; set; }

        //public DelegationsDetailsViewModel DelegationDetailRequest { get; set; }

        //public InternshipScholarshipsDetailsViewModel InternshipScholarshipDetailRequest { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public PassengerOrdersItinerariesBLL PassengerOrdersItineraryRequest { get; set; }

        public List<PassengerOrdersItinerariesBLL> PassengerOrdersItineraries { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public PassengerOrdersEscortsViewModel PassengerOrdersEscortRequest { get; set; }

        public List<PassengerOrdersEscortsBLL> PassengerOrdersEscorts { get; set; }

    }
}