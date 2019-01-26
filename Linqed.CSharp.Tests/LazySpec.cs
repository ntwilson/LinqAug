using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Linqed.Prelude;

namespace Linqed.CSharpTests {
  [TestFixture]
  public class LazySpec {
    [Test]
    public void ConstructsALazyFromAFunc() { 
      Lazy<string> myLazy = LazyValue(() => "hello");
      myLazy.Value.ShouldBe("hello");
    }
  }
}
