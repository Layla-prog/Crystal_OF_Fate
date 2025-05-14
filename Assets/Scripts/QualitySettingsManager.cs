using UnityEngine;
using UnityEngine.UI;

public class QualitySettingsManager : MonoBehaviour
{
    public Dropdown qualityDropdown;

    void Start()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));

        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    public void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("QualitySetting", index);
    }
}
