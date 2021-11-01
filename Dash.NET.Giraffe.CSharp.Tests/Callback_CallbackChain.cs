using System;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp.Giraffe;
using Dash.NET.CSharp.DCC;
using Dash.NET.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace Documentation.Examples
{
    class Callback_CallbackChain
    {
        class Country
        {
            public string name { get; set; }
            public string[] cities { get; set; }
        }

        public static void RunExample()
        {
            var countries = new List<Country>()
            {
                new Country() { name = "America", cities = new string[] { "New York City", "San Francisco", "Cincinnati" } },
                new Country() { name = "Canada", cities = new string[] { "Montreal", "Toronto", "Ottawa" } }
            };

            var layout =
                Html.div(
                    Attr.children(
                        RadioItems.radioItems(
                            id: "countries-radio",
                            RadioItems.Attr.options(countries.Select(x => RadioItemsOption.Init(label: x.name, value: x.name, disabled: false))),
                            RadioItems.Attr.value("America")
                        ),
                        Html.hr(),
                        RadioItems.radioItems(id: "cities-radio"),
                        Html.hr(),
                        Html.div(Attr.id("display-selected-values")),
                        Html.div(Attr.id("test"))
                    )
                );

            var setCitiesOption =
                Callback.Create(
                    input: new[]
                    {
                        ("countries-radio", ComponentProperty.Value)
                    },
                    output: new[]
                    {
                        ("cities-radio", ComponentProperty.CustomProperty("options"))
                    },
                    handler: (string selectedCountry) =>
                    {
                        var radioItemsOptionArr = countries
                            .Where(x => x.name == selectedCountry)
                            .First()
                            .cities
                            .Select(x => RadioItemsOption.Init(label: x, value: x))
                            .ToArray();

                        return new[]
                        {
                            CallbackResult.Create(("cities-radio", ComponentProperty.CustomProperty("options")), radioItemsOptionArr)
                        };
                    },
                    preventInitialCall: false
                );

            var setCitiesValue =
                Callback.Create(
                    input: new[]
                    {
                        ("cities-radio", ComponentProperty.CustomProperty("options")),
                    },
                    output: new[]
                    {
                        ("test", ComponentProperty.Children)
                    },
                    handler: (RadioItemsOption<string, string>[] a) =>
                    {
                        string val = a[0].value;
                        return new[]
                        {
                            CallbackResult.Create(("test", ComponentProperty.Children), val)
                        };
                    }
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout)
                .addCallback(setCitiesOption)
                .addCallback(setCitiesValue);

            var config = new DashGiraffeConfig(
                hostName: "localhost",
                logLevel: LogLevel.Debug,
                ipAddress: "*",
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
