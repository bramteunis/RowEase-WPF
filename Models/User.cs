using System.Data;

namespace Models
{
    public class User
    {
        public int? id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public int? roleId { get; set; }
        public Certificate certificate { get; set; }
        public string? residence { get; set; }
        public string? address { get; set; }
        public Role role { get; set; }
        public string? accountCreated { get; set; }
        public string? memberSince { get; set; }

        public User(int id, string firstName, string lastName, string email, Role role, Certificate certificate, string residence, string address, string accountCreated, string? memberSince)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            roleId = role!.id;
            this.certificate = certificate;
            this.residence = residence;
            this.address = address;
            this.role = role;
            this.accountCreated = accountCreated;
            this.memberSince = memberSince;
        }
    }
}