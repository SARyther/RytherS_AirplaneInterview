using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class AltitudeText : MonoBehaviour
{
    #region Fields & Properties

    [SerializeField]
    private Airplane airplane = null;

    private Text text = null;

    #endregion Fields & Properties

    void Start()
    {
        if (this.airplane == null)
        {
            Debug.LogError("AltitudeText does not have an Airplane assigned.");
        }

        this.text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        this.text.text = this.airplane.Altitude.ToString("F2");
    }
}
