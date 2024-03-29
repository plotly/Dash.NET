namespace Dash.NET.DCC

//open Dash.NET
//open System
//open Plotly.NET
//open Newtonsoft.Json

/////<summary>
/////Logout button to submit a form post request to the &#96;logout_url&#96; prop.
/////Usage is intended for dash-deployment-server authentication.
/////DDS usage:
/////&#96;dcc.LogoutButton(logout_url=os.getenv('DASH_LOGOUT_URL'))&#96;
/////Custom usage:
/////- Implement a login mechanism.
/////- Create a flask route with a post method handler.
/////&#96;@app.server.route('/logout', methods=['POST'])&#96;
/////  - The logout route should perform what's necessary for the user to logout.
/////  - If you store the session in a cookie, clear the cookie:
/////  &#96;rep = flask.Response(); rep.set_cookie('session', '', expires=0)&#96;
/////- Create a logout button component and assign it the logout_url
/////&#96;dcc.LogoutButton(logout_url='/logout')&#96;
/////See https://dash.plotly.com/dash-core-components/logout_button
/////for more documentation and examples.
/////</summary>
//[<RequireQualifiedAccess>]
//module LogoutButton =
//    ///<summary>
//    ///record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)'
//    ///</summary>
//    type LoadingStateType =
//        { IsLoading: bool
//          PropName: string
//          ComponentName: string }
//        static member convert(this) =
//            box
//                {| is_loading = this.IsLoading
//                   prop_name = this.PropName
//                   component_name = this.ComponentName |}

//    ///<summary>
//    ///• label (string; default Logout) - Text of the button
//    ///&#10;
//    ///• logout_url (string) - Url to submit a post logout request.
//    ///&#10;
//    ///• style (record) - Style of the button
//    ///&#10;
//    ///• method (string; default post) - Http method to submit the logout form.
//    ///&#10;
//    ///• className (string) - CSS class for the button.
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///</summary>
//    type Prop =
//        | Label of string
//        | LogoutUrl of string
//        | Style of obj
//        | Method of string
//        | ClassName of string
//        | LoadingState of LoadingStateType
//        static member toDynamicMemberDef(prop: Prop) =
//            match prop with
//            | Label (p) -> "label", box p
//            | LogoutUrl (p) -> "logout_url", box p
//            | Style (p) -> "style", box p
//            | Method (p) -> "method", box p
//            | ClassName (p) -> "className", box p
//            | LoadingState (p) -> "loading_state", p |> LoadingStateType.convert

