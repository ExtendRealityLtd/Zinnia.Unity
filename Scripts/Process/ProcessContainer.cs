namespace VRTK.Core.Process
{
    using System;
    using VRTK.Core.Utility;

    /// <summary>
    /// A proxy class used to make a <see cref="IProcessable"/> interface appear in the Unity Inspector.
    /// </summary>
    [Serializable]
    public sealed class ProcessContainer : InterfaceContainer<IProcessable>
    {
    }
}