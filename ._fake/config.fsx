namespace FSharp.Bugsnag.Config

#r @"./packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.EnvironmentHelper
open System.IO

module Common =
  let RootDir = Directory.GetCurrentDirectory()

module Source =
  open Common

  let SolutionFile = !! (Path.Combine(RootDir, "*.sln"))

module Build =
  let TestAssemblies = !! "tests/**/*.Tests.dll" -- "**/obj/**/*.Tests.dll"
  let DotNetVersion = "4.5"
  let MSBuildArtifacts = !! "src/**/bin/**/*.*" ++ "src/**/obj/**/*.*"

module Nuget =
  let ApiEnvVar = "NUGET_API_KEY"
  let ApiKey = environVar ApiEnvVar
  let PackageDirName = "nupkgs"

module Release =
  let Items = !! "**/bin/Release/*"
  let Nuspec = "FSharpBugsnag.nuspec"

  let Project = "FSharp.Bugsnag"
  let Authors = [ "Mathew Glodack" ]
  let Description = "F# Bugsnag Error Reporting Wrapper"
  let WorkingDir = "bin"
  let OutputPath = WorkingDir
