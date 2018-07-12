namespace Linqed

open System
open System.Runtime.CompilerServices

[<Extension>]
module DistinctByExtension = 

  /// <summary>
  /// Returns a sequence that contains no duplicate entries according to the
  /// generic hash and equality comparisons on the keys returned by the given key-generating function.
  /// If an element occurs multiple times in the sequence then the later occurrences are discarded.
  /// </summary>
  [<Extension>]
  let DistinctBy (xs, projection:Func<_,_>) = 
    let projection a = projection.Invoke a 
    Seq.distinctBy projection xs
