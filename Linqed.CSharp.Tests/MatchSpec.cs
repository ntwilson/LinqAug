using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Linqed;

namespace Linqed.CSharpTests {
  [TestClass]
  public class MatchSpec {
    [TestMethod]
    public void MatchesAnEnumerableOnWhetherItHasElementsOrNot() {
      var didExecuteInner = false;

      1.To(12).Match(
        oneOrMore: (head, tail) => {
          didExecuteInner = true;
          head.ShouldBe(1);
          tail.ShouldSatisfy(it => it.SequenceEqual(2.To(12)));
        },
        otherwise: _ => Assert.Fail("failed to match 1 .. 12 with head & tail")
      );

      didExecuteInner.ShouldBe(true);
    }

    [TestMethod]
    public void ReturnsAValueFromTheMatchIfOneIsGiven() {
      1.To(4).Match(
        oneOrMore: (head, tail) => tail.Concat(new[] { head }),
        otherwise: _ => new[] { 1 }
      ).ShouldSatisfy(it => it.SequenceEqual(new[] { 2, 3, 4, 1 }));
    }

    [TestMethod]
    public void PassesTheWholeEnumerableToTheMismatchBranch() { 
      var didExecuteInner = false;
      0.To(-1).Match(
        oneOrMore: (head, tail) => Assert.Fail($"Unexpectedly matched an empty enumerable with ({head}, {tail})"),
        otherwise: xs => {
          didExecuteInner = true;
          xs.ShouldSatisfy(it => !it.Any());
        }
      );

      didExecuteInner.ShouldBe(true);
    }

    [TestMethod]
    public void AllowsADefaultValueOnAMismatch() { 
      0.To(-1).Match(
        oneOrMore: (head, tail) => head,
        otherwise: _ => 5
      ).ShouldBe(5);
    }

    
    [TestMethod]
    public void MatchesAnEnumerableWithTwoElements() {
      var didExecuteInner = false;

      1.To(12).Match(
        twoOrMore: (head1, head2, tail) => {
          didExecuteInner = true;
          head1.ShouldBe(1);
          head2.ShouldBe(2);
          tail.ShouldSatisfy(it => it.SequenceEqual(3.To(12)));
        },
        otherwise: _ => Assert.Fail("failed to match 1 .. 12 with 2 heads & tail")
      );

      didExecuteInner.ShouldBe(true);
    }

    [TestMethod]
    public void ReturnsAValueFromATwoHeadMatchIfOneIsGiven() {
      1.To(4).Match(
        twoOrMore: (head1, head2, tail) => tail.Concat(new[] { head1, head2 }),
        otherwise: _ => new[] { 1 }
      ).ShouldSatisfy(it => it.SequenceEqual(new[] { 3, 4, 1, 2 }));
    }

    [TestMethod]
    public void PassesTheWholeEnumerableToThe2HeadMismatchBranch() { 
      var didExecuteInner = false;
      1.To(1).Match(
        twoOrMore: (head1, head2, tail) => Assert.Fail($"Unexpectedly matched an enumerable that only has one element with ({head1}, {head2}, {tail})"),
        otherwise: xs => {
          didExecuteInner = true;
          xs.ShouldSatisfy(it => it.SequenceEqual(new[] { 1 }));
        }
      );

      didExecuteInner.ShouldBe(true);
    }

    [TestMethod]
    public void AllowsADefaultValueOnA2HeadMismatch() { 
      1.To(1).Match(
        twoOrMore: (head1, head2, tail) => head1,
        otherwise: _ => 5
      ).ShouldBe(5);
    }

    [TestMethod]
    public void LetsYouSpecifyMultipleMatches() { 
      1.To(4).Match(
        fourOrMore: (a, b, c, d, tail) => true,
        threeOrMore: (a, b, c, tail) => false,
        otherwise: _ => false
      ).ShouldBe(true);

      1.To(3).Match(
        fourOrMore: (a, b, c, d, tail) => false,
        threeOrMore: (a, b, c, tail) => true,
        otherwise: _ => false
      ).ShouldBe(true);
    }

    [TestMethod]
    public void CanMatchExactlyANumberOfElements() { 
      1.To(3).Match(
        exactlyTwo: (a, b) => false,
        exactlyThree: (a, b, c) => a + b + c == 6,
        exactlyFour: (a, b, c, d) => false,
        otherwise: _ => false
      ).ShouldBe(true);

      1.To(4).Match(
        exactlyTwo: (a, b) => false,
        exactlyThree: (a, b, c) => false,
        otherwise: _ => true
      ).ShouldBe(true);
    }

    [TestMethod]
    public void CanMatchExactCountWithAnActionInsteadOfFunc() { 
      var ran = false;
      1.To(3).Match(
        exactlyTwo: (a, b) => { Assert.Fail(); },
        exactlyThree: (a, b, c) => { ran = true; },
        exactlyFour: (a, b, c, d) => { Assert.Fail(); },
        otherwise: _ => { Assert.Fail(); }
      );

      ran.ShouldBe(true);
      ran = false;

      1.To(4).Match(
        exactlyTwo: (a, b) => { Assert.Fail(); },
        exactlyThree: (a, b, c) => { Assert.Fail(); },
        otherwise: _ => { ran = true; }
      );
      ran.ShouldBe(true);
    }
  }
}
