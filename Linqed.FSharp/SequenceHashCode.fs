namespace Linqed

open System.Runtime.CompilerServices

[<Extension>]
module SequenceHashCode =

  /// O(n). Calculates a hash code on the sequence based on the elements contained within.
  /// This is useful for data structures that need structural equality and include an IEnumerable. 
  [<Extension>]
  let SequenceHashCode (xs:_ seq) = 
    let somePrime = 79
    somePrime ^^^ hash (Seq.toArray xs)
  