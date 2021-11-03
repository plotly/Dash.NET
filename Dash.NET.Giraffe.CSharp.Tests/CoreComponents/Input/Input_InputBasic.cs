using System;
using Dash.NET.CSharp.Giraffe;
using Microsoft.Extensions.Logging;
using Dash.NET.CSharp.DCC;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp;

namespace Documentation.Examples
{
    class Input_InputBasic
    {
        public static void RunExample()
        {
            var input1 = "input1";
            var input2 = "input2";
            var output = "output";
            var outputTarget = (output, ComponentProperty.Children);

            var externalStylesheets = new[] { "https://codepen.io/chriddyp/pen/bWLwgP.css" };

            var layout =
                Html.div(
                    Attr.children(
                        Html.i(
                            Attr.children(
                                Html.text("Try typing in input 1 & 2, and observe how debounce is impacting the callbacks.Press Enter and / or Tab key in Input 2 to cancel the delay")
                            )
                        ),
                        Html.br(),
                        Input.input(
                            id: input1,
                            Input.Attr.inputType(InputType.Text()),
                            Input.Attr.placeholder(""),
                            Input.Attr.value(""),
                            Input.Attr.style(Style.StyleProperty("marginRight", "10px"))
                        ),
                        Input.input(
                            id: input2,
                            Input.Attr.inputType(InputType.Text()),
                            Input.Attr.placeholder(""),
                            Input.Attr.value(""),
                            Input.Attr.debounce(true)
                        ),
                        Html.div(Attr.id(output))
                    )
                );

            var callback =
                Callback.Create(
                    input: new[]
                    {
                        (input1, ComponentProperty.Value),
                        (input2, ComponentProperty.Value)
                    },
                    output: new[]
                    {
                        outputTarget
                    },
                    handler: (string input1, string input2) =>
                    {
                        return new[]
                        {
                            CallbackResult.Create(outputTarget, ("Input 1: " + input1 + " and Input 2: " + input2))
                        };
                    },
                    preventInitialCall: true
                );

            var dashApp = DashApp
                .initDefault()
                .appendCSSLinks(externalStylesheets)
                .withLayout(layout)
                .addCallback(callback);

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
