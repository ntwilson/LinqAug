namespace Linqed

open System
open System.Runtime.CompilerServices

module Seq =
  /// <summary>
  /// Determines if the two sequences are equal according to the
  /// equality comparer that is given.
  /// </summary>
  let equalsWith comparer (xs:'a seq) (ys:'a seq) =
    Seq.forall2 comparer xs ys
    && 
    Seq.length xs = Seq.length ys

module List = 
  /// <summary>
  /// Determines if the two lists are equal according to the
  /// equality comparer that is given.
  /// </summary>
  let equalsWith comparer (xs:'a list) (ys:'a list) = 
    List.length xs = List.length ys
    &&
    List.forall2 comparer xs ys

module Array = 
  /// <summary>
  /// Determines if the two arrays are equal according to the
  /// equality comparer that is given.
  /// </summary>
  let equalsWith comparer (xs:'a[]) (ys:'a[]) =
    Array.length xs = Array.length ys
    &&
    Array.forall2 comparer xs ys

[<Extension>]
module EqualsWithExtension =
  /// <summary>
  /// Determines if the two sequences are equal according to the
  /// equality comparer that is given.
  /// </summary>
  [<Extension>]
  let EqualsWith (xs, ys, comparer:Func<'a, 'a, bool>) = 
    let comparer a b = comparer.Invoke (a, b)
    Seq.equalsWith comparer xs ys
