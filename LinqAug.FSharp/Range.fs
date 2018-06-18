namespace LinqAug

open System.Collections.Generic
open System.Runtime.CompilerServices

type IntStepper (start:int, step:int) = 
  member this.To finish : int seq = upcast [start .. step .. finish]

type FloatStepper (start:float, step:float) =
  /// <summary>
  /// Returns an IEnumerable that ranges from the lower bound to the 
  /// upper bound, inclusive, with the step as the
  /// interval between each value.
  /// </summary>
  member this.To finish : float seq = upcast [start .. step .. finish]

[<Extension>]
type Range() = 
  /// <summary>
  /// Returns an IEnumerable that ranges from the lower bound to the 
  /// upper bound, inclusive, with the step as the
  /// interval between each value.
  /// </summary>
  [<Extension>]
  static member To (a:int, b:int) : int seq =
    upcast [a .. b]

  [<Extension>]
  /// <summary>
  /// Returns an IEnumerable that ranges from the lower bound to the 
  /// upper bound, inclusive.
  /// </summary>
  static member To (a:float, b:float) : float seq = 
    upcast [a .. b]

  /// <summary>
  /// Returns an intermediary object for the step of a range.  Used in 
  /// conjuction with `.To` will return an IEnumerable that ranges from
  /// the lower bound to the upper bound, inclusive, with the step as the
  /// interval between each value.
  /// </summary>
  [<Extension>]
  static member Step (a, step) = 
    IntStepper (a, step)

  /// <summary>
  /// Returns an intermediary object for the step of a range.  Used in 
  /// conjuction with `.To` will return an IEnumerable that ranges from
  /// the lower bound to the upper bound, inclusive, with the step as the
  /// interval between each value.
  /// </summary>
  [<Extension>]
  static member Step (a, step) = 
    FloatStepper (a, step)  
