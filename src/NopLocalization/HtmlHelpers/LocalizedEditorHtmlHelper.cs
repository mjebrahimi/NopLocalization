//using Microsoft.AspNetCore.Html;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc.Routing;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Net;
//using System.Text;

//namespace NopLocalization
//{
//    //Nuget Package : Microsoft.AspNetCore.Mvc.ViewFeatures
//    public static class LocalizedEditorHtmlHelper
//    {
//        /// <summary>
//        /// Generate editor for localizable entities
//        /// </summary>
//        /// <typeparam name="T">Type</typeparam>
//        /// <typeparam name="TLocalizedModelLocal">Type</typeparam>
//        /// <param name="helper">HTML helper</param>
//        /// <param name="name">ID of control</param>
//        /// <param name="standardTemplate">Template for standard (default) values</param>
//        /// <param name="localizedTemplate">Template with localizable values</param>
//        /// <returns></returns>
//        public static IHtmlContent LocalizedEditor<TModel, TEntity, TLocalizedModel>(
//            this IHtmlHelper<ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>> helper,
//            string name,
//            Func<TModel, IHtmlContent> standardTemplate,
//            Func<int, IHtmlContent> localizedTemplate)
//            where TModel : class, ILocalizedModelForEdit<TModel, TEntity, TLocalizedModel>
//            where TEntity : class, ILocalizable
//            where TLocalizedModel : class, ILocalizedLocaleModel
//        {
//            //TODO: code review
//            using (var serviceScope = ApplicationServiceProvider.CreateServiceScope())
//            {
//                var htmlBuilder = new HtmlContentBuilder();

//                if (helper.ViewData.Model.Locales.Count > 0)
//                {
//                    var urlHelper = new UrlHelper(helper.ViewContext); //TODO: resolve using dependency injection

//                    htmlBuilder.AppendHtmlLine($"<div id=\"{name}\" class=\"nav-tabs-custom nav-tabs-localized-fields\">");
//                    htmlBuilder.AppendHtmlLine("<ul class=\"nav nav-tabs\">");

//                    //default tab
//                    htmlBuilder.AppendHtmlLine("<li class=\"active\">");
//                    htmlBuilder.AppendHtmlLine(
//                        $"<a data-tab-name=\"{name}-fa-IR-tab\" href=\"#{name}-fa-IR-tab\" data-toggle=\"tab\"><img alt=\'\' src=\'{urlHelper.Content("~/Content/flags/ir.png")}\'> فارسی</a>");
//                    htmlBuilder.AppendHtmlLine("</li>");

//                    var languageRepository = serviceScope.ServiceProvider.GetRequiredService<ILanguageRepository>();
//                    foreach (var locale in helper.ViewData.Model.Locales)
//                    {
//                        //languages
//                        var language = languageRepository.GetByIdCached(locale.LanguageId);
//                        if (language == null)
//                            throw new Exception("Language cannot be loaded");

//                        htmlBuilder.AppendHtmlLine("<li>");
//                        //var iconUrl = urlHelper.Content("~/Content/flags/" + ((List<Country>)language.Countries)[0]?.TwoLetterISORegionName + ".png");
//                        htmlBuilder.AppendHtmlLine(
//                            $"<a data-tab-name=\"{name}-{language.LanguageCode}-tab\" href=\"#{name}-{language.LanguageCode}-tab\" data-toggle=\"tab\"><img alt='' src='{null}'> {WebUtility.HtmlEncode(language.Name)}</a>");

//                        htmlBuilder.AppendHtmlLine("</li>");
//                    }
//                    htmlBuilder.AppendHtmlLine("</ul>");

//                    //default tab
//                    htmlBuilder.AppendHtmlLine("<div class=\"tab-content\">");
//                    htmlBuilder.AppendHtmlLine($"<div class=\"tab-pane fade in active\" id=\"{name}-fa-IR-tab\">");

//                    var templateHtml = standardTemplate((TModel)helper.ViewData.Model);
//                    htmlBuilder.AppendHtml(templateHtml);
//                    htmlBuilder.AppendLine();

//                    htmlBuilder.AppendHtmlLine("</div>");

//                    for (int i = 0; i < helper.ViewData.Model.Locales.Count; i++)
//                    {
//                        //languages
//                        var language = languageRepository.GetByIdCached(helper.ViewData.Model.Locales[i].LanguageId);

//                        htmlBuilder.AppendHtmlLine($"<div class=\"tab-pane fade\" id=\"{name}-{language.LanguageCode}-tab\">");

//                        var localizedHtml = localizedTemplate(i);
//                        htmlBuilder.AppendHtml(localizedHtml);
//                        htmlBuilder.AppendLine();

//                        htmlBuilder.AppendHtmlLine("</div>");
//                    }

//                    htmlBuilder.AppendHtmlLine("</div>");
//                    htmlBuilder.AppendHtmlLine("</div>");

//                    //writer.Write(new HtmlString(tabStrip.ToString()));
//                }
//                else
//                {
//                    //standardTemplate((TModel)helper.ViewData.Model).WriteTo(helper.ViewContext.Writer, HtmlEncoder.Default);

//                    var standardHtml = standardTemplate((TModel)helper.ViewData.Model);
//                    htmlBuilder.AppendHtml(standardHtml);
//                }

//                return htmlBuilder;
//            }
//        }
//    }
//}
