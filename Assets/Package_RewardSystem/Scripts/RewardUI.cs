using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RewardSystem;

public class RewardUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Button levelRewardButton;
    public Button dailyRewardButton;
    public InputField levelInput;

    public TextMeshProUGUI rewardDetailsText; // Text to display reward info (requires TextMeshPro)

    private void Start()
    {
        levelRewardButton.onClick.AddListener(ClaimLevelReward);
        //dailyRewardButton.onClick.AddListener(ClaimDailyReward);
        UpdateRewardDisplay(""); // Clear the text initially
    }

    private void OnEnable()
    {
        RewardManager.OnRewardClaimed += HandleClaimedReward;
    }

    private void HandleClaimedReward(int level, Reward reward)
    {
        if (reward != null)
        {
            //RewardManager.Instance.ClaimLevelReward(level);
            UpdateRewardDisplay($"Level {level} Reward: {reward.type} x {reward.quantity}");
        }
        else
        {
            UpdateRewardDisplay($"No reward found for Level {level}");
        }
        //throw new System.NotImplementedException();
    }

    void ClaimLevelReward()
    {
        //if (int.TryParse(levelInput.text, out int level))
        //{
        //    Reward reward = RewardManager.Instance.ClaimLevelReward(level);
        //    if (reward != null)
        //    {
        //        RewardManager.Instance.ClaimLevelReward(level);
        //        UpdateRewardDisplay($"Level {level} Reward: {reward.type} x {reward.quantity}");
        //    }
        //    else
        //    {
        //        UpdateRewardDisplay($"No reward found for Level {level}");
        //    }
        //}
        //else
        //{
        //    UpdateRewardDisplay("Invalid Level Input.");
        //}
        if (int.TryParse(levelInput.text, out int level))
        {
            RewardManager.Instance.ClaimLevelReward(level);
        }

        
    }

    //void ClaimDailyReward()
    //{
    //    Reward reward = RewardManager.Instance.GetDailyReward();
    //    if (reward != null)
    //    {
    //        RewardManager.Instance.ClaimDailyReward();
    //        UpdateRewardDisplay($"Daily Reward: {reward.type} x {reward.quantity}");
    //    }
    //    else
    //    {
    //        UpdateRewardDisplay("No daily reward available.");
    //    }
    //}

    void UpdateRewardDisplay(string message)
    {
        rewardDetailsText.text = message;
    }
}
