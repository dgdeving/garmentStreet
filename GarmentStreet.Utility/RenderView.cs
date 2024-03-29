﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentStreet.Utility
{
    public class RenderView
    {
        public static async Task<string> RenderViewToStringAsync(string viewName, object model, ControllerContext controllerContext, string location, bool isPartial = false)
        {
            var actionContext = controllerContext as ActionContext;

            var serviceProvider = controllerContext.HttpContext.RequestServices;
            var razorViewEngine = serviceProvider.GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;
            var tempDataProvider = serviceProvider.GetService(typeof(ITempDataProvider)) as ITempDataProvider;

            using (var sw = new StringWriter())
            {
                var viewResult = razorViewEngine.GetView(location, viewName, !isPartial);

                if (viewResult?.View == null)
                    throw new ArgumentException($"{viewName} does not match any available view");

                var viewDictionary =
                    new ViewDataDictionary(new EmptyModelMetadataProvider(),
                        new ModelStateDictionary())
                    { Model = model };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }

        }

        public static async Task<string> RenderPageToStringAsync(string pageName, object model, ControllerContext controllerContext, bool isPartial = false)
        {
            var actionContext = controllerContext as ActionContext;

            var serviceProvider = controllerContext.HttpContext.RequestServices;
            var razorViewEngine = serviceProvider.GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;
            var tempDataProvider = serviceProvider.GetService(typeof(ITempDataProvider)) as ITempDataProvider;

            using (var sw = new StringWriter())
            {
                var viewResult = razorViewEngine.FindView(actionContext, pageName, !isPartial);

                if (viewResult?.View == null)
                    throw new ArgumentException($"{pageName} does not match any available view");

                var viewDictionary =
                    new ViewDataDictionary(new EmptyModelMetadataProvider(),
                        new ModelStateDictionary())
                    { Model = model };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }

        }

    }
}
