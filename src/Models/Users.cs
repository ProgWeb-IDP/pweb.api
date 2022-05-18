namespace HelpARefugee.Models
{
    public class Users
    {
        /*public Users(string authToken, string firstName, string lastName, string birthDate, string gender, string phoneNumber, string country, string city, string street, string zipCode, int isVolunteer = 0, int isAdmin = 0, string registerDate = "unknown")
        {
            this.authToken = authToken;
            this.isVolunteer = isVolunteer;
            this.isAdmin = isAdmin;
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
            this.gender = gender;
            this.phoneNumber = phoneNumber;
            this.country = country;
            this.city = city;
            this.street = street;
            this.zipCode = zipCode;
            this.registerDate = registerDate;
        }*/
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
