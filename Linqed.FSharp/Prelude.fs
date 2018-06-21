module Linqed.Prelude

open System
open System.Linq


/// Creates a new System.Lazy<T> from the Func given.
/// Infers the type of Lazy from the arguments.
let LazyValue (value:Func<'T>) = System.Lazy<'T> value

/// Creates a new IEnumerable<T> from the values given.
/// Infers the type of IEnumerable from the arguments.
let Seq ([<ParamArray>] values) : _ seq = Array.toSeq values

/// Creates a new Dictionary<TKey, TValue> from the key/value tuples given.
/// Infers the types of the keys and values from the arguments.
let Dict ([<ParamArray>] keysAndValues : (struct ('TKey * 'TValue))[] ) = 
  keysAndValues.ToDictionary 
    ( keySelector = (fun (struct (key, value)) -> key),
      elementSelector = (fun (struct (key, value)) -> value))
