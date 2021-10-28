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
            public Country(string name, string[] cities)
            {
                this.name = name;
                this.cities = cities;
            }
            public string name { get; set; }
            public string[] cities { get; set; }
        }
        public static void RunExample()
        {
            var countries = new List<Country>()
            {
                new Country("America", new string[] { "New York City", "San Francisco", "Cincinnati"}),
                new Country("Canada", new string[] { "Montreal", "Toronto", "Ottawa" })
            };

            countries.Where(x => x.name == "America").Select(x => x.cities);

            var layout =
                Html.div(
                    Attr.children(
                        RadioItems.radioItems(
                            id: "countries-radio",
                            RadioItems.Attr.options(countries.Select(x => RadioItemsOption.Init(label: x.name, value: x.name, disabled: false)).ToArray()),
                            RadioItems.Attr.value("America")
                        ),
                        Html.hr(),
                        RadioItems.radioItems(id: "cities-radio"),
                        Html.hr(),
                        Html.div(Attr.id("display-selected-values"))
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
                        var test = new[] { RadioItemsOption.Init(label: selectedCountry, value: selectedCountry) };
                        return new[]
                        {
                            CallbackResult.Create(("cities-radio", ComponentProperty.CustomProperty("options")), test)
                        };
                    },
                    preventInitialCall: false
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout)
                .addCallback(setCitiesOption);

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
