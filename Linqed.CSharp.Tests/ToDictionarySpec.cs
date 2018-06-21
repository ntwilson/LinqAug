using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Linqed.Prelude;

namespace Linqed.CSharpTests {
  [TestClass]
  public class ToDictionarySpec {

    [TestMethod]
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
