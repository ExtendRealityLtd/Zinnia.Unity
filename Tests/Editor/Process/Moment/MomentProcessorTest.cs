using Zinnia.Process;
using Zinnia.Process.Moment;
using Zinnia.Process.Moment.Collection;

namespace Test.Zinnia.Process.Moment
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using UnityEngine;

    public class MomentProcessorTest
    {
        private GameObject containingObject;
        private MomentProcessor subject;

        [SetUp]
        public void SetUp()
        {
            containingObject = new GameObject("MomentProcessorTest");
            subject = containingObject.AddComponent<MomentProcessor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(containingObject);
        }

        [Test]
        public void Process()
        {
            List<string> results = new List<string>();

            //Create first processor
            MomentProcessor processor1 = containingObject.AddComponent<MomentProcessor>();
            processor1.ProcessMoment = MomentProcessor.Moment.None;
            processor1.Processes = containingObject.AddComponent<MomentProcessObservableList>();
            processor1.Processes.Add(CreateMockProcess(ref results, "A"));
            processor1.Processes.Add(CreateMockProcess(ref results, "B"));
            MomentProcess processor1Moment = containingObject.AddComponent<MomentProcess>();
            processor1Moment.Source = GetProcessContainer(processor1);

            //Create second processor
            MomentProcessor processor2 = containingObject.AddComponent<MomentProcessor>();
            processor2.ProcessMoment = MomentProcessor.Moment.None;
            processor2.Processes = containingObject.AddComponent<MomentProcessObservableList>();
            processor2.Processes.Add(CreateMockProcess(ref results, "C"));
            processor2.Processes.Add(CreateMockProcess(ref results, "D"));
            MomentProcess processor2Moment = containingObject.AddComponent<MomentProcess>();
            processor2Moment.Source = GetProcessContainer(processor2);

            subject.ProcessMoment = MomentProcessor.Moment.None;
            MomentProcess managerProcess = containingObject.AddComponent<MomentProcess>();
            managerProcess.Source = GetProcessContainer(subject);
            subject.Processes = containingObject.AddComponent<MomentProcessObservableList>();
            subject.Processes.Add(processor1Moment);
            subject.Processes.Add(processor2Moment);

            Assert.IsEmpty(results);
            subject.Process();
            Assert.AreEqual("A,B,C,D", string.Join(",", results));

            results.Clear();
            subject.Processes.Clear();
            Assert.IsEmpty(results);

            subject.Processes.Add(processor2Moment);
            subject.Processes.Add(processor1Moment);

            subject.Process();
            Assert.AreEqual("C,D,A,B", string.Join(",", results));
        }

        private MomentProcess CreateMockProcess(ref List<string> data, string value)
        {
            MockProcess mockProcess = containingObject.AddComponent<MockProcess>();
            MomentProcess momentProcess = containingObject.AddComponent<MomentProcess>();
            momentProcess.Source = GetProcessContainer(mockProcess);
            mockProcess.thisValue = value;
            mockProcess.values = data;
            return momentProcess;
        }

        private ProcessContainer GetProcessContainer(IProcessable item)
        {
            return new ProcessContainer { Interface = item };
        }

        private class MockProcess : MonoBehaviour, IProcessable
        {
            public string thisValue;
            public List<string> values;

            public void Process()
            {
                values.Add(thisValue);
            }
        }
    }
}