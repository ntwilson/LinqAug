namespace Linqed

open System
open System.Linq
open System.Runtime.CompilerServices

[<Extension>]
module ToDictionaryExtension =

  /// <summary>
  /// Creates a new <c>Dictionary&lt;TKey, TValue></c> from the (key,value) tuples given.
  /// Infers types <c>TKey</c> and <c>TValue</c> from the arguments.
  /// </summary>
  [<Extension>]
  let ToDictionary (keysAndValues : (struct ('TKey * 'TValue)) seq ) =
    keysAndValues.ToDictionary (
      keySelector = (fun (struct (key, value)) -> key),
      elementSelector = (fun (struct (key, value)) -> value))
