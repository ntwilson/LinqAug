using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Linqed.Prelude;

namespace Linqed.CSharpTests {
  [TestFixture]
  public class ToDictionarySpec {

    [Test]
    public void ConstructsADictionaryFromAnEnumerableOfTuples() { 
      Seq(
        (1, "hi"),
        (2, "hello")
      ).ToDictionary().ShouldSatisfy(it => it.SequenceEqual(
        new Dictionary<int, string>() { 
          { 1, "hi" },
          { 2, "hello" },
        }
      ));
    }
  }
}
