namespace Linqed

open System
open System.Collections
open System.Collections.Generic
open FSharpx.Collections
open LazyList

/// A lazy sequence constrained to be finite in length.
type FiniteSeq<'a when 'a : comparison> (xs : LazyList<'a>) = 
  //this implementation is modeled off of how array hashes are computed, here: 
  //https://github.com/Microsoft/visualfsharp/blob/master/src/fsharp/FSharp.Core/prim-types.fs
  //starting at line 1680
  let hashCode = 
    lazy (
      let inline hashCombine nr x y = (x <<< 1) + y + 631 * nr

      let first18 = Seq.truncate 18 (Seq.indexed xs)
      first18 |> Seq.fold (fun acc (i, x) -> hashCombine i acc (hash x)) 0 
    )

  let length =
    lazy (LazyList.length xs)

  member this.Values = xs
  member this.Length = length.Value

  interface IEnumerable<'a> with
    member this.GetEnumerator () = (xs :> _ seq).GetEnumerator()
    member this.GetEnumerator () : IEnumerator = upcast (xs :> _ seq).GetEnumerator()      

  interface IComparable with
    member this.CompareTo x =
      
      let compareLengths (ys : _ seq) =
        match ys with
        | :? FiniteSeq<'a> as ys -> compare (this.Length) (ys.Length)
        | :? ('a[]) 
        | :? ('a list)
        | :? (ICollection<'a>) -> compare (this.Length) (Seq.length ys)
        | _ -> compare (this.Length) (Seq.length (Seq.truncate (this.Length + 1) ys))

      let compareElements (ys : _ seq) = 
        match 
            Seq.map2 compare xs ys 
            |> Seq.tryFind ((<>) 0)
          with
          | Some i -> i
          | None -> 0

      match x with
      | :? ('a seq) as ys -> 
        match compareElements ys with
        | 0 -> compareLengths ys 
        | i -> i 
      | _ -> invalidArg "x" (sprintf "Can't compare a %s with a %s" (this.GetType().Name) (x.GetType().Name)) 

  interface IEquatable<FiniteSeq<'a>> with
    member this.Equals x = (this :> IComparable).CompareTo x = 0

  override this.Equals x = (this :> IEquatable<FiniteSeq<'a>>).Equals x  

  override this.GetHashCode () = hashCode.Value

type FSeq<'a when 'a : comparison> = FiniteSeq<'a>

type fseq<'a when 'a : comparison> = FiniteSeq<'a>

[<AutoOpen>]
module FSeqBuilder = 
  /// A lazy sequence constrained to be finite in length.  There is no possible runtime check
  /// for whether or not a seq is infinite, so this is more of an assertion of the programmer
  /// that this particular seq is finite.
  let fseq xs = FiniteSeq xs
  let (|FiniteSeq|) (xs:_ fseq) = xs.Values
  let (|FSeq|) (xs:_ fseq) = xs.Values

module FiniteSeq =
  /// Returns the length of the sequence
  let inline length (FSeq xs) = LazyList.length xs

  /// Applies a function to each element of the collection, threading an accumulator argument
  /// through the computation. If the input function is <c>f</c> and the elements are <c>i0...iN</c>
  /// then computes <c>f (... (f s i0)...) iN</c>
  let inline fold f initialState (FSeq xs) = LazyList.fold f initialState xs
  
  /// Returns true if the sequence contains no elements, false otherwise.
  let inline isEmpty (FSeq xs) = LazyList.isEmpty xs

  /// Applies a function to each element of the sequence, threading an accumulator argument
  /// through the computation. Begin by applying the function to the first two elements.
  /// Then feed this result into the function along with the third element and so on.
  /// Return the final result.  
  /// Returns None if the sequence is empty
  let inline tryReduce f (FSeq xs) = 
    match xs with
    | Cons (head, tail) -> Some (LazyList.fold f head tail)
    | Nil -> None

  /// Builds a new collection whose elements are the results of applying the given function
  /// to each of the elements of the collection. The given function will be applied
  /// as elements are demanded using the MoveNext method on enumerators retrieved from the
  /// object.
  let inline map f (FSeq xs) = fseq (LazyList.map f xs)
  
  /// Builds a new collection whose elements are the results of applying the given function
  /// to each of the elements of the collection. The integer index passed to the
  /// function indicates the index (from 0) of element being transformed.
  let mapi f (FSeq xs) =
    fseq (LazyList.map2 f (LazyList.ofSeq (Seq.initInfinite id)) xs)

  /// Returns a new collection containing only the elements of the collection
  /// for which the given predicate returns "true". This is a synonym for Seq.where.
  let inline filter f (FSeq xs) = fseq (LazyList.filter f xs)

  /// Wraps the two given enumerations as a single concatenated enumeration.
  let inline append (FSeq xs) (FSeq ys) = fseq (LazyList.append xs ys)

  /// Combines the given enumeration-of-enumerations as a single concatenated enumeration.
  let inline concat (FSeq xs : FiniteSeq<FiniteSeq<'a>>) = fseq (xs |> LazyList.map (|FSeq|) |> LazyList.concat)

  /// O(n), where n is count. Return the list which on consumption will remove of at most 'n' elements of
  /// the input list.
  let inline drop n (FSeq xs) = fseq (LazyList.drop n xs)

  /// Views the given array as a finite sequence.
  let inline ofArray xs = fseq (LazyList.ofArray xs)
  
  /// Views the given seq as a finite sequence.  There is no runtime validation
  /// that the seq is actually finite, so this is a programmer assertion that the
  /// seq will be finite.
  let inline ofSeq xs = fseq (LazyList.ofSeq xs) 

  /// Views the given list as a finite sequence.  
  let inline ofList xs = fseq (LazyList.ofList xs)
  
  /// Returns a new sequence with the elements in reverse order.
  let inline rev (FSeq xs) = fseq (LazyList.rev xs)

  /// Like fold, but computes on-demand and returns the sequence of intermediary and final results.
  let inline scan f initialState (FSeq xs) = fseq (LazyList.scan f initialState xs)

  /// Builds an array from the given collection.
  let inline toArray (FSeq xs) = LazyList.toArray xs

  /// Builds a List from the given collection.
  let inline toList (FSeq xs) = LazyList.toList xs
  
  /// Views the given FiniteSeq as a sequence.
  let inline toSeq (FSeq xs) : _ seq = upcast xs

  /// Returns a sequence that when enumerated returns at most N elements.
  let truncate n (FSeq xs) = fseq (LazyList.ofSeq (Seq.truncate n xs))  

  /// Returns the first element for which the given function returns True.
  /// Return None if no such element exists.
  let inline tryFind predicate (FSeq xs) = LazyList.tryFind predicate xs
  
  /// Returns the first element of the sequence.
  let inline tryHead (FSeq xs) = LazyList.tryHead xs

  let tryMap2 f (FSeq xs) (FSeq ys)  =
    //TODO
    () 
 

  /// O(n), where n is count. Return option the list which skips the first 'n' elements of
  /// the input list.
  let inline trySkip n (FSeq xs) = Option.map fseq (LazyList.trySkip n xs)
  
  /// O(1). Return option the list corresponding to the remaining items in the sequence.
  /// Forces the evaluation of the first cell of the list if it is not already evaluated.
  let inline tryTail (FSeq xs) = Option.map fseq (LazyList.tryTail xs)
  
  /// O(n), where n is count. Return the list which on consumption will consist of at most 'n' elements of
  /// the input list.
  let inline tryTake n (FSeq xs) = Option.map fseq (LazyList.tryTake n xs)
  
  /// O(1). Returns tuple of head element and tail of the list.
  let inline tryUncons (FSeq xs) = 
    match LazyList.tryUncons xs with
    | Some (head, tail) -> Some (head, fseq tail)
    | None -> None

  let tryZip (FSeq xs) (FSeq ys) =
    // TODO 
    ()

  /// Combines the two sequences into a list of pairs. The two sequences need not have equal lengths:
  /// when one sequence is exhausted any remaining elements in the other
  /// sequence are ignored.
  let inline zip (FSeq xs) (FSeq ys) = fseq (LazyList.zip xs ys)
  
module FSeq =
  /// Returns the length of the sequence
  let inline length xs = FiniteSeq.length xs

  /// Applies a function to each element of the collection, threading an accumulator argument
  /// through the computation. If the input function is <c>f</c> and the elements are <c>i0...iN</c>
  /// then computes <c>f (... (f s i0)...) iN</c>
  let inline fold f initialState xs = FiniteSeq.fold f initialState xs

  /// Returns true if the sequence contains no elements, false otherwise.
  let inline isEmpty xs = FiniteSeq.isEmpty xs

  /// Applies a function to each element of the sequence, threading an accumulator argument
  /// through the computation. Begin by applying the function to the first two elements.
  /// Then feed this result into the function along with the third element and so on.
  /// Return the final result.  
  /// Returns None if the sequence is empty
  let inline tryReduce f xs = FiniteSeq.tryReduce f xs

  /// Builds a new collection whose elements are the results of applying the given function
  /// to each of the elements of the collection. The given function will be applied
  /// as elements are demanded using the MoveNext method on enumerators retrieved from the
  /// object.
  let inline map f xs = FiniteSeq.map f xs

  /// Builds a new collection whose elements are the results of applying the given function
  /// to each of the elements of the collection. The integer index passed to the
  /// function indicates the index (from 0) of element being transformed.
  let inline mapi f xs = FiniteSeq.mapi f xs

  /// Returns a new collection containing only the elements of the collection
  /// for which the given predicate returns "true". This is a synonym for Seq.where.
  let inline filter f xs = FiniteSeq.filter f xs

  /// Wraps the two given enumerations as a single concatenated enumeration.
  let inline append xs ys = FiniteSeq.append xs ys

  /// Combines the given enumeration-of-enumerations as a single concatenated enumeration.
  let inline concat xs = FiniteSeq.concat xs

  /// O(n), where n is count. Return the list which on consumption will remove of at most 'n' elements of
  /// the input list.
  let inline drop n xs = FiniteSeq.drop n xs

  /// Views the given array as a finite sequence.
  let inline ofArray xs = FiniteSeq.ofArray xs

  /// Views the given seq as a finite sequence.  There is no runtime validation
  /// that the seq is actually finite, so this is a programmer assertion that the
  /// seq will be finite.
  let inline ofSeq xs = FiniteSeq.ofSeq xs 

  /// Views the given list as a finite sequence.  
  let inline ofList xs = FiniteSeq.ofList xs

  /// Returns a new sequence with the elements in reverse order.
  let inline rev xs = FiniteSeq.rev xs

  /// Like fold, but computes on-demand and returns the sequence of intermediary and final results.
  let inline scan f initialState xs = FiniteSeq.scan f initialState xs

  /// Builds an array from the given collection.
  let inline toArray xs = FiniteSeq.toArray xs

  /// Builds a List from the given collection.
  let inline toList xs = FiniteSeq.toList xs

  /// Views the given FiniteSeq as a sequence.
  let inline toSeq xs = FiniteSeq.toSeq

  /// Returns a sequence that when enumerated returns at most N elements.
  let inline truncate n xs = FiniteSeq.truncate n xs

  /// Returns the first element for which the given function returns True.
  /// Return None if no such element exists.
  let inline tryFind predicate xs = FiniteSeq.tryFind predicate xs

  /// Returns the first element of the sequence.
  let inline tryHead xs = FiniteSeq.tryHead xs

  /// O(n), where n is count. Return option the list which skips the first 'n' elements of
  /// the input list.
  let inline trySkip n xs = FiniteSeq.trySkip n xs

  /// O(1). Return option the list corresponding to the remaining items in the sequence.
  /// Forces the evaluation of the first cell of the list if it is not already evaluated.
  let inline tryTail xs = FiniteSeq.tryTail xs

  /// O(n), where n is count. Return the list which on consumption will consist of at most 'n' elements of
  /// the input list.
  let inline tryTake n xs = FiniteSeq.tryTake n xs

  /// O(1). Returns tuple of head element and tail of the list.
  let inline tryUncons xs = FiniteSeq.tryUncons xs

  /// Combines the two sequences into a list of pairs. The two sequences need not have equal lengths:
  /// when one sequence is exhausted any remaining elements in the other
  /// sequence are ignored.
  let inline zip xs ys = FiniteSeq.zip xs ys
  
