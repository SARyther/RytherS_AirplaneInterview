using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a decent idea, but doesn't work super well with all the quick movements and rotations. A simple parent object for the camera helps keep orientation/control as well.
[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    #region Fields & Properties

    [SerializeField]
    private GameObject viewTarget = null;
    [SerializeField]
    private float followDistance = 3.0f;
    [SerializeField]
    private float movementSpeed = 10.0f;
    [SerializeField]
    private float rotationSpeed = 3.0f;
    [SerializeField] [Tooltip("Should the camera get closer if something is in between the target and the follow distance?")]
    private bool autoCorrect = true;

    #endregion Fields & Properties

    // Start is called before the first frame update
    void Start()
    {
        if (this.viewTarget == null)
        {
            Debug.LogError("No target specified for FollowCamera.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Found some help for the rotation on a forum page and tweeked it slightly. https://answers.unity.com/questions/600577/camera-rotation-around-player-while-following.html
        float x = Input.GetAxis("Mouse X") * rotationSpeed;
        float y = Input.GetAxis("Mouse Y") * rotationSpeed;
        Vector3 positionOffset = this.transform.position - this.viewTarget.transform.position;

        positionOffset = Quaternion.AngleAxis(x, Vector3.up) * positionOffset; //Calculate left/right rotation
        positionOffset = Quaternion.AngleAxis(y, Vector3.right) * positionOffset; //Calculate up/down rotation

        Vector3 targetPosition = this.viewTarget.transform.position + positionOffset;

        //Set distance from target
        float clampDistance = this.followDistance;
        Vector3 direction = (targetPosition - this.viewTarget.transform.position).normalized;

        if (this.autoCorrect && Physics.Raycast(this.viewTarget.transform.position, direction, out RaycastHit hitInfo, this.followDistance))
        {
            clampDistance = hitInfo.distance;
        }

        targetPosition = this.viewTarget.transform.position + (direction * clampDistance);

        //Update position and rotation
        this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * this.movementSpeed);
        this.transform.LookAt(this.viewTarget.transform.position);
    }
}
