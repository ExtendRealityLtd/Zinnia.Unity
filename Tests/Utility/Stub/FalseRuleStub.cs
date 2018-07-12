using VRTK.Core.Rule;

namespace Test.VRTK.Core.Utility.Stub
{
    using UnityEngine;

    [AddComponentMenu("")]
    public class FalseRuleStub : IRule
    {
        public bool Accepts(object target)
        {
            return false;
        }
    }
}