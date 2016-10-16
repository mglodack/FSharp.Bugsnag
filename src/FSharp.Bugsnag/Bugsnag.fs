namespace FSharp.Bugsnag

module Reporting =
  open FSharp.Bugsnag.Client
  open FSharp.Bugsnag.Formatting.ExceptionConverter
  open FSharp.Bugsnag.Internal
  open FSharp.Bugsnag.Types
  open Newtonsoft.Json
  open Newtonsoft.Json.Converters
  open System
  open System.Reflection

  let private _bugsnagAppSettingDefaults () =
    {
      ApiKey = None
      ReleaseStage = ReleaseStage.Development
      UseSSL = true
      User = None
      AppInfo = None
      Device = None
      MetaData = None
    }

  let private _notifier () =
    {
      Name = "FSharp.Bugsnag"
      Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
      Url = "https://github.com/mglodack/FSharp.Bugsnag"
    }

  let private _preparePayload(bugsnagMapParams, severity, ex : Exception) =
    let parameters =
      _bugsnagAppSettingDefaults()
      |> bugsnagMapParams

    let payload =
      {
        ApiKey = parameters.ApiKey
        Notifier = _notifier()
        Events = convert(parameters, severity, ex)
      }

    let settings = new JsonSerializerSettings(NullValueHandling = NullValueHandling.Ignore)
    settings.Converters.Add(new IdiomaticDuConverter())
    JsonConvert.SerializeObject(payload, settings)

  let notify(bugsnagMapParams, severity, ex : Exception) =
    send(_preparePayload(bugsnagMapParams, severity, ex))

  let notifyAsync(bugsnagMapParams, severity, ex : Exception) =
    sendAsync(_preparePayload(bugsnagMapParams, severity, ex))

