using Zinnia.Tracking.Collision;
using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Tracking.Collision
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Assert = UnityEngine.Assertions.Assert;

    public class CollisionIgnorerTest
    {
        private GameObject containingObject;
        private CollisionIgnorer subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            containingObject.SetActive(false);
            subject = containingObject.AddComponent<CollisionIgnorer>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator CollisionsIgnored()
        {
            WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
            GameObject source1 = CreateCollidable("source1");
            GameObject source2 = CreateCollidable("source2");
            GameObject target1 = CreateCollidable("target1");
            GameObject target2 = CreateCollidable("target2");

            Vector3 source1Origin = Vector3.left + Vector3.forward;
            Vector3 source2Origin = Vector3.left + Vector3.back;
            Vector3 target1Origin = Vector3.right + Vector3.forward;
            Vector3 target2Origin = Vector3.right + Vector3.back;

            source1.transform.position = source1Origin;
            source2.transform.position = source2Origin;
            target1.transform.position = target1Origin;
            target2.transform.position = target2Origin;

            CollisionChecker source1Collisions = source1.AddComponent<CollisionChecker>();
            CollisionChecker source2Collisions = source2.AddComponent<CollisionChecker>();

            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();

            containingObject.SetActive(true);

            subject.Sources.Add(source1);
            subject.Targets.Add(target1);
            yield return waitForFixedUpdate;
            Assert.IsFalse(source1Collisions.isColliding);

            //make source1 touch target1 -> no collision
            source1.transform.position = target1Origin;
            yield return waitForFixedUpdate;
            Assert.IsFalse(source1Collisions.isColliding);

            //make source1 touch target2 -> collision
            source1.transform.position = target2Origin;
            yield return waitForFixedUpdate;
            Assert.IsTrue(source1Collisions.isColliding);

            //move source1 out
            source1.transform.position = source1Origin;

            //add target2
            subject.Targets.Add(target1);
            subject.Targets.SetAt(target2, 1);

            //make source1 touch target2 -> no collision
            source1.transform.position = target2Origin;
            yield return waitForFixedUpdate;
            Assert.IsFalse(source1Collisions.isColliding);

            //move source1 out
            source1.transform.position = source1Origin;

            //make source2 touch target1 -> collision
            source2.transform.position = target1Origin;
            yield return waitForFixedUpdate;
            Assert.IsTrue(source2Collisions.isColliding);

            //make source2 touch target2->collision
            source2.transform.position = target2Origin;
            yield return waitForFixedUpdate;
            Assert.IsTrue(source2Collisions.isColliding);

            //move source 2 out
            source2.transform.position = source2Origin;

            //add source2
            subject.Sources.Add(source1);
            subject.Sources.SetAt(source2, 1);

            //make source2 touch target1 -> no collision
            source2.transform.position = target1Origin;
            yield return waitForFixedUpdate;
            Assert.IsFalse(source2Collisions.isColliding);

            //make source2 touch target2 -> no collision
            source2.transform.position = target2Origin;
            yield return waitForFixedUpdate;
            Assert.IsFalse(source2Collisions.isColliding);

            //move source 2 out
            source2.transform.position = source2Origin;

            //remove source2
            subject.Sources.Remove(source2);

            //make source2 touch target1 -> collision
            source2.transform.position = target1Origin;
            yield return waitForFixedUpdate;
            Assert.IsTrue(source2Collisions.isColliding);

            //make source2 touch target2->collision
            source2.transform.position = target2Origin;
            yield return waitForFixedUpdate;
            Assert.IsTrue(source2Collisions.isColliding);

            //move source 2 out
            source2.transform.position = source2Origin;

            //remove target2
            subject.Targets.Remove(target2);

            //make source1 touch target1 -> no collision
            source1.transform.position = target1Origin;
            yield return waitForFixedUpdate;
            Assert.IsFalse(source1Collisions.isColliding);

            //make source1 touch target2 -> collision
            source1.transform.position = target2Origin;
            yield return waitForFixedUpdate;
            Assert.IsTrue(source1Collisions.isColliding);

            //move source 1 out
            source1.transform.position = source1Origin;

            //remove target1
            subject.Targets.Remove(target1);

            //make source1 touch target1 -> collision
            source1.transform.position = target1Origin;
            yield return waitForFixedUpdate;
            Assert.IsTrue(source1Collisions.isColliding);

            //make source1 touch target2->collision
            source1.transform.position = target2Origin;
            yield return waitForFixedUpdate;
            Assert.IsTrue(source1Collisions.isColliding);

            Object.Destroy(source1);
            Object.Destroy(source2);
            Object.Destroy(target1);
            Object.Destroy(target2);
        }

        [UnityTest]
        public IEnumerator CollisionsResumedOnDisable()
        {
            WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
            GameObject source = CreateCollidable("source");
            GameObject target = CreateCollidable("target");
            Vector3 sourceOrigin = Vector3.left + Vector3.forward;
            Vector3 targetOrigin = Vector3.right + Vector3.forward;
            source.transform.position = sourceOrigin;
            target.transform.position = targetOrigin;

            CollisionChecker sourceCollisions = source.AddComponent<CollisionChecker>();

            subject.Sources = containingObject.AddComponent<GameObjectObservableList>();
            subject.Targets = containingObject.AddComponent<GameObjectObservableList>();

            containingObject.SetActive(true);

            subject.Sources.Add(source);
            subject.Targets.Add(target);

            yield return waitForFixedUpdate;
            Assert.IsFalse(sourceCollisions.isColliding);

            //make source touch target -> no collision
            source.transform.position = targetOrigin;
            yield return waitForFixedUpdate;
            Assert.IsFalse(sourceCollisions.isColliding);

            //move source out
            source.transform.position = sourceOrigin;

            subject.enabled = false;

            //make source touch target -> collision
            source.transform.position = targetOrigin;
            yield return waitForFixedUpdate;
            Assert.IsTrue(sourceCollisions.isColliding);

            Object.Destroy(source);
            Object.Destroy(target);
        }

        protected GameObject CreateCollidable(string name)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = name;
            obj.GetComponent<Collider>().isTrigger = true;
            Rigidbody rb = obj.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            return obj;
        }

        protected class CollisionChecker : MonoBehaviour
        {
            public bool isColliding;

            public void OnTriggerEnter(Collider other)
            {
                isColliding = true;
            }

            public void OnTriggerExit(Collider other)
            {
                isColliding = false;
            }
        }
    }
}