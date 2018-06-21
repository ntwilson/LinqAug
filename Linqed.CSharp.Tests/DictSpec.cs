using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Linqed.Prelude;

namespace Linqed.CSharpTests {
  [TestClass]
  public class DictSpec {
    [TestMethod]
    public void ConstructsADictionaryFromTuples() { 
      Dict(
        (1, "Hi"),
        (2, "Hello")
      ).ShouldSatisfy(it => 
        it.SequenceEqual(new Dictionary<int, string>() { 
          { 1, "Hi" },
          {2, "Hello" },
        }));
    }
  }
}
