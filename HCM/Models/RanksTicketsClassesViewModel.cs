using HCM.Classes.CustomAttributes;
using HCMBLL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HCM.Models
{
    public class RanksTicketsClassesViewModel //: IValidatableObject
    {
        public int RankTicketClassID { get; set; }

        [CustomDisplay("RankNameText")]
        public List<RanksBLL> Ranks { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public RanksBLL Rank { get; set; }

        [CustomDisplay("TicketClassText")]
        public List<TicketsClassesBLL> TicketsClasses { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Globalization), ErrorMessageResourceName = "RequiredFieldText")]
        public TicketsClassesBLL TicketClass { get; set; }
    }
}