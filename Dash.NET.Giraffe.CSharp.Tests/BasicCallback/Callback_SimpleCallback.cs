using System;
using Dash.NET.CSharp.DCC;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp.Giraffe;
using Dash.NET.CSharp;

namespace Documentation.Examples
{
    class Callback_SimpleCallback
    {
        public static void RunExample()
        {
            var layout =
                Html.div(
                    Attr.children(
                        Html.h6(
                            Attr.children("Change the value in the text box to see callbacks in action!")
                        ),
                        Html.div(
                            Attr.children(
                                Html.text("Input: "),
                                Input.input(
                                    id: "my-input",
                                    Input.Attr.value("Initual value"),
                                    Input.Attr.inputType(InputType.Text())
                                )
                            )
                        ),
                        Html.br(),
                        Html.div(
                            Attr.id("my-output")
                        )
                    )
                );
            
            var updateOutputDivCallback =
                Callback.Create(
                    input: new[]
                    {
                        ("my-input", ComponentProperty.Value)
                    },
                    output: new[]
                    {
                        ("my-output", ComponentProperty.Children)
                    },
                    handler: (string x) =>
                    {
                        return new[]
                        {
                            CallbackResult.Create(("my-output", ComponentProperty.Children), ("Output: " + x))
                        };
                    },
                    preventInitialCall: false
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout)
                .addCallback(updateOutputDivCallback);

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
