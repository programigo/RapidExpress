using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RapidExpress.Services.Implementations
{
	public class TemplateHelperService : ITemplateHelperService
	{
		private IRazorViewEngine razorViewEngine;
		private IServiceProvider serviceProvider;
		private ITempDataProvider tempDataProvider;

		public TemplateHelperService(
			IRazorViewEngine engine,
			IServiceProvider serviceProvider,
			ITempDataProvider tempDataProvider)
		{
			this.razorViewEngine = engine;
			this.serviceProvider = serviceProvider;
			this.tempDataProvider = tempDataProvider;
		}

		public async Task<string> GetTemplateHtmlAsString(string viewName)
		{
			var httpContext = new DefaultHttpContext()
			{
				RequestServices = this.serviceProvider
			};
			var actionContext = new ActionContext(
					httpContext, new RouteData(), new ActionDescriptor());

			using (StringWriter sw = new StringWriter())
			{
				var viewResult = this.razorViewEngine.FindView(
						actionContext, viewName, false);

				if (viewResult.View == null)
				{
					return string.Empty;
				}

				var viewDataDictionary = new ViewDataDictionary(
					new EmptyModelMetadataProvider(),
					new ModelStateDictionary()
				);

				var viewContext = new ViewContext(
					actionContext,
					viewResult.View,
					viewDataDictionary,
					new TempDataDictionary(actionContext.HttpContext, this.tempDataProvider),
					sw,
					new HtmlHelperOptions()
				);

				await viewResult.View.RenderAsync(viewContext);

				return sw.ToString();
			}
		}
	}
}
