namespace Test

open System.IO
open Compile
open NUnit.Framework
open FsUnitTyped

[<TestFixture>]
module TestFoo =

    let private sourceCode =
        """namespace MyCode

[<AutoOpen>]
module MyModule =
    let input =
        Compile.ImportInFsi.someUsefulThing
        |> List.head
    let output =
        "hi"
    let test () = (input = output)
"""

    [<Test>]
    let ``Compile a DLL which we'll go and import from FSI`` () =
        /// Create a DLL, which references some DLL we control (in this case, it's from Compile.fsproj).
        ///
        let outputDll, depsDir = DoIt.compile (FileInfo "/tmp/out.dll") sourceCode

        // Now, `dotnet fsi` and execute the following:
#if FALSE
#r @"Test/bin/Debug/net5.0/assemblies/Compile.dll";;
#r @"/tmp/out.dll";;
MyCode.MyModule.input;;
#endif
        // The output is:
        (*
        > MyCode.MyModule.input;;
Binding session to '/tmp/out.dll'...
Binding session to '/Users/Patrick/Documents/GitHub/using-fcs/Test/bin/Debug/net5.0/assemblies/Compile.dll'...
System.TypeInitializationException: The type initializer for '<StartupCode$out>.$Input' threw an exception.
 ---> System.MissingMethodException: Method not found: 'Microsoft.FSharp.Collections.FSharpList`1<System.String> Compile.ImportInFsi.get_someUsefulThing()'.
   --- End of inner exception stack trace ---
   at MyCode.MyModule.get_input()
   at <StartupCode$FSI_0002>.$FSI_0002.main@()
Stopped due to error
        *)

        // Note, however, that from the FSI session we *can* directly get `Compile.ImportInFsi.someUsefulThing`.
        // The missing method arises only when we're going via the DLL `/tmp/out.dll`, getting *it* to ask for
        // `someUsefulThing`.
        ()
