using System;
using Dash.NET.CSharp.Giraffe;
using Dash.NET.CSharp.DCC;
using static Dash.NET.CSharp.Dsl;
using Microsoft.Extensions.Logging;
using Dash.NET.CSharp;
using System.Linq;

namespace Documentation.Examples
{
    class Dropdown_DropdownDynamicOptions
    {
        public static void RunExample()
        {
            var options = new DropdownOption<string, string>[]
            {
                DropdownOption.Init<string, string>(label: "New York City", value: "NYC", disabled: false, title: "New York City"),
                DropdownOption.Init<string, string>(label: "Montreal", value: "MTL", disabled: false, title: "Montreal"),
                DropdownOption.Init<string, string>(label: "San Francisco", value: "SF", disabled: false, title: "San Francisco")
            };

            var layout =
                Html.div(
                    Attr.children(
                        Html.div(
                            Attr.children(
                                Html.text("Single dynamic Dropdown"),
                                Dropdown.dropdown(
                                    id: "my-dynamic-dropdown",
                                    Dropdown.Attr.searchable(true),
                                    Dropdown.Attr.searchValue(""),
                                    Dropdown.Attr.options<string, string>()
                                )
                            )
                        ),
                        Html.div(
                            Attr.children(
                                Html.text("Multi dynamic Dropdown"),
                                Dropdown.dropdown(
                                    id: "my-multi-dynamic-dropdown",
                                    Dropdown.Attr.searchable(true),
                                    Dropdown.Attr.searchValue(""),
                                    Dropdown.Attr.options<string, string>(),
                                    Dropdown.Attr.value(""),
                                    Dropdown.Attr.multi(true)
                                )
                            )
                        )
                    )                    
                );

            var updateOptionsCallback =
                Callback.Create(
                    input: new[]
                    {
                        ("my-dynamic-dropdown", ComponentProperty.CustomProperty("search_value"))
                    },
                    output: new[]
                    {
                        ("my-dynamic-dropdown", ComponentProperty.CustomProperty("options"))
                    },
                    handler: (string sval) =>
                    {
                        return new[]
                        {
                            CallbackResult.Create(("my-dynamic-dropdown", ComponentProperty.CustomProperty("options")), options.Where(x => x.label.Contains(sval) && sval != ""))
                        };
                    },
                    preventInitialCall: false
                );

            var updateMultiOptionsCallback =
                Callback.Create(
                    input: new[]
                    {
                        ("my-multi-dynamic-dropdown", ComponentProperty.CustomProperty("search_value")),
                        ("my-multi-dynamic-dropdown", ComponentProperty.CustomProperty("value"))

                    },
                    output: new[]
                    {
                        ("my-multi-dynamic-dropdown", ComponentProperty.CustomProperty("options"))
                    },
                    handler: (string sval, string[] dval) =>
                    {
                        var optionsResult = options.Where(x => (x.label.Contains(sval) && sval != "") || (dval != null && dval.Contains(x.value)));
                        return new[]
                        {
                            CallbackResult.Create(("my-multi-dynamic-dropdown", ComponentProperty.CustomProperty("options")), optionsResult)
                        };
                    },
                    preventInitialCall: false
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout)
                .addCallback(updateOptionsCallback)
                .addCallback(updateMultiOptionsCallback);

            var config = new DashGiraffeConfig(
                hostName: "localhost",
                logLevel: LogLevel.Information,
                ipAddress: "127.0.0.1",
                port: 8050,
                errorHandler: (Exception err) => err.Message
            );

            dashApp.run(
                args: new string[] { },
                config
            );
        }
    }
}
