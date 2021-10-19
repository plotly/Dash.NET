using System;
//using Dash.NET.Giraffe.CSharp;
using Dash.NET.CSharp.DCC;
using Plotly.NET;
using Giraffe;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using static Dash.NET.CSharp.Giraffe.DashApp;

// namespace Dash.NET.Giraffe.CSharp.Example // TODO : I changed the namespace here because it was automatically opening types from the Dash.NET namespace (while they should be hidden for C# users, who only go through Dash.NET.CSharp)
namespace Dash.Giraffe.CSharp.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var myGraph = Plotly.NET.Chart2D.Chart.Line<int, int, int>(new List<Tuple<int, int>>() { Tuple.Create(1, 1), Tuple.Create(2, 2) });

            var layout =
                Html.div(
                    Attr.children(
                        Html.h1(
                            Attr.children("Hello world from Dash.NET and Giraffe!")
                        ),
                        Html.h2(
                            Attr.children("Take a look at this graph:")
                        ),
                        Graph.Graph(
                            "my-ghraph-id",
                            Graph.Attr.figure(GenericChart.toFigure(myGraph)),
                            Graph.Attr.style(
                                Style.StyleProperty("marginLeft", "12px"),
                                Css.borderLeftWidth(2)
                            )
                        ),
                        CallbacksExample.CallbacksHtml()
                    )
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout)
                .addCallback(CallbacksExample.CallbackArrayInput());

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
