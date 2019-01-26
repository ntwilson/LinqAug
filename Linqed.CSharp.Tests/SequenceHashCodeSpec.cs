using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Linqed;
using static Linqed.Prelude;

namespace Linqed.CSharpTests {
  [TestFixture]
  public class SequenceHashCodeSpec {
    [Test]
    public void ComputesAHashCodeBasedOnValuesContainedInASequence() { 
      1.To(6).SequenceHashCode()
        .ShouldBe(1.To(6).SequenceHashCode());

      1.To(6).SequenceHashCode()
        .ShouldNotBe(1.To(7).SequenceHashCode());

      6.Step(-1).To(1).SequenceHashCode()
        .ShouldNotBe(1.To(6).SequenceHashCode());
    }
  }
}