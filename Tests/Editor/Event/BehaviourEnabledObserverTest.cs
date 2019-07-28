using Zinnia.Event;
using Zinnia.Data.Collection.List;

namespace Test.Zinnia.Event
{
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.Collections;
    using NUnit.Framework;
    using Test.Zinnia.Utility.Mock;
    using Assert = UnityEngine.Assertions.Assert;

    public class BehaviourEnabledObserverTest
    {
        private GameObject containingObject;
        private BehaviourEnabledObserver subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject();
            containingObject.SetActive(false);
            subject = containingObject.AddComponent<BehaviourEnabledObserver>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [UnityTest]
        public IEnumerator CheckStateOnEnable()
        {
            UnityEventListenerMock activeAndEnabledMock = new UnityEventListenerMock();
            subject.ActiveAndEnabled.AddListener(activeAndEnabledMock.Listen);

            CheckState behaviourToCheck = containingObject.AddComponent<CheckState>();
            behaviourToCheck.enabled = false;

            BehaviourObservableList behaviours = containingObject.AddComponent<BehaviourObservableList>();
            subject.Behaviours = behaviours;

            containingObject.SetActive(true);

            behaviours.Add(behaviourToCheck);

            Assert.IsFalse(activeAndEnabledMock.Received);

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(activeAndEnabledMock.Received);

            behaviourToCheck.enabled = true;

            yield return new WaitForEndOfFrame();

            Assert.IsTrue(activeAndEnabledMock.Received);
        }

        [UnityTest]
        public IEnumerator CheckStateOnEnableInsideLimitedTime()
        {
            UnityEventListenerMock activeAndEnabledMock = new UnityEventListenerMock();
            subject.ActiveAndEnabled.AddListener(activeAndEnabledMock.Listen);

            CheckState behaviourToCheck = containingObject.AddComponent<CheckState>();
            behaviourToCheck.enabled = false;

            BehaviourObservableList behaviours = containingObject.AddComponent<BehaviourObservableList>();
            subject.Behaviours = behaviours;
            subject.MaximumRunTime = 0.1f;

            containingObject.SetActive(true);

            behaviours.Add(behaviourToCheck);

            Assert.IsFalse(activeAndEnabledMock.Received);

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(activeAndEnabledMock.Received);

            behaviourToCheck.enabled = true;

            yield return new WaitForSeconds(0.05f);

            Assert.IsTrue(activeAndEnabledMock.Received);
        }

        [UnityTest]
        public IEnumerator CheckStateOnEnableOutsideLimitedTime()
        {
            UnityEventListenerMock activeAndEnabledMock = new UnityEventListenerMock();
            subject.ActiveAndEnabled.AddListener(activeAndEnabledMock.Listen);

            CheckState behaviourToCheck = containingObject.AddComponent<CheckState>();
            behaviourToCheck.enabled = false;

            BehaviourObservableList behaviours = containingObject.AddComponent<BehaviourObservableList>();
            subject.Behaviours = behaviours;
            subject.MaximumRunTime = 0.1f;

            containingObject.SetActive(true);

            behaviours.Add(behaviourToCheck);

            Assert.IsFalse(activeAndEnabledMock.Received);

            yield return new WaitForSeconds(0.11f);

            Assert.IsFalse(activeAndEnabledMock.Received);

            behaviourToCheck.enabled = true;

            yield return new WaitForEndOfFrame();

            Assert.IsFalse(activeAndEnabledMock.Received);
        }
    }

    public class CheckState : MonoBehaviour
    {
        private void OnEnable() { }
    }
}