using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace TopList.ViewModels
{
    public class SearchOption
    {
        public string Query { get; set; }

        public string Category { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string Sort { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var dict = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(Query))
            {
                dict.Add("query", Query);
            }

            if (!string.IsNullOrWhiteSpace(Category))
            {
                dict.Add("category", Category);
            }


            return dict;
        }

   

        public IList<string> GetCategories()
        {
            return string.IsNullOrWhiteSpace(Category) ? new List<string>() : Category.Split(new[] { "--" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public string ToJson()
        {
            var jsonSetting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            jsonSetting.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
            return JsonConvert.SerializeObject(this, jsonSetting);
        }
    }
}
