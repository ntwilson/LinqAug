using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using LinqAug;

namespace LinqAug.CSharpTests {
  [TestClass]
  public class MatchSpec {
    
    [TestMethod]
    public void MatchesAnEnumerableOnWhetherItHasElementsOrNot() {
      var didExecuteInner = false;

      1.To(12).Match(
        (head, tail) => {
          didExecuteInner = true;
          head.ShouldBe(1);
          tail.ShouldSatisfy(it => it.SequenceEqual(2.To(12)));
        },
        _ => Assert.Fail("failed to match 1 .. 12 with head & tail")
      );

      didExecuteInner.ShouldBe(true);
    }

    [TestMethod]
    public void ReturnsAValueFromTheMatchIfOneIsGiven() {
      1.To(4).Match(
        (head, tail) => tail.Concat(new[] { head }),
        _ => new[] { 1 }
      ).ShouldSatisfy(it => it.SequenceEqual(new[] { 2, 3, 4, 1 }));
    }
  }
}
