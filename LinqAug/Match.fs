namespace LinqAug

open System
open System.Collections.Generic
open System.Runtime.CompilerServices

module Match =
  let (|SeqWithOne|_|) xs = 
    Seq.tryHead xs 
    |> Option.map (fun head -> (head, Seq.skip 1 xs))

open Match

[<Extension>]
type Match () =

  [<Extension>]
  static member Match (xs, oneElement:Func<'a, IEnumerable<'a>, 'b>, noMatch:Func<IEnumerable<'a>, 'b>) = 
    match xs with
    | SeqWithOne (head, tail) -> oneElement.Invoke (head, tail)
    | _ -> noMatch.Invoke xs

  [<Extension>]
  static member Match (xs, oneElement:Action<'a, IEnumerable<'a>>, noMatch:Action<IEnumerable<'a>>) = 
    match xs with
    | SeqWithOne (head, tail) -> oneElement.Invoke (head, tail)
    | _ -> noMatch.Invoke xs

