namespace Linqed

open System.Runtime.CompilerServices

type IntStepper (start:int, step:int) = 
  /// <summary>
  /// Returns an <c>IEnumerable&lt;int></c> that ranges from the lower 
  /// bound to the upper bound, inclusive, with the step as the
  /// interval between each value.
  /// </summary>
  member this.To finish : int seq = upcast [start .. step .. finish]

type FloatStepper (start:float, step:float) =
  /// <summary>
  /// Returns an <c>IEnumerable&lt;double></c> that ranges from the lower 
  /// bound to the upper bound, inclusive, with the step as the
  /// interval between each value.
  /// </summary>
  member this.To finish : float seq = upcast [start .. step .. finish]

[<Extension>]
type RangeExtensions() = 
  /// <summary>
  /// Returns an <c>IEnumerable&lt;int></c> that ranges from the lower 
  /// bound to the upper bound, inclusive.
  /// </summary>
  [<Extension>]
  static member To (a:int, b:int) : int seq =
    upcast [a .. b]

  /// <summary>
  /// Returns an <c>IEnumerable&lt;double></c> that ranges from the lower 
  /// bound to the upper bound, inclusive.
  /// </summary>
  [<Extension>]
  static member To (a:float, b:float) : float seq = 
    upcast [a .. b]

  /// <summary>
  /// Returns an intermediary object for the step of a range.  Used in 
  /// conjuction with <c>.To</c> will return an <c>IEnumerable&lt;int></c> 
  /// that ranges from the lower bound to the upper bound, inclusive, 
  /// with the step as the interval between each value.
  /// </summary>
  [<Extension>]
  static member Step (a, step) = 
    IntStepper (a, step)

  /// <summary>
  /// Returns an intermediary object for the step of a range.  Used in 
  /// conjuction with <c>.To</c> will return an <c>IEnumerable&lt;double></c> 
  /// that ranges from the lower bound to the upper bound, inclusive, 
  /// with the step as the interval between each value.
  /// </summary>
  [<Extension>]
  static member Step (a, step) = 
    FloatStepper (a, step)  
