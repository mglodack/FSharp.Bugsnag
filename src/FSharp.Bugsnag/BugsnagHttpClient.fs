namespace FSharp.Bugsnag

module Client =
  open System
  open FSharp.Data
  open FSharp.Data.HttpRequestHeaders

  let private _bugsnagUrl = "https://notify.bugsnag.com"

  let send (body : string) =
    Http.AsyncRequest(
      _bugsnagUrl,
      headers = [ ContentType HttpContentTypes.Json ],
      httpMethod = "POST",
      body = TextRequest body)

