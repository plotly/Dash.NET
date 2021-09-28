using System;
//using Dash.NET.Giraffe;
using Dash.NET.Giraffe.CSharp;
//using Dash.NET.Html;
using Dash.NET.DCC;
using Plotly.NET;
//using Microsoft.Extensions.Logging;
using Giraffe;
using System.Collections.Generic;
using Microsoft.FSharp.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.FSharp.Core;
using static DashAppCSharp;
//using Dash.NET;
using static Dash.NET.HtmlCSharp;

namespace Dash.NET.Giraffe.CSharp.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var myGraph = Plotly.NET.Chart.Line<int, int, int>(new List<Tuple<int, int>>() { Tuple.Create(1, 1), Tuple.Create(2, 2) });

            var layout =
                CHtml.div(
                    CAttr.children(
                        CHtml.h1(
                            CAttr.children("Hello world from Dash.NET and Giraffe!")
                        ),
                        CHtml.h2(
                            CAttr.children("Take a look at this graph:")
                        ),
                        Graph.CGraph("my-ghraph-id", Graph.Attr.figure(GenericChart.toFigure(myGraph)))
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
