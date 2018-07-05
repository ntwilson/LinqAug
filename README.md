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



#### `FiniteSeq`

There are three main sequence types in F#: Array, List, and Seq.  Working with Lists makes any interop with C# intolerable.  Arrays are mutable, eager, and certain operations (like `tail` or `cons`) perform poorly.  Of course, we can choose whichever of the two is better suited for our use upon creation, and then use `Seq` everywhere else, but the `Seq` provides many challenges because of the fact that it _could_ be infinite.  The worst is that it has reference equality instead of structural equality (which means if you include it in any records or union types, those types lose their structural equality as well).  There are also several functions in the `Seq` module that should never be called for an infinite sequence, and you will only discover a violation of this at runtime (like `length`, `fold`, `rev`, etc.).  This library introduces the FiniteSeq type for a lazily evaluated sequence that is constrained to be finite.  It has structural equality, and can be constructed from any other sequence in _O(1)_ time, just like a `seq`.  The functions in the FiniteSeq module are safe for use with any FiniteSeq.  It's built on top of the `LazyList` type provided by the [FSharpx.Collections](http://fsprojects.github.io/FSharpx.Collections/) package.

The FiniteSeq type also uses the alias `fseq` and the FiniteSeq module also uses the alias `FSeq`

For example:

```F#
type TimeSeries = 
  { 
    Times : DateTime fseq
    Values : float fseq
  }

let rng = new Random ()
let values = 
  [0 .. 100] |> Seq.map (fun _ -> rng.NextDouble() * 100.0) 
  |> fseq |> FSeq.filter (fun i -> i > 50.0)
let times = 
  fseq { 0.0 .. (float (FSeq.length values)) } 
  |> FSeq.map (fun i -> startTime.AddHours i)
let ts = { Times = times; Values = values}
if (ts = someOtherTimeSeries) then ...
```



#### `InfiniteSeq`

To go along with FiniteSeq, it's helpful to have a type for representing a sequence that's known to be infinite.  Some functions are excluded from the InfiniteSeq module (like `fold`), while others are known to be safe for use (like `head`).  An InfiniteSeq can be created with the `InfiniteSeq.init` function (which behaves exactly like `Seq.initInfinite`), and the functions in the `InfiniteSeq` module are safe to use for infinite sequences (well, as safe as you can be.  You can certainly construct a sequence that will hang forever as soon as you try to do anything useful with it, e.g., the sequence: 

`InfiniteSeq.init (fun _ -> 0) |> InfiniteSeq.filter ((<>) 0)` will hang if you were to use any eager calculations with it, like `take` or `head` or `find`).



#### `NonEmptySeq`

It's helpful to have a type representing a sequence that's known to be non-empty.  This type is built on top of FiniteSeq (if you're sequence is infinite, you should use the InfiniteSeq type instead).  There are two ways to get a NonEmptySeq.  One is a pattern matcher with any seq (though if the seq is infinite, be aware that the returned value is built on FiniteSeq and in danger of hanging infinitely for certain operations):

```F#
match xs with // `xs` is any seq, but should be finite
| Empty -> ...
| NotEmpty nes -> ... // `nes` is a NonEmptySeq
```

The other is to construct it with a head and tail

```F#
let nes = NonEmptySeq.create 0 (fseq {1 .. 10})
```

NonEmptySeq contains several functions that aren't included in the FiniteSeq module because they could throw, such as `head`, `tail`, `uncons`, and `reduce`.