using System;
using System.Linq;
using System.Collections.Generic;

namespace Linqed {
	public static class MatchExtensions {

		/// <summary>
		/// Matches an <c>IEnumerable&lt;T></c> with how many elements it has.  If the enumerable has 
		/// at least as many elements as the specified matching function, calls the matching function given 
		/// with the first elements of the enumerable.  It will call the function with the most matches
		/// possible.  So if you supply a <c>twoOrMore</c> function and a <c>fourOrMore</c> function, 
		/// it will give preference to the <c>fourOrMore</c> function if that matches.  
		/// Note: For readability, it is recommended that you use named arguments, and pass the 
		/// arguments in reverse order, starting with the most matches, and ending with the <c>otherwise</c>
		/// function.  This will list the matches in the order of precedence.
		/// For example:
		/// <code>
		/// var sumOfFirstTwoOrThree = 
		///   listOfInts.Match(
		///     threeOrMore: (a, b, c, rest) => a + b + c, 
		///     twoOrMore: (a, b, rest) => a + b,
		///     otherwise: _ => 0);
		/// </code>
		/// </summary>
		public static TOut Match<TIn, TOut>(this IEnumerable<TIn> xs, 
			Func<IEnumerable<TIn>, TOut> otherwise, 
			Func<TIn, IEnumerable<TIn>, TOut> oneOrMore = null,
			Func<TIn, TIn, IEnumerable<TIn>, TOut> twoOrMore = null,
			Func<TIn, TIn, TIn, IEnumerable<TIn>, TOut> threeOrMore = null,
			Func<TIn, TIn, TIn, TIn, IEnumerable<TIn>, TOut> fourOrMore = null,
			Func<TIn, TIn, TIn, TIn, TIn, IEnumerable<TIn>, TOut> fiveOrMore = null
		) {
			List<TIn> xsCached = null;
			if (fiveOrMore != null) {
				if (xsCached == null) xsCached = xs.Take(5).ToList();
				if (xsCached.Count == 5) 
					return fiveOrMore(xsCached[0], xsCached[1], xsCached[2], xsCached[3], xsCached[4], xs.Skip(5));
			}
			if (fourOrMore != null) {
				if (xsCached == null) xsCached = xs.Take(4).ToList();
				if (xsCached.Count == 4) 
					return fourOrMore(xsCached[0], xsCached[1], xsCached[2], xsCached[3], xs.Skip(4));
			}
			if (threeOrMore != null) {
				if (xsCached == null) xsCached = xs.Take(3).ToList();
				if (xsCached.Count == 3) 
					return threeOrMore(xsCached[0], xsCached[1], xsCached[2], xs.Skip(3));
			}
			if (twoOrMore != null) {
				if (xsCached == null) xsCached = xs.Take(2).ToList();
				if (xsCached.Count == 2) 
					return twoOrMore(xsCached[0], xsCached[1], xs.Skip(2));
			}
			if (oneOrMore != null) {
				if (xsCached == null) xsCached = xs.Take(1).ToList();
				if (xsCached.Count == 1) 
					return oneOrMore(xsCached[0], xs.Skip(1));
			}

			return otherwise(xs);
		}

		/// <summary>
		/// Matches an <c>IEnumerable&lt;T></c> with how many elements it has.  If the enumerable has 
		/// at least as many elements as the specified matching function, calls the matching function given 
		/// with the first elements of the enumerable.  It will call the function with the most matches
		/// possible.  So if you supply a <c>twoOrMore</c> function and a <c>fourOrMore</c> function, 
		/// it will give preference to the <c>fourOrMore</c> function if that matches.  
		/// Note: For readability, it is recommended that you use named arguments, and pass the 
		/// arguments in reverse order, starting with the most matches, and ending with the <c>otherwise</c>
		/// function.  This will list the matches in the order of precedence.
		/// For example:
		/// <code>
		/// listOfInts.Match(
		///   threeOrMore: (a, b, c, rest) => { Console.WriteLine(a + b + c); }, 
		///   twoOrMore: (a, b, rest) => { Console.WriteLine(a + b); },
		///   otherwise: _ => { Console.WriteLine("Wrong number of values"); });
		/// </code>
		/// </summary>
		public static void Match<TIn>(this IEnumerable<TIn> xs, 
			Action<IEnumerable<TIn>> otherwise, 
			Action<TIn, IEnumerable<TIn>> oneOrMore = null,
			Action<TIn, TIn, IEnumerable<TIn>> twoOrMore = null,
			Action<TIn, TIn, TIn, IEnumerable<TIn>> threeOrMore = null,
			Action<TIn, TIn, TIn, TIn, IEnumerable<TIn>> fourOrMore = null,
			Action<TIn, TIn, TIn, TIn, TIn, IEnumerable<TIn>> fiveOrMore = null
		) {
			List<TIn> xsCached = null;
			if (fiveOrMore != null) {
				if (xsCached == null) xsCached = xs.Take(5).ToList();
				if (xsCached.Count == 5) {
					fiveOrMore(xsCached[0], xsCached[1], xsCached[2], xsCached[3], xsCached[4], xs.Skip(5));
					return;
				}
			}
			if (fourOrMore != null) {
				if (xsCached == null) xsCached = xs.Take(4).ToList();
				if (xsCached.Count == 4) {
					fourOrMore(xsCached[0], xsCached[1], xsCached[2], xsCached[3], xs.Skip(4));
					return;
				}
			}
			if (threeOrMore != null) {
				if (xsCached == null) xsCached = xs.Take(3).ToList();
				if (xsCached.Count == 3) {
					threeOrMore(xsCached[0], xsCached[1], xsCached[2], xs.Skip(3));
					return;
				}
			}
			if (twoOrMore != null) {
				if (xsCached == null) xsCached = xs.Take(2).ToList();
				if (xsCached.Count == 2) {
					twoOrMore(xsCached[0], xsCached[1], xs.Skip(2));
					return;
				}
			}
			if (oneOrMore != null) {
				if (xsCached == null) xsCached = xs.Take(1).ToList();
				if (xsCached.Count == 1) {
					oneOrMore(xsCached[0], xs.Skip(1));
					return;
				}
			}

			otherwise(xs);
		}

