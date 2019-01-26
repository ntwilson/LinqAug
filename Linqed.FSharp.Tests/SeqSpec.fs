module Linqed.FSharp.Tests.SeqSpec

open NUnit.Framework
open Swensen.Unquote

open Linqed

let x = ()

let toEquatable xs = Seq.map Seq.toList xs |> Seq.toList

[<Test>]
let ``can split an empty list`` () =
  test 
    <@
      Seq.split ((=) 100) [] |> toEquatable = [[]]
    @>

[<Test>]
let ``can split a single element that doesn't satisfy the predicate`` () =
  test
    <@
      Seq.split ((=) 100) [1] |> toEquatable = [[1]]
    @>

[<Test>]
let ``can split a single element that does satisfy the predicate`` () = 
  test
    <@
      Seq.split ((=) 100) [100] |> toEquatable = [[];[100]]
    @>

[<Test>]
let ``can split multiple elements satisfying the predicate in a row`` () =
  test
    <@
      Seq.split ((=) 100) [100;100;100;1;2;100;100] |> toEquatable = 
        [[];[100];[100];[100;1;2];[100];[100]]
    @>

[<Test>]
let ``can split a sequence of 1 pairwise`` () =
  test 
    <@
      Seq.splitPairwise (=) [1] |> toEquatable = [[1]]
    @>

[<Test>]
let ``can split a single pair that matches the predicate`` () =
  test
    <@
      Seq.splitPairwise (=) [1;1] |> toEquatable = [[1];[1]]
    @>

[<Test>]
let ``can split a single pair that doesn't match the predicate`` () =
  test
    <@
      Seq.splitPairwise (=) [1;2] |> toEquatable = [[1;2]]
    @>

[<Test>]
let ``can split on multiple different pairs`` () = 
  test
    <@
      Seq.splitPairwise (=) [1;1;2;3;4;4;4;5;6;6] |> toEquatable =
        [[1];[1;2;3;4];[4];[4;5;6];[6]]
    @>

[<Test>]
let ``can split infinite sequences`` () = 
  let every4thValueIs100 = 
    Seq.initInfinite (fun i -> if i % 4 = 0 then 100 else i)

  test
    <@ 
      let xs = Seq.split ((=) 100) (every4thValueIs100)

      Seq.take 4 xs |> toEquatable = 
        [ 
          []
          [100;1;2;3]
          [100;5;6;7]
          [100;9;10;11]
        ]
    @>

[<Test>]
let ``can split infinite sequences pairwise`` () = 
  let every4thValueIs100 = 
    Seq.initInfinite (fun i -> if i % 4 = 0 then 100 else i)

  test
    <@ 
      let xs =
        Seq.splitPairwise (fun a b -> abs (a - b) > 10) (every4thValueIs100)

      Seq.take 7 xs |> toEquatable = 
        [ 
          [100]
          [1;2;3]
          [100]
          [5;6;7]
          [100]
          [9;10;11]
          [100]
        ]
    @>
