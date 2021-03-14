namespace Test

open Compile
open NUnit.Framework
open System.IO
open System.Runtime.InteropServices

[<TestFixture>]
module TestFoo =

    let private mscorlib = Path.Combine (RuntimeEnvironment.GetRuntimeDirectory (), "mscorlib.dll")
    let private netstandard = Path.Combine (RuntimeEnvironment.GetRuntimeDirectory (), "netstandard.dll")

    [<Test>]
    let way1 () =
        [|
            "--simpleresolution"
            "--noframework"
            "-r"
            "FSharp.Core.dll"
            "-r"
            mscorlib
            "-r"
            netstandard
        |]
        |> Compile.go

    [<Test>]
    let way2 () =
        [|
            "-r"
            "FSharp.Core.dll"
            "-r"
            mscorlib
            "-r"
            netstandard
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
            mscorlib
            "-r"
            netstandard
        |]
        |> Compile.go

    [<Test>]
    let way5 () =
        [|
            "--noframework"
            "-r"
            "FSharp.Core.dll"
            "-r"
            mscorlib
            "-r"
            netstandard
        |]
        |> Compile.go

    [<Test>]
    let way6 () =
        [|
            "--simpleresolution"
            "--noframework"
        |]
        |> Compile.go
