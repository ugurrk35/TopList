using System.ComponentModel.DataAnnotations;
using TopList.Entity.Base;

namespace TopList.Entity.EntityModels
{
    public class Category : EntityBase
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(450)]
        public string Slug { get; set; }

        [StringLength(450)]
        public string MetaTitle { get; set; }

        [StringLength(450)]
        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsPublished { get; set; }

        public bool IncludeInMenu { get; set; }

        public bool IsDeleted { get; set; }

        public long? ParentId { get; set; }

        public Category Parent { get; set; }

        public IList<Category> Children { get; protected set; } = new List<Category>();
        public long? ThumbnailImageId { get; set; }
        public Media ThumbnailImage { get; set; }
    }
}