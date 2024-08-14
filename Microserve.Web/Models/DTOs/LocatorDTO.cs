﻿namespace Microserve.Web.Models.DTOs
{
    public class LocatorDTO
    {
        public int LocatorId { get; set; }
        public required string Name { get; set; }
        public required string GpsCordinator { get; set; }
        public required string Location { get; set; }
        public required string State { get; set; }
        public required string LGA { get; set; } 
        public  string? Phone_number { get; set; }
        public  string? Business_status { get; set; }
        

    }
}