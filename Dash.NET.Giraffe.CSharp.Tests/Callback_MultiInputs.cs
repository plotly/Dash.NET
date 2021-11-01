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
using Feliz;

namespace Documentation.Examples
{
    class Callback_MultiInputs
    {
        class CountryIndicator
        {
            [Index(0)]
            [Default("Null")]
            public string country { get; set; }
            [Index(1)]
            [Default("Null")]
            public string indicator { get; set; }
            [Index(2)]
            public int year { get; set; }
            [Index(3)]
            [Default(0)]
            [NumberStyles(NumberStyles.Number | NumberStyles.AllowExponent)]
            public decimal value { get; set; }
        }
        public static void RunExample()
        {
            var csvUrl = "https://plotly.github.io/datasets/country_indicators.csv";
            var csv= new StreamReader(new HttpClient().GetStreamAsync(csvUrl).Result);
            var rows = new CsvReader(csv, CultureInfo.InvariantCulture).GetRecords<CountryIndicator>().ToList();

            var available_indicators = rows.GroupBy(x => x.indicator).Select(x => x.First().indicator).ToList();

            Func<string, string, string, string, int, GenericChart.Figure> scatterPlot =
                (string xaxisColumnName, string yaxisColumnName, string xaxisType, string yaxisType, int year) =>
                {
                    var filteredRows = rows.Where(x => x.year == year);
                    var xData = filteredRows.Where(x => x.indicator == xaxisColumnName).Select(x => x.value);
                    var yData = filteredRows.Where(x => x.indicator == yaxisColumnName).Select(x => x.value);
                    var countryData = filteredRows.Select(x => x.country).ToArray();

                    var chart =
                        Chart2D.Chart.Scatter<decimal, decimal, string>(
                            x: xData,
                            y: yData,
                            mode: StyleParam.Mode.Markers,
                            ShowLegend: true,
                            Labels: countryData
                        )
                        .WithXAxis(
                            Plotly.NET.LayoutObjects.LinearAxis.init<int, int, int, int, int, int>(
                                AxisType: (xaxisType == "Linear") ? StyleParam.AxisType.Linear : StyleParam.AxisType.Log,
                                Title: Title.init(xaxisColumnName)
                            )
                        )
                        .WithYAxis(
                            Plotly.NET.LayoutObjects.LinearAxis.init<int, int, int, int, int, int>(
                                AxisType: (yaxisType == "Linear") ? StyleParam.AxisType.Linear : StyleParam.AxisType.Log,
                                Title: Title.init(yaxisColumnName)
                            )
                        );
                    var fig = GenericChart.toFigure(chart);
                    return fig;
                };

            var layout =
                Html.div(
                    Attr.children(
                        Html.div(
                            Attr.children(
                                Dropdown.dropdown(
                                    id: "xaxis-column",
                                    Dropdown.Attr.options(
                                        available_indicators.Select(x => DropdownOption.Init(label: x, value: x, disabled: false, title: x)).ToArray()
                                    ),
                                    Dropdown.Attr.value("Fertility rate, total (births per woman)")
                                ),
                                RadioItems.radioItems(
                                    id: "xaxis-type",
                                    RadioItems.Attr.options(
                                        RadioItemsOption.Init(label: "Linear", value: "Linear", disabled: false),
                                        RadioItemsOption.Init(label: "Log", value: "Log", disabled: false)
                                    ),
                                    RadioItems.Attr.value("Linear"),
                                    RadioItems.Attr.labelStyle(Css.displayInlineBlock)
                                )
                            ),
                            Attr.style(Css.width(length.perc(48)), Css.displayInlineBlock)
                        ),
                        Html.div(
                            Attr.children(
                                Dropdown.dropdown(
                                    id: "yaxis-column",
                                    Dropdown.Attr.options(
                                        available_indicators.Select(x => DropdownOption.Init(label: x, value: x, disabled: false, title: x)).ToArray()
                                    ),
                                    Dropdown.Attr.value("Life expectancy at birth, total (years)")
                                ),
                                RadioItems.radioItems(
                                    id: "yaxis-type",
                                    RadioItems.Attr.options(
                                        RadioItemsOption.Init(label: "Linear", value: "Linear", disabled: false),
                                        RadioItemsOption.Init(label: "Log", value: "Log", disabled: false)
                                    ),
                                    RadioItems.Attr.value("Linear"),
                                    RadioItems.Attr.labelStyle(Css.displayInlineBlock)
                                )
                            ),
                            Attr.style(Css.width(length.perc(48)), Css.floatRight, Css.displayInlineBlock)
                        ),
                        Graph.graph("indicator-graphic"),
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
            var updateOutputTableCallback =
                Callback.Create(
                    input: new[]
                    {
                        ("xaxis-column", ComponentProperty.Value),
                        ("yaxis-column", ComponentProperty.Value),
                        ("xaxis-type", ComponentProperty.Value),
                        ("yaxis-type", ComponentProperty.Value),
                        ("year-slider", ComponentProperty.Value)
                    },
                    output: new[]
                    {
                        ("indicator-graphic", ComponentProperty.CustomProperty("figure"))
                    },
                    handler: (string xaxisColumnName, string yaxisColumnName, string xaxisType, string yaxisType, string yearValue) =>
                    {
                        var inputYear = int.Parse(yearValue);
                        return new[]
                        {
                            CallbackResult.Create(("indicator-graphic", ComponentProperty.CustomProperty("figure")), scatterPlot(xaxisColumnName, yaxisColumnName, xaxisType, yaxisType, inputYear))
                        };
                    },
                    preventInitialCall: false
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout)
                .addCallback(updateOutputTableCallback);

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
