module StacktraceParsingTests

open Xunit
open FsUnit.Xunit
open FSharp.Bugsnag.Formatting.StacktraceParser

[<Literal>]
let basicStacktraceLine = "at Microsoft.FSharp.Core.Operators.Raise[T](Exception exn)"

[<Literal>]
let explicitStacktraceLine = "at <StartupCode$query_xaxkqc>.$Main.main@() in C:\Users\glodack\AppData\Local\Temp\LINQPad5\_hemovvds\query_xaxkqc.fs:line 6"

[<Fact>]
let ``parseFile should return [file] when not found``() =
  parseFile basicStacktraceLine
  |> should equal "[file]"

[<Fact>]
let ``parseFile should return correc when found``() =
  parseFile explicitStacktraceLine
  |> should equal "C:\Users\glodack\AppData\Local\Temp\LINQPad5\_hemovvds\query_xaxkqc.fs"

[<Fact>]
let ``parseMethodName should return method name when found``() =
  parseMethodName basicStacktraceLine
  |> should equal "Microsoft.FSharp.Core.Operators.Raise[T](Exception exn)"

[<Fact>]
let ``parseLineNumber should return line number when exists``() =
  parseLineNumber explicitStacktraceLine
  |> should equal 6

[<Fact>]
let ``parseLineNumber should return -1 when line number is not found``() =
  parseLineNumber basicStacktraceLine
  |> should equal -1