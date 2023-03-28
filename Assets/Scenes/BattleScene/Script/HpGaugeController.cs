using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGaugeController : MonoBehaviour
{
    float _value = 1;

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

    private Slider[] hpGauge;
    public event Action DeadEnd; //HpÇ™0à»â∫Ç…Ç»Ç¡ÇΩç€ÇÃÉCÉxÉìÉg

    // Start is called before the first frame update
    void Start()
    {
        hpGauge = GetComponentsInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        hpGauge[0].value = Value;
        hpGauge[1].value = Value;

        if (Value <= 0) //HpÇ™0à»â∫Ç©îªíË
        {
            DeadEnd.Invoke();
        }
    }
}
