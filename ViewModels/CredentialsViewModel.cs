using CountriesListApi.ViewModels.Validations;
using FluentValidation.Attributes;

namespace CountriesListApi.ViewModels
{
    [Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}