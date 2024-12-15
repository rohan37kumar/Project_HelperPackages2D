using UnityEngine;

namespace RewardSystem
{
    [CreateAssetMenu(fileName = "Reward", menuName = "RewardSystem/Reward")]
    public class Reward : ScriptableObject
    {
        public string rewardName;         // Name of the reward
        public string description;        // Description for UI or tooltip
        public int value;                 // Reward value (e.g., coins, XP, items)
        public Sprite icon;               // Optional: Icon for the reward
        public RewardType type;           // Reward type (Daily, Weekly, etc.)

        public enum RewardType
        {
            Daily,
            Weekly,
            LevelCompletion
        }
    }
}
