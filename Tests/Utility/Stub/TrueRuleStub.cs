using VRTK.Core.Rule;

namespace Test.VRTK.Core.Utility.Stub
{
    using UnityEngine;

    [AddComponentMenu("")]
    public class TrueRuleStub : IRule
    {
        public bool Accepts(object target)
        {
            return true;
        }
    }
}