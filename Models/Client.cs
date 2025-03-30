using System.Collections.Generic;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public bool VisitsRegularly { get; set; }
    
    public List<Device> Devices { get; set; }
}