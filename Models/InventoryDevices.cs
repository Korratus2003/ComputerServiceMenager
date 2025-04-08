using System.Collections.Generic;

public class InventoryDevice
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public decimal DefaultPrice { get; set; }
    

    public List<Sale> Sales { get; set; }
}