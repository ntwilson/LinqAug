using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Linqed.Prelude;

namespace Linqed.CSharpTests {
  [TestFixture]
  public class ArraySpec {
    [Test]
    public void ConstructsAnArrayFromValuesPassedIn() { 
      var myValues = Array(1, 2, 3, 4);
      myValues.ShouldSatisfy(they => they.SequenceEqual(new[] { 1, 2, 3, 4 }));
    }
  }
}
