using System;
using System.Threading.Tasks;
using PersianUtils.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Web.Core
{
    /// <summary>
    /// Persian Ye Ke Model Binder
    /// </summary>
    public class YeKeModelBinder : IModelBinder
    {

        /// <summary>
        /// Attempts to bind a model.
        /// </summary>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var logger = bindingContext.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
            var fallbackBinder = new SimpleTypeModelBinder(bindingContext.ModelType, logger);

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return fallbackBinder.BindModelAsync(bindingContext);
            }
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var valueAsString = valueProviderResult.FirstValue;
            if (string.IsNullOrWhiteSpace(valueAsString))
            {
                return fallbackBinder.BindModelAsync(bindingContext);
            }

            var model = valueAsString.ApplyCorrectYeKe();
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}