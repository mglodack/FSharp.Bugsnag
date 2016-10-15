namespace FSharp.Bugsnag

module Types =
  open Newtonsoft.Json.Serialization

  type Severity = Error | Warning | Info
  type ReleaseStage = Development | Staging | Production

  type Notifier =
    {
      [<JsonProperty(PropertyName = "name")>]
      Name : string
      [<JsonProperty(PropertyName = "version")>]
      Version : string
      [<JsonProperty(PropertyName = "url")>]
      Url : string
    }

  type StackTrace =
    {
      [<JsonProperty(PropertyName = "file")>]
      File : string
      [<JsonProperty(PropertyName = "lineNumber")>]
      LineNumber : int
      [<JsonProperty(PropertyName = "columnNumber")>]
      ColumnNumber : int option
      [<JsonProperty(PropertyName = "method")>]
      Method : string
      [<JsonProperty(PropertyName = "inProject")>]
      InProject : bool option
    }

  type Exception =
    {
      [<JsonProperty(PropertyName = "errorClass")>]
      ErrorClass : string
      [<JsonProperty(PropertyName = "message")>]
      Message : string option
      [<JsonProperty(PropertyName = "stacktrace")>]
      StackTrace : StackTrace list
    }


  type User =
    {
      [<JsonProperty(PropertyName = "id")>]
      Id : string option
      [<JsonProperty(PropertyName = "name")>]
      Name : string option
      [<JsonProperty(PropertyName = "email")>]
      Email : string option
    }

  type AppInfo =
    {
      [<JsonProperty(PropertyName = "version")>]
      Version : string
      [<JsonProperty(PropertyName = "releaseStage")>]
      ReleaseStage : ReleaseStage
      [<JsonProperty(PropertyName = "type")>]
      Type : string
    }

  type Device =
    {
      [<JsonProperty(PropertyName = "osVersion")>]
      OsVersion : string option
      [<JsonProperty(PropertyName = "hostname")>]
      Hostname : string option
    }

  type Event =
    {
      [<JsonProperty(PropertyName = "payloadVersion")>]
      PayloadVersion : int
      [<JsonProperty(PropertyName = "exceptions")>]
      Exceptions : Exception list
      // TODO: Threads
      [<JsonProperty(PropertyName = "context")>]
      Context : string option
      [<JsonProperty(PropertyName = "groupingHash")>]
      GroupingHash : string
      [<JsonProperty(PropertyName = "severity")>]
      Severity : Severity
      [<JsonProperty(PropertyName = "app")>]
      App : AppInfo
      [<JsonProperty(PropertyName = "device")>]
      Device : Device
    }

  type BugsnagPaylod =
    {
      [<JsonProperty(PropertyName = "apiKey")>]
      ApiKey : string
      [<JsonProperty(PropertyName = "notifier")>]
      Notifier : Notifier
      [<JsonProperty(PropertyName = "events")>]
      Events : Event list
    }

