using System;

public class Service
{
    public int Id { get; set; }
    public int DeviceId { get; set; }
    public int TechnicianId { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; }
    public DateTime ServiceDate { get; set; }

    public Device Device { get; set; }
    public Technician Technician { get; set; }
}