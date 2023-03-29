using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GutsGaugeController : MonoBehaviour
{
    float _value = 0;

    public float Value
    { 
        get => _value;
        set
        {
            if (value < 0) //HpGaugeがマイナスになるのを制限
            {
                _value = 0;
            }
            else if (value > 1) //HpGaugeが1より大きくなるのを制限
            {
                _value = 1;
            }
            else
            { 
                _value = value;
            }
        }
    }

    private Slider[] gutsGauge;

    // Start is called before the first frame update
    void Start()
    {
        gutsGauge = GetComponentsInChildren<Slider>(); //子オブジェクトからSliderを取得
    }

    // Update is called once per frame
    void Update()
    {
        gutsGauge[0].value = Value; //LeftSliderの値を更新
        gutsGauge[1].value = Value; //RightSliderの値を更新
    }
}
