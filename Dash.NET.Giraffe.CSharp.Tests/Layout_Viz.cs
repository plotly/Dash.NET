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
        public static void RunExample()
        {
            //Using the CsvReader feels like it complicates things more than the original example?
            var csv = new HttpClient().GetStringAsync("https://gist.githubusercontent.com/chriddyp/5d1ea79569ed194d432e56108a04d188/raw/a9f9e8076b837d541398e999dcbac2b2826a81f8/gdp-life-exp-2007.csv").Result;
            //using (var testCsv = new CsvReader(new StringReader(csv), CultureInfo.InvariantCulture))
            //{
            //    var anonType = new
            //    {
            //        country = string.Empty,
            //        population = default(decimal),
            //        //life_expectancy = default(decimal),
            //        //gdp_per_capita = default(decimal)
            //    };



            //    var records = testCsv.GetRecords(anonType).ToList();
            //    Console.WriteLine("aAA");
            //    Console.WriteLine(records.);

            //}

            //Tried to parse the csv this way, but I ended up with too many nested lists. It works but it's not ideal.
            var stringSeparators = new string[] { "\"" };
            var rows = csv
                .Split("\n")
                .Skip(1)
                .SkipLast(1)
                .Select(x => x.Split(stringSeparators, StringSplitOptions.None)
                .Select(y =>
                {
                    if (y.StartsWith(",") || y.EndsWith(","))
                        return (y.Split(",", StringSplitOptions.RemoveEmptyEntries));
                    else
                        return new string[] { y };
                })
                ).ToList();


            foreach (var item in rows)
            {
                foreach (var item2 in item)
                {
                    //You need to cycle three times the list to access everything. Need to improve.
                    foreach (var item3 in item2)
                    {
                        Console.Write(item2 + " ");
                    }
                }
                Console.Write("\n");
            }
            Console.WriteLine(rows);

            //var csv = new HttpClient().GetStringAsync("https://gist.githubusercontent.com/chriddyp/5d1ea79569ed194d432e56108a04d188/raw/a9f9e8076b837d541398e999dcbac2b2826a81f8/gdp-life-exp-2007.csv").Result;
            //var headers = csv
            //    .Split("\n")
            //    .First()
            //    .Split(",");
            //var rows = csv
            //    .Split("\n")
            //    .Skip(1)
            //    .SkipLast(1)
            //    .Select(x => x.Split(new string[]))
            //    .ToList();
            ////Console.WriteLine(rows[0][3]);
            //foreach (var item in rows)
            //{
            //    foreach (var item2 in item)
            //    {
            //        Console.Write(item2 + " ");
            //    }
            //    Console.Write("\n");

            //}
            //Console.WriteLine(rows[3][0]);
            //var testList = rows.Select(x => x[3]).ToList();
            //foreach (var item in testList)
            //{
            //    Console.WriteLine(item);
            //}
            //var fig = Plotly.NET.Chart2D.Chart.Bubble<decimal, decimal, decimal>(
            //    x: rows.Select(x => decimal.Parse(x[4])),
            //    y: rows.Select(x => decimal.Parse(x[5])),
            //    sizes: rows.Select(x => int.Parse(x[3])));
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
