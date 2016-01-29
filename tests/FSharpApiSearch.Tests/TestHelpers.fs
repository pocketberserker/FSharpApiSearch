﻿module TestHelpers
open FSharpApiSearch.Types

let rec updateSource newSource = function
  | Variable (_, name) -> Variable (newSource, name)
  | Identity _ as x -> x
  | Arrow xs -> Arrow (List.map (updateSource newSource) xs)
  | Generic (id, xs) -> Generic (id, List.map (updateSource newSource) xs)
  | Tuple xs -> Tuple (List.map (updateSource newSource) xs)
  | StaticMethod (parameters, returnType) -> StaticMethod(List.map (updateSource newSource) parameters, updateSource newSource returnType)
  | Unknown as x -> x

let toStaticMethod = function
  | Arrow xs ->
    match xs with
    | [ Tuple arguments; returnType ] -> StaticMethod(arguments, returnType)
    | [ argument; returnType ] -> StaticMethod([ argument ], returnType)
    | _ -> Unknown
  | _ -> Unknown

module DSL =
  let identity name = Identity name
  let variable name = Variable (Query, name)
  let arrow xs = Arrow xs
  let generic id xs = Generic (id, xs)
  let tuple xs = Tuple xs