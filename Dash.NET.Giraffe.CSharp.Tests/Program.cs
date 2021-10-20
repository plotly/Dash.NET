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
using static Dash.NET.CSharp.Giraffe.DashApp;

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
//getting_started_markdown
using System;
using System.Net;
using System.IO;
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
        public static string GetCSV(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();

            return results;
        }
        static void Main(string[] args)
        {
            
            List<string> splitted = new List<string>();
            string fileList = GetCSV("https://gist.githubusercontent.com/chriddyp/c78bf172206ce24f77d6363a2d754b59/raw/c353e8ef842413cae56ae3920b8fd78468aa4cb2/usa-agricultural-exports-2011.csv");
            string[] tempStr;

            tempStr = fileList.Split(',');

            splitted.Add("");

            foreach (string item in tempStr)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    splitted.Add(item);
                }
            }


            List<DashComponent> testList = new List<DashComponent>(){ };
            testList.Add(Html.th(Attr.children("hello")));
            testList.Add(Html.th(Attr.children("good bye")));
            testList.Add(Html.th(Attr.children("good afternoon")));

            var testList2 = testList.ToArray();

            var layout =
                Html.div(
                    Attr.children(
                        Html.h4(Attr.children("US Agriculture Exports (2011)")),
                        Html.table(
                            Attr.children(
                                Html.thead(
                                    Attr.children(
                                        testList2
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