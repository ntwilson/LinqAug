namespace Linqed

open System.Runtime.CompilerServices

open ResultDotNet

[<Extension>]
module SplitHeadAndTailExtension =
  /// <summary>
  /// Split the first element of the sequence from the rest of the sequence, returning both as a tuple.  
  /// For example, <c>new[] {1, 2, 3, 4}.SplitHeadAndTail()</c> would return <c>(1, [2,3,4])</c>.
  /// Exceptions:
  ///   <c>System.ArgumentException</c>: Thrown when the input does not have any elements.
  /// </summary>
  [<Extension>]
  let SplitHeadAndTail (xs:_ seq) = 
    struct (Seq.head xs, Seq.tail xs)

  /// <summary>
  /// Split the first element of the sequence from the rest of the sequence, returning both as a tuple.  
  /// For example, <c>new[] {1, 2, 3, 4}.SplitHeadAndTail()</c> would return <c>(1, [2,3,4])</c>.
  /// Returns the answer as a ResultDotNet <c>Result</c> object.  
  /// If the IEnumerable is empty, will return an <c>Error</c> of string.
  /// </summary>
  [<Extension>]
  let SplitHeadAndTailSafe (xs:_ seq) =
    let length = 
      let xs = Seq.truncate 1 xs
      Seq.length xs

    if length > 0 
    then Ok (SplitHeadAndTail xs)
    else Error "Cannot separate the first element of an empty sequence"
    