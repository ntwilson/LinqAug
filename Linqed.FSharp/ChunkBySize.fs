namespace Linqed

open System.Runtime.CompilerServices

open ResultDotNet

[<Extension>]
module ChunkBySize = 
  /// Divides the input sequence into chunks of size at most chunkSize.
  [<Extension>]
  let ChunkBySize xs chunkSize = 
    Seq.chunkBySize chunkSize xs

  /// Divides the input sequence into chunks of size at most chunkSize.
  [<Extension>]
  let ChunkBySizeSafe xs chunkSize = 
    // https://github.com/ntwilson/SafetyFirst/blob/master/SafetyFirst/Seq.fs line 33
    if chunkSize <= 0 
    then Microsoft.FSharp.Core.Error "Cannot ChunkBySize with a negative size"
    else Microsoft.FSharp.Core.Ok <| Seq.chunkBySize chunkSize xs
    |> Result.FromFs
    