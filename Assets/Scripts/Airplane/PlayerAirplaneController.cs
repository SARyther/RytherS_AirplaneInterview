using UnityEngine;

public class PlayerAirplaneController : BaseAirplaneController
{
    #region Fields & Properties

    [SerializeField] [Range(0.0f, 2.0f)] [Tooltip("How much of the horizontal input for roll should translate into yaw as well? (Higher values may help with turning without using Yaw Switch)")]
    private float rollIntoYawRatio = 0.0f;

    #endregion Fields & Properties

    // Update is called once per frame
    void Update()
    {
        float pitch = Input.GetAxis("Vertical");
        float roll = 0.0f;
        float yaw = 0.0f;

        float horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetButton("YawSwitch"))
        {
            yaw = horizontalInput;
        }
        else
        {
            roll = -horizontalInput;
            yaw = horizontalInput * rollIntoYawRatio;
        }

        this.airplane.Rotate(pitch, roll, yaw);

        float thrustInput = Input.GetAxis("Thrust");
        float thrust = thrustInput == 0.0f ? (Input.GetButton("Thrust") ? 1.0f : 0.0f) : thrustInput; //If the thrustInput axis is 0, check for the button alternative. Otherwise, just use the axis input.

        this.airplane.Thrust(thrust);
    }
}
