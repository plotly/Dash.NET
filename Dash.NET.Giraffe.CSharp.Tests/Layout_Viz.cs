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
using System.Net.Http;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace Documentation.Examples
{
    class Layout_Viz
    {
        class LifeExpectancyGdp
        {
            [Index(0)]
            public int id { get; set; }
            [Index(1)]
            public string country { get; set; }
            [Index(2)]
            public string continent { get; set; }
            [Index(3)]
            public decimal population { get; set; }
            [Index(4)]
            public decimal life_expectancy { get; set; }
            [Index(5)]
            public decimal gdp_per_capita { get; set; }
        }

        public static void RunExample()
        {
            var csvUrl = "https://gist.githubusercontent.com/chriddyp/5d1ea79569ed194d432e56108a04d188/raw/a9f9e8076b837d541398e999dcbac2b2826a81f8/gdp-life-exp-2007.csv";
            var csv = new StreamReader(new HttpClient().GetStreamAsync(csvUrl).Result);
            var rows = new CsvReader(csv, CultureInfo.InvariantCulture).GetRecords<LifeExpectancyGdp>().ToList();
            
            Console.WriteLine(rows);


            var layout =
                    Html.div(

                    );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout);

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
