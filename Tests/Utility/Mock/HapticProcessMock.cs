using Zinnia.Haptics;

namespace Test.Zinnia.Utility.Mock
{
    using UnityEngine;

    [AddComponentMenu("")]
    public class HapticProcessMock : HapticProcess
    {
        protected override void DoBegin() { }
        protected override void DoCancel() { }
    }
}