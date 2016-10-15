namespace FSharp.Bugsnag

module Reporting =
  open FSharp.Bugsnag.Types
  open FSharp.Bugsnag.ExceptionConverter
  open FSharp.Bugsnag.Client
  open Newtonsoft.Json.Converters
  open System
  open System.Reflection
  open Newtonsoft.Json

  let bugsnagAppSettingDefaults () =
    {
      ApiKey = None
      ReleaseStage = ReleaseStage.Development
      UseSSL = true
      User = None
      AppInfo = None
      Device = None
      MetaData = None
    }

  let notifier () =
    {
      Name = "FSharp.Bugsnag"
      Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
      Url = "https://github.com/mglodack/FSharp.Bugsnag"
    }

  let private _verifyApiKey apiKey =
    match apiKey with
    | Some key -> key
    | None -> "ERROR"

  let private _preparePayload(bugsnagMapParams, severity, ex : Exception) =
    let parameters =
      bugsnagAppSettingDefaults()
      |> bugsnagMapParams

    let payload =
      {
        ApiKey = _verifyApiKey parameters.ApiKey
        Notifier = notifier()
        Events = convert(parameters, severity, ex)
      }

    JsonConvert.SerializeObject(payload)

  let notify(bugsnagMapParams, severity, ex : Exception) =
    send(_preparePayload(bugsnagMapParams, severity, ex))

  let notifyAsync(bugsnagMapParams, severity, ex : Exception) =
    sendAsync(_preparePayload(bugsnagMapParams, severity, ex))
