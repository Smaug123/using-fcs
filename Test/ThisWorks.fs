namespace Test

open FsUnitTyped
open System.IO
open System.Runtime.InteropServices
open FSharp.Compiler.SourceCodeServices
open NUnit.Framework

module Foo2 =

    let private mscorlib = Path.Combine (RuntimeEnvironment.GetRuntimeDirectory (), "mscorlib.dll")
    let private netstandard = Path.Combine (RuntimeEnvironment.GetRuntimeDirectory (), "netstandard.dll")

    [<Test>]
    let fooo () =
        let checker = FSharpChecker.Create()
        let fn = Path.GetTempFileName()
        let fn2 = Path.ChangeExtension(fn, ".fsx")
        let fn3 = Path.ChangeExtension(fn, ".dll")

        File.WriteAllText(fn2, """
        module M

        type C() =
           member x.P = 1

        let x = 3 + 4
        """)

        let required =
            [|
                "FSharp.Core.dll"
                "System.Private.CoreLib"
                "System.Runtime"
                "System.Runtime.Numerics"
                "System.Collections"
                "System.Net.Requests"
                "System.Net.WebClient"
            |]
            |> Array.collect (fun i -> [| "-r" ; i |])

        let errors1, exitCode1 =
            Array.append
                [|
                    "fsc.exe"
                    "-o"
                    fn3
                    "-a"
                    fn2
                    "-r"
                    mscorlib
                    "-r"
                    netstandard
                    "-I"
                    Directory.GetParent(mscorlib).FullName
                    "--noframework"
                    "--simpleresolution"
                |]
                required
            |> checker.Compile
            |> Async.RunSynchronously

        errors1
        |> shouldBeEmpty