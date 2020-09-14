using UnityEngine;

public class AIAirplaneController : BaseAirplaneController
{
    #region Fields & Properties

    [SerializeField]
    private Airplane targetPlane = null;
    [SerializeField]
    private Sensor[] sensors = null;
    [SerializeField]
    private float followDistance = 5.0f;
    [SerializeField]
    private float avoidanceStrength = 10000.0f;


    #endregion Fields & Properties

    protected override void Start()
    {
        base.Start();

        if (targetPlane == null)
        {
            Debug.LogError("AIAirplaneController does not have a target assigned.");
        }

        if (sensors == null)
        {
            Debug.LogError("AIAirplaneController does not have sensors assigned.");
        }
    }

    void Update()
    {
        if ((this.transform.position - this.targetPlane.transform.position).magnitude > this.followDistance)
        {
            SteerToward(GetTargetDirection());
        }
    }

    private Vector3 GetTargetDirection()
    {
        Vector3 targetPosition = this.targetPlane.transform.position;

        for (int i = 0; i < this.sensors.Length; i++)
        {
            if (sensors[i].Query(out float obstacleDistance))
            {
                //If there is an obstacle in one direction, move target in oposite direction (gets stronger as obstacle gets closer)
                targetPosition += -sensors[i].transform.forward * (this.avoidanceStrength / obstacleDistance);
            }
        }

        Vector3 direction = (targetPosition - this.transform.position).normalized;

        Debug.DrawLine(this.transform.position, this.transform.position + direction * 3, Color.blue);

        return direction;
    }

    /*
     * For some reason, when the target plane is behind the AI, this stops working.
     * The direction is still correct, but I must be messing something up with the rotations.
     * Got tired of trying to fix it, so I moved on. Everything I found online just said to use "Transform.LookAt()", but I wanted to use the Rotate function of my Airplane.
     * I would have asked someone to help me look at it a LONG time ago if on a team.
     */
    private void SteerToward(Vector3 targetDirection)
    {
        bool isSteering = false;
        Vector3 rotation = targetDirection - this.transform.forward;

        float angle = Vector3.Angle(this.transform.forward, targetDirection);
        if (angle > 1.0f) //Stop rotating when facing the right direction
        {
            isSteering = true;
            float pitch = -rotation.y;
            float roll = rotation.x; //This isn't perfect, but I think it looks a little better than just pitch and yaw
            float yaw = rotation.x * Mathf.Sign(this.transform.right.x); //Reverse if facing the other direction (prevents only being able to turn one way)

            this.airplane.Rotate(pitch, roll, yaw);
        }

        float thrust = isSteering ? 0.75f : Vector3.Dot(this.transform.forward, targetDirection); //If we are moving in a different direction in order to avoid something, we don't want to stop.
        this.airplane.Thrust(Mathf.Clamp(thrust, 0.1f, 1.0f)); //We don't want to move backwards, and we don't want to stop.
    }
}
