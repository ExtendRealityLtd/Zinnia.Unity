using Zinnia.Rule;

namespace Test.Zinnia.Utility.Stub
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