using System;
using System.Collections.Generic;
using Dash.NET.CSharp.Giraffe;
using Dash.NET.CSharp.DCC;
using static Dash.NET.CSharp.Dsl;
using Plotly.NET;
using Microsoft.Extensions.Logging;

namespace Documentation.Examples
{
    class Layout_MoreAboutHtmlComponents
    {
        public static void RunExample()
        {
            var colors = new Dictionary<string, string>
            {
                {"background", "#111111"},
                {"text", "#7FDBFF" }
            };
            var dataSF = new List<Tuple<string, int>>() { Tuple.Create("Apples", 4), Tuple.Create("Oranges", 1), Tuple.Create("Bananas", 2) };
            var dataMontreal = new List<Tuple<string, int>>() { Tuple.Create("Apples", 2), Tuple.Create("Oranges", 4), Tuple.Create("Bananas", 5) };

            var figSF = Plotly.NET.Chart2D.Chart.Column<String, int, int, int, int>(dataSF, "SF", true);
            var figMontreal = Plotly.NET.Chart2D.Chart.Column<String, int, int, int, int>(dataMontreal, "Montreal", true);

            var fig = Chart.Combine(new List<GenericChart.GenericChart>() { figSF, figMontreal }).WithLayout(
                Layout.init<bool>(
                    PlotBGColor: Color.fromHex(colors["background"]),
                    PaperBGColor: Color.fromHex(colors["background"]),
                    Font: Font.init(Color: Color.fromHex(colors["text"]))
                )
            );

            var layout =
                Html.div(
                    Attr.children(
                        Html.h1(
                            Attr.children("Hello Dash!"),
                            Attr.style(
                                Style.StyleProperty("textAlign", "center"),
                                Style.StyleProperty("color", colors["text"])
                            )
                        ),
                        Html.div(
                            Attr.children("Dash: A web application framework for your data."),
                            Attr.style(
                                Style.StyleProperty("textAlign", "center"),
                                Style.StyleProperty("color", colors["text"])
                            )
                        ),
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
