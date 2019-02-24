namespace Test.Zinnia.Utility
{
    using UnityEngine;

    public class TimeSettingOverride
    {
        private float savedFixedDeltaTime;
        private float savedMaximumDeltaTime;
        private float savedTimeScale;
        private float savedMaximumParticleDeltaTime;

        public TimeSettingOverride(float fixedDeltaTime, float maximumDeltaTime, float timeScale, float maximumParticleDeltaTime)
        {
            OverrideTime(fixedDeltaTime, maximumDeltaTime, timeScale, maximumParticleDeltaTime);
        }

        public virtual void OverrideTime(float fixedDeltaTime, float maximumDeltaTime, float timeScale, float maximumParticleDeltaTime)
        {
            savedFixedDeltaTime = Time.fixedDeltaTime;
            savedMaximumDeltaTime = Time.maximumDeltaTime;
            savedTimeScale = Time.timeScale;
            savedMaximumParticleDeltaTime = Time.maximumParticleDeltaTime;

            Time.fixedDeltaTime = fixedDeltaTime;
            Time.maximumDeltaTime = maximumDeltaTime;
            Time.timeScale = timeScale;
            Time.maximumParticleDeltaTime = maximumParticleDeltaTime;
        }

        public virtual void ResetTime()
        {
            OverrideTime(savedFixedDeltaTime, savedMaximumDeltaTime, savedTimeScale, savedMaximumParticleDeltaTime);
        }
    }
}