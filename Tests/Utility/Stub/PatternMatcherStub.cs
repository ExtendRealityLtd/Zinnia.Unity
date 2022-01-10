using Zinnia.Pattern;

namespace Test.Zinnia.Utility.Stub
{
    using UnityEngine;

    [AddComponentMenu("")]
    public class PatternMatcherStub : PatternMatcher
    {
        public string source;

        public void MockEnable()
        {
            OnEnable();
        }

        protected override string DefineSourceString()
        {
            return source;
        }
    }
}