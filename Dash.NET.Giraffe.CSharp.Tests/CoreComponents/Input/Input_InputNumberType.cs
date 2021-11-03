using System;
using Dash.NET.CSharp.Giraffe;
using Microsoft.Extensions.Logging;
using Dash.NET.CSharp.DCC;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp;

namespace Documentation.Examples
{
    class Input_InputNumberType
    {
        public static void RunExample()
        {
            var dfalse = "dfalse";
            var dtrue = "dtrue";
            var input_range = "input_range";
            var number_out = "number_out";
            var outputTarget = (number_out, ComponentProperty.Children);

            var layout =
                Html.div(
                    Attr.children(
                        Input.input(
                            id: dfalse,
                            Input.Attr.inputType(InputType.Number()),
                            Input.Attr.placeholder("Debounce False"),
                            Input.Attr.value(null)
                        ),
                        Input.input(
                            id: dtrue,
                            Input.Attr.inputType(InputType.Number()),
                            Input.Attr.placeholder("Debounce True"),
                            Input.Attr.value(null),
                            Input.Attr.debounce(true)
                        ),
                        Input.input(
                            id: input_range,
                            Input.Attr.inputType(InputType.Number()),
                            Input.Attr.placeholder("Input with range"),
                            Input.Attr.value(null),
                            Input.Attr.min(10), Input.Attr.max(100), Input.Attr.step(3)
                        ),
                        Html.br(),
                        Html.div(Attr.id(number_out))
                    )
                );

            var callback =
                Callback.Create(
                    input: new[]
                    {
                        (dfalse, ComponentProperty.Value),
                        (dtrue, ComponentProperty.Value),
                        (input_range, ComponentProperty.Value)
                    },
                    output: new[]
                    {
                        outputTarget
                    },
                    handler: (int? fval, int? tval, int? rangeval) =>
                    {
                        return new[]
                        {
                            CallbackResult.Create(outputTarget, ("dfalse: " + fval + " dtrue: " + tval + " range: " + rangeval))
                        };
                    },
                    preventInitialCall: true
                );

            var dashApp = DashApp
                .initDefault()
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
