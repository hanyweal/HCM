using HCM.Classes.CustomAttributes;
using HCMBLL;
using HCMBLL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class PassengerOrdersViewModelOld : BaseViewModel, IValidatableObject
    {
        public int PassengerOrderID { get; set; }

        [CustomDisplay("PassengerOrderTypeText")]
        public PassengerOrdersTypesBLL PassengerOrderType { get; set; }

        [CustomDisplay("PassengerOrderTypeText")]
        public int PassengerOrderTypeID { get; set; }

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


        public DelegationsDetailsViewModel DelegationDetailRequest { get; set; }

        public InternshipScholarshipsDetailsViewModel InternshipScholarshipDetailRequest { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public PassengerOrdersItinerariesBLL PassengerOrdersItineraryRequest { get; set; }

        public List<PassengerOrdersItinerariesBLL> PassengerOrdersItineraries { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.PassengerOrderTypeID == Convert.ToInt32(PassengerOrdersTypesEnum.Delegation))
            {
                if (DelegationDetailRequest.DelegationDetailID == 0)
                    yield return new ValidationResult(Resources.Globalization.RequiredDelegationDetailsText);
            }
            else if (this.PassengerOrderTypeID == Convert.ToInt32(PassengerOrdersTypesEnum.InternshipScholarship))
            {
                if (InternshipScholarshipDetailRequest.InternshipScholarshipDetailID == 0)
                    yield return new ValidationResult(Resources.Globalization.RequiredInternshipScholarshipDetailsText);
            }
        }
    }
}