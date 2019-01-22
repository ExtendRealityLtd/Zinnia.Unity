using Zinnia.Rule;

namespace Test.Zinnia.Utility.Stub
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