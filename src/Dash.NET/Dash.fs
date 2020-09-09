namespace Dash.NET

open FSharp.Plotly
open Newtonsoft.Json

type HotReloadSettings = {
    intervall : int
    max_retry : int
}

//todo: use standard casing and add serialization flags instead.
type DashConfig = {
    url_base_pathname           : string Option
    requests_pathname_prefix    : string
    serve_locally               : bool
    ui                          : bool
    props_check                 : bool
    show_undo_redo              : bool
    suppress_callback_exceptions: bool
    update_title                : string
    //hot_reload                  : HotReloadSettings
}

module Defaults = 

    let defaultConfig = {
        url_base_pathname           = None
        requests_pathname_prefix    = "/"
        ui                          = true
        props_check                 = true
        serve_locally               = false
        show_undo_redo              = false
        suppress_callback_exceptions= false
        update_title                = "Updating..."
        //hot_reload                  = {intervall = 3000; max_retry = 8}
    }