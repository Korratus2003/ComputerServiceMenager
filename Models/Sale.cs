using System;

public class Sale
{
    public int Id { get; set; }
    public int InventoryDeviceId { get; set; }
    public int DeviceId { get; set; }
    public decimal Price { get; set; }
    public DateTime SellDate { get; set; }

    public InventoryDevice InventoryDevice { get; set; }
    public Device Device { get; set; }
}