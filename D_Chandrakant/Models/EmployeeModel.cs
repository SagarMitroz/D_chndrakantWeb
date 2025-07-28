namespace D_Chandrakant.Models
{
    public class EmployeeModel
    {
        public EmployeeModel() {
            Employees = new List<EmployeeModel>();
        }
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int RoleIdfk { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Mobile1 { get; set; }

        public string Mobile2 { get; set; }

        public string Email { get; set; }

        public DateTime Bdate { get; set; }

        public DateTime Adate { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }


        public List<EmployeeModel> Employees { get; set; }

    }
}
