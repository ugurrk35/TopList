using TopList.Entity.Base;

namespace TopList.Entity.EntityModels
{
    public class CompanyLink : EntityBase
    {
        public long CompanyId { get; set; }

        public Company Company { get; set; }

        public long LinkedCompanyId { get; set; }

        public Company LinkedCompany { get; set; }
        public int LinkTypeEnum { get; set; }

        public CompanyLinkType LinkType { get; set; }
    }
}
