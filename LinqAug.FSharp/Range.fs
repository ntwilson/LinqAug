namespace LinqAug

open System.Collections.Generic
open System.Runtime.CompilerServices

type IntStepper (start:int, step:int) = 
  member this.To finish : int seq = upcast [start .. step .. finish]

type FloatStepper (start:float, step:float) =
  member this.To finish : float seq = upcast [start .. step .. finish]

[<Extension>]
type Range() = 
  /// <summary>
  /// Returns an IEnumerable that ranges from the lower bound to the 
  /// upper bound, inclusive.
  /// </summary>
  [<Extension>]
  static member To (a:int, b:int) : int seq =
    upcast [a .. b]

  [<Extension>]
  static member To (a:float, b:float) : float seq = 
    upcast [a .. b]

  [<Extension>]
  static member Step (a, step) = 
    IntStepper (a, step)

  [<Extension>]
  static member Step (a, step) = 
    FloatStepper (a, step)  
