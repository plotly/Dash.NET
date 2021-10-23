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

namespace Documentation.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Layout_FirstExample.RunExample();
            //Layout_MoreAboutHtmlComponents.RunExample();
            //Layout_Markdown.RunExample();
            //Layout_ReusableComponents.RunExample();
            //Layout_Viz.RunExample(); //Not fully working at the moment
            //Layout_CoreComponents.RunExample();
        }
    }
}