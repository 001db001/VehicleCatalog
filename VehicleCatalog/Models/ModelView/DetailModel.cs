﻿namespace VehicleCatalog.Models.ModelView
{
    public class DetailModel
    {
        public VehicleModelVM ModelDetail { get; set; }
        public VehicleMakeVM MakeDetail { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }
        public int Id { get; set; }
        public int MakeId { get; set; }

    }
}
