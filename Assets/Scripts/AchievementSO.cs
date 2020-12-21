using UnityEngine;

namespace ColorSnipersU.ScriptableObjects
{
    public enum AchievementType
    {
        CloseWindow,
        NearMiss,
        Miss,
        ObtainStars
    }

    [CreateAssetMenu(fileName = "Achievement", menuName = "Achievement", order = 1)]
    public class AchievementSO : ScriptableObject
    {
        public AchievementType Type;
        public int Count;
        public string GPGSID;
    }
}
