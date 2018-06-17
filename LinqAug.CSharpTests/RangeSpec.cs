using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using LinqAug;

namespace LinqAug.CSharpTests {
  [TestClass]
  public class RangeSpec {
    
    [TestMethod]
    public void BuildsASimpleRange() {
      1.To(4).ShouldSatisfy(it => it.SequenceEqual(new[] { 1,2,3,4 }));
    }
  }

  public static class Should {
    public static void ShouldBe<T>(this T a, T b) {
      Assert.AreEqual(a, b);
    }

    public static void ShouldSatisfy<T>(this T a, Func<T, bool> condition) {
      Assert.IsTrue(condition(a));
    }
  }
}
