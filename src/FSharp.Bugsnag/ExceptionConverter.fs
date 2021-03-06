﻿namespace FSharp.Bugsnag.Formatting

module ExceptionConverter =
  open FSharp.Bugsnag.Formatting.StacktraceParser
  open FSharp.Bugsnag.Types
  open System
  open System.Text.RegularExpressions

  [<Literal>]
  let private _unknownType = "UNKNOWN_TYPE"

  [<Literal>]
  let private _unknownMethod = "UNKNOWN_METHOD"

  let rec unwrapException (ex : System.Exception) =
    match ex with
    | null -> List.empty<System.Exception>
    | _ -> ex :: (unwrapException ex.InnerException)

  let convertToBugsnagExceptions(unwrappedExceptions : System.Exception list) =
    unwrappedExceptions
    |> List.map (fun ex ->
      {
        ErrorClass = ex.GetType().Name
        Message = Some(ex.Message)
        StackTrace = convertStackTrace ex.StackTrace
      })

  let getTargetSite (ex : System.Exception) =
    match ex.TargetSite with
    null -> sprintf "%s::%s" _unknownType _unknownMethod
    | _ -> ex.TargetSite.Name

  let getGroupingHash (ex : System.Exception) =
    match ex with
    | null -> ""
    | _ -> sprintf "%s@%s" (ex.GetType().Name) (getTargetSite ex)

  let getDefaultContext (ex : System.Exception) =
    match ex.TargetSite with
    | null -> sprintf "%s::%s" _unknownType _unknownMethod
    | _ -> sprintf "%A::%s" (ex.TargetSite.DeclaringType) (ex.TargetSite.Name)

  let convertWithContext (settings : BugsnagAppSettings, severity : Severity, getContext, ex : System.Exception) =
    [
      {
        PayloadVersion = "2"
        Exceptions = unwrapException ex |> convertToBugsnagExceptions
        Context = Some((getContext ex))
        GroupingHash = (getGroupingHash ex)
        Severity = severity
        App = settings.AppInfo
        Device = settings.Device
      }
    ]

  let convert (settings : BugsnagAppSettings, severity : Severity, ex : System.Exception) =
    convertWithContext(settings, severity, getDefaultContext, ex)


