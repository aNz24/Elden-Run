using UnityEngine;
using UnityEngine.UI;

public class UI_Color_Button : MonoBehaviour
{
    [Header("Color")]
    private float redValue;
    private float greenValue;
    private float blueValue;

    [SerializeField] Image colorImage;

    private void Awake()
    {
        redValue  = colorImage.color.r * 255;
        greenValue = colorImage.color.g * 255;
        blueValue = colorImage.color.b * 255;
    }

    private void Update()
    {
        Color32 hairColor;

        byte red = (byte)  PlayerUIManager.instance.localPlayer.playerNetWorkManager.hairColorRed.Value;
        byte green = (byte)PlayerUIManager.instance.localPlayer.playerNetWorkManager.hairColorGreen.Value;
        byte blue = (byte)PlayerUIManager.instance.localPlayer.playerNetWorkManager.hairColorBlue.Value;

        hairColor = new Color32(red, green, blue, 255);

        colorImage.color = hairColor;
    }

    public void SetSilderValuesToColor()
    {
        TitleScreenManager.instance.SetRedColorSilder(redValue);
        TitleScreenManager.instance.SetGreenColorSilder(greenValue);
        TitleScreenManager.instance.SetBlueColorSilder(blueValue);
        TitleScreenManager.instance.PreviewHairColor();

    }

    public void ComfirmColor()
    {
        TitleScreenManager.instance.CloseChooseHairColorSubMenu();
        PlayerUIManager.instance.localPlayer.playerBodyManager.SetHairColor();
    }
}
