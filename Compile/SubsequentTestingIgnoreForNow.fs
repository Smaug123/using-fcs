module Compile.SubsequentTestingIgnoreForNow

open System.IO
open System.Text
open FSharp.Compiler.Interactive.Shell

module IgnoreThisForNow =
    let run (dllToTest : FileInfo) (sourceDlls : DirectoryInfo) =
        let sbout = StringBuilder ()
        let sberr = StringBuilder ()
        let inStream = new StringReader("")
        let outStream = new StringWriter (sbout)
        let errStream = new StringWriter (sberr)

        let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration ()
        let defaultArgs = [| "fsi.exe" ; "--noninteractive" ; "--nologo" ; "--gui-" |]
        let fsiSession = FsiEvaluationSession.Create (fsiConfig, defaultArgs, inStream, outStream, errStream)
        let e = fsiSession.EvalInteractionNonThrowing (sprintf "#r @\"%s\"" dllToTest.FullName)
        // Import the required libraries
        let e = fsiSession.EvalInteractionNonThrowing (sprintf "#r @\"%s/Compile.dll\"" sourceDlls.FullName)

        let firstOutput =
            match fsiSession.EvalExpressionNonThrowing "NotebookCode.Amanuensis2.input" with
            | Choice2Of2 i, errs -> failwithf "oh no! %+A, %+A" i errs
            | Choice1Of2 (Some e), [||] -> e
            | Choice1Of2 x, y -> failwithf "oh no 2! %+A, %+A" x y

        let f = fsiSession.EvalInteractionNonThrowing "open Compile"
        let secondOutput =
            match fsiSession.EvalExpressionNonThrowing "SeparateThing.inc 8" with
            | Choice2Of2 i, errs -> failwithf "oh no! %+A, %+A" i errs
            | Choice1Of2 (Some e), [||] -> e
            | Choice1Of2 x, y -> failwithf "oh no 2! %+A, %+A" x y

        firstOutput.ReflectionValue |> unbox<int>,
        secondOutput.ReflectionValue |> unbox<int>
