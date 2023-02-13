using Microsoft.AspNetCore.Mvc.Rendering;

namespace TopList.ViewModels
{
    public class CompanysByCategory
    {
        public long CategoryId { get; set; }

        public long? ParentCategorId { get; set; }

        public string CategoryName { get; set; }

        public string CategorySlug { get; set; }

        public string CategoryMetaTitle { get; set; }

        public string CategoryMetaKeywords { get; set; }

        public string CategoryMetaDescription { get; set; }

        public int TotalCompany { get; set; }

        public IList<CompanyThumbnail> Companys { get; set; } = new List<CompanyThumbnail>();

        public FilterOption FilterOption { get; set; }

        public SearchOption CurrentSearchOption { get; set; }

      
    }
}
