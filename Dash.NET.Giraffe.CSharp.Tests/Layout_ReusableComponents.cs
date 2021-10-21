using System;
using Giraffe;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using System.Linq;
using Dash.NET.CSharp.Giraffe;

namespace Documentation.Examples
{
    class Layout_ReusableComponents
    {
        public static void RunExample()
        {
            List<string> splitted = new List<string>();
            var csv = Program.GetCSV("https://gist.githubusercontent.com/chriddyp/c78bf172206ce24f77d6363a2d754b59/raw/c353e8ef842413cae56ae3920b8fd78468aa4cb2/usa-agricultural-exports-2011.csv");
            var headers = csv
                .Split("\n")
                .First()
                .Split(",");
            var rows = csv
                .Split("\n")
                .Skip(1)
                .SkipLast(1)
                .Select(x => x.Split(","))
                .ToList();


            var layout =
                Html.div(
                    Attr.children(
                        Html.h4(Attr.children("US Agriculture Exports (2011)")),
                        Html.table(
                            Attr.children(
                                Html.thead(
                                    Attr.children(
                                        headers.Select(x => Html.th(Attr.children(x)))
                                    )
                                ),
                                Html.tbody(
                                    Attr.children(
                                        rows.Select(x => Html.tr(Attr.children(x.Select(x => Html.td(Attr.children(x))))))
                                    )
                                )
                            )
                        )
                    )
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout);

            var config = new DashGiraffeConfig(
                hostName: "localhost",
                logLevel: LogLevel.Debug,
                ipAddress: "*",
                port: 8000,
                errorHandler: (Exception err) => Core.text(err.Message)
            );

            dashApp.run(
                args: new string[] { },
                config
            );
        }
    }
}
