namespace FSharp.Bugsnag

module ExceptionConverter =
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

  let parseFile (line : string) =
    let ``match`` = Regex.Match(line, "in (.+):line");
    if (``match``.Groups.Count < 2) then "[file]"
    else ``match``.Groups.Item(1).Value

  let parseMethodName (line : string) =
    let ``match`` = Regex.Match(line, "at ([^)]+[)])")
    if (``match``.Groups.Count < 2) then "[method]"
    else ``match``.Groups.Item(1).Value

  let parseLineNumber (line : string) =
    let ``match`` = Regex.Match(line, ":line ([0-9]+)")
    if (``match``.Groups.Count < 2) then -1
    else Convert.ToInt32(``match``.Groups.Item(1).Value)

  let convertToStackTrace (line : string) : StackTrace =
    {
        File = parseFile line
        LineNumber = parseLineNumber line
        ColumnNumber = None
        Method = parseMethodName line
        InProject = None
      }

  let convertStackTrace (stackTrace : string) : StackTrace list =
    match stackTrace with
    null -> List.empty<StackTrace>
    | _ ->
      stackTrace.Split([|'\n'|], StringSplitOptions.RemoveEmptyEntries)
      |> Array.toList
      |> List.map convertToStackTrace

  let convertToBugsnagExceptions(unwrappedExceptions : System.Exception list) =
    unwrappedExceptions
    |> List.map (fun ex ->
      {
        ErrorClass = ex.GetType().Name
        Message = Some(ex.Message)
        // TODO: Fill in stacktrace
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
        Exceptions =
          unwrapException ex
          |> convertToBugsnagExceptions
        Context = Some((getContext ex))
        GroupingHash = (getGroupingHash ex)
        Severity = severity
        App = settings.AppInfo
        Device = settings.Device
      }
    ]

  let convert (settings : BugsnagAppSettings, severity : Severity, ex : System.Exception) =
    convertWithContext(settings, severity, getDefaultContext, ex)


