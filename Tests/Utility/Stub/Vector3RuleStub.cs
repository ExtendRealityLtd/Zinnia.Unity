using Zinnia.Rule;
using Zinnia.Extension;

namespace Test.Zinnia.Utility.Stub
{
    using UnityEngine;

    [AddComponentMenu("")]
    public class Vector3RuleStub : Vector3Rule
    {
        public Vector3 toMatch;

        protected override bool Accepts(Vector3 targetVector3)
        {
            return toMatch.ApproxEquals(targetVector3);
        }
    }
}