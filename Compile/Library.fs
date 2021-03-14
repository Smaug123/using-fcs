namespace Compile

open System.IO
open FSharp.Compiler.SourceCodeServices

[<RequireQualifiedAccess>]
module Compile =
    let go (args : string []) : unit =
        let pro = FSharpChecker.Create ()
        let tmpSrc = "input.fs"
        File.WriteAllText (tmpSrc, "namespace Bar\nmodule Foo =\n  let a = 1 + 1")

        let args =
            Array.append
                [|
                    "fsc.exe"
                    "-o"
                    "out.dll"
                    "-a"
                    tmpSrc
                |]
                args

        let errors, rc =
            args
            |> pro.Compile
            |> Async.RunSynchronously

        match errors with
        | [||] -> ()
        | e -> failwithf "%s" (e |> Array.map (fun i -> i.Message) |> String.concat "\n")
