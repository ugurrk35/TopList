using TopList.Entity.EntityModels;

namespace TopList.ViewModels
{
    public class CompanyThumbnail
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }
        public Media ThumbnailImage { get; set; }

        public string ThumbnailUrl { get; set; }

        public static CompanyThumbnail FromCompany(Company company)
        {
            var companyThumbnail = new CompanyThumbnail
            {
                Id = company.Id,
                Name = company.Name,
                Slug = company.Slug,
                ThumbnailImage = company.ThumbnailImage,
            };

            return companyThumbnail;
        }
    }
}
