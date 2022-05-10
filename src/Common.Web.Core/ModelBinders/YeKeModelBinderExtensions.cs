using System;
using Microsoft.AspNetCore.Mvc;

namespace Common.Web.Core
{
    /// <summary>
    /// Ye Ke Model Binder Extensions
    /// </summary>
    public static class YeKeModelBinderExtensions
    {
        /// <summary>
        /// Inserts YeKeModelBinderProvider at the top of the MvcOptions.ModelBinderProviders list.
        /// </summary>
        public static MvcOptions UseYeKeModelBinder(this MvcOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.ModelBinderProviders.Insert(0, new YeKeModelBinderProvider());
            return options;
        }
    }
}