namespace HCMBLL.Enums
{
    public enum PassengerOrdersValidationEnum
    {
        Done = 1,
        RejectedBecauseAlreadyTookIt = 2,
        RejectedBecauseItineraryRequired = 3,
        RejectedBecausePassengerOrderAlreadyTook = 4,
        RejectedBecausePassengerOrderItineraryCityAlreadyExist = 5,
        RejectedBecausePassengerOrderCompensation = 6,
        RejectedBecausePassengerOrderEscortAlreadyExist=7,
        RejectedBecauseEscortRequired =8,
        RejectedBecauseMaxEscortMemberInOrdersIsFour =9,
    }
}
