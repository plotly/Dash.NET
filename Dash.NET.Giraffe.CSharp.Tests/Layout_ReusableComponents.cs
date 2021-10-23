using System;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using System.Linq;
using Dash.NET.CSharp.Giraffe;
using System.Net.Http;

namespace Documentation.Examples
{
    class Layout_ReusableComponents
    {
        public static void RunExample()
        {
            var csv = new HttpClient().GetStringAsync("https://gist.githubusercontent.com/chriddyp/c78bf172206ce24f77d6363a2d754b59/raw/c353e8ef842413cae56ae3920b8fd78468aa4cb2/usa-agricultural-exports-2011.csv").Result;
            var rows = csv.Split("\n").SkipLast(1).Select(x => x.Split(","));
            var headers = rows.First();
            var data = rows.Skip(1);

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
                                        data.Select(x => Html.tr(Attr.children(x.Select(x => Html.td(Attr.children(x))))))
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
                logLevel: LogLevel.Information,
                ipAddress: "*",
                port: 8000,
                errorHandler: (Exception err) => err.Message
            );

            dashApp.run(
                args: new string[] { },
                config
            );
        }
    }
}
