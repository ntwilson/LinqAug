namespace LinqAug

open System.Collections.Generic
open System.Runtime.CompilerServices

[<Extension>]
module Range = 
  [<Extension>]
  let To (a:int) (b:int) : IEnumerable<int> =
    upcast [ a .. b ]
