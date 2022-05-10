﻿using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Web.Core
{
    /// <summary>
    /// Sets the CultureInfo.CurrentCulture and CultureInfo.CurrentUICulture to the specified culture.
    /// </summary>
    public sealed class CustomLanguageActionFilterAttribute : ActionFilterAttribute
    {
        private readonly string _culture;

        /// <summary>
        /// Initializes a new instance of the CultureInfo class based on the culture specified by name.
        /// </summary>
        public string Culture { get; }

        /// <summary>
        /// Sets the CultureInfo.CurrentCulture and CultureInfo.CurrentUICulture to the specified culture.
        /// </summary>
        public CustomLanguageActionFilterAttribute(string culture)
        {
            _culture = culture ?? throw new ArgumentNullException(nameof(culture));
            Culture = culture;
        }

        /// <summary>
        /// Sets the CultureInfo.CurrentCulture and CultureInfo.CurrentUICulture to the specified culture.
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            CultureInfo.CurrentCulture = new CultureInfo(_culture);
            CultureInfo.CurrentUICulture = new CultureInfo(_culture);
            base.OnActionExecuting(context);
        }
    }
}