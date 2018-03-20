using System.Threading.Tasks;
using CountriesListApi.Data;
using CountriesListApi.Models;
using CountriesListApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using CountriesList.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;

namespace CountriesListApi.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ClaimsPrincipal _caller;
        public FavoritesController(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _caller = httpContextAccessor.HttpContext.User;
        }
        public async Task<object> Get(int? page = null, int pageSize = 10)
        {
            List<object> countriesList = new List<object>();
            var userId = _caller.Claims.Single(c => c.Type == "id");
            object countries = null;
            var favorites = _dbContext.Favorites.Where(c => c.UserId == userId.Value);


            countries = await PaginatedList<Favorites>.CreateAsync(favorites, page ?? 1, pageSize);
            foreach(var favorite in (List<Favorites>)countries)
            {
                var country = _dbContext.Country.Where(x => x.CountryId == favorite.CountryId).FirstOrDefault();
                countriesList.Add(CountryMaker.PopulateFullCountryData(country, true));
            }
            return new
            {
                countries = countriesList.ToList(),
                page = page,
                pageSize = pageSize
            };
        }

        [HttpPost("{code}")]
        public string Post(string code)
        {
            var country = _dbContext.Country.Where(x => x.ShortName.Equals(code, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            var userId = _caller.Claims.Single(c => c.Type == "id");

            if (country != null)
            {
                bool isFavorite = _dbContext.Favorites.Any(x => x.UserId.Equals(userId.Value) && x.CountryId.Equals(country.CountryId));
                if (!isFavorite)
                {
                    _dbContext.Favorites.Add(new Favorites() { UserId = userId.Value.ToString(), CountryId = country.CountryId });
                    _dbContext.SaveChanges();
                    return "Added";
                }
                else
                {
                    return "Already Added";
                }
            }
            else
            {
                return "Invalid country code";
            }
        }

        // DELETE api/values/5
        [HttpDelete("{code}")]
        public string Delete(string code)
        {
            var country = _dbContext.Country.Where(x => x.ShortName.Equals(code)).FirstOrDefault();
            var userId = _caller.Claims.Single(c => c.Type == "id");
            if (country != null)
            {
                var favorite = _dbContext.Favorites.AsNoTracking().SingleOrDefault(x => x.UserId.Equals(userId.Value) && x.CountryId.Equals(country.CountryId));
                if (favorite == null)
                {
                    return "Not set as favorite";
                }
                else
                {
                    _dbContext.Favorites.Remove(favorite);
                    _dbContext.SaveChanges();
                    return "Removed from favorites";
                }
            }
            else
            {
                return "Invalid country code";
            }
        }
    }
}