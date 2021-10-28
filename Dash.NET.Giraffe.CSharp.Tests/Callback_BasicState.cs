using System;
using Dash.NET.CSharp.DCC;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp.Giraffe;
using Dash.NET.CSharp;

namespace Documentation.Examples
{
    class Callback_BasicState
    {
        public static void RunExample()
        {
            var layout =
                Html.div(
                    Attr.children(
                        Input.input(
                            id: "input-1-state",
                            Input.Attr.inputType(InputType.Text()),
                            Input.Attr.value("Montreal")
                        ),
                        Input.input(
                            id: "input-2-state",
                            Input.Attr.inputType(InputType.Text()),
                            Input.Attr.value("Canada")
                        ),
                        Html.button(
                            Attr.id("submit-button-state"),
                            Attr.n_clicks(0),
                            Attr.children("Submit")
                        ),
                        Html.div(Attr.id("output-state"))
                    )
                );

            var setOutputDivValue =
                Callback.Create(
                    input: new[]
                    {
                        ("submit-button-state", ComponentProperty.CustomProperty("n_clicks"))
                    },
                    output: new[]
                    {
                        ("output-state", ComponentProperty.Children)
                    },
                    handler: (int nClicks, string input1, string input2) =>
                    {
                        return new[]
                        {
                            CallbackResult.Create(("output-state", ComponentProperty.Children), ("The button has been pressed " + nClicks + " times, Input 1 is " + input1 + ", Input 2 is " + input2))
                        };
                    },
                    state: new[]
                    {
                        ("input-1-state", ComponentProperty.Value),
                        ("input-2-state", ComponentProperty.Value)
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
