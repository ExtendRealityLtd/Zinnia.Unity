namespace VRTK.Core.Data.Type
{
    using NUnit.Framework;

    public class Vector3StateTest
    {
        [Test]
        public void DefaultStateTrue()
        {
            Vector3State actualResult = Vector3State.True;
            Assert.IsTrue(actualResult.xState);
            Assert.IsTrue(actualResult.yState);
            Assert.IsTrue(actualResult.zState);
        }

        [Test]
        public void DefaultStateFalse()
        {
            Vector3State actualResult = Vector3State.False;
            Assert.IsFalse(actualResult.xState);
            Assert.IsFalse(actualResult.yState);
            Assert.IsFalse(actualResult.zState);
        }

        [Test]
        public void CustomInitialState()
        {
            Vector3State actualResult = new Vector3State(false, true, false);
            Assert.IsFalse(actualResult.xState);
            Assert.IsTrue(actualResult.yState);
            Assert.IsFalse(actualResult.zState);
        }
    }
}