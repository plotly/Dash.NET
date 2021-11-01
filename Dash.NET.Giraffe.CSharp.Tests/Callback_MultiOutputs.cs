using System;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp.Giraffe;
using Dash.NET.CSharp.DCC;
using Dash.NET.CSharp;

namespace Documentation.Examples
{
    class Callback_MultiOutputs
    {
        public static void RunExample()
        {
            var layout =
                Html.div(
                    Attr.children(
                        Input.input(
                            id: "num-multi",
                            Input.Attr.inputType(InputType.Number()),
                            Input.Attr.value(5)
                        ),
                        Html.table(
                            Attr.children(
                                Html.tr(
                                    Attr.children(
                                        Html.td(Attr.children(Html.text("x"), Html.sup(Attr.children(2)))),
                                        Html.td(Attr.id("square"))
                                    )
                                ),
                                Html.tr(
                                    Attr.children(
                                        Html.td(Attr.children(Html.text("x"), Html.sup(Attr.children(3)))),
                                        Html.td(Attr.id("cube"))
                                    )
                                ),
                                Html.tr(
                                    Attr.children(
                                        Html.td(Attr.children(Html.text(2), Html.sup(Attr.children("x")))),
                                        Html.td(Attr.id("twos"))
                                    )
                                ),
                                Html.tr(
                                    Attr.children(
                                        Html.td(Attr.children(Html.text(3), Html.sup(Attr.children("x")))),
                                        Html.td(Attr.id("threes"))
                                    )
                                ),
                                Html.tr(
                                    Attr.children(
                                        Html.td(Attr.children(Html.text("x"), Html.sup(Attr.children("x")))),
                                        Html.td(Attr.id("x^x"))
                                    )
                                )
                            )
                        )
                    )
                );

            var updateOutputTableCallback =
                Callback.Create(
                    input: new[]
                    {
                        ("num-multi", ComponentProperty.Value)
                    },
                    output: new[]
                    { 
                        ("square", ComponentProperty.Children),
                        ("cube", ComponentProperty.Children),
                        ("twos", ComponentProperty.Children),
                        ("threes", ComponentProperty.Children),
                        ("x^x", ComponentProperty.Children)
                    },
                    handler: (string inputValue) =>
                    {
                        var inputNumber = Double.Parse(inputValue);
                        return new[]
                        {
                            CallbackResult.Create(("square", ComponentProperty.Children), Math.Pow(inputNumber, 2)),
                            CallbackResult.Create(("cube", ComponentProperty.Children), Math.Pow(inputNumber, 3)),
                            CallbackResult.Create(("twos", ComponentProperty.Children), Math.Pow(2, inputNumber)),
                            CallbackResult.Create(("threes", ComponentProperty.Children), Math.Pow(3, inputNumber)),
                            CallbackResult.Create(("x^x", ComponentProperty.Children), Math.Pow(inputNumber, inputNumber))

                        };
                    },
                    preventInitialCall: false
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout)
                .addCallback(updateOutputTableCallback);

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
