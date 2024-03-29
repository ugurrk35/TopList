﻿namespace TopList.ViewModels
{
    public class CategoryListItem
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public bool IncludeInMenu { get; set; }

        public bool IsPublished { get; set; }

        public long? ParentId { get; set; }
    }
}
