namespace Linqed

open System
open System.Collections
open System.Collections.Generic
open FSharpx.Collections
open LazyList

type FiniteSeq<'a when 'a : comparison> (xs : LazyList<'a>) = 
  let asArray = 
    lazy (LazyList.toArray xs)

  let hashCode =
    lazy (
      let somePrime = 13
      hash asArray.Value ^^^ somePrime
    )

  member private this.AsArray = asArray.Value
  member this.Values = xs

  interface IEnumerable<'a> with
    member this.GetEnumerator () = (xs :> _ seq).GetEnumerator()
    member this.GetEnumerator () : IEnumerator = upcast (xs :> _ seq).GetEnumerator()      

  interface IComparable with
    member this.CompareTo x =
      match x with
      | :? FiniteSeq<'a> as y -> compare (asArray.Value) (y.AsArray)
      | _ -> -100      

  interface IEquatable<FiniteSeq<'a>> with
    member this.Equals x = (this :> IComparable).CompareTo x = 0

  override this.Equals x = (this :> IEquatable<FiniteSeq<'a>>).Equals x  

  override this.GetHashCode () = hashCode.Value

type FSeq<'a when 'a : comparison> = FiniteSeq<'a>

type fseq<'a when 'a : comparison> = FiniteSeq<'a>

[<AutoOpen>]
module FSeqBuilder = 
  let fseq xs = FiniteSeq xs
  let (|FiniteSeq|) (xs:_ fseq) = xs.Values
  let (|FSeq|) (xs:_ fseq) = xs.Values

module FiniteSeq =
  let inline length (FSeq xs) = LazyList.length xs
  let inline fold f initialState (FSeq xs) = LazyList.fold f initialState xs
  let inline isEmpty (FSeq xs) = LazyList.isEmpty xs
  let inline tryReduce f (FSeq xs) = 
    match xs with
    | Cons (head, tail) -> Some (LazyList.fold f head tail)
    | Nil -> None

  let inline map f (FSeq xs) = fseq (LazyList.map f xs)
  let mapi f (FSeq xs) =
    fseq (LazyList.map2 f (LazyList.ofSeq (Seq.initInfinite id)) xs)
  let inline filter f (FSeq xs) = fseq (LazyList.filter f xs)
  let inline append (FSeq xs) (FSeq ys) = fseq (LazyList.append xs ys)
  let inline concat (FSeq xs : FiniteSeq<FiniteSeq<'a>>) = fseq (xs |> LazyList.map (|FSeq|) |> LazyList.concat)
  let inline drop n (FSeq xs) = fseq (LazyList.drop n xs)
  let inline ofArray xs = fseq (LazyList.ofArray xs)
  let inline ofSeq xs = fseq (LazyList.ofSeq xs) 
  let inline ofList xs = fseq (LazyList.ofList xs)
  let inline rev (FSeq xs) = fseq (LazyList.rev xs)
  let inline scan f initialState (FSeq xs) = fseq (LazyList.scan f initialState xs)
  let inline toArray (FSeq xs) = LazyList.toArray xs
  let inline toList (FSeq xs) = LazyList.toList xs
  let inline toSeq (FSeq xs) : _ seq = upcast xs
  let truncate n (FSeq xs) = fseq (LazyList.ofSeq (Seq.truncate n xs))  
  let inline tryFind predicate (FSeq xs) = LazyList.tryFind predicate xs
  let inline tryHead (FSeq xs) = LazyList.tryHead xs
  let inline trySkip n (FSeq xs) = Option.map fseq (LazyList.trySkip n xs)
  let inline tryTail (FSeq xs) = Option.map fseq (LazyList.tryTail xs)
  let inline tryTake n (FSeq xs) = Option.map fseq (LazyList.tryTake n xs)
  let inline tryUncons (FSeq xs) = 
    match LazyList.tryUncons xs with
    | Some (head, tail) -> Some (head, fseq tail)
    | None -> None
  let inline zip (FSeq xs) (FSeq ys) = fseq (LazyList.zip xs ys)
  
module FSeq =
  let inline length xs = FiniteSeq.length xs
  let inline fold f initialState xs = FiniteSeq.fold f initialState xs
  let inline isEmpty xs = FiniteSeq.isEmpty xs
  let inline tryReduce f xs = FiniteSeq.tryReduce f xs
  let inline map f xs = FiniteSeq.map f xs
  let inline mapi f xs = FiniteSeq.mapi f xs
  let inline filter f xs = FiniteSeq.filter f xs
  let inline append xs ys = FiniteSeq.append xs ys
  let inline concat xs = FiniteSeq.concat xs
  let inline drop n xs = FiniteSeq.drop n xs
  let inline ofArray xs = FiniteSeq.ofArray xs
  let inline ofSeq xs = FiniteSeq.ofSeq xs 
  let inline ofList xs = FiniteSeq.ofList xs
  let inline rev xs = FiniteSeq.rev xs
  let inline scan f initialState xs = FiniteSeq.scan f initialState xs
  let inline truncate n xs = FiniteSeq.truncate n xs
  let inline toArray xs = FiniteSeq.toArray xs
  let inline toList xs = FiniteSeq.toList xs
  let inline toSeq xs = FiniteSeq.toSeq
  let inline tryFind predicate xs = FiniteSeq.tryFind predicate xs
  let inline tryHead xs = FiniteSeq.tryHead xs
  let inline trySkip n xs = FiniteSeq.trySkip n xs
  let inline tryTail xs = FiniteSeq.tryTail xs
  let inline tryTake n xs = FiniteSeq.tryTake n xs
  let inline tryUncons xs = FiniteSeq.tryUncons xs
  let inline zip xs ys = FiniteSeq.zip xs ys
  
