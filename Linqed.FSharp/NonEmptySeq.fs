namespace Linqed

open System.Collections.Generic

open FSharpx.Collections

type NonEmptySeq<'a when 'a : comparison> = 
  private
    | NonEmptySeq of FiniteSeq<'a>

  interface IEnumerable<'a> with
    member this.GetEnumerator () = 
      let (NonEmptySeq xs) = this
      in (xs :> _ seq).GetEnumerator ()

    member this.GetEnumerator () : System.Collections.IEnumerator = 
      let (NonEmptySeq xs) = this
      in upcast ((xs :> _ seq).GetEnumerator ())
  
[<AutoOpen>]
module NonEmptySeqMatcher = 
  let (|Empty|NotEmpty|) xs = 
    if FSeq.isEmpty xs
    then Empty
    else NotEmpty (NonEmptySeq xs)

module NonEmptySeq =
  let create head (FSeq tail) = NonEmptySeq (FiniteSeq (LazyList.cons head tail))
  let head (NonEmptySeq xs) = Seq.head xs
  let tail (NonEmptySeq xs) = FiniteSeq.ofSeq (Seq.tail xs)
  let uncons xs = (head xs, tail xs)
  let reduce f (NonEmptySeq xs) = Seq.reduce f xs

  let length (NonEmptySeq xs) = FiniteSeq.length xs
  let fold f initialState (NonEmptySeq xs) = FiniteSeq.fold f initialState xs
  let map f (NonEmptySeq xs) = NonEmptySeq (FiniteSeq.map f xs)
  let mapi f (NonEmptySeq xs) =
    NonEmptySeq (FiniteSeq.mapi f xs)
  let map2 f (NonEmptySeq xs) (NonEmptySeq ys) =
    NonEmptySeq (FiniteSeq.map2 f xs ys)
  let filter f (NonEmptySeq xs) = FiniteSeq.filter f xs
  let append xs (NonEmptySeq ys) = NonEmptySeq (FiniteSeq.append xs ys)
  let concat (NonEmptySeq xs : NonEmptySeq<NonEmptySeq<'a>>) = 
    NonEmptySeq (xs |> FiniteSeq.map (fun (NonEmptySeq x) -> x) |> FiniteSeq.concat)
  let drop n (NonEmptySeq xs) = FiniteSeq.drop n xs
  let rev (NonEmptySeq xs) = NonEmptySeq (FiniteSeq.rev xs)
  let scan f initialState (NonEmptySeq xs) = NonEmptySeq (FiniteSeq.scan f initialState xs)
  let toArray (NonEmptySeq xs) = FiniteSeq.toArray xs
  let toList (NonEmptySeq xs) = FiniteSeq.toList xs
  let toSeq (NonEmptySeq xs) : _ seq = upcast xs
  let tryFind predicate (NonEmptySeq xs) = FiniteSeq.tryFind predicate xs
  let tryMap2 f (NonEmptySeq xs) (NonEmptySeq ys) = Option.map NonEmptySeq (FiniteSeq.tryMap2 f xs ys)
  let trySkip n (NonEmptySeq xs) = FiniteSeq.trySkip n xs
  let tryTake n (NonEmptySeq xs) = FiniteSeq.tryTake n xs
  let tryZip (NonEmptySeq xs) (NonEmptySeq ys) = NonEmptySeq (FiniteSeq.zip xs ys)
  let zip (NonEmptySeq xs) (NonEmptySeq ys) = NonEmptySeq (FiniteSeq.zip xs ys)
  
