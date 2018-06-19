namespace Linqed

[<AutoOpen>]
module Match =
  /// <summary>
  /// Matches a seq that has at least one element.  On a match, returns a tuple with
  /// the head and tail of the seq.
  /// </summary>
  let (|SeqOneOrMore|_|) xs = 
    Seq.tryHead xs 
    |> Option.map (fun head -> (head, Seq.skip 1 xs))

  /// <summary>
  /// Matches a seq that has at least two elements.  On a match, returns a tuple with
  /// the two heads and the tail of the seq.
  /// </summary>
  let (|SeqTwoOrMore|_|) xs =
    let truncated = Seq.truncate 2 xs |> Seq.toList
    match truncated with
    | [head1; head2] -> Some (head1, head2, Seq.skip 2 xs)
    | _ -> None

  /// <summary>
  /// Matches a seq that has at least three elements.  On a match, returns a tuple with
  /// the three heads and the tail of the seq.
  /// </summary>
  let (|SeqThreeOrMore|_|) xs =
    let truncated = Seq.truncate 3 xs |> Seq.toList
    match truncated with
    | [head1; head2; head3] -> Some (head1, head2, head3, Seq.skip 3 xs)
    | _ -> None

  /// <summary>
  /// Matches a seq that has at least four elements.  On a match, returns a tuple with
  /// the four heads and the tail of the seq.
  /// </summary>
  let (|SeqFourOrMore|_|) xs =
    let truncated = Seq.truncate 4 xs |> Seq.toList
    match truncated with
    | [head1; head2; head3; head4] -> Some (head1, head2, head3, head4, Seq.skip 4 xs)
    | _ -> None

  /// <summary>
  /// Matches a seq that has at least five elements.  On a match, returns a tuple with
  /// the five heads and the tail of the seq.
  /// </summary>
  let (|SeqFiveOrMore|_|) xs =
    let truncated = Seq.truncate 5 xs |> Seq.toList
    match truncated with
    | [head1; head2; head3; head4; head5] -> Some (head1, head2, head3, head4, head5, Seq.skip 5 xs)
    | _ -> None
