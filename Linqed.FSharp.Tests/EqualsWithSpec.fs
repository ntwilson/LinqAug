module Linqed.FSharp.Tests.EqualsWithSpec

open NUnit.Framework
open Swensen.Unquote

open Linqed

[<Test>]
let ``can determine if two arrays are approximately equal`` () =
  let (<~>) a b = abs (a - b) < 1E-9
  test 
    <@
      [|for i in 0.0 .. 5.0 -> 0.1 * i|] <> [|0.0; 0.1; 0.2; 0.3; 0.4; 0.5|]
      &&
      [|for i in 0.0 .. 5.0 -> 0.1 * i|] |> Array.equalsWith (<~>) [|0.0; 0.1; 0.2; 0.3; 0.4; 0.5|]
      &&
      not ([|1 .. 5|] |> Array.equalsWith (=) [|1 .. 4|])
    @>

[<Test>]
let ``can determine if two sequences are approximately equal`` () = 
  let (<~>) a b = abs (a - b) < 1E-9
  test 
    <@
      [for i in 0.0 .. 5.0 -> 0.1 * i] |> Seq.equalsWith (<~>) [0.0; 0.1; 0.2; 0.3; 0.4; 0.5]
      &&
      not ([1 .. 5] |> Seq.equalsWith (=) [|1 .. 4|])
    @>

