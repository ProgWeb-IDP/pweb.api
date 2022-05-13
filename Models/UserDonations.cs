namespace HelpARefugee.Models
{
    public class UserDonations
    {
        public int donationId { get; set; }

        public int userId { get; set; }

        public int volunteerId { get; set; }

        public int donationRequestId { get; set; }

        public int quantityDonated { get; set; }

        public string emissionDate { get; set; }

        public string collectionDate { get; set; }

        public string completionDate { get; set; }

        public int donationStatus { get; set; }

    }
}
