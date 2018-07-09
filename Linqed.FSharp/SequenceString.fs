namespace Linqed

open System.Runtime.CompilerServices

[<Extension>]
module SequenceString =
  /// Prints out the contents of an IEnumerable.  Safe for use with infinite
  /// sequences, since it will only print out the first few elements of a lazy sequence. 
  [<Extension>]
  let SequenceString (xs:_ seq) = sprintf "%A" xs
  