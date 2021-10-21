//getting_started_layout_1
/*
using System;
//using Dash.NET.Giraffe.CSharp;
using Dash.NET.CSharp.DCC;
using Plotly.NET;
using Giraffe;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp.Giraffe;

// namespace Dash.NET.Giraffe.CSharp.Example // TODO : I changed the namespace here because it was automatically opening types from the Dash.NET namespace (while they should be hidden for C# users, who only go through Dash.NET.CSharp)
namespace Dash.Giraffe.CSharp.Example.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataSF = new List<Tuple<string, int>>() { Tuple.Create("Apples", 4), Tuple.Create("Oranges", 1), Tuple.Create("Bananas", 2) };
            var dataMontreal = new List<Tuple<string, int>>() { Tuple.Create("Apples", 2), Tuple.Create("Oranges", 4), Tuple.Create("Bananas", 5) };

            var figSF = Plotly.NET.Chart2D.Chart.Column<String, int, int, int, int>(dataSF, "SF", true);
            var figMontreal = Plotly.NET.Chart2D.Chart.Column<String, int, int, int, int>(dataMontreal, "Montreal", true);

            var figList = new List<GenericChart.GenericChart>() { figSF, figMontreal };

            var fig = Chart.Combine(figList);

            var layout =
                Html.div(
                    Attr.children(
                        Html.h1(
                            Attr.children("Hello Dash!")
                            ),
                        Html.div(
                            Attr.children("Dash: A web application framework for your data")
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
                logLevel: LogLevel.Debug,
                ipAddress: "*",
                port: 49246,
                errorHandler: (Exception err) => Core.text(err.Message)
            );

            dashApp.run(
                args: new string[] { },
                config
            );
        }
    }
}
*/
//-----------------------------------------------------------------------
//getting_started_layout_2
/*
using System;
using Dash.NET.CSharp.DCC;
using Plotly.NET;
using Giraffe;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using static Dash.NET.CSharp.Giraffe.DashApp;

namespace Dash.Giraffe.CSharp.Example.Tests
{
    class Program
    {
        static void Main(string[] args)
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

            var figList = new List<GenericChart.GenericChart>() { figSF, figMontreal };

            var fig = Chart.Combine(figList).WithLayout(
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
                        Graph.Graph(
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
                logLevel: LogLevel.Debug,
                ipAddress: "*",
                port: 49246,
                errorHandler: (Exception err) => Core.text(err.Message)
            );

            dashApp.run(
                args: new string[] { },
                config
            );
        }
    }
}
*/
//-----------------------------------------------------------------------
//getting_started_markdown
/*
using System;
using System.Net;
using Dash.NET.CSharp.DCC;
using Giraffe;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Giraffe.DashApp;

namespace Dash.Giraffe.CSharp.Example.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var layout =
                Markdown.markdown(
                    "markdown",
                    Markdown.Attr.children(
                        "### Dash and Markdown\n" +
                        "Dash apps can be written in Markdown.\n" +
                        "Dash uses the [CommonMark](http://commonmark.org/)\nspecification of Markdown.\n" +
                        "Check out their [60 Second Markdown Tutorial](http://commonmark.org/help/)\n" +
                        "if this is your first introduction to Markdown!"
                        )
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout);

            var config = new DashGiraffeConfig(
                hostName: "localhost",
                logLevel: LogLevel.Debug,
                ipAddress: "*",
                port: 49246,
                errorHandler: (Exception err) => Core.text(err.Message)
            );

            dashApp.run(
                args: new string[] { },
                config
            );
        }
    }
}
*/
//-----------------------------------------------------------------------
//getting_started_table
/*
using System;
using System.Net;
using System.IO;
using Dash.NET.CSharp.DCC;
using Plotly.NET;
using Giraffe;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp.Giraffe;
using System.Linq;
using System.Globalization;

namespace Dash.Giraffe.CSharp.Example.Tests
{
    class Program
    {
        public static string GetCSV(string url)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            var resp = (HttpWebResponse)req.GetResponse();

            var sr = new StreamReader(resp.GetResponseStream());
            var results = sr.ReadToEnd();
            sr.Close();

            return results;
        }
        static void Main(string[] args)
        {

            List<string> splitted = new List<string>();
            var csv = GetCSV("https://gist.githubusercontent.com/chriddyp/c78bf172206ce24f77d6363a2d754b59/raw/c353e8ef842413cae56ae3920b8fd78468aa4cb2/usa-agricultural-exports-2011.csv");
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
                                    headers.Select(x => Html.th(Attr.children(x))).ToArray()
                                    )
                                ),
                            Html.tbody(
                                Attr.children(
                                    rows.Select(x => Html.tr(Attr.children(x.Select(x => Html.td(Attr.children(x))).ToArray())))
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
*/
//example getting_started_viz
//-----------------------------------------------------------------------
using System;
using System.Net;
using System.IO;
using Dash.NET.CSharp.DCC;
using Plotly.NET;
using Giraffe;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp.Giraffe;
using System.Linq;
using System.Globalization;

namespace Dash.Giraffe.CSharp.Example.Tests
{
    class Program
    {
        public static string GetCSV(string url)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            var resp = (HttpWebResponse)req.GetResponse();

            var sr = new StreamReader(resp.GetResponseStream());
            var results = sr.ReadToEnd();
            sr.Close();

            return results;
        }
        static void Main(string[] args)
        {

            List<string> splitted = new List<string>();
            var csv = GetCSV("https://gist.githubusercontent.com/chriddyp/5d1ea79569ed194d432e56108a04d188/raw/a9f9e8076b837d541398e999dcbac2b2826a81f8/gdp-life-exp-2007.csv");
            var headers = csv
                .Split("\n")
                .First()
                .Split(",");
            var rows = csv
                .Split("\n")
                .Skip(1)
                .SkipLast(1)
                .Select(x => x.Split('"'))
                .ToList();
            //Console.WriteLine(rows[0][3]);
            foreach (var item in rows)
            {
                foreach (var item2 in item)
                {
                    Console.Write(item2 + " ");
                }
                Console.Write("\n");

            }
            //Console.WriteLine(rows[3][0]);
            //var testList = rows.Select(x => x[3]).ToList();
            //foreach (var item in testList)
            //{
            //    Console.WriteLine(item);
            //}
            //var fig = Plotly.NET.Chart2D.Chart.Bubble<decimal, decimal, decimal>(
            //    x: rows.Select(x => decimal.Parse(x[4])),
            //    y: rows.Select(x => decimal.Parse(x[5])),
            //    sizes: rows.Select(x => int.Parse(x[3])));
            var layout =
            Html.div(
                //Attr.children(
                //    Graph.graph(
                //            "my-graph",
                //            Graph.Attr.figure(GenericChart.toFigure(fig))
                //            )

                //    )
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