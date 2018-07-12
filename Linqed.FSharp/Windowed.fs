namespace Linqed

open System.Runtime.CompilerServices

[<Extension>]
module WindowedExtension =

  /// <summary>
  /// Returns a sequence that yields sliding windows containing elements drawn from the input
  /// sequence. Each window is returned as a fresh array.
  /// </summary>
  [<Extension>]
  let Windowed xs windowSize = 
    Seq.windowed windowSize xs
    