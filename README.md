# LINQED
LINQ Extended Design

This library contains additional extension methods for C# Enumerables inspired by LINQ, as well as extensions to the F# Seq, List, and Array modules.

## C# Extensions

### Extension Methods

The following are all extension methods available on existing .NET types.

#### `.Match`

Provides a safe way to access up to the first five elements of an `IEnumerable<T>`.  Instead of using indexing or `.ElementAt(i)`, which could throw an Exception if improperly used, the `.Match` extension method lets you check the length of the enumerable at the same time as accessing the values.

```C#
var (pathToLook, fileNameMask, extensionFilter) = 
  commandLineArgs.Match(
    threeOrMore: (path, mask, filter, rest) => (path, mask, filter),
    twoOrMore: (path, mask, rest) => (path, mask, "*.*"),
    otherwise: _ => throw new ArgumentException("Invalid command line arguments. "
                                                + "Expecting at least two arguments.")
  );
```



#### `.To`

Provides a convenient syntax for making ranges.  Works with integers and doubles.  Two syntaxes are available:

```C#
var twelveVals = 1.To(12)
```

and:

```C#
var sixOddVals = 1.Step(2).To(11)
```



#### `.ToDictionary`

Creates a Dictionary from an IEnumerable of tuples.  For example:

```C#
var namesByID = 
  customerIDs.Zip(customerNames, (id, name) => (id, name)).ToDictionary();
```



### Static Functions

These static functions are convenient for use with C# 6's `using static` directives.  By adding `using static Linqed.Prelude` to the top of your file, all of these static functions will be brought in scope.

#### `Seq`

Creates a new IEnumerable just with a params array of items.  Rather than typing out

```C#
IEnumerable<int> xs = new[] { 1, 2, 3 };
```

you can instead type:

```C#
IEnumerable<int> xs = Seq(1, 2, 3);
```



#### `LazyValue`

Creates new Lazies without specifying the type.  Rather than typing out

```C#
var xs = new Lazy<IEnumerable<double>>(() => new[] { 1.0, 2.0, 3.0 });
```

you can instead type:

```C#
var xs = LazyValue(() => new[] { 1.0, 2.0, 3.0 });
```



#### `Dict`

Creates new Dictionaries just with a params array of tuples, without needing to specify the type.  Rather than typing out

```C#
var myDictionary = new Dictionary<DateTime, Lazy<IEnumerable<double>>>() {
  { DateTime.Today, new Lazy<IEnumerable<double>>(() => new[] { 1.0, 2.0, 3.0 }) },
  { DateTime.Today.AddDays(1), new Lazy<IEnumerable<double>>(() => new[] { 4.0, 5.0, 6.0 }) },
};
```

you can instead type:

```C#
var myDictionary = Dict(
  (DateTime.Today, new Lazy<IEnumerable<double>>(() => new[] { 1.0, 2.0, 3.0 })),
  (DateTime.Today.AddDays(1), new Lazy<IEnumerable<double>>(() => new[] { 4.0, 5.0, 6.0 }))
);
```

and it will infer the type of the Dictionary from the values provided.  Combine this with the `Seq` and `LazyValue` factories, and it can be simplified further to just:

```C#
var myDictionary = Dict(
  (DateTime.Today, LazyValue(() => Seq(1.0, 2.0, 3.0))),
  (DateTime.Today.AddDays(1), LazyValue(() => Seq(4.0, 5.0, 6.0)))
);
```





## F# Extensions

#### `(|SeqOneOrMore|_|)` .. `(|SeqFiveOrMore|_|)`

Provides safe active pattern matchers for any Seq to get up to the first 5 elements plus the remaining elements.  Works very similarly to the cons `(::)` operator for lists, but with any Seq.  Note that using these matchers will iterate the first n elements of the seq twice, so consider caching a seq if the cost of iterating is expensive.

```F#
let (pathToLook, fileNameMask, extensionFilter) = 
  match commandLineArgs with
  | SeqThreeOrMore (path, mask, filter, _) -> (path, mask, filter)
  | SeqTwoOrMore (path, mask, _) -> (path, mask, "*.*")
  | _ -> invalidArg "commandLineArgs" ("Invalid command line arguments. "
                                       + "Expecting at least two arguments.")
```

