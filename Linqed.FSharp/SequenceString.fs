namespace Linqed

open System.Runtime.CompilerServices

[<Extension>]
module SequenceStringExtension =
  /// <summary>
  /// Prints out the contents of an <c>IEnumerable&lt;T></c>.  Safe for use with infinite
  /// sequences, since it will only print out the first few elements of a lazy sequence. 
  /// </summary>
  [<Extension>]
  let SequenceString (xs:_ seq) = sprintf "%A" xs
  