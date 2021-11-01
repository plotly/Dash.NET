using System;
using Dash.NET.CSharp.DCC;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp.Giraffe;
using Dash.NET.CSharp;

namespace Documentation.Examples
{
    class Callback_BasicInput
    {
        public static void RunExample()
        {
            var layout =
                Html.div(
                    Attr.children(
                        Input.input(
                            id: "input-1",
                            Input.Attr.inputType(InputType.Text()),
                            Input.Attr.value("Montreal")
                        ),
                        Input.input(
                            id: "input-2",
                            Input.Attr.inputType(InputType.Text()),
                            Input.Attr.value("Canada")
                        ),
                        Html.div(Attr.id("output-state"))
                    )
                );
            var setOutputDivValue =
                Callback.Create(
                    input: new[]
                    {
                        ("input-1", ComponentProperty.Value),
                        ("input-2", ComponentProperty.Value)
                    },
                    output: new[]
                    {
                        ("output-state", ComponentProperty.Children)
                    },
                    handler: (string input1, string input2) =>
                    {
                        return new[]
                        {
                            CallbackResult.Create(("output-state", ComponentProperty.Children), ("Input 1 is " + input1 + ", Input 2 is " + input2))
                        };
                    },
                    preventInitialCall: false
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout)
                .addCallback(setOutputDivValue);

            var config = new DashGiraffeConfig(
                hostName: "localhost",
                logLevel: LogLevel.Information,
                ipAddress: "*",
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
