using Zinnia.Data.Type;

namespace Test.Zinnia.Data.Type
{
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools.Utils;

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
        public void DefaultStateXYAxis()
        {
            Vector3State actualResult = Vector3State.XYOnly;
            Assert.IsTrue(actualResult.xState);
            Assert.IsTrue(actualResult.yState);
            Assert.IsFalse(actualResult.zState);
        }

        [Test]
        public void DefaultStateXZAxis()
        {
            Vector3State actualResult = Vector3State.XZOnly;
            Assert.IsTrue(actualResult.xState);
            Assert.IsFalse(actualResult.yState);
            Assert.IsTrue(actualResult.zState);
        }

        [Test]
        public void DefaultStateYZAxis()
        {
            Vector3State actualResult = Vector3State.YZOnly;
            Assert.IsFalse(actualResult.xState);
            Assert.IsTrue(actualResult.yState);
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
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            Assert.That(new Vector3State(false, false, false).ToVector3(), Is.EqualTo(new Vector3(0f, 0f, 0f)).Using(comparer));
            Assert.That(new Vector3State(true, true, true).ToVector3(), Is.EqualTo(new Vector3(1f, 1f, 1f)).Using(comparer));
            Assert.That(new Vector3State(true, false, true).ToVector3(), Is.EqualTo(new Vector3(1f, 0f, 1f)).Using(comparer));
            Assert.That(new Vector3State(false, true, false).ToVector3(), Is.EqualTo(new Vector3(0f, 1f, 0f)).Using(comparer));
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