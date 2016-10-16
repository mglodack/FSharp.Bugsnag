namespace FSharp.Bugsnag.Formatting

// Thanks to Andrew Seward for the parsing helpers.
// https://github.com/awseward/Bugsnag.NET/blob/master/lib/Bugsnag.Common/Extensions/CommonExtensions.cs
module StacktraceParser =
  open FSharp.Bugsnag.Types
  open System
  open System.Text.RegularExpressions

  let split (stackTrace : string) : string list =
    stackTrace.Split([|'\n'|], StringSplitOptions.RemoveEmptyEntries)
    |> Array.toList

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
      split stackTrace
      |> List.map convertToStackTrace

