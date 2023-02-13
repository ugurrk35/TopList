using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;
using TopList.Entity.Base;

namespace TopList.Entity.EntityModels
{
    public class Media : EntityBase
    {
        [StringLength(450)]
        public string Caption { get; set; }

        public int FileSize { get; set; }

        [StringLength(450)]
        public string FileName { get; set; }
        public int MediaTypeEnum { get; set; }

        public MediaType MediaType { get; set; }
    }
}
