namespace FSharp.Bugsnag
open System

module Reporting =
  open FSharp.Bugsnag.Types

  type BugsnagConfig =
    {
      ApiKey: string option
      ReleaseStage: ReleaseStage
      UseSSL: bool
    }

  let BugsnagConfigDefaults () =
    {
      ApiKey = None
      ReleaseStage = ReleaseStage.Development
      UseSSL = true
    }
