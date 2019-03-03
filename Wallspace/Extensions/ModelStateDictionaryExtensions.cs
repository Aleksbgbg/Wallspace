namespace Wallspace.Extensions
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public static class ModelStateDictionaryExtensions
    {
        public static void AddIdentityErrors(this ModelStateDictionary modelState, IdentityResult identityResult)
        {
            modelState.AddIdentityErrors(identityResult.Errors);
        }

        public static void AddIdentityErrors(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
        {
            foreach (IdentityError error in errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}