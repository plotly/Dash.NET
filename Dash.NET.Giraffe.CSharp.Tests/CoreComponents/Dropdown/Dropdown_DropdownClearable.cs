using System;
using Dash.NET.CSharp.Giraffe;
using Dash.NET.CSharp.DCC;
using static Dash.NET.CSharp.Dsl;
using Microsoft.Extensions.Logging;

namespace Documentation.Examples
{
    class Dropdown_DropdownClearable
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
                            Dropdown.Attr.value("MTL"),
                            Dropdown.Attr.clearable(false)
                        )
                    )
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout);

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
