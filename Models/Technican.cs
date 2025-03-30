using System;
using System.Collections.Generic;

public class Technician
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime EmploymentDate { get; set; }

    public List<Service> Services { get; set; }
    public User User { get; set; }
}