//    ///<summary>
//    ///A list of children or a property for this dash component
//    ///</summary>
//    type Attr =
//        | Prop of Prop
//        | Children of list<DashComponent>
//        ///<summary>
//        ///Text of the button
//        ///</summary>
//        static member label(p: string) = Prop(Label p)
//        ///<summary>
//        ///Url to submit a post logout request.
//        ///</summary>
//        static member logoutUrl(p: string) = Prop(LogoutUrl p)
//        ///<summary>
//        ///Style of the button
//        ///</summary>
//        static member style(p: obj) = Prop(Style p)
//        ///<summary>
//        ///Http method to submit the logout form.
//        ///</summary>
//        static member method(p: string) = Prop(Method p)
//        ///<summary>
//        ///CSS class for the button.
//        ///</summary>
//        static member className(p: string) = Prop(ClassName p)
//        ///<summary>
//        ///Object that holds the loading state object coming from dash-renderer
//        ///</summary>
//        static member loadingState(p: LoadingStateType) = Prop(LoadingState p)
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: int) = Children([ Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: string) = Children([ Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: float) = Children([ Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: System.Guid) = Children([ Html.text value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: DashComponent) = Children([ value ])
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: list<DashComponent>) = Children(value)
//        ///<summary>
//        ///The child or children of this dash component
//        ///</summary>
//        static member children(value: seq<DashComponent>) = Children(List.ofSeq value)

//    ///<summary>
//    ///Logout button to submit a form post request to the &#96;logout_url&#96; prop.
//    ///Usage is intended for dash-deployment-server authentication.
//    ///DDS usage:
//    ///&#96;dcc.LogoutButton(logout_url=os.getenv('DASH_LOGOUT_URL'))&#96;
//    ///Custom usage:
//    ///- Implement a login mechanism.
//    ///- Create a flask route with a post method handler.
//    ///&#96;@app.server.route('/logout', methods=['POST'])&#96;
//    ///  - The logout route should perform what's necessary for the user to logout.
//    ///  - If you store the session in a cookie, clear the cookie:
//    ///  &#96;rep = flask.Response(); rep.set_cookie('session', '', expires=0)&#96;
//    ///- Create a logout button component and assign it the logout_url
//    ///&#96;dcc.LogoutButton(logout_url='/logout')&#96;
//    ///See https://dash.plotly.com/dash-core-components/logout_button
//    ///for more documentation and examples.
//    ///</summary>
//    type LogoutButton() =
//        inherit DashComponent()
//        static member applyMembers
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?label: string,
//                ?logoutUrl: string,
//                ?style: obj,
//                ?method: string,
//                ?className: string,
//                ?loadingState: LoadingStateType
//            ) =
//            (fun (t: LogoutButton) ->
//                let props = DashComponentProps()
//                DynObj.setValue props "id" id
//                DynObj.setValue props "children" children
//                DynObj.setValueOpt props "label" (label |> Option.map box)
//                DynObj.setValueOpt props "logoutUrl" (logoutUrl |> Option.map box)
//                DynObj.setValueOpt props "style" (style |> Option.map box)
//                DynObj.setValueOpt props "method" (method |> Option.map box)
//                DynObj.setValueOpt props "className" (className |> Option.map box)
//                DynObj.setValueOpt props "loadingState" (loadingState |> Option.map LoadingStateType.convert)
//                DynObj.setValue t "namespace" "dash_core_components"
//                DynObj.setValue t "props" props
//                DynObj.setValue t "type" "LogoutButton"
//                t)

//        static member init
//            (
//                id: string,
//                children: seq<DashComponent>,
//                ?label: string,
//                ?logoutUrl: string,
//                ?style: obj,
//                ?method: string,
//                ?className: string,
//                ?loadingState: LoadingStateType
//            ) =
//            LogoutButton.applyMembers
//                (id,
//                 children,
//                 ?label = label,
//                 ?logoutUrl = logoutUrl,
//                 ?style = style,
//                 ?method = method,
//                 ?className = className,
//                 ?loadingState = loadingState)
//                (LogoutButton())

//    ///<summary>
//    ///Logout button to submit a form post request to the &#96;logout_url&#96; prop.
//    ///Usage is intended for dash-deployment-server authentication.
//    ///DDS usage:
//    ///&#96;dcc.LogoutButton(logout_url=os.getenv('DASH_LOGOUT_URL'))&#96;
//    ///Custom usage:
//    ///- Implement a login mechanism.
//    ///- Create a flask route with a post method handler.
//    ///&#96;@app.server.route('/logout', methods=['POST'])&#96;
//    ///  - The logout route should perform what's necessary for the user to logout.
//    ///  - If you store the session in a cookie, clear the cookie:
//    ///  &#96;rep = flask.Response(); rep.set_cookie('session', '', expires=0)&#96;
//    ///- Create a logout button component and assign it the logout_url
//    ///&#96;dcc.LogoutButton(logout_url='/logout')&#96;
//    ///See https://dash.plotly.com/dash-core-components/logout_button
//    ///for more documentation and examples.
//    ///&#10;
//    ///Properties:
//    ///&#10;
//    ///• id (string) - Id of the button.
//    ///&#10;
//    ///• label (string; default Logout) - Text of the button
//    ///&#10;
//    ///• logout_url (string) - Url to submit a post logout request.
//    ///&#10;
//    ///• style (record) - Style of the button
//    ///&#10;
//    ///• method (string; default post) - Http method to submit the logout form.
//    ///&#10;
//    ///• className (string) - CSS class for the button.
//    ///&#10;
//    ///• loading_state (record with the fields: 'is_loading: boolean (optional)', 'prop_name: string (optional)', 'component_name: string (optional)') - Object that holds the loading state object coming from dash-renderer
//    ///</summary>
//    let logoutButton (id: string) (attrs: list<Attr>) =
//        let props, children =
//            List.fold
//                (fun (props, children) (a: Attr) ->
//                    match a with
//                    | Prop (prop) -> prop :: props, children
//                    | Children (child) -> props, child @ children)
//                ([], [])
//                attrs

//        let t = LogoutButton.init (id, children)

//        let componentProps =
//            match t.TryGetTypedValue<DashComponentProps> "props" with
//            | Some (p) -> p
//            | None -> DashComponentProps()

//        Seq.iter
//            (fun (prop: Prop) ->
//                let fieldName, boxedProp = Prop.toDynamicMemberDef prop
//                DynObj.setValue componentProps fieldName boxedProp)
//            props

//        DynObj.setValue t "props" componentProps
//        t :> DashComponent
