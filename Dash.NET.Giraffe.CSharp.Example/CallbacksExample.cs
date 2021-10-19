using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dash.NET.CSharp.DCC;
using static Dash.NET.CSharp.Dsl;
using static Dash.NET.CSharp.DCC.ComponentPropTypes; // TODO : Annoying that we need to open this as static as well?
using Dash.NET.CSharp;

namespace Dash.Giraffe.CSharp.Example
{
    static class CallbacksExample
    {
        internal static DashComponent CallbacksHtml()
        {
            var html =
                Html.div(
                    Attr.children(
                        Dropdown.dropdown(
                            "testInput1", 
                            Dropdown.Attr.options(
                                DropdownOption.Init("1", "1"),
                                DropdownOption.Init(2, 2),
                                DropdownOption.Init(3L, 3L),
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
                        Html.button(
                            Attr.className("button is-primary"),
                            Attr.id("testInput3"),
                            Attr.children("Click ME 2!")
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
            var test =
                Callback.Create(
                    input: new[] {
                        ("testInput2", ComponentProperty.N_Clicks),
                        //("testInput3", ComponentProperty.N_Clicks)
                        //("output-1", ComponentProperty.Children),
                        //("output-2", ComponentProperty.Children)
                    },
                    output: new[] {
                        ("output-5", ComponentProperty.Children)
                    },
                    handler: (float x, float y) => {
                        return new[] {
                            CallbackResult.Create(("output-1", ComponentProperty.Children), x.ToString() + " and " + y.ToString())
                        };
                    },
                    state: new[]
                    {
                        ("testInput3", ComponentProperty.N_Clicks)
                    }
                );

            return test;
        }

        internal static void CallbackClickInput()
        {

        }
    }
}
