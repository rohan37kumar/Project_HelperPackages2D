using UnityEngine;

namespace RewardSystem
{
    [CreateAssetMenu(fileName = "Reward", menuName = "RewardSystem/Reward")]
    public class Reward : ScriptableObject
    {
        public string rewardID;         // Name of the reward
        public string description;        // Description for UI or tooltip
        public int quantity;                 // Reward value (e.g., coins, XP, items)
        public Sprite icon;               // Optional: Icon for the reward
        public RewardType type;           // Reward type (Daily, Weekly, etc.)
        public DeliveryType rewardDelivery;
        public string customData;
        public enum DeliveryType
        {
            Daily,
            Weekly,
            LevelCompletion
        }

        public enum RewardType
        {
            Coins,
            Gems,
            Hints,
            Item,
            Experience,
            Custom
        }
    }
}
