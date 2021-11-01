﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dash.NET.CSharp.DCC;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp;
using Plotly.NET;
using Dash.NET.CSharp.Giraffe;

namespace Dash.Giraffe.CSharp.Example
{
    static class CallbacksExample
    {
        internal static DashApp CallbacksExampleDashApp()
        {
            var layout = CallbacksExample.ExampleHtml();

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout)
                .addCallback(CallbacksExample.CallbackArrayInput())
                .addCallback(CallbacksExample.CallbackClickInput());

            return dashApp;
        }

        internal static DashComponent ExampleHtml()
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
                        Graph.graph(
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

            return layout;
        }

        internal static DashComponent CallbacksHtml()
        {
            var html =
                Html.div(
                    Attr.children(
                        Dropdown.dropdown(
                            "testInput1", 
                            Dropdown.Attr.options(
                                DropdownOption.Init(1.0, 1.0),
                                DropdownOption.Init(2.0, 2.0),
                                DropdownOption.Init(3.0, 3.0),
                                DropdownOption.Init(4.1, 4.1)
                            ),
                            Dropdown.Attr.multi(true)
                        ),
                        Html.br(),
                        Html.label(
                            Attr.children(
                                Html.text("Selected values (multiplied by number of clicks) :")
                            )
                        ),
                        Html.br(),
                        Html.div(Attr.id("output-1")),
                        Html.div(Attr.id("output-2")),
                        Html.div(Attr.id("output-3")),
                        Html.div(Attr.id("output-4")),
                        Html.button(
                            Attr.className("button is-primary"),
                            Attr.id("testInput2"),
                            Attr.children("Click ME!")
                        ),
                        Html.br(),
                        Html.label(
                            Attr.children(Html.text("Number of clicks:"))
                        ),
                        Html.div(Attr.id("output-5"))
                    )
                );

            return html;
        }

        internal static Callback CallbackArrayInput()
        {
            return Callback.Create(
                input: new[] {
                    ("testInput1", ComponentProperty.Value),
                },
                output: new[] {
                    ("output-1", ComponentProperty.Children),
                    ("output-2", ComponentProperty.Children)
                },
                handler: (float[] inputs, float nclicks) => {
                    return new[] {
                        CallbackResult.Create(("output-1", ComponentProperty.Children), inputs.Last() * nclicks * 1),
                        CallbackResult.Create(("output-2", ComponentProperty.Children), inputs.Last() * nclicks * 2)
                    };
                },
                state: new[]
                {
                    ("testInput2", ComponentProperty.N_Clicks)
                }
            );
        }

        internal static Callback CallbackClickInput()
        {
            return Callback.Create(
                input: new[]
                {
                    ("testInput2", ComponentProperty.N_Clicks)
                },
                output: new[]
                {
                    ("output-5", ComponentProperty.Children)
                },
                handler: (float x) =>
                {
                    return new[]
                    {
                        CallbackResult.Create(("output-5", ComponentProperty.Children), x.ToString())
                    };
                }
            );
        }
    }
}
