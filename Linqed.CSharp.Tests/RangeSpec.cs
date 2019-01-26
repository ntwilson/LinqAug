using System;
using System.Linq;
using NUnit.Framework;

using Linqed;

namespace Linqed.CSharpTests {
  [TestFixture]
  public class RangeSpec {
    
    [Test]
    public void BuildsASimpleRange() {
      1.To(4).ShouldSatisfy(it => it.SequenceEqual(new[] { 1,2,3,4 }));
    }

    [Test]
    public void BuildsADoubleRange() { 
      1.0.To(3.0).ShouldSatisfy(it => it.SequenceEqual(new[] { 1.0, 2.0, 3.0 }));
    }

    [Test]
    public void BuildsARangeWithAStep() { 
      1.Step(2).To(7).ShouldSatisfy(it => it.SequenceEqual(new[] { 1, 3, 5, 7 }));
    }

    [Test]
    public void BuildsADoubleRangeWithAStep() {
      1.0.Step(0.5).To(3.0).ShouldSatisfy(it => it.SequenceEqual(new[] { 1.0, 1.5, 2.0, 2.5, 3.0 }));
    }
  }
}
