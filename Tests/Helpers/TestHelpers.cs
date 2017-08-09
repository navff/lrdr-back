using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace Tests.Helpers
{
    public static class TestHelpers
    {
        public static void ValidateModel(this ApiController controller, object viewModel)
        {
            controller.ModelState.Clear();
            
            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResult = new List<ValidationResult>();
            Validator.TryValidateObject(viewModel, validationContext, validationResult, true);

            foreach (var result in validationResult)
            {
                foreach (var name in result.MemberNames)
                {
                    controller.ModelState.AddModelError(name, result.ErrorMessage);
                }
            }
        }
    }
}
