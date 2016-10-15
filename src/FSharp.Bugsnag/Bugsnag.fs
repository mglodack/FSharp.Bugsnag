namespace FSharp.Bugsnag

module Client =
  type Severity = Error | Warning | Info

  type ReleaseStage = Development | Staging | Production

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

