using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Linqed;
using static Linqed.Prelude;

namespace Linqed.CSharpTests {
  [TestClass]
  public class SplitHeadAndTailSpec {
    [TestMethod]
    public void SplitsTheHeadAndTailOfNonEmptySequences() {
      var (head, tail) = Seq(1, 2, 3, 4).SplitHeadAndTail();
      head.ShouldBe(1);
      tail.ShouldSatisfy(it => it.SequenceEqual(Seq(2, 3, 4)));

      var result = Seq(1, 2, 3, 4).SplitHeadAndTailSafe();
      (head, tail) = result.Expect();
      head.ShouldBe(1);
      tail.ShouldSatisfy(it => it.SequenceEqual(Seq(2, 3, 4)));

      (head, tail) = Seq(1).SplitHeadAndTail();
      head.ShouldBe(1);
      tail.ShouldSatisfy(it => !it.Any());

      result = Seq(1).SplitHeadAndTailSafe();
      (head, tail) = result.Expect();
      head.ShouldBe(1);
      tail.ShouldSatisfy(it => !it.Any());
    }

    [TestMethod]
    public void ErrorsWhenSplittingTheHeadAndTailOfEmptySequences() { 
      Action act;
      act = () => Enumerable.Empty<int>().SplitHeadAndTail();
      act.ShouldThrow();

      var result = Enumerable.Empty<int>().SplitHeadAndTailSafe();
      result.ShouldSatisfy(it => it.IsError);
    }
  }
}
