using ColorSnipersU.MonoBehaviors.MainGameScene;
using UnityEngine;

namespace ColorSnipersU.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Levels", menuName = "Levels", order = 1)]    
    public class LevelsSO :ScriptableObject
    {
        public LevelBase[] Levels;
        public static LevelsSO instance;

        public LevelsSO()
        {
            instance = this;
        }
    }
}
