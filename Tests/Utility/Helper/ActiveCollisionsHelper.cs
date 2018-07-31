using VRTK.Core.Tracking.Collision;
using VRTK.Core.Tracking.Collision.Active;

namespace Test.VRTK.Core.Utility.Helper
{
    public static class ActiveCollisionsHelper
    {
        public static string GetNamesOfActiveCollisions(ActiveCollisionsContainer.EventData list, string separator = ",")
        {
            string returnString = "";
            for (int i = 0; i < list.activeCollisions.Count; i++)
            {
                CollisionNotifier.EventData data = list.activeCollisions[i];
                returnString += data.collider.name;
                if (i < list.activeCollisions.Count - 1)
                {
                    returnString += separator;
                }
            }
            return returnString;
        }
    }
}