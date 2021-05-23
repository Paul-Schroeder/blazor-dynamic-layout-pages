using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Reflection;

namespace LayoutSwapper.Components
{
    public class DynamicRouteView : RouteView
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Renders the component.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/>.</param>
        protected override void Render(RenderTreeBuilder builder)
        {
            var pageLayoutType = RouteData.PageType.GetCustomAttribute<LayoutAttribute>()?.LayoutType
                ?? DefaultLayout;
            string selectedCSS = string.Empty;

            if (RouteData.PageType.GetCustomAttribute<DynamicLayoutAttribute>() != null)
            {
                int? layoutParam = null;
                int? cssParam = null;

                var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
                Console.WriteLine($"AbsoluteUri: '{uri}'");

                if (Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("l", out var param1))
                {
                    layoutParam = Int32.Parse(param1);
                }

                if (Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("c", out var param2))
                {
                    cssParam = Int32.Parse(param2);
                }

                if (layoutParam.HasValue)
                {
                    Console.WriteLine($"Layout param = '{layoutParam}'");
                    switch (layoutParam.Value)
                    {
                        case 1:
                            pageLayoutType = typeof(Pages.AlternateLayout);
                            break;

                        case 2:
                            pageLayoutType = Type.GetType("blazor_dynamic_layoutcss.Shared.MainLayout, blazor-dynamic-layoutcss");
                            break;
                    }

                    switch (cssParam.Value)
                    {
                        case 1:
                            selectedCSS = "_content/LayoutSwapper/css/sample1.css";
                            break;

                        case 2:
                            selectedCSS = "_content/LayoutSwapper/css/sample2.css";
                            break;
                    }

                    Console.WriteLine($"pageLayoutTypeName type: '{pageLayoutType.FullName}'");
                    Console.WriteLine($"selectedCSS: '{selectedCSS}'");
                }

                if (cssParam.HasValue)
                {
                    Console.WriteLine($"CSS param = '{cssParam}'");
                }
            }

            var fiRenderPageWithParametersDelegate = typeof(RouteView)
              .GetField("_renderPageWithParametersDelegate", BindingFlags.Instance | BindingFlags.NonPublic);
            var _renderPageWithParametersDelegate = fiRenderPageWithParametersDelegate.GetValue(this);

            builder.OpenComponent<LayoutView>(0);
            builder.AddAttribute(1, nameof(LayoutView.Layout), pageLayoutType);
            builder.AddAttribute(2, nameof(LayoutView.ChildContent), _renderPageWithParametersDelegate);
            builder.CloseComponent();
            builder.AddMarkupContent(3, $"<link href=\"{selectedCSS}\" rel=\"stylesheet\">\r\n\r\n");
        }
    }
}