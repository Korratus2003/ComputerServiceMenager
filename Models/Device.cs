using System.Collections.Generic;

public class Device
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string Name { get; set; }
    
    public string Type { get; set; }
    
    public Client Client { get; set; }
    public List<Service> Services { get; set; }
    public List<Sale> Sales { get; set; }
}