﻿module internal FSharpApiSearch.Hack

open Microsoft.FSharp.Compiler.SourceCodeServices

let genericArguments (t: FSharpType) =
  let args = t.GenericArguments |> Seq.toList
  if t.HasTypeDefinition && t.TypeDefinition.DisplayName = "float" then // HACK: Inferred float is with MeasureOne.
    args |> List.filter (fun x -> not x.HasTypeDefinition || (x.HasTypeDefinition && x.TypeDefinition.DisplayName <> "MeasureOne"))
  else
    args

let inferredFloatFullName = Some "Microsoft.FSharp.Core.float`1"
let measureOneFullName = Some "Microsoft.FSharp.Core.CompilerServices.MeasureOne"
let isFloat (t: FSharpType) = t.HasTypeDefinition && t.TypeDefinition.TryFullName = inferredFloatFullName && (let arg = t.GenericArguments.[0] in arg.HasTypeDefinition &&  arg.TypeDefinition.TryFullName = measureOneFullName)

let isAbbreviation (t: FSharpType) = t.IsAbbreviation || isFloat t // HACK: IsAbbreviation of Inferred float is false.

let isMeasure (t: FSharpType) = t.HasTypeDefinition && t.TypeDefinition.IsMeasure // HACK: The herz measure is infinit loop of type abbreviation. 