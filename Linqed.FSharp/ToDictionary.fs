namespace Linqed

open System
open System.Linq
open System.Runtime.CompilerServices

[<Extension>]
module ToDictionary =

  /// Creates a new Dictionary<TKey, TValue> from the key/value tuples given.
  /// Infers the types of the keys and values from the arguments.
  [<Extension>]
  let ToDictionary (keysAndValues : (struct ('TKey * 'TValue)) seq ) =
    keysAndValues.ToDictionary (
      keySelector = (fun (struct (key, value)) -> key),
      elementSelector = (fun (struct (key, value)) -> value))
