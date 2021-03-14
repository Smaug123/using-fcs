namespace Test

open Compile
open NUnit.Framework

[<TestFixture>]
module TestFoo =

    [<Test>]
    let way1 () =
        [|
            "--simpleresolution"
            "--noframework"
            "-r"
            "FSharp.Core.dll"
            "-r"
            "/usr/local/share/dotnet/sdk/5.0.103/ref/mscorlib.dll"
            "-r"
            "/usr/local/share/dotnet/sdk/5.0.103/ref/netstandard.dll"
        |]
        |> Compile.go

    [<Test>]
    let way2 () =
        [|
            "-r"
            "FSharp.Core.dll"
            "-r"
            "/usr/local/share/dotnet/sdk/5.0.103/ref/mscorlib.dll"
            "-r"
            "/usr/local/share/dotnet/sdk/5.0.103/ref/netstandard.dll"
        |]
        |> Compile.go

    [<Test>]
    let way3 () =
        [|
        |]
        |> Compile.go

    [<Test>]
    let way4 () =
        [|
            "--simpleresolution"
            "-r"
            "FSharp.Core.dll"
            "-r"
            "/usr/local/share/dotnet/sdk/5.0.103/ref/mscorlib.dll"
            "-r"
            "/usr/local/share/dotnet/sdk/5.0.103/ref/netstandard.dll"
        |]
        |> Compile.go

    [<Test>]
    let way5 () =
        [|
            "--noframework"
            "-r"
            "FSharp.Core.dll"
            "-r"
            "/usr/local/share/dotnet/sdk/5.0.103/ref/mscorlib.dll"
            "-r"
            "/usr/local/share/dotnet/sdk/5.0.103/ref/netstandard.dll"
        |]
        |> Compile.go

    [<Test>]
    let way6 () =
        [|
            "--simpleresolution"
            "--noframework"
        |]
        |> Compile.go
