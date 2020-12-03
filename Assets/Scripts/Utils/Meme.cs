using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "Meme", menuName = "ScriptableObjects/Meme", order = 1)]
    public class Meme : ScriptableObject
    {
        public string sceneName;
        public string memeName;
        public string description;
    }
}
