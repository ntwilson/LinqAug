# LINQED
LINQ Extended Design

This library contains additional extension methods for C# Enumerables inspired by LINQ, as well as extensions to the F# Seq, List, and Array modules.

## C# Extensions

### Extension Methods

The following are all extension methods available on existing .NET types.

#### `.ChunkBySize`

Divides an IEnumerable into chunks of size at most chunkSize.  For example:

```c#
Enumerable.Range(1, 10).ChunkBySize(3)
```

will return an `IEnumerable<Array<int>>` containing the values: 

`[ [ 1, 2, 3 ], [ 4, 5, 6 ], [ 7, 8, 9 ], [ 10 ] ]`



#### `.DistinctBy`

Returns a sequence that contains no duplicate entries according to the generic hash and equality comparisons on the keys returned by the given key-generating function.  If an element occurs multiple times in the sequence then the later occurrences are discarded.  For example:

```C#
var allCustomers = allSales.Select(sale => sale.Customer).DistinctBy(cust => cust.ID)
```



#### `.EqualsWith`

Determines if the two sequences are equal according to equality comparer that is given.

```C#
if (xs.EqualsWith(new[] { 1.0, 2.0, 3.0 }, (x, y) => Math.Abs(x - y) < 1E-9))
```



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



There are overloads for matching with at least n elements (shown above), or matching with exactly n elements:

```C#
var (pathToLook, fileNameMask, extensionFilter) = 
  commandLineArgs.Match(
    exactlyThree: (path, mask, filter) => (path, mask, filter),
    exactlyTwo: (path, mask) => (path, mask, "*.*"),
    otherwise: _ => throw new ArgumentException("Invalid command line arguments. "
                                                + "Expecting at least two arguments.")
  );
```



#### `.SequenceHashCode`

Computes the hash code of an IEnumerable based on its _contents_ rather than on its memory address.  So two sequences that contain the same elements will have the same hash code from this function.



#### `.SequenceString`

Returns a string representation of the given sequence.  For example, 

`new List<int> { 1, 2, 3, 4 }.ToString()`

will return "System.Collections.Generic.List`1[System.Int32]" but

`new List<int> { 1, 2, 3, 4 }.SequenceString()`  

will return "seq [1; 2; 3; 4]"

> Note: this is safe for use by infinite sequences, because it will only print out the first few elements of a lazy sequence



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



#### `.Windowed`

Returns a sequence that yields sliding windows containing elements drawn from the input sequence. Each window is returned as a fresh array.  For example:

```C#
Enumerable.Range(1, 5).Windowed(3)
```

will return an `IEnumerable<Array<int>>` containing the values:

`[ [ 1, 2, 3 ], [ 2, 3, 4 ], [ 3, 4, 5 ] ]`



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



#### `Array`

Similar to `Seq`, but creates an array.  Rather than typing out

`var xs = new[] { 1, 2, 3 };`

you can type:

`var xs = Array(1, 2, 3);`



#### `List`

Similar to `Seq`, but creates a `System.Collections.Generic.List<T>`.  Rather than typing out

`var xs = new List<int>() { 1, 2, 3 };`

you can type:

`var xs = List(1, 2, 3);`



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



#### `Seq/Array/List.equalsWith`

Determines if the two sequences are equal according to the equality comparer that is given.

```F#
if xs |> List.equalsWith (fun a b -> abs (a - b) < 1E-9) [ 1.0; 2.0; 3.0 ] then ...
```

