using System;
using Dash.NET.CSharp.Giraffe;
using Dash.NET.CSharp.DCC;
using static Dash.NET.CSharp.Dsl;
using Microsoft.Extensions.Logging;
using Dash.NET.CSharp;

namespace Documentation.Examples
{
    class Dropdown_Dropdown
    {
        public static void RunExample()
        {
            var layout =
                Html.div(
                    Attr.children(
                        Dropdown.dropdown(
                            id: "demo-dropdown",
                            Dropdown.Attr.options(
                                DropdownOption.Init<string, string>(label: "New York City", value: "NYC", disabled: false, title: "New York City"),
                                DropdownOption.Init<string, string>(label: "Montreal", value: "MTL", disabled: false, title: "Montreal"),
                                DropdownOption.Init<string, string>(label: "San Francisco", value: "SF", disabled: false, title: "San Francisco")
                            ),
                            Dropdown.Attr.value("NYC")
                        ),
                        Html.div(Attr.id("dd-output-container"))
                    )                    
                );

            var changeTextCallback =
                Callback.Create(
                    input: new[]
                    {
                        ("demo-dropdown", ComponentProperty.Value)
                    },
                    output: new[]
                    {
                        ("dd-output-container", ComponentProperty.Children)
                    },
                    handler: (string dval) =>
                    {
                        return new[]
                        {
                            CallbackResult.Create(("dd-output-container", ComponentProperty.Children), "You have selected " + dval)
                        };
                    },
                    preventInitialCall: false
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout)
                .addCallback(changeTextCallback);

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
