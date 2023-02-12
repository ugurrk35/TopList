using TopList.Entity.Base;

namespace TopList.Entity.EntityModels
{
    public class CompanyMedia : EntityBase
    {
        public long CompanyId { get; set; }

        public Company Company { get; set; }

        public long MediaId { get; set; }

        public Media Media { get; set; }

    }
}
