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
            //Layout_FirstExample.RunExample();
            //Layout_MoreAboutHtmlComponents.RunExample();
            //Layout_Markdown.RunExample();
            //Layout_ReusableComponents.RunExample();
            //Layout_Viz.RunExample(); //The F# and C# outputs are the same, but different from the online example output
            //Layout_CoreComponents.RunExample();

            //Callback_SimpleCallback.RunExample();
            //Callback_SimpleSlider.RunExample();
            Callback_MultiInputs.RunExample(); //The F# and C# outputs are the same, but different from the online example output
            //Callback_MultiOutputs.RunExample();
            //Callback_CallbackChain.RunExample(); // TODO : Finish
            //Callback_BasicInput.RunExample();
            //Callback_BasicState.RunExample();
        }
    }
}