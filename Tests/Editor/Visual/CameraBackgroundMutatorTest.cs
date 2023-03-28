using Zinnia.Data.Collection.List;
using Zinnia.Visual;

namespace Test.Zinnia.Visual
{
    using NUnit.Framework;
    using UnityEngine;

    public class CameraBackgroundMutatorTest
    {
        private GameObject containingObject;
        private CameraBackgroundMutator subject;
        private Color defaultColor = Color.white;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("CameraBackgroundMutatorTest");
            subject = containingObject.AddComponent<CameraBackgroundMutator>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void MutateAndRestore()
        {
            GameObject camera1Container = new GameObject("CameraBackgroundMutatorTest_Camera1Container");
            GameObject camera2Container = new GameObject("CameraBackgroundMutatorTest_Camera2Container");
            GameObject camera3Container = new GameObject("CameraBackgroundMutatorTest_Camera3Container");

            Camera camera1 = camera1Container.AddComponent<Camera>();
            Camera camera2 = camera2Container.AddComponent<Camera>();
            Camera camera3 = camera3Container.AddComponent<Camera>();

            camera1.backgroundColor = defaultColor;
            camera2.backgroundColor = defaultColor;
            camera3.backgroundColor = defaultColor;

            UnityObjectObservableList cameraList = containingObject.AddComponent<UnityObjectObservableList>();

            cameraList.Add(camera1);
            cameraList.Add(camera2);
            cameraList.Add(camera3);

            subject.TargetCameras = cameraList;
            subject.TargetClearFlags = CameraClearFlags.Color;
            subject.TargetBackgroundColor = Color.clear;

            Assert.AreEqual(CameraClearFlags.Skybox, camera1.clearFlags);
            Assert.AreEqual(CameraClearFlags.Skybox, camera2.clearFlags);
            Assert.AreEqual(CameraClearFlags.Skybox, camera3.clearFlags);

            Assert.AreEqual(defaultColor, camera1.backgroundColor);
            Assert.AreEqual(defaultColor, camera2.backgroundColor);
            Assert.AreEqual(defaultColor, camera3.backgroundColor);

            subject.Mutate();

            Assert.AreEqual(CameraClearFlags.Color, camera1.clearFlags);
            Assert.AreEqual(CameraClearFlags.Color, camera2.clearFlags);
            Assert.AreEqual(CameraClearFlags.Color, camera3.clearFlags);

            Assert.AreEqual(Color.clear, camera1.backgroundColor);
            Assert.AreEqual(Color.clear, camera2.backgroundColor);
            Assert.AreEqual(Color.clear, camera3.backgroundColor);

            subject.Restore();

            Assert.AreEqual(CameraClearFlags.Skybox, camera1.clearFlags);
            Assert.AreEqual(CameraClearFlags.Skybox, camera2.clearFlags);
            Assert.AreEqual(CameraClearFlags.Skybox, camera3.clearFlags);

            Assert.AreEqual(defaultColor, camera1.backgroundColor);
            Assert.AreEqual(defaultColor, camera2.backgroundColor);
            Assert.AreEqual(defaultColor, camera3.backgroundColor);

            Object.DestroyImmediate(camera1Container);
            Object.DestroyImmediate(camera2Container);
            Object.DestroyImmediate(camera3Container);
        }

        [Test]
        public void MutateOnInactiveGameObject()
        {
            GameObject camera1Container = new GameObject("CameraBackgroundMutatorTest_Camera1Container");

            Camera camera1 = camera1Container.AddComponent<Camera>();

            camera1.backgroundColor = defaultColor;

            UnityObjectObservableList cameraList = containingObject.AddComponent<UnityObjectObservableList>();

            cameraList.Add(camera1);

            subject.TargetCameras = cameraList;
            subject.TargetClearFlags = CameraClearFlags.Color;
            subject.TargetBackgroundColor = Color.clear;

            Assert.AreEqual(CameraClearFlags.Skybox, camera1.clearFlags);
            Assert.AreEqual(defaultColor, camera1.backgroundColor);

            subject.gameObject.SetActive(false);

            subject.Mutate();

            Assert.AreEqual(CameraClearFlags.Skybox, camera1.clearFlags);
            Assert.AreEqual(defaultColor, camera1.backgroundColor);

            Object.DestroyImmediate(camera1Container);
        }

        [Test]
        public void MutateOnInactiveComponent()
        {
            GameObject camera1Container = new GameObject("CameraBackgroundMutatorTest_Camera1Container");

            Camera camera1 = camera1Container.AddComponent<Camera>();

            camera1.backgroundColor = defaultColor;

            UnityObjectObservableList cameraList = containingObject.AddComponent<UnityObjectObservableList>();

            cameraList.Add(camera1);

            subject.TargetCameras = cameraList;
            subject.TargetClearFlags = CameraClearFlags.Color;
            subject.TargetBackgroundColor = Color.clear;

            Assert.AreEqual(CameraClearFlags.Skybox, camera1.clearFlags);
            Assert.AreEqual(defaultColor, camera1.backgroundColor);

            subject.enabled = false;

            subject.Mutate();

            Assert.AreEqual(CameraClearFlags.Skybox, camera1.clearFlags);
            Assert.AreEqual(defaultColor, camera1.backgroundColor);

            Object.DestroyImmediate(camera1Container);
        }
    }
}