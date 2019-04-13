using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;

namespace NopLocalization
{
    public class CultureRouteConstraint : IRouteConstraint
    {
        public string RouteDataStringKey { get; }
        public IEnumerable<string> SupportedCultures { get; }

        public CultureRouteConstraint(IEnumerable<string> supportedCultures, string routeDataStringKey = "culture")
        {
            SupportedCultures = supportedCultures;
            RouteDataStringKey = routeDataStringKey;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(RouteDataStringKey))
                return false;

            var culture = values[RouteDataStringKey].ToString();
            return SupportedCultures.Any(p => p.Equals(culture, StringComparison.OrdinalIgnoreCase));
        }
    }
}
