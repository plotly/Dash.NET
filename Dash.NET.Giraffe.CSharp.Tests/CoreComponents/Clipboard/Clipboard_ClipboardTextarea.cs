using System;
using Dash.NET.CSharp.Giraffe;
using Microsoft.Extensions.Logging;
using Dash.NET.CSharp.DCC;
using static Dash.NET.CSharp.Dsl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.Examples
{
    class Clipboard_ClipboardTextarea
    {
        public static void RunExample()
        {
            var layout =
                Html.div(
                    Attr.children(
                        //There's no clipboard CoreComponent yet
                    )
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout);

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
