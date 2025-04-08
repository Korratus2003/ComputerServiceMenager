public class User
{
    public int Id { get; set; }
    public int TechnicianId { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }
    public string Login { get; set; }
    
    public Technician Technician { get; set; }
}