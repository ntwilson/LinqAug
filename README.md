# LinqAug
Additional extension methods for C# Enumerables inspired by LINQ.
Additional extensions to the F# Seq, List, and Array modules.

## C# Extensions

#### `.Match`

Provides a safe way to access up to the first five elements of an `IEnumerable<T>`.  Instead of using indexing or `.ElementAt(i)`, which could throw an Exception if improperly used, the `.Match` extension method lets you check the length of the enumerable at the same time as accessing the values.

```c#
var (pathToLook, fileNameMask, extensionFilter) = 
  commandLineArgs.Match(
    threeOrMore: (path, mask, filter, rest) => (path, mask, filter),
    twoOrMore: (path, mask, rest) => (path, mask, "*.*"),
    otherwise: _ => throw new ArgumentException("Invalid command line arguments. "
                                                + "Expecting at least two arguments.")
  );
```



## F# Extensions

#### `(|SeqOneOrMore|_|)` .. `(|SeqFiveOrMore|_|)`

Provides safe active pattern matchers for any Seq to get up to the first 5 elements plus the remaining elements.  Works very similarly to the cons `(::)` operator for lists, but with any Seq.

```F#
let (pathToLook, fileNameMask, extensionFilter) = 
  match commandLineArgs with
  | SeqThreeOrMore (path, mask, filter, _) -> (path, mask, filter)
  | SeqTwoOrMore (path, mask, _) -> (path, mask, "*.*")
  | _ -> invalidArg "commandLineArgs" ("Invalid command line arguments. "
                                       + "Expecting at least two arguments.")
```

