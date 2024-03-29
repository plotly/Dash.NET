﻿using System;
using System.Collections.Generic;
using Dash.NET.CSharp.Giraffe;
using Dash.NET.CSharp.DCC;
using static Dash.NET.CSharp.Dsl;
using Plotly.NET;
using Microsoft.Extensions.Logging;

namespace Documentation.Examples
{
    class Layout_FirstExample
    {
        public static void RunExample()
        {
            var dataSF = new List<Tuple<string, int>>() { Tuple.Create("Apples", 4), Tuple.Create("Oranges", 1), Tuple.Create("Bananas", 2) };
            var dataMontreal = new List<Tuple<string, int>>() { Tuple.Create("Apples", 2), Tuple.Create("Oranges", 4), Tuple.Create("Bananas", 5) };

            var figSF = Chart2D.Chart.Column<string, int, int, int, int>(dataSF, "SF", true);
            var figMontreal = Chart2D.Chart.Column<string, int, int, int, int>(dataMontreal, "Montreal", true);

            var fig = Chart.Combine(new List<GenericChart.GenericChart>() { figSF, figMontreal });

            var layout =
                Html.div(
                    Attr.children(
                        Html.h1(Attr.children("Hello Dash")),
                        Html.div(Attr.children("Dash: A web application framework for your data")),
                        Graph.graph(
                            "my-graph",
                            Graph.Attr.figure(GenericChart.toFigure(fig))
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
