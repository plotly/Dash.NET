using System;
//using Dash.NET.Giraffe.CSharp;
//using Dash.NET.CSharp.DCC;
using Plotly.NET;
using Giraffe;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
//using static Dash.NET.CSharp.Html;

namespace Dash.NET.Giraffe.CSharp.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = Html.Html;

            var myGraph = Plotly.NET.Chart.Line<int, int, int>(new List<Tuple<int, int>>() { Tuple.Create(1, 1), Tuple.Create(2, 2) });

            var layout =
                Html.div(
                    CAttr.children(
                        CHtml.h1(
                            CAttr.children("Hello world from Dash.NET and Giraffe!")
                        ),
                        CHtml.h2(
                            CAttr.children("Take a look at this graph:")
                        ),
                        Graph.Graph("my-ghraph-id", Graph.Attr.figure(GenericChart.toFigure(myGraph)))
                    )
                );

            var dashApp = DashAppCSharp.DashApp
                .initDefault()
                .withLayout(layout);

            var config = new DashAppCSharp.DashGiraffeConfig(
                hostName: "localhost",
                logLevel: LogLevel.Debug,
                errorHandler: (Exception err) => Core.text(err.Message)
            );

            dashApp.run(
                args: new string[] { },
                config
            );
        }
    }
}
