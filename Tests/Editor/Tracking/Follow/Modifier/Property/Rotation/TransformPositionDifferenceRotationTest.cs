using Zinnia.Tracking.Follow.Modifier.Property.Rotation;

namespace Test.Zinnia.Tracking.Follow.Modifier.Property.Rotation
{
    using UnityEngine;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class TransformPositionDifferenceRotationTest
    {
        private GameObject containingObject;
        private TransformPositionDifferenceRotation subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            subject = containingObject.AddComponent<TransformPositionDifferenceRotation>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(subject);
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Modify()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();

            target.transform.position = new Vector3(0f, 0f, 0f);
            target.transform.localRotation = Quaternion.identity;

            source.transform.position = new Vector3(0.5f, 0f, -0.5f);
            subject.Modify(source, target);
            source.transform.position = new Vector3(0.5f, 0.5f, -0.5f);
            subject.Modify(source, target);
            source.transform.position = new Vector3(0.5f, 1f, -0.5f);
            subject.Modify(source, target);

            Assert.AreEqual(new Quaternion(0.3f, -0.1f, 0.3f, 0.9f).ToString(), target.transform.localRotation.ToString());

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void ModifyInactiveGameObject()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();
            subject.gameObject.SetActive(false);

            target.transform.position = new Vector3(0f, 0f, 0f);
            target.transform.localRotation = Quaternion.identity;

            source.transform.position = new Vector3(0.5f, 0f, -0.5f);
            subject.Modify(source, target);
            source.transform.position = new Vector3(0.5f, 0.5f, -0.5f);
            subject.Modify(source, target);
            source.transform.position = new Vector3(0.5f, 1f, -0.5f);
            subject.Modify(source, target);

            Assert.AreEqual(Quaternion.identity, target.transform.localRotation);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }


        [Test]
        public void ModifyInactiveComponent()
        {
            GameObject source = new GameObject();
            GameObject target = new GameObject();
            subject.enabled = false;

            target.transform.position = new Vector3(0f, 0f, 0f);
            target.transform.localRotation = Quaternion.identity;

            source.transform.position = new Vector3(0.5f, 0f, -0.5f);
            subject.Modify(source, target);
            source.transform.position = new Vector3(0.5f, 0.5f, -0.5f);
            subject.Modify(source, target);
            source.transform.position = new Vector3(0.5f, 1f, -0.5f);
            subject.Modify(source, target);

            Assert.AreEqual(Quaternion.identity, target.transform.localRotation);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(target);
        }
    }
}