namespace HelpARefugee.Models
{
    public class Users
    {
        public int userId { get; set; }

        public string authToken { get; set; }

        public int isVolunteer { get; set; }

        public int isAdmin { get; set; }

        public string firstName { get; set; } 

        public string lastName { get; set; } 

        public string birthDate { get; set; }

        public string gender { get; set; }

        public string phoneNumber { get; set; }

        public string country { get; set; }

        public string city { get; set; }

        public string street { get; set; } 

        public string address { get; set; }

        public string zipCode { get; set; }

        public string registerDate { get; set; }
    }
}
