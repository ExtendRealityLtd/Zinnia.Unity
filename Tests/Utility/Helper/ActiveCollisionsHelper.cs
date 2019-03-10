using Zinnia.Tracking.Collision;
using Zinnia.Tracking.Collision.Active;

namespace Test.Zinnia.Utility.Helper
{
    public static class ActiveCollisionsHelper
    {
        public static string GetNamesOfActiveCollisions(ActiveCollisionsContainer.EventData list, string separator = ",")
        {
            string returnString = "";
            for (int index = 0; index < list.ActiveCollisions.Count; index++)
            {
                CollisionNotifier.EventData data = list.ActiveCollisions[index];
                returnString += data.ColliderData.name;
                if (index < list.ActiveCollisions.Count - 1)
                {
                    returnString += separator;
                }
            }
            return returnString;
        }
    }
}