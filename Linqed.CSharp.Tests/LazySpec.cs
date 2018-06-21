using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Linqed.Prelude;

namespace Linqed.CSharpTests {
  [TestClass]
  public class LazySpec {
    [TestMethod]
    public void ConstructsALazyFromAFunc() { 
      Lazy<string> myLazy = LazyValue(() => "hello");
      myLazy.Value.ShouldBe("hello");
    }
  }
}
