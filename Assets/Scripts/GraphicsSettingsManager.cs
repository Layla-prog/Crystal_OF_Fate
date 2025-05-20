using UnityEngine;
using TMPro;

public class GraphicsSettingsManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject shadowofftextLINE, shadowlowtextLINE, shadowhightextLINE;
    public GameObject texturelowtextLINE, texturemedtextLINE, texturehightextLINE;
    public TMP_Text motionblurtext;

    void Start()
    {
        ApplyAllSettings(); // Load settings on startup
    }

    public void ApplyAllSettings()
    {
        ApplyShadows(PlayerPrefs.GetInt("Shadows", 2));
        ApplyTextures(PlayerPrefs.GetInt("Textures", 2));
        ApplyMotionBlur(PlayerPrefs.GetInt("MotionBlur", 0));
    }

    // --------- SHADOW QUALITY ----------
    public void ShadowsOff()
    {
        PlayerPrefs.SetInt("Shadows", 0);
        ApplyShadows(0);
    }

    public void ShadowsLow()
    {
        PlayerPrefs.SetInt("Shadows", 1);
        ApplyShadows(1);
    }

    public void ShadowsHigh()
    {
        PlayerPrefs.SetInt("Shadows", 2);
        ApplyShadows(2);
    }

    private void ApplyShadows(int level)
    {
        switch (level)
        {
            case 0:
                QualitySettings.shadowCascades = 0;
                QualitySettings.shadowDistance = 0;
                shadowofftextLINE.SetActive(true);
                shadowlowtextLINE.SetActive(false);
                shadowhightextLINE.SetActive(false);
                break;
            case 1:
                QualitySettings.shadowCascades = 2;
                QualitySettings.shadowDistance = 75;
                shadowofftextLINE.SetActive(false);
                shadowlowtextLINE.SetActive(true);
                shadowhightextLINE.SetActive(false);
                break;
            case 2:
                QualitySettings.shadowCascades = 4;
                QualitySettings.shadowDistance = 500;
                shadowofftextLINE.SetActive(false);
                shadowlowtextLINE.SetActive(false);
                shadowhightextLINE.SetActive(true);
                break;
        }
    }

    // --------- TEXTURE QUALITY ----------
    public void TexturesLow()
    {
        PlayerPrefs.SetInt("Textures", 0);
        ApplyTextures(0);
    }

    public void TexturesMed()
    {
        PlayerPrefs.SetInt("Textures", 1);
        ApplyTextures(1);
    }

    public void TexturesHigh()
    {
        PlayerPrefs.SetInt("Textures", 2);
        ApplyTextures(2);
    }

    private void ApplyTextures(int level)
    {
        QualitySettings.masterTextureLimit = 2 - level; // 0 = high, 1 = med, 2 = low
        texturelowtextLINE.SetActive(level == 0);
        texturemedtextLINE.SetActive(level == 1);
        texturehightextLINE.SetActive(level == 2);
    }

    // --------- MOTION BLUR (custom, based on post-processing or your system) ----------
    public void ToggleMotionBlur()
    {
        int current = PlayerPrefs.GetInt("MotionBlur", 0);
        int newValue = (current == 0) ? 1 : 0;
        PlayerPrefs.SetInt("MotionBlur", newValue);
        ApplyMotionBlur(newValue);
    }

    private void ApplyMotionBlur(int isOn)
    {
        motionblurtext.text = (isOn == 1) ? "on" : "off";
        // Apply blur effect (requires PostProcessing Volume reference, not shown here)
    }
}
