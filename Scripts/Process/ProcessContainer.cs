namespace VRTK.Core.Process
{
    using System;
    using VRTK.Core.Utility;

    /// <summary>
    /// The ProcessContainer is the proxy class used to make a process interface appear in the Unity Inspector.
    /// </summary>
    [Serializable]
    public sealed class ProcessContainer : InterfaceContainer<IProcessable>
    {
    }
}