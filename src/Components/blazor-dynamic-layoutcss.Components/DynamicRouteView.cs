using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Reflection;

namespace blazor_dynamic_layoutcss.Components
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
                    //string pageLayoutTypeName = "Client.Shared.MainLayoutDynamic";
                    //Console.WriteLine($"Using pageLayoutTypeName '{pageLayoutTypeName}'");
                    switch (layoutParam.Value)
                    {
                        case 1:
                            //pageLayoutType = typeof(blazor_dynamic_layoutcss.Components.SecondLayout); //  Type.GetType(pageLayoutTypeName);
                            break;

                        case 2:
                            //pageLayoutType = typeof(Client.Shared.NestedLayout); //  Type.GetType(pageLayoutTypeName);
                            //Console.WriteLine("The full Type name is {0}.\n", pageLayoutType.FullName);
                            //pageLayoutType = Type.GetType("MyFirstBlazor.Client.Shared.NestedLayout, MyFirstBlazor.Client");
                            pageLayoutType = Type.GetType("MyFirstBlazor.Components.FooLayout, MyFirstBlazor.Components");

                            break;

                        default:
                            break;
                    }

                    switch (cssParam.Value)
                    {
                        case 1:
                            selectedCSS = "css/artist1.css";
                            break;

                        case 2:
                            selectedCSS = "css/artist2.css";
                            break;
                    }

                    Console.WriteLine($"pageLayoutTypeName type: '{pageLayoutType.Name}'");
                    Console.WriteLine("The full Type name is {0}.\n", pageLayoutType.FullName);
                }

                if (cssParam.HasValue)
                {
                    Console.WriteLine($"CSS param = '{cssParam}'");
                }

                //Type myType1 = Type.GetType("Client.Shared.MainLayoutDynamic, MyFirstBlazor.Client");
                //Console.WriteLine("The full name is {0}.\n", myType1.FullName);
                //pageLayoutType = myType1;
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