using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Type
{
    using NUnit.Framework;
    using UnityEngine;
    using Assert = UnityEngine.Assertions.Assert;

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
        public void DefaultStateXAxis()
        {
            Vector3State actualResult = Vector3State.XOnly;
            Assert.IsTrue(actualResult.xState);
            Assert.IsFalse(actualResult.yState);
            Assert.IsFalse(actualResult.zState);
        }

        [Test]
        public void DefaultStateYAxis()
        {
            Vector3State actualResult = Vector3State.YOnly;
            Assert.IsFalse(actualResult.xState);
            Assert.IsTrue(actualResult.yState);
            Assert.IsFalse(actualResult.zState);
        }

        [Test]
        public void DefaultStateZAxis()
        {
            Vector3State actualResult = Vector3State.ZOnly;
            Assert.IsFalse(actualResult.xState);
            Assert.IsFalse(actualResult.yState);
            Assert.IsTrue(actualResult.zState);
        }

        [Test]
        public void CustomInitialState()
        {
            Vector3State actualResult = new Vector3State(false, true, false);
            Assert.IsFalse(actualResult.xState);
            Assert.IsTrue(actualResult.yState);
            Assert.IsFalse(actualResult.zState);
        }

        [Test]
        public void ToVector3()
        {
            Assert.AreEqual(new Vector3(0f, 0f, 0f), new Vector3State(false, false, false).ToVector3());
            Assert.AreEqual(new Vector3(1f, 1f, 1f), new Vector3State(true, true, true).ToVector3());
            Assert.AreEqual(new Vector3(1f, 0f, 1f), new Vector3State(true, false, true).ToVector3());
            Assert.AreEqual(new Vector3(0f, 1f, 0f), new Vector3State(false, true, false).ToVector3());
        }

        [Test]
        public void Comparison()
        {
            Vector3State stateA = Vector3State.True;
            Vector3State stateB = Vector3State.True;
            Vector3State stateC = Vector3State.False;

            Assert.IsTrue(stateA.Equals(stateB));
            Assert.IsFalse(stateA.Equals(stateC));
            Assert.IsFalse(stateB.Equals(stateC));
            Assert.AreEqual(stateA, stateB);
            Assert.AreNotEqual(stateA, stateC);
            Assert.AreNotEqual(stateB, stateC);
        }

        [Test]
        public void ConvertToString()
        {
            Vector3State rangeA = new Vector3State(true, false, true);
            Assert.AreEqual("{ xState = True | yState = False | zState = True }", rangeA.ToString());
        }
    }
}