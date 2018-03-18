using System.ComponentModel.DataAnnotations;

namespace CountriesListApi.Models.Entities
{
    public class Country{
        [Key]
        public string CountryId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}