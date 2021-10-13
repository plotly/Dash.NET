using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dash.NET.CSharp.DCC;
using static Dash.NET.CSharp.Html;
using static Dash.NET.CSharp.DCC.ComponentPropTypes; // TODO : Annoying that we need to open this as static as well?

namespace Dash.Giraffe.CSharp.Example
{
    static class CallbacksExample
    {
        static DashComponent CallbacksHtml()
        {
            var html =
                Html.div(
                    Attr.children(
                        Dropdown.dropdown(
                            "testInput1", 
                            Dropdown.Attr.options(
                                DropdownOption.Init("1", "1")
                            )
                        )
                    )
                );

            return html;
        }
    }
}
