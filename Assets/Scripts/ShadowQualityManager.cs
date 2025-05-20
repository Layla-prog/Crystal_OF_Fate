using UnityEngine;

public class ShadowQualityManager : MonoBehaviour
{
    public GameObject shadowOffLine;
    public GameObject shadowLowLine;
    public GameObject shadowHighLine;

    void Start()
    {
        int shadowSetting = PlayerPrefs.GetInt("Shadows", 2); // Default to High
        ApplyShadowSetting(shadowSetting);
    }

    public void ApplyShadowSetting(int setting)
    {
        switch (setting)
        {
            case 0:
                QualitySettings.shadowCascades = 0;
                QualitySettings.shadowDistance = 0;
                SetLines(true, false, false);
                break;

            case 1:
                QualitySettings.shadowCascades = 2;
                QualitySettings.shadowDistance = 75;
                SetLines(false, true, false);
                break;

            case 2:
            default:
                QualitySettings.shadowCascades = 4;
                QualitySettings.shadowDistance = 500;
                SetLines(false, false, true);
                break;
        }
    }

    private void SetLines(bool off, bool low, bool high)
    {
        if (shadowOffLine != null) shadowOffLine.SetActive(off);
        if (shadowLowLine != null) shadowLowLine.SetActive(low);
        if (shadowHighLine != null) shadowHighLine.SetActive(high);
    }
}
