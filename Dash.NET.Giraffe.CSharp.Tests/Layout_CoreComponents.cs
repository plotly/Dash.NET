﻿using System;
using Dash.NET.CSharp.DCC;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using static Dash.NET.CSharp.Dsl;
using Dash.NET.CSharp.Giraffe;


namespace Documentation.Examples
{
    class Layout_CoreComponents
    {
        public static void RunExample()
        {
            var layout =
                Html.div(
                    Attr.children(
                        Html.label(Attr.children("Dropdown")),
                        Dropdown.dropdown("dropdown",
                            Dropdown.Attr.options(
                                DropdownOption.Init(label: "New York City", value: "NYC", disabled: false, title: "New York City"),
                                DropdownOption.Init(label: "Montréal", value: "MTL", disabled: false, title: "Montréal"),
                                DropdownOption.Init(label: "San Francisco", value: "SF", disabled: false, title: "San Francisco")
                            ),
                            Dropdown.Attr.value("MTL")
                        ),

                        Html.label(Attr.children("Multi-Select Dropdown")),
                        Dropdown.dropdown("multi-select-dropdown",
                            Dropdown.Attr.options(
                                DropdownOption.Init(label: "New York City", value: "NYC", disabled: false, title: "New York City"),
                                DropdownOption.Init(label: "Montréal", value: "MTL", disabled: false, title: "Montréal"),
                                DropdownOption.Init(label: "San Francisco", value: "SF", disabled: false, title: "San Francisco")
                            ),
                            Dropdown.Attr.value("MTL"),
                            Dropdown.Attr.multi(true)
                        ),

                        Html.label(Attr.children("Radio Items")),
                        RadioItems.radioItems("radioitems",
                            RadioItems.Attr.options(
                                RadioItemsOption.Init(label: "New York City", value: "NYC", disabled: false),
                                RadioItemsOption.Init(label: "Montréal", value: "MTL", disabled: false),
                                RadioItemsOption.Init(label: "San Francisco", value: "SF", disabled: false)
                            ),
                            RadioItems.Attr.value("MLT")
                        ),

                        Html.label(Attr.children("Checkboxes")),
                        Checklist.checklist("checkboxes",
                            Checklist.Attr.options(
                                ChecklistOption.Init(label: "New York City", value: "NYC", disabled: false),
                                ChecklistOption.Init(label: "Montréal", value: "MTL", disabled: false),
                                ChecklistOption.Init(label: "San Francisco", value: "SF", disabled: false)
                            )
                        ),

                        Html.label(Attr.children("Text Input")),
                        Input.input("text-input",
                            new Input.Attr[] {
                                Input.Attr.inputType(InputType.Text()),
                                Input.Attr.value("MLT")
                            }
                        ),

                        Html.label(Attr.children("Slider")),
                        Slider.slider("slider",
                            new Slider.Attr[] {
                                Slider.Attr.min(0),
                                Slider.Attr.max(9),
                                Slider.Attr.marks(
                                        new Dictionary<double, Slider.Mark>()
                                        {
                                            {1.0, Slider.Mark.Value("Label 1")},
                                            {2.0, Slider.Mark.Value("2")},
                                            {3.0, Slider.Mark.Value("3")},
                                            {4.0, Slider.Mark.Value("4")},
                                            {5.0, Slider.Mark.Value("5")}
                                        }
                                )
                            }
                        )
                    ),
                    Attr.style(Style.StyleProperty("columnCount", "2"))
                );

            var dashApp = DashApp
                .initDefault()
                .withLayout(layout);

            var config = new DashGiraffeConfig(
                hostName: "localhost",
                logLevel: LogLevel.Information,
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
