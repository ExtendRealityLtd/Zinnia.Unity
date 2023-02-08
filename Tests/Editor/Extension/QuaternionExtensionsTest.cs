using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.TestTools;
    using UnityEngine.TestTools.Utils;

    public class QuaternionExtensionsTest
    {
        [Test]
        public void ApproxEquals()
        {
            Quaternion a = Quaternion.identity;
            Quaternion b = Quaternion.identity;

            Assert.IsTrue(a.ApproxEquals(b));

            a = new Quaternion(1f, 1f, 1f, 1f);
            b = new Quaternion(1f, 1f, 1f, 1f);

            Assert.IsTrue(a.ApproxEquals(b));

            a = Quaternion.identity;
            b = new Quaternion(1f, 1f, 1f, 1f);

            Assert.IsFalse(a.ApproxEquals(b));

            a = new Quaternion(1f, 1f, 1f, 1f);
            b = new Quaternion(0.999f, 0.999f, 0.999f, 0.999f);

            Assert.IsFalse(a.ApproxEquals(b));

            a = new Quaternion(1f, 1f, 1f, 1f);
            b = new Quaternion(0.999f, 0.999f, 0.999f, 0.999f);

            Assert.IsTrue(a.ApproxEquals(b, 0.001f));
        }

        [UnityTest]
        public IEnumerator SmoothDamp()
        {
            Quaternion current = Quaternion.identity;
            Quaternion target = new Quaternion(10f, 10f, 10f, 10f);
            Quaternion derivative = Quaternion.identity;
            Quaternion expected = Quaternion.identity;
            Quaternion result = Quaternion.identity;
            float duration = 1f;
            QuaternionEqualityComparer comparer = new QuaternionEqualityComparer(0.1f);

            Assert.That(result, Is.EqualTo(expected).Using(comparer));

            yield return null;

            result = QuaternionExtensions.SmoothDamp(current, target, ref derivative, duration);

            Assert.That(result, Is.EqualTo(expected).Using(comparer));

            yield return new WaitForSeconds(0.5f);

            expected = new Quaternion(0.002568f, 0.002568f, 0.002568f, 0.999990f);
            result = QuaternionExtensions.SmoothDamp(current, target, ref derivative, duration);

            Assert.That(result, Is.EqualTo(expected).Using(comparer));

            yield return new WaitForSeconds(0.5f);

            expected = new Quaternion(0.004397f, 0.004397f, 0.004397f, 0.999970f); ;
            result = QuaternionExtensions.SmoothDamp(current, target, ref derivative, duration);

            Assert.That(result, Is.EqualTo(expected).Using(comparer));
        }
    }
}