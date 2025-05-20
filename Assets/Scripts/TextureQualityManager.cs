using UnityEngine;

public class TextureQualityManager : MonoBehaviour
{
    public GameObject textureLowLine;
    public GameObject textureMedLine;
    public GameObject textureHighLine;

    void Start()
    {
        int textureSetting = PlayerPrefs.GetInt("Textures", 2); // Default to High
        ApplyTextureSetting(textureSetting);
    }

    public void ApplyTextureSetting(int setting)
    {
        switch (setting)
        {
            case 0: // Low
                QualitySettings.masterTextureLimit = 2;
                SetLines(true, false, false);
                break;

            case 1: // Medium
                QualitySettings.masterTextureLimit = 1;
                SetLines(false, true, false);
                break;

            case 2: // High
            default:
                QualitySettings.masterTextureLimit = 0;
                SetLines(false, false, true);
                break;
        }
    }

    private void SetLines(bool low, bool med, bool high)
    {
        if (textureLowLine != null) textureLowLine.SetActive(low);
        if (textureMedLine != null) textureMedLine.SetActive(med);
        if (textureHighLine != null) textureHighLine.SetActive(high);
    }
}
