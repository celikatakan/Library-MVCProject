namespace Library_MVCProject.Models
{
    public class User
    {
        public User()
        {
            JoinDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
