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
    }

    // Update is called once per frame
    void Update()
    {
        gutsGauge[0].value = Value;
        gutsGauge[1].value = Value;
    }
}
