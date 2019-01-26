using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Linqed.Prelude;

namespace Linqed.CSharpTests {
  [TestFixture]
  public class DictSpec {
    [Test]
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
