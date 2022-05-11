namespace HelpARefugee.Models
{
    public class RequestForDonations
    {
        public int donationRequestId { get; set; }

        public int volunteerId { get; set; }

        public int requestStatus { get; set; }

        public string resourceType { get; set; }

        public string quantityNeeded { get; set; }

        public string shortDescription { get; set; }

        public string emissionDate { get; set; }

        public string processingDate { get; set; }

        public string completionDate { get; set; }
    }
}
