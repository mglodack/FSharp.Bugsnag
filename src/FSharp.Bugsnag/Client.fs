namespace FSharp.Bugsnag

module Client =
  open FSharp.Bugsnag.Types
  open FSharp.Data
  open FSharp.Data.HttpRequestHeaders
  open System

  let private _bugsnagUrl = "https://notify.bugsnag.com"

  let private _matchResponseStatusCode statusCode =
    match statusCode with
    | 200 -> Success SuccessResponseTypes.OK
    | 400 -> Failure ErrorResponseTypes.BadRequest
    | 408 -> Failure ErrorResponseTypes.RequestTimeout
    | _ -> Failure (ErrorResponseTypes.Unknown statusCode)

  let send body =
    try
      let response =
        Http.Request(
          _bugsnagUrl,
          headers = [ ContentType HttpContentTypes.Json ],
          httpMethod = "POST",
          body = TextRequest body)

      _matchResponseStatusCode response.StatusCode
    with
    | :? System.Exception as ex -> Failure (ErrorResponseTypes.Exception ex)

  // TODO: Fix async/await
  let sendAsync body =
    Http.AsyncRequest(
      _bugsnagUrl,
      headers = [ ContentType HttpContentTypes.Json ],
      httpMethod = "POST",
      body = TextRequest body)
