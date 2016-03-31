#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.NpmHelper

let buildDir = "./build"
let testDir = "./tests"
let clientDir = "./client"
let clientAssetDir = clientDir @@ "public"
let assetBuildDir = buildDir @@ "public"

Target "Clean" (fun _ -> CleanDirs [buildDir; testDir; clientAssetDir; assetBuildDir])

Target "BuildApp" (fun _ ->
          !! "src/**/*.fsproj"
            -- "src/**/*.Tests.fsproj"
            |> MSBuildRelease buildDir "Build"
            |> Log "AppBuild-Output: "
)

Target "BuildTests" (fun _ ->
          !! "src/**/*.Tests.fsproj"
          |> MSBuildDebug testDir "Build"
          |> Log "BuildTests-Output: "
)

Target "RunUnitTests" (fun _ ->
          !! (testDir + "/*.Tests.dll")
          |> NUnit (fun p ->
                      {p with ToolPath = "packages/NUnit.Runners/tools/"})
)

Target "Client" (fun _ ->
       Npm (fun p ->
              { p with
                  Command = Install Standard
                  WorkingDirectory = clientDir
              })
       Npm (fun p ->
              { p with
                  Command = (Run "build")
                  WorkingDirectory = clientDir
              })
       CopyRecursive clientAssetDir assetBuildDir true |> ignore
   )

"Clean"
  ==> "BuildApp"
  ==> "BuildTests"
  ==> "RunUnitTests"
  ==> "Client"

RunTargetOrDefault "Client"