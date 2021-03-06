﻿using VehicleCatalog.Service.Models;
using X.PagedList;

namespace VehicleCatalog.Models.ModelView
{
    public class ModelIndexModel
    {
        public IPagedList<Model> ModelList { get; set; }
        public int MakeId { get; set; }
        public string Name { get; set; }
        public string SortStatus { get; set; }
        public string SearchString { get; set; }
        public IPagination Pagination { get; set; }
    }
}
