using Zinnia.Extension;

namespace Test.Zinnia.Extension
{
    using NUnit.Framework;
    using UnityEngine;

    public class LayerMaskExtensionsTest
    {
        private LayerMask subject;

        [SetUp]
        public void SetUp()
        {
            subject = new LayerMask();
        }

        [Test]
        public void SetByInt()
        {
            Assert.IsFalse(subject == (subject | (1 << 1)));
            Assert.IsFalse(subject == (subject | (1 << 2)));

            subject = subject.Set(1);

            Assert.IsTrue(subject == (subject | (1 << 1)));
            Assert.IsFalse(subject == (subject | (1 << 2)));

            subject = subject.Set(2);

            Assert.IsFalse(subject == (subject | (1 << 1)));
            Assert.IsTrue(subject == (subject | (1 << 2)));
        }

        [Test]
        public void SetByString()
        {
            Assert.IsFalse(subject == (subject | (1 << 1)));
            Assert.IsFalse(subject == (subject | (1 << 2)));

            subject = subject.Set("TransparentFX");

            Assert.IsTrue(subject == (subject | (1 << 1)));
            Assert.IsFalse(subject == (subject | (1 << 2)));

            subject = subject.Set("Ignore Raycast");

            Assert.IsFalse(subject == (subject | (1 << 1)));
            Assert.IsTrue(subject == (subject | (1 << 2)));
        }
    }
}