		/// <summary>
		/// Matches an <c>IEnumerable&lt;T></c> with how many elements it has.  If the enumerable has 
		/// exactly as many elements as the specified matching function, calls the matching function given 
		/// with each element of the enumerable.  
		/// Note: For readability, it is recommended that you use named arguments, and pass the 
		/// arguments in reverse order, starting with the most matches, and ending with the <c>otherwise</c>
		/// function.
		/// For example:
		/// <code>
		/// var sumOfTwoOrThree = 
		///   listOfInts.Match(
		///     exactlyThree: (a, b, c) => a + b + c, 
		///     exactlyTwo: (a, b) => a + b,
		///     otherwise: _ => false);
		/// </code>
		/// </summary>
		public static TOut Match<TIn, TOut>(this IEnumerable<TIn> xs, 
			Func<IEnumerable<TIn>, TOut> otherwise, 
			Func<TIn, TOut> exactlyOne = null,
			Func<TIn, TIn, TOut> exactlyTwo = null,
			Func<TIn, TIn, TIn, TOut> exactlyThree = null,
			Func<TIn, TIn, TIn, TIn, TOut> exactlyFour = null,
			Func<TIn, TIn, TIn, TIn, TIn, TOut> exactlyFive = null
		) {
			List<TIn> xsCached = null;
			if (exactlyFive != null) {
				if (xsCached == null) xsCached = xs.Take(6).ToList();
				if (xsCached.Count == 5) 
					return exactlyFive(xsCached[0], xsCached[1], xsCached[2], xsCached[3], xsCached[4]);
			}
			if (exactlyFour != null) {
				if (xsCached == null) xsCached = xs.Take(5).ToList();
				if (xsCached.Count == 4) 
					return exactlyFour(xsCached[0], xsCached[1], xsCached[2], xsCached[3]);
			}
			if (exactlyThree != null) {
				if (xsCached == null) xsCached = xs.Take(4).ToList();
				if (xsCached.Count == 3) 
					return exactlyThree(xsCached[0], xsCached[1], xsCached[2]);
			}
			if (exactlyTwo != null) {
				if (xsCached == null) xsCached = xs.Take(3).ToList();
				if (xsCached.Count == 2) 
					return exactlyTwo(xsCached[0], xsCached[1]);
			}
			if (exactlyOne != null) {
				if (xsCached == null) xsCached = xs.Take(2).ToList();
				if (xsCached.Count == 1) 
					return exactlyOne(xsCached[0]);
			}

			return otherwise(xs);
		}

		/// <summary>
		/// Matches an <c>IEnumerable&lt;T></c> with how many elements it has.  If the enumerable has 
		/// exactly as many elements as the specified matching function, calls the matching function 
		/// given with each element of the enumerable.  
		/// Note: For readability, it is recommended that you use named arguments, and pass the 
		/// arguments in reverse order, starting with the most matches, and ending with the <c>otherwise</c>
		/// function.
		/// For example:
		/// <code>
		/// listOfInts.Match(
		///   exactlyThree: (a, b, c) => { Console.WriteLine(a + b + c); }, 
		///   exactlyTwo: (a, b) => { Console.WriteLine(a + b); },
		///   otherwise: _ => { Console.WriteLine("Wrong number of values"); });
		/// </code>
		/// </summary>
		public static void Match<TIn>(this IEnumerable<TIn> xs, 
			Action<IEnumerable<TIn>> otherwise, 
			Action<TIn> exactlyOne = null,
			Action<TIn, TIn> exactlyTwo = null,
			Action<TIn, TIn, TIn> exactlyThree = null,
			Action<TIn, TIn, TIn, TIn> exactlyFour = null,
			Action<TIn, TIn, TIn, TIn, TIn> exactlyFive = null
		) {
			List<TIn> xsCached = null;
			if (exactlyFive != null) {
				if (xsCached == null) xsCached = xs.Take(6).ToList();
				if (xsCached.Count == 5) {
					exactlyFive(xsCached[0], xsCached[1], xsCached[2], xsCached[3], xsCached[4]);
					return;
				}
			}
			if (exactlyFour != null) {
				if (xsCached == null) xsCached = xs.Take(5).ToList();
				if (xsCached.Count == 4) {
					exactlyFour(xsCached[0], xsCached[1], xsCached[2], xsCached[3]);
					return;
				}
			}
			if (exactlyThree != null) {
				if (xsCached == null) xsCached = xs.Take(4).ToList();
				if (xsCached.Count == 3) {
					exactlyThree(xsCached[0], xsCached[1], xsCached[2]);
					return;
				}
			}
			if (exactlyTwo != null) {
				if (xsCached == null) xsCached = xs.Take(3).ToList();
				if (xsCached.Count == 2) {
					exactlyTwo(xsCached[0], xsCached[1]);
					return;
				}
			}
			if (exactlyOne != null) {
				if (xsCached == null) xsCached = xs.Take(2).ToList();
				if (xsCached.Count == 1) {
					exactlyOne(xsCached[0]);
					return;
				}
			}

			otherwise(xs);
		}
	}
}
