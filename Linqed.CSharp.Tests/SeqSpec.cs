using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Linqed.Prelude;

namespace Linqed.CSharpTests {
  [TestClass]
  public class SeqSpec {
    [TestMethod]
    public void ConstructsAnEnumerableFromValuesPassedIn() { 
      var myValues = Seq(1, 2, 3, 4);
      myValues.ShouldSatisfy(they => they.SequenceEqual(new[] { 1, 2, 3, 4 }));
    }
  }
}
