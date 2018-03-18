using System.ComponentModel.DataAnnotations;

namespace CountriesListApi.Models.Entities
{
    public class Favorites
    {
        [Key]
        public string FavoriteId { get; set; }
        public string CountryId { get; set; }
        public string UserId { get; set; }
    }
}