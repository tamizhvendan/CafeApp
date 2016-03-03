#r "packages/FAKE/tools/FakeLib.dll"
open Fake

let buildDir = "./build"
let testDir = "./tests"

Target "Clean" (fun _ -> CleanDir buildDir)

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

"Clean"
  ==> "BuildApp"
  ==> "BuildTests"
  ==> "RunUnitTests"

RunTargetOrDefault "RunUnitTests"