namespace Compile

open System.IO
open FSharp.Compiler.SourceCodeServices

[<RequireQualifiedAccess>]
module DoIt =
    let compile (outputDllLocation : FileInfo) (sourceCode : string) : FileInfo * DirectoryInfo =
        let pro = FSharpChecker.Create ()
        let tmpSrc = "input.fs"
        File.WriteAllText (tmpSrc, sourceCode)

        let required =
            let dir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory ()
            [|
                "System"
                "System.Private.CoreLib"
                "System.Numerics"
                "System.Runtime"
                "System.Runtime.Numerics"
                "System.Collections"
                "System.Net.Requests"
                "System.Net.WebClient"
                "mscorlib"
                "netstandard"
            |]
            |> Array.map (fun i -> FileInfo (Path.Combine (dir, i + ".dll")))

        let depsDir = DirectoryInfo "assemblies"
        depsDir.Delete true
        depsDir.Create ()

        // Dependencies of .NET
        for i in required do
            File.Copy (i.FullName, Path.Combine (depsDir.FullName, i.Name))

        // Dependencies of the assembly under test - in this case, so as to avoid
        // having to make another assembly in this build, this is just ourself (Compile.fsproj)
        let deps =
            [|
                "FSharp.Core.dll"
                "Compile.dll"
            |]
            |> Array.map (fun i ->
                let target = FileInfo (Path.Combine (depsDir.FullName, i))
                File.Copy (i, target.FullName)
                target
            )

        // Absolutely all the dependencies
        let required =
            required
            |> Array.append deps
            |> Array.collect (fun i -> [| "-r" ; Path.Combine (depsDir.FullName, i.Name) |])

        if outputDllLocation.Exists then outputDllLocation.Delete ()
        let args =
            Array.append
                [|
                    "fsc.exe"
                    "-o"
                    outputDllLocation.FullName
                    "-a"
                    tmpSrc
                    "--noframework"
                    "--simpleresolution"
                |]
                required

        let errors, rc =
            args
            |> pro.Compile
            |> Async.RunSynchronously

        match errors with
        | [||] -> outputDllLocation, depsDir
        | e -> failwithf "%s" (e |> Array.map (fun i -> i.Message) |> String.concat "\n")
