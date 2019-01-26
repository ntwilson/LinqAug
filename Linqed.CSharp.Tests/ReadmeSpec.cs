using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Linqed.Prelude;

namespace Linqed.CSharpTests {
  [TestFixture]
  public class ReadmeSpec {
    [Test]
    public void ConstructsADictionaryLikeInTheReadme() { 
      var myComplexDict = Dict(
        (DateTime.Today, LazyValue(() => Seq(1.0, 2.0, 3.0))),
        (DateTime.Today.AddDays(1), LazyValue(() => Seq(4.0, 5.0, 6.0)))
      );

      var expectedDict = 
        new Dictionary<DateTime, Lazy<IEnumerable<double>>>() { 
          { DateTime.Today, new Lazy<IEnumerable<double>>(() => new[] { 1.0, 2.0, 3.0 }) },
          { DateTime.Today.AddDays(1), new Lazy<IEnumerable<double>>(() => new[] { 4.0, 5.0, 6.0 }) },
        };

      foreach (var key in myComplexDict.Keys) {
        myComplexDict[key].Value.ShouldSatisfy(it => it.SequenceEqual(
          expectedDict[key].Value));
      }
    }
  }
}
