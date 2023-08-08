using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderText;

    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener((v) =>
        {
            _sliderText.text = v.ToString("0.00");
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
