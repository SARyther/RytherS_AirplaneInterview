using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    #region Fields & Properties

    [SerializeField]
    private float senseDistance = 20.0f;

    #endregion Fields & Properties

    public bool Query(out float obstacleDistance)
    {
        bool obstaclePresent = Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hitInfo, senseDistance);
        obstacleDistance = -1.0f;

        if (obstaclePresent)
        {
            obstacleDistance = hitInfo.distance;
            Debug.DrawLine(this.transform.position, hitInfo.point, Color.red);
        }

        return obstaclePresent;
    }
}
