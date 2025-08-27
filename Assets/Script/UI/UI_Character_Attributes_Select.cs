using UnityEngine;

public class UI_Character_Attributes_Select : MonoBehaviour
{
    [SerializeField] CharacterAttributes sliderAttribute;

    public void SetCurrentSelectedAttribute()
    {
        PlayerUIManager.instance.playerUILevelUpManager.currentSelectedAttribute = sliderAttribute;
    }
}
