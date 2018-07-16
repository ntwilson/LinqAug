namespace Linqed

module Seq = 

  /// <summary>
  /// Add a single element to the end of the input sequence.  
  /// </summary  
  let inline add x xs = Seq.append xs [x]

  /// <summary>
  /// Add a single element to the beginning of the input sequence.  
  /// </summary  
  let inline cons head tail = Seq.append [head] tail

  /// <summary>
  /// Determines if the two sequences are equal according to the
  /// equality comparer that is given.
  /// </summary>
  let equalsWith comparer (xs:'a seq) (ys:'a seq) =
    Seq.forall2 comparer xs ys
    && 
    Seq.length xs = Seq.length ys

  /// <summary>
  /// Split a sequence into a tuple of its head and tail, i.e., its
  /// first element, and the remaining elements.
  /// Note this will produce the head element twice, so if elements are computationally
  /// expensive to produce, it is recommended that you cache the sequence first.
  /// Exceptions:
  /// <c>System.ArgumentException</c>: Thrown when the input does not have any elements.
  /// </summary>
  let inline uncons xs = Seq.head xs, Seq.tail xs

  /// <summary>
  /// Split a sequence into a tuple of its head and tail, i.e., its
  /// first element, and the remaining elements.
  /// Note this will produce the head element twice, so if elements are computationally
  /// expensive to produce, it is recommended that you cache the sequence first.
  /// Returns an <c>Error</c> when the input does not have any elements.
  /// </summary>
  let unconsSafe xs = 
    if Seq.isEmpty xs
    then Error "Cannot extract the first element of the input sequence, since it is empty"
    else Ok (uncons xs)

  /// <summary>
  /// Split a sequence into a tuple of its head and tail, i.e., its
  /// first element, and the remaining elements.
  /// Note this will produce the head element twice, so if elements are computationally
  /// expensive to produce, it is recommended that you cache the sequence first.
  /// Returns an <c>Error</c> when the input does not have any elements.
  /// </summary>
  let inline uncons' xs = unconsSafe xs  

  /// <summary>
  /// Splits a sequence at every occurrence of an element satisfying <c>isSplitElement</c>.
  /// The split occurs immediately before each element that satisfies <c>isSplitElement</c>,
  /// and the element satisfying <c>isSplitElement</c> will be included as the first element of 
  /// the sequence following the split.
  /// Returning a two dimensional sequence, <c>split</c> is guaranteed to return at least one 
  /// element in the outer sequence, though that element may be an empty sequence.
  /// For example:
  /// <code>
  /// Seq.split ((=) 100) [1;2;3;100;100;4;100;5;6]
  ///   //returns [[1;2;3];[100];[100;4];[100;5;6]]
  /// </code>
  /// </summary>
  let split isSplitElement xs = 
    let notSplitElement = not << isSplitElement
    let singleSplit input = (Seq.takeWhile notSplitElement input, Seq.skipWhile notSplitElement input)

    let rec split' input =
      seq { 
        match uncons' input with
        | Ok (head, tail) ->        
          let contiguous, restOfInput = singleSplit tail
          let nextChunk = Seq.append [head] contiguous
          yield nextChunk
          yield! split' restOfInput
        | Error _ -> ()        
      }

    let upToFirstSplit, remainder = singleSplit xs
    in cons upToFirstSplit (split' remainder)


  let private uncurry f (a, b) = f a b

  /// <summary>
  /// Splits a sequence between each pair of adjacent elements that satisfy <c>splitBetween</c>.
  /// Returning a two dimensional sequence, <c>splitPairwise</c> is guaranteed to return at least 
  /// one element in the outer sequence, though that element may be an empty sequence.
  /// For example:
  /// <code>
  /// Seq.splitPairwise (=) [1;1;2;3;4;4;4;5]
  ///   //returns [[0;1];[1;2;3;4];[4];[4;5]]
  /// </code>
  /// </summary>
  let splitPairwise splitBetween xs =
    if not (Seq.isEmpty xs)
    then 
      let splitChunks = 
        Seq.pairwise xs
        |> split (uncurry splitBetween)
        |> Seq.map (Seq.map snd)
      let firstGroup = cons (Seq.head xs) (Seq.head splitChunks)
      in cons firstGroup (Seq.tail splitChunks)
    else seq { yield Seq.empty }
