using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Linqed.Prelude;

namespace Linqed.CSharpTests {
  [TestClass]
  public class ListSpec {
    [TestMethod]
    public void ConstructsAListFromValuesPassedIn() { 
      var myValues = List(1, 2, 3, 4);
      myValues.ShouldSatisfy(they => they.SequenceEqual(new[] { 1, 2, 3, 4 }));
    }
  }
}
