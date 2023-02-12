using System.ComponentModel.DataAnnotations;
using TopList.Entity.Base;

namespace TopList.Entity.EntityModels
{
    public abstract class CompanyInfo : EntityBase
    {
        private bool isDeleted;

        protected CompanyInfo()
        {
            CreatedOn = DateTimeOffset.Now;
            LatestUpdatedOn = DateTimeOffset.Now;
        }

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

        public bool IsPublished { get; set; }

        public DateTimeOffset? PublishedOn { get; set; }

        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                if (value)
                {
                    IsPublished = false;
                }
            }
        }


        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset LatestUpdatedOn { get; set; }

        // ileride user eklenicek
        

        //public long CreatedById { get; set; }

        //public User CreatedBy { get; set; }

        //public long LatestUpdatedById { get; set; }

        //public User LatestUpdatedBy { get; set; }
    }
}
