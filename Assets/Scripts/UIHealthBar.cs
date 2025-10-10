using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMax(int max)
    {
        if (slider != null) slider.maxValue = max;
        if (slider != null) slider.value = max;
    }

    public void Set(int value)
    {
        if (slider != null) slider.value = value;
    }
}
