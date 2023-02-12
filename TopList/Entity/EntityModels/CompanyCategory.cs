using TopList.Entity.Base;

namespace TopList.Entity.EntityModels
{
    public class CompanyCategory : EntityBase
    {
        public bool IsFeaturedCompany { get; set; }

        public long CategoryId { get; set; }

        public long CompanyId { get; set; }

        public Category Category { get; set; }

        public Company Company { get; set; }
    }
}
