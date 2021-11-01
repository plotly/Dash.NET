using System;
using Dash.NET.CSharp.DCC;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp.Giraffe;
using Plotly.NET;
using Dash.NET.CSharp;
using CsvHelper.Configuration.Attributes;
using CsvHelper;
using System.IO;
using System.Net.Http;
using System.Globalization;
using System.Linq;

namespace Documentation.Examples
{
    class Callback_SimpleSlider
    {
        class LifeExpectancyGdp
        {
            [Index(0)]
            public string country { get; set; }
            [Index(1)]
            public int year { get; set; }
            [Index(2)]
            public decimal pop { get; set; }
            [Index(3)]
            public string continent { get; set; }
            [Index(4)]
            public decimal lifeExp { get; set; }
            [Index(5)]
            public decimal gdpPercap { get; set; }
        }
        public static void RunExample()
        {
            var csvUrl = "https://raw.githubusercontent.com/plotly/datasets/master/gapminderDataFiveYear.csv";
            var csv = new StreamReader(new HttpClient().GetStreamAsync(csvUrl).Result);
            var rows = new CsvReader(csv, CultureInfo.InvariantCulture).GetRecords<LifeExpectancyGdp>().ToList();

            Func<int, GenericChart.Figure> bubbleChart =
                (int year) =>
                {
                    var filteredRows = rows.Where(x => x.year == year);
                    var labels =
                        filteredRows.Select(x =>
                            "<br>continent=" + x.continent +
                            "<br>gdp per capita=" + x.gdpPercap +
                            "<br>life expectancy=" + x.lifeExp +
                            "<br>population=" + x.pop
                        );

                    var chart =
                        Chart2D.Chart.Bubble<decimal, decimal, string>(
                            x: filteredRows.Select(x => x.gdpPercap),
                            y: filteredRows.Select(x => x.lifeExp),
                            sizes: filteredRows.Select(x => Decimal.ToInt32(x.pop / 10000000)),
                            ShowLegend: true,
                            Labels: labels.ToList()
                        )
                        .WithXAxis(
                            Plotly.NET.LayoutObjects.LinearAxis.init<int, int, int, int, int, int>(AxisType: StyleParam.AxisType.Log, Title: Title.init("Gdp per capita"))
                        )
                        .WithYAxis(
                            Plotly.NET.LayoutObjects.LinearAxis.init<int, int, int, int, int, int>(Title: Title.init("Life expectancy"))
                            );
                    var fig = GenericChart.toFigure(chart);
                    return fig;
                };

            var layout =
                Html.div(
                    Attr.children(
                        Graph.graph(id: "graph-with-slider"),
                        Slider.slider(
                            id: "year-slider",
                            Slider.Attr.min(rows.Select(x => x.year).Min()),
                            Slider.Attr.max(rows.Select(x => x.year).Max()),
                            Slider.Attr.value(rows.Select(x => x.year).Min()),
                            Slider.Attr.marks(
                                rows.GroupBy(x => x.year).Select(x => x.First()).ToDictionary(
                                    x => Convert.ToDouble(x.year), x => Slider.Mark.Value(x.year.ToString())
                                )
                            ),
                            Slider.Attr.step(5)
                        )
                    )
                );

            var updateFigureCallback =
                Callback.Create(
                    input: new[]
                    {
                        ("year-slider", ComponentProperty.Value)
                    },
                    output: new[]
                    {
                        ("graph-with-slider", ComponentProperty.CustomProperty("figure"))
                    },
                    handler: (string inputValue) =>
                    {
                        var inputYear = int.Parse(inputValue);
                        return new[]
                        {
                            CallbackResult.Create(("graph-with-slider", ComponentProperty.CustomProperty("figure")), bubbleChart(inputYear))
                        };
                    },
                    preventInitialCall: false //I added this so when the page is loaded for the first time, there's a loaded chart displayed. For some reason this line isn't on the F# docs
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout)
                .addCallback(updateFigureCallback);

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
