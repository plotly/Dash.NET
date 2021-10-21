using Dash.NET.CSharp.DCC;
using Dash.NET.CSharp.Giraffe;
using Giraffe;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.Examples
{
    class Layout_Markdown
    {
        public static void RunExample()
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
