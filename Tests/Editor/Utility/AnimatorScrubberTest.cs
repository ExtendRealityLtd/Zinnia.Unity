using Zinnia.Extension;
using Zinnia.Utility;

namespace Test.Zinnia.Utility
{
    using NUnit.Framework;
    using System.Collections;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.TestTools;
    using UnityEngine.TestTools.Utils;

    public class AnimatorScrubberTest
    {
        private GameObject containingObject;
        private AnimatorScrubber subject;
        private Animator animator;

        [SetUp]
        public void SetUp()
        {
            containingObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            containingObject.name = "AnimatorScrubberTest";
            containingObject.SetActive(false);
            animator = containingObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath("Assets/Zinnia.Unity/Tests/TestResources/Animation/TestAnimatorController.controller", typeof(RuntimeAnimatorController));
            subject = containingObject.AddComponent<AnimatorScrubber>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(containingObject);
        }

        [UnityTest]
        public IEnumerator Scrub()
        {
            Vector3EqualityComparer comparer = new Vector3EqualityComparer(0.1f);
            subject.Timeline = animator;
            subject.AnimationName = "TestAnimation";
            containingObject.SetActive(true);

            Assert.AreEqual(Vector3.zero, containingObject.transform.position);

            subject.Scrub(0.2f);
            yield return new WaitForEndOfFrame();
            Assert.That(containingObject.transform.position, Is.EqualTo(Vector3.one * 0.2f).Using(comparer));

            subject.Scrub(0.5f);
            yield return new WaitForEndOfFrame();
            Assert.That(containingObject.transform.position, Is.EqualTo(Vector3.one * 0.5f).Using(comparer));

            subject.Scrub(0.9f);
            yield return new WaitForEndOfFrame();
            Assert.That(containingObject.transform.position, Is.EqualTo(Vector3.one * 0.9f).Using(comparer));

            subject.Scrub(1f);
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(Vector3.zero, containingObject.transform.position);
        }

        [UnityTest]
        public IEnumerator ScrubNoAnimator()
        {
            subject.AnimationName = "TestAnimation";
            animator.speed = 0;
            containingObject.SetActive(true);

            Assert.AreEqual(Vector3.zero, containingObject.transform.position);
            subject.Scrub(0.2f);
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(containingObject.transform.position.ApproxEquals(Vector3.zero, 0.1f));
        }

        [UnityTest]
        public IEnumerator ScrubNoAnimationName()
        {
            subject.Timeline = animator;
            containingObject.SetActive(true);

            Assert.AreEqual(Vector3.zero, containingObject.transform.position);
            subject.Scrub(0.2f);
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(containingObject.transform.position.ApproxEquals(Vector3.zero, 0.1f));
        }

        [UnityTest]
        public IEnumerator ScrubNoInactiveComponent()
        {
            subject.Timeline = animator;
            subject.AnimationName = "TestAnimation";
            subject.enabled = false;
            containingObject.SetActive(true);

            Assert.AreEqual(Vector3.zero, containingObject.transform.position);
            subject.Scrub(0.2f);
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(containingObject.transform.position.ApproxEquals(Vector3.zero, 0.1f));
        }
    }
}