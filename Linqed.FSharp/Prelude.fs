module Linqed.Prelude

open System
open System.Linq

/// <summary>
/// Creates a new <c>System.Lazy&lt;T></c> from the <c>Func&lt;T></c> given.
/// Infers type <c>T</c> from the arguments.
/// </summary>
let LazyValue (value:Func<'T>) = System.Lazy<'T> value

/// <summary>
/// Creates a new <c>Dictionary&lt;TKey, TValue></c> from the 
/// (key,value) tuples given.  Infers types <c>TKey</c> and <c>TValue</c> 
/// from the arguments.
/// </summary>
let Dict ([<ParamArray>] keysAndValues : (struct ('TKey * 'TValue))[] ) = 
  keysAndValues.ToDictionary 
    ( keySelector = (fun (struct (key, value)) -> key),
      elementSelector = (fun (struct (key, value)) -> value))

/// <summary>
/// Creates a new <c>IEnumerable&lt;T></c> from the values given.
/// Infers type <c>T</c> from the arguments.
/// </summary>
let Seq ([<ParamArray>] values) = Array.toSeq values

/// <summary>
/// Creates a new <c>List&lt;T></c> from the values given.
/// Infers type <c>T</c> from the arguments.
/// </summary>
let List ([<ParamArray>] values) = Array.toList values

/// <summary>
/// Creates a new <c>T[]</c> from the values given.
/// Infers type <c>T</c> from the arguments.
/// </summary>
let Array ([<ParamArray>] values:'a[]) = values



