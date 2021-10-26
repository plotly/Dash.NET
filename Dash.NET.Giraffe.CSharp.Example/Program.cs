using System;
using Dash.NET.CSharp.DCC;
using Plotly.NET;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp.Giraffe;
using Dash.NET.CSharp;
using System.Net;
using System.IO;
using System.Linq;
using System.Globalization;
using Microsoft.FSharp.Core;
using System.Net.Http;
using CsvHelper;

namespace Dash.Giraffe.CSharp.Example
{
    class Iris
    {
        public decimal sepal_length { get; set; }
        public decimal sepal_width { get; set; }
        public decimal petal_length { get; set; }
        public decimal petal_width { get; set; }
        public string species { get; set; }
        public int species_id { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Read and parse CSV

            var csv = new StreamReader(new HttpClient().GetStreamAsync("https://raw.githubusercontent.com/plotly/datasets/master/iris-id.csv").Result);
            var rows = new CsvReader(csv, CultureInfo.InvariantCulture).GetRecords<Iris>().ToList();

            // Use plotly charts

            Func<decimal, decimal, GenericChart.Figure> scatterPlot =
                (decimal low, decimal high) => {
                    var filtered = rows.Where(x => x.petal_width > low && x.petal_width < high);
                    var points = filtered.Select(x => Tuple.Create(x.sepal_width, x.sepal_length));
                    var petal_length = filtered.Select(x => 6 * (int)x.petal_length);
                    // Map species to different colors
                    var spec = filtered.Select(x => x.species);
                    var colors = spec.Select(x =>
                    {
                        if (x == "setosa")
                            return Color.fromHex("#4287f5");
                        else if (x == "versicolor")
                            return Color.fromHex("#cb23fa");
                        else
                            return Color.fromHex("#23fabd");
                    });

                    var markers = Plotly.NET.TraceObjects.Marker.init(
                        MultiSize: new FSharpOption<IEnumerable<int>>(petal_length),
                        Colors: new FSharpOption<IEnumerable<Color>>(colors), // Doesn't do anything !!!
                        Color: Color.fromColors(colors)
                    ); // It should not be necessary to create explicit FSharp Options here, this is something that should be improved in Plotly.NET

                    var chart = Chart2D.Chart
                        .Scatter<decimal, decimal, decimal>(points, StyleParam.Mode.Markers)
                        .WithMarker(markers)
                        .WithTitle("Iris Dataset")
                        .WithXAxisStyle(Title.init("Sepal Width"))
                        .WithYAxisStyle(Title.init("Sepal Length"));

                    var fig = GenericChart.toFigure(chart);

                    return fig;
                };


            var layout =
                Html.div(
                    Attr.children(
                        Graph.graph(id: "my-graph-id"),
                        Html.p(Attr.children("Petal Width:")),
                        RangeSlider.rangeSlider(
                            id: "range-slider",
                            RangeSlider.Attr.min(0),
                            RangeSlider.Attr.max(2.5),
                            RangeSlider.Attr.step(0.1),
                            RangeSlider.Attr.marks(
                                new Dictionary<double, RangeSlider.Mark>()
                                {
                                    {  0.0, RangeSlider.Mark.Value("0.0") },
                                    {  2.5, RangeSlider.Mark.Value("2.5") }
                                }
                            ),
                            RangeSlider.Attr.value(0.5, 2.0),
                            RangeSlider.Attr.tooltip(RangeSlider.TooltipOptions.Init(true, RangeSlider.TooltipPlacement.Bottom))
                        )
                    )
                );

            var callback =
                Callback.Create(
                    input: new[] {
                        ("range-slider", ComponentProperty.Value)
                    },
                    output: new[] {
                        ("my-graph-id", ComponentProperty.CustomProperty("figure"))
                    },
                    handler: (decimal[] sliderRange) => {
                        var r1 = sliderRange[0];
                        var r2 = sliderRange[1];
                        var low = r1 < r2 ? r1 : r2;
                        var high = r1 < r2 ? r2 : r1;
                        return new[] {
                            CallbackResult.Create(("my-graph-id", ComponentProperty.CustomProperty("figure")), scatterPlot(low, high)),
                        };
                    },
                    preventInitialCall: false
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout)
                .addCallback(callback);

            var config = new DashGiraffeConfig(
                hostName: "localhost",
                logLevel: LogLevel.Debug,
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
