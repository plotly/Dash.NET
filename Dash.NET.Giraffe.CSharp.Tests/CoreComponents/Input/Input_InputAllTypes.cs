using System;
using Dash.NET.CSharp.Giraffe;
using Microsoft.Extensions.Logging;
using Dash.NET.CSharp.DCC;
using static Dash.NET.CSharp.Dsl;
using System.Linq;
using Dash.NET.CSharp;

namespace Documentation.Examples
{
    class Input_InputAllTypes
    {
        public static void RunExample()
        {
            var out_all_types = "out-all-types";
            var outputTarget = (out_all_types, ComponentProperty.Children);

            var ALLOWED_TYPES = new[]
            {
                InputType.Text(),
                InputType.Number(),
                InputType.Password(),
                InputType.Email(),
                InputType.Search(),
                InputType.Tel(),
                InputType.Url(),
                InputType.Range(),
                InputType.Hidden()
            };

            var layout =
                Html.div(
                    Attr.children(
                        ALLOWED_TYPES.Select(x =>
                            Input.input(
                                id: x.ToString(),
                                Input.Attr.inputType(x),
                                Input.Attr.placeholder(x.ToString()),
                                Input.Attr.value("")
                            )
                        ).Append(Html.div(Attr.id(out_all_types)))
                    )
                );

            var callback =
                Callback.Create(
                    input: ALLOWED_TYPES.Select(x => (x.ToString(), ComponentProperty.Value)).ToArray(),
                    output: new[] 
                    {
                        outputTarget
                    },
                    handler: (string textInput, string numberInput, string passwordInput, string emailInput, string searchInput, string telInput, string urlInput, string rangeInput, string hiddenInput) =>
                    {
                        var inputs = new[] { textInput, numberInput, passwordInput, emailInput, searchInput, telInput, urlInput, rangeInput, hiddenInput };
                        var stringResult = inputs.Where(x => x != "");
                        return new[]
                        {
                            CallbackResult.Create(outputTarget, String.Join(" | ", stringResult))
                        };
                    }
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
