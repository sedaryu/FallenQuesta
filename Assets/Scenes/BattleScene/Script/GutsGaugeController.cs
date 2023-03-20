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
            if (value < 0)
            {
                _value = 0;
            }
            else if (value > 1)
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
        gutsGauge = GetComponentsInChildren<Slider>();
        //gutsGauge[0].minValue = 0;
        //gutsGauge[1].minValue = 0;
        //gutsGauge[0].maxValue = 1;
        //gutsGauge[1].maxValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        gutsGauge[0].value = Value;
        gutsGauge[1].value = Value;
    }
}
