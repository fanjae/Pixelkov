using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterSelector : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Slider slider;
    private int minValue = 0;
    private int maxValue = 99;

    // UI 데이터를 기반으로 반환하는 프로퍼티
    public int Count
    {
        get
        {
            if(slider != null)
            {
                return (int)slider.value;
            }
            if(inputField != null)
            {
                return int.Parse(inputField.text);
            }
            return 0;
        }
    }
    private void OnEnable()
    {
        slider.value = 0;
        inputField.text = "0";
    }

    public void Init(int min, int max)
    {
        minValue = min;
        maxValue = max;

        slider.minValue = minValue;
        slider.maxValue = maxValue;
    }

    public void OnChangeSlider()
    {
        if (inputField == null || slider == null) return;

        // Slider의 value는 Inspector에서 정수로 제한
        inputField.text = slider.value.ToString();
    }

    public void OnChangeInputField()
    {
        if (inputField == null || slider == null) return;
        // 입력 제한은 Inspector에 InputField -> Content Type을 Integer Number로 설정해서 제한
        // 따라서 InputField에 들어오는 값은 무조건 정수이고, 공백 처리와 범위 제한만 하면 충분
        if (inputField.text.Equals("")) return;

        int value = int.Parse(inputField.text);
        value = Mathf.Clamp(value, minValue, maxValue);
        slider.value = value;
        inputField.text = slider.value.ToString();
    }
}
