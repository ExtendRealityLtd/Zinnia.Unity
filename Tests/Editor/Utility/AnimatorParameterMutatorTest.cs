using Zinnia.Utility;

namespace Test.Zinnia.Utility
{
    using NUnit.Framework;
    using UnityEditor;
    using UnityEngine;

    public class AnimatorParameterMutatorTest
    {
        private GameObject containingObject;
        private AnimatorParameterMutator subject;
        private Animator animator;

        [SetUp]
        public void SetUp()
        {
            containingObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            containingObject.name = "AnimatorParameterMutatorTest";
            animator = containingObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath("Assets/Zinnia.Unity/Tests/TestResources/Animation/TestAnimatorController.controller", typeof(RuntimeAnimatorController));
            subject = containingObject.AddComponent<AnimatorParameterMutator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(containingObject);
        }

        [Test]
        public void FloatValue()
        {
            subject.Timeline = animator;
            subject.ParameterName = "FloatTest";
            Assert.AreEqual(0f, animator.GetFloat("FloatTest"));

            subject.FloatValue = 1f;

            Assert.AreEqual(1f, animator.GetFloat("FloatTest"));
            Assert.AreEqual(1f, subject.FloatValue);
        }

        [Test]
        public void FloatValueNoAnimator()
        {
            subject.ParameterName = "FloatTest";
            Assert.AreEqual(0f, animator.GetFloat("FloatTest"));

            subject.FloatValue = 1f;

            Assert.AreEqual(0f, animator.GetFloat("FloatTest"));
            Assert.AreEqual(0f, subject.FloatValue);
        }

        [Test]
        public void FloatValueNoParameter()
        {
            subject.Timeline = animator;
            Assert.AreEqual(0f, animator.GetFloat("FloatTest"));

            subject.FloatValue = 1f;

            Assert.AreEqual(0f, animator.GetFloat("FloatTest"));
            Assert.AreEqual(0f, subject.FloatValue);
        }

        [Test]
        public void FloatValueInactiveGameObject()
        {
            subject.Timeline = animator;
            subject.ParameterName = "FloatTest";
            subject.gameObject.SetActive(false);
            Assert.AreEqual(0f, animator.GetFloat("FloatTest"));

            subject.FloatValue = 1f;

            Assert.AreEqual(0f, animator.GetFloat("FloatTest"));
            Assert.AreEqual(0f, subject.FloatValue);
        }

        [Test]
        public void FloatValueInactiveComponent()
        {
            subject.Timeline = animator;
            subject.ParameterName = "FloatTest";
            subject.enabled = false;
            Assert.AreEqual(0f, animator.GetFloat("FloatTest"));

            subject.FloatValue = 1f;

            Assert.AreEqual(0f, animator.GetFloat("FloatTest"));
            Assert.AreEqual(0f, subject.FloatValue);
        }

        [Test]
        public void IntegerValue()
        {
            subject.Timeline = animator;
            subject.ParameterName = "IntegerTest";
            Assert.AreEqual(0, animator.GetInteger("IntegerTest"));

            subject.IntegerValue = 1;

            Assert.AreEqual(1, animator.GetInteger("IntegerTest"));
            Assert.AreEqual(1, subject.IntegerValue);
        }

        [Test]
        public void IntegerValueNoAnimator()
        {
            subject.ParameterName = "IntegerTest";
            Assert.AreEqual(0, animator.GetInteger("IntegerTest"));

            subject.IntegerValue = 1;

            Assert.AreEqual(0, animator.GetInteger("IntegerTest"));
            Assert.AreEqual(0, subject.IntegerValue);
        }

        [Test]
        public void IntegerValueNoParameter()
        {
            subject.Timeline = animator;
            Assert.AreEqual(0, animator.GetInteger("IntegerTest"));

            subject.IntegerValue = 1;

            Assert.AreEqual(0, animator.GetInteger("IntegerTest"));
            Assert.AreEqual(0, subject.IntegerValue);
        }

        [Test]
        public void IntegerValueInactiveGameObject()
        {
            subject.Timeline = animator;
            subject.ParameterName = "IntegerTest";
            subject.gameObject.SetActive(false);
            Assert.AreEqual(0, animator.GetInteger("IntegerTest"));

            subject.IntegerValue = 1;

            Assert.AreEqual(0, animator.GetInteger("IntegerTest"));
            Assert.AreEqual(0, subject.IntegerValue);
        }

        [Test]
        public void IntegerValueInactiveComponent()
        {
            subject.Timeline = animator;
            subject.ParameterName = "IntegerTest";
            subject.enabled = false;
            Assert.AreEqual(0, animator.GetInteger("IntegerTest"));

            subject.IntegerValue = 1;

            Assert.AreEqual(0, animator.GetInteger("IntegerTest"));
            Assert.AreEqual(0, subject.IntegerValue);
        }

