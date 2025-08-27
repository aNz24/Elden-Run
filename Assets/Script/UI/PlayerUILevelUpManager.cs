using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUILevelUpManager : PlayerUIMenu
{
    [Header("Levels")]
    [SerializeField] int[] playerLevels = new int[100];
    [SerializeField] int basedLevelCost = 83;
    [SerializeField] int totalLevelUpCost = 0;

    [Header("Character Stats")]
    [SerializeField] TextMeshProUGUI characterLevelText;
    [SerializeField] TextMeshProUGUI runesHeldText;
    [SerializeField] TextMeshProUGUI runesNeededText;
    [SerializeField] TextMeshProUGUI vigorLevelText;
    [SerializeField] TextMeshProUGUI mindLevelText;
    [SerializeField] TextMeshProUGUI enduranceLevelText;
    [SerializeField] TextMeshProUGUI strengthLevelText;
    [SerializeField] TextMeshProUGUI dexterityLevelText;
    [SerializeField] TextMeshProUGUI intelligenceLevelText;
    [SerializeField] TextMeshProUGUI faithLevelText;

    [Header("Projected Character Stats")]
    [SerializeField] TextMeshProUGUI projectedCharacterLevelText;
    [SerializeField] TextMeshProUGUI projectedRunesHeldText;
    [SerializeField] TextMeshProUGUI projectedVigorLevelText;
    [SerializeField] TextMeshProUGUI projectedMindLevelText;
    [SerializeField] TextMeshProUGUI projectedEnduranceLevelText;
    [SerializeField] TextMeshProUGUI projectedStrengthLevelText;
    [SerializeField] TextMeshProUGUI projectedDexterityLevelText;
    [SerializeField] TextMeshProUGUI projectedIntelligenceLevelText;
    [SerializeField] TextMeshProUGUI projectedFaithLevelText;

    [Header("Slider")]
    public CharacterAttributes currentSelectedAttribute;
    public Slider vigorSlider;
    public Slider mindSlider;
    public Slider enduranceSlider;
    public Slider strengthSlider;
    public Slider dexteritySlider;
    public Slider intelligenceSlider;
    public Slider faithSlider;

    [Header("Button")]
    [SerializeField] Button confirmLevelButton;

    private void Awake()
    {
        SetAllLevelCost();
    }

    public override void OpenMenu()
    {
        base.OpenMenu();

        SetCurrentStats();
    }

    private void SetCurrentStats()
    {
        //CHARACTER LEVEL
        characterLevelText.text = PlayerUIManager.instance.localPlayer.characterStatManager.CaculateCharacterLevelBasedOnAttributes().ToString();
        projectedCharacterLevelText.text = PlayerUIManager.instance.localPlayer.characterStatManager.CaculateCharacterLevelBasedOnAttributes().ToString();

        // RUNES
        runesHeldText.text = PlayerUIManager.instance.localPlayer.playerStatManager.runes.ToString();
        projectedRunesHeldText.text = PlayerUIManager.instance.localPlayer.playerStatManager.runes.ToString();
        runesNeededText.text = "0";

        // ATTRIBUTES
        vigorLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.vigor.Value.ToString();
        projectedVigorLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.vigor.Value.ToString();
        vigorSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetWorkManager.vigor.Value;

        mindLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.mind.Value.ToString();
        projectedMindLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.mind.Value.ToString();
        mindSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetWorkManager.mind.Value;

        enduranceLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.endurance.Value.ToString();
        projectedEnduranceLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.endurance.Value.ToString();
        enduranceSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetWorkManager.endurance.Value;

        strengthLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.strength.Value.ToString();
        projectedStrengthLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.strength.Value.ToString();
        strengthSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetWorkManager.strength.Value;

        dexterityLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.dexterity.Value.ToString();
        projectedDexterityLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.dexterity.Value.ToString();
        dexteritySlider.minValue = PlayerUIManager.instance.localPlayer.playerNetWorkManager.dexterity.Value;

        intelligenceLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.intelligence.Value.ToString();
        projectedIntelligenceLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.intelligence.Value.ToString();
        intelligenceSlider.minValue = PlayerUIManager.instance.localPlayer.playerNetWorkManager.intelligence.Value;

        faithLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.faith.Value.ToString();
        projectedFaithLevelText.text = PlayerUIManager.instance.localPlayer.playerNetWorkManager.faith.Value.ToString();
        faithSlider .minValue = PlayerUIManager.instance.localPlayer.playerNetWorkManager.faith.Value;

        vigorSlider.Select();
        vigorSlider.OnSelect(null);

    }

    public void UpdateSliderBasedOnCurrentlySelectedAttribut()
    {
        PlayerManager player = PlayerUIManager.instance.localPlayer;

        switch (currentSelectedAttribute)
        {
            case CharacterAttributes.Vigor:
                projectedVigorLevelText.text = vigorSlider.value.ToString();
                break;
            case CharacterAttributes.Mind:
                projectedMindLevelText.text = mindSlider.value.ToString();
                break;
            case CharacterAttributes.Endurance:
                projectedEnduranceLevelText.text = enduranceSlider.value.ToString();
                break;
            case CharacterAttributes.Strength:
                projectedStrengthLevelText.text = strengthSlider.value.ToString();
                break;
            case CharacterAttributes.Dexterity:
                projectedDexterityLevelText.text = dexteritySlider.value.ToString();
                break;
            case CharacterAttributes.Intelligence:
                projectedIntelligenceLevelText.text = intelligenceSlider.value.ToString();
                break;
            case CharacterAttributes.Faith:
                projectedFaithLevelText.text = faithSlider.value.ToString();
                break;
            default:
                break;
        }

        CalculateLevelCost(
            player.characterStatManager.CaculateCharacterLevelBasedOnAttributes(),
            player.characterStatManager.CaculateCharacterLevelBasedOnAttributes(true));

        projectedCharacterLevelText.text = player.characterStatManager.CaculateCharacterLevelBasedOnAttributes(true).ToString();
        runesNeededText.text = totalLevelUpCost.ToString();

        // CACULATER COST
        if (totalLevelUpCost > player.playerStatManager.runes)
        {
            confirmLevelButton.enabled = false;
        }
        else
        {
            confirmLevelButton.enabled = true;
        }

        ChangeTextColorsDependingOnCosts();
    }

    public void ComfirmLevels()
    {
        PlayerManager player = PlayerUIManager.instance.localPlayer;

        // DEDUCT COST FROM TOTAL RUNES
        player.playerStatManager.runes -= totalLevelUpCost;

        PlayerUIManager.instance.playerUIHudManager.runesCountText.text = player.playerStatManager.runes.ToString();

        // SET NEW STATS


        player.playerNetWorkManager.vigor.Value = Mathf.RoundToInt(vigorSlider.value);
        player.playerNetWorkManager.mind.Value = Mathf.RoundToInt(mindSlider.value);
        player.playerNetWorkManager.endurance.Value = Mathf.RoundToInt(enduranceSlider.value);
        player.playerNetWorkManager.strength.Value = Mathf.RoundToInt(strengthSlider.value);
        player.playerNetWorkManager.dexterity.Value = Mathf.RoundToInt(dexteritySlider.value);
        player.playerNetWorkManager.intelligence.Value = Mathf.RoundToInt(intelligenceSlider.value);
        player.playerNetWorkManager.faith.Value = Mathf.RoundToInt(faithSlider.value);

        SetCurrentStats();
        ChangeTextColorsDependingOnCosts();

        WorldSaveGameManager.instance.SaveGame();
    }

    public void SetAllLevelCost()
    {
        for (int i = 0; i < playerLevels.Length; i++)
        {
            if(i == 0)
                continue;

            playerLevels[i] = basedLevelCost + (50 * i);
        }
    }

    private void CalculateLevelCost(int currentLevel, int projectedLevel)
    {
        int totalCost = 0;

        for (int i = 0; i < projectedLevel; i++)
        {
            if (i < currentLevel)
                continue;

            if(i >playerLevels.Length)
                continue;

            totalCost += playerLevels[i];
        }


        totalLevelUpCost = totalCost;

        projectedRunesHeldText.text = (PlayerUIManager.instance.localPlayer.playerStatManager.runes - totalCost).ToString();

        if (totalCost > PlayerUIManager.instance.localPlayer.playerStatManager.runes)
        {
            projectedRunesHeldText.color = Color.red;   
        }
        else
        {
            projectedRunesHeldText.color = Color.white;
        }

    }

    private void ChangeTextColorsDependingOnCosts()
    {
        PlayerManager player = PlayerUIManager.instance.localPlayer;

        int projectedVigorLevel = Mathf.RoundToInt(vigorSlider.value);
        int projectedEnduranceLevel = Mathf.RoundToInt(enduranceSlider.value);
        int projectedMindLevel = Mathf.RoundToInt(mindSlider.value);
        int projectedStrengthLevel = Mathf.RoundToInt(strengthSlider.value);
        int projectedDexterityLevel = Mathf.RoundToInt(dexteritySlider.value);
        int projectedIntelligenceLevel = Mathf.RoundToInt(intelligenceSlider.value);
        int projectedFaithLevel = Mathf.RoundToInt(faithSlider.value);

        ChangeTextFieldToSpecificColorBasedOnStat(player, projectedVigorLevelText, player.playerNetWorkManager.vigor.Value,projectedVigorLevel);
        ChangeTextFieldToSpecificColorBasedOnStat(player, projectedMindLevelText , player.playerNetWorkManager.mind.Value,projectedMindLevel);
        ChangeTextFieldToSpecificColorBasedOnStat(player, projectedStrengthLevelText , player.playerNetWorkManager.strength.Value,projectedStrengthLevel);
        ChangeTextFieldToSpecificColorBasedOnStat(player, projectedEnduranceLevelText, player.playerNetWorkManager.endurance.Value,projectedEnduranceLevel);
        ChangeTextFieldToSpecificColorBasedOnStat(player, projectedDexterityLevelText, player.playerNetWorkManager.dexterity.Value,projectedDexterityLevel);
        ChangeTextFieldToSpecificColorBasedOnStat(player, projectedIntelligenceLevelText, player.playerNetWorkManager.intelligence.Value,projectedIntelligenceLevel);
        ChangeTextFieldToSpecificColorBasedOnStat(player, projectedFaithLevelText, player.playerNetWorkManager.faith.Value,projectedFaithLevel);


        int projectedPlayerLevel = player.characterStatManager.CaculateCharacterLevelBasedOnAttributes(true);
        int playerLevel = player.characterStatManager.CaculateCharacterLevelBasedOnAttributes();

        if (projectedPlayerLevel == playerLevel)
        {
            projectedRunesHeldText.color = Color.white;
            projectedCharacterLevelText.color = Color.white;
            runesNeededText.color = Color.white;
        }

        if (totalLevelUpCost <= player.playerStatManager.runes)
        {
            runesNeededText.color = Color.white;

            if (projectedPlayerLevel > playerLevel)
            {
                projectedRunesHeldText.color = Color.red;
                projectedCharacterLevelText.color = Color.blue;

            }
        }
        else
        {
            runesNeededText.color = Color.red;


            if (projectedPlayerLevel > playerLevel)
                projectedCharacterLevelText.color = Color.red;
        }
    }

    private void ChangeTextFieldToSpecificColorBasedOnStat(PlayerManager player , TextMeshProUGUI textField, int stat, int projectedStat)
    {
        if(projectedStat == stat)
            textField.color = Color.white;

        if (totalLevelUpCost <= player.playerStatManager.runes)
        {
            runesNeededText.color = Color.white;

            if (projectedStat > stat)
            {
                textField.color = Color.blue;
            }
            else
            {
                textField.color = Color.white;
            }
        }
        else
        {
            if (projectedStat > stat)
            {
                textField.color = Color.red;
            }
            else
            {
                textField.color = Color.white;
            }
        }
    }

    public void AddRuens()
    {
        PlayerUIManager.instance.localPlayer.playerStatManager.AddRunes(10000);
    }
}
