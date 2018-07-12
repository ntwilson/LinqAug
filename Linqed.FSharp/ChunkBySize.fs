namespace Linqed

open System.Runtime.CompilerServices

open ResultDotNet

[<Extension>]
module ChunkBySizeExtension = 
  /// <summary>
  /// Divides the input sequence into chunks of size at most <c>chunkSize</c>.
  /// </summary>
  [<Extension>]
  let ChunkBySize xs chunkSize = 
    Seq.chunkBySize chunkSize xs

  /// <summary>
  /// Divides the input sequence into chunks of size at most <c>chunkSize</c>.
  /// </summary>
  [<Extension>]
  let ChunkBySizeSafe xs chunkSize = 
    // https://github.com/ntwilson/SafetyFirst/blob/master/SafetyFirst/Seq.fs line 33
    if chunkSize <= 0 
    then Microsoft.FSharp.Core.Error "Cannot ChunkBySize with a negative size"
    else Microsoft.FSharp.Core.Ok <| Seq.chunkBySize chunkSize xs
    |> Result.FromFs
    