        [Test]
        public void BoolValue()
        {
            subject.Timeline = animator;
            subject.ParameterName = "BoolTest";
            Assert.IsFalse(animator.GetBool("BoolTest"));

            subject.BoolValue = true;

            Assert.IsTrue(animator.GetBool("BoolTest"));
            Assert.IsTrue(subject.BoolValue);
        }

        [Test]
        public void BoolValueNoAnimator()
        {
            subject.ParameterName = "BoolTest";
            Assert.IsFalse(animator.GetBool("BoolTest"));

            subject.BoolValue = true;

            Assert.IsFalse(animator.GetBool("BoolTest"));
            Assert.IsFalse(subject.BoolValue);
        }

        [Test]
        public void BoolValueNoParameter()
        {
            subject.Timeline = animator;
            Assert.IsFalse(animator.GetBool("BoolTest"));

            subject.BoolValue = true;

            Assert.IsFalse(animator.GetBool("BoolTest"));
            Assert.IsFalse(subject.BoolValue);
        }

        [Test]
        public void BoolValueInactiveGameObject()
        {
            subject.Timeline = animator;
            subject.ParameterName = "BoolTest";
            subject.gameObject.SetActive(false);
            Assert.IsFalse(animator.GetBool("BoolTest"));

            subject.BoolValue = true;

            Assert.IsFalse(animator.GetBool("BoolTest"));
            Assert.IsFalse(subject.BoolValue);
        }

        [Test]
        public void BoolValueInactiveComponent()
        {
            subject.Timeline = animator;
            subject.ParameterName = "BoolTest";
            subject.enabled = false;
            Assert.IsFalse(animator.GetBool("BoolTest"));

            subject.BoolValue = true;

            Assert.IsFalse(animator.GetBool("BoolTest"));
            Assert.IsFalse(subject.BoolValue);
        }

        [Test]
        public void SetTrigger()
        {
            subject.Timeline = animator;
            subject.ParameterName = "TriggerTest";

            Assert.IsFalse(animator.GetBool("TriggerTest"));

            subject.SetTrigger();

            Assert.IsTrue(animator.GetBool("TriggerTest"));
        }

        [Test]
        public void SetTriggerNoAnimator()
        {
            subject.ParameterName = "TriggerTest";

            Assert.IsFalse(animator.GetBool("TriggerTest"));
            subject.SetTrigger();
            Assert.IsFalse(animator.GetBool("TriggerTest"));
        }

        [Test]
        public void SetTriggerNoParameter()
        {
            subject.Timeline = animator;

            Assert.IsFalse(animator.GetBool("TriggerTest"));
            subject.SetTrigger();
            Assert.IsFalse(animator.GetBool("TriggerTest"));
        }

        [Test]
        public void SetTriggerInactiveGameObject()
        {
            subject.Timeline = animator;
            subject.ParameterName = "TriggerTest";
            subject.gameObject.SetActive(false);

            Assert.IsFalse(animator.GetBool("TriggerTest"));
            subject.SetTrigger();
            Assert.IsFalse(animator.GetBool("TriggerTest"));
        }

        [Test]
        public void SetTriggerInactiveComponent()
        {
            subject.Timeline = animator;
            subject.ParameterName = "TriggerTest";
            subject.enabled = false;

            Assert.IsFalse(animator.GetBool("TriggerTest"));
            subject.SetTrigger();
            Assert.IsFalse(animator.GetBool("TriggerTest"));
        }

        [Test]
        public void ResetTrigger()
        {
            subject.Timeline = animator;
            subject.ParameterName = "TriggerTest";

            Assert.IsFalse(animator.GetBool("TriggerTest"));

            animator.SetTrigger("TriggerTest");

            Assert.IsTrue(animator.GetBool("TriggerTest"));

            subject.ResetTrigger();

            Assert.IsFalse(animator.GetBool("TriggerTest"));
        }

        [Test]
        public void ResetTriggerNoAnimator()
        {
            subject.ParameterName = "TriggerTest";

            Assert.IsFalse(animator.GetBool("TriggerTest"));

            animator.SetTrigger("TriggerTest");

            Assert.IsTrue(animator.GetBool("TriggerTest"));

            subject.ResetTrigger();

            Assert.IsTrue(animator.GetBool("TriggerTest"));
        }

        [Test]
        public void ResetTriggerNoParameter()
        {
            subject.Timeline = animator;

            Assert.IsFalse(animator.GetBool("TriggerTest"));

            animator.SetTrigger("TriggerTest");

            Assert.IsTrue(animator.GetBool("TriggerTest"));

            subject.ResetTrigger();

            Assert.IsTrue(animator.GetBool("TriggerTest"));
        }

        [Test]
        public void ResetTriggerInactiveComponent()
        {
            subject.Timeline = animator;
            subject.ParameterName = "TriggerTest";
            subject.enabled = false;

            Assert.IsFalse(animator.GetBool("TriggerTest"));

            animator.SetTrigger("TriggerTest");

            Assert.IsTrue(animator.GetBool("TriggerTest"));

            subject.ResetTrigger();

            Assert.IsTrue(animator.GetBool("TriggerTest"));
        }
    }
}