using System.ComponentModel.DataAnnotations;
using System.Formats.Asn1;

namespace TopList.Entity.EntityModels
{
    public class Company : CompanyInfo
    {
        [StringLength(450)]
        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public bool IsFeatured { get; set; }


        [StringLength(450)]
        public string NormalizedName { get; set; }

        public Media ThumbnailImage { get; set; }

        public IList<CompanyMedia> Medias { get; protected set; } = new List<CompanyMedia>();

        public IList<CompanyLink> CompanyLinks { get; protected set; } = new List<CompanyLink>();

        public IList<CompanyLink> LinkedCompanyLinks { get; protected set; } = new List<CompanyLink>();

        public IList<CompanyCategory> Categories { get; protected set; } = new List<CompanyCategory>();


        public void AddCategory(CompanyCategory category)
        {
            category.Company = this;
            Categories.Add(category);
        }

        public void AddMedia(CompanyMedia media)
        {
            media.Company = this;
            Medias.Add(media);
        }




        public void AddCompanyLinks(CompanyLink CompanyLink)
        {
            CompanyLink.Company = this;
            CompanyLinks.Add(CompanyLink);
        }




        public Company Clone()
        {
            var company = new Company();
            company.Name = Name;
            company.MetaTitle = MetaTitle;
            company.MetaKeywords = MetaKeywords;
            company.MetaDescription = MetaDescription;
            company.ShortDescription = ShortDescription;
            company.Description = Description;
            company.IsPublished = IsPublished;
            
            company.NormalizedName = NormalizedName;
       
            company.Slug = Slug;

            foreach (var category in Categories)
            {
                company.AddCategory(new CompanyCategory
                {
                    CategoryId = category.CategoryId
                });
            }

            return company;
        }
    }
}