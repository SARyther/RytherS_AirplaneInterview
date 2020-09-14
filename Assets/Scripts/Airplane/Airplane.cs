using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Airplane : MonoBehaviour
{
    #region Fields & Properties

    [HideInInspector]
    public Rigidbody rigidBody = null;

    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField] [Range(1.0f, 90.0f)]
    private float rotationSpeed = 30.0f;
    [SerializeField]
    private float thrustStrength = 100.0f;
    [SerializeField]
    private ParticleSystem[] thrustEffects = null;
    [SerializeField]
    private GameObject lWingFlap = null, rWingFlap = null, tailFlap = null;

    private float[] initialEffectEmmissions = null;

    private float audioSourceInitialVolume, audioSourceInitialPitch;

    public float Altitude { get; private set; }
    public static float MaxAltitude { get { return 13716.0f; } } //The approximate maximum altitude for a passenger plane =)

    #endregion Fields & Properties

    private void Start()
    {
        this.rigidBody = this.GetComponent<Rigidbody>();

        if (this.audioSource != null)
        {
            this.audioSourceInitialVolume = this.audioSource.volume;
            this.audioSourceInitialPitch = this.audioSource.pitch;
            this.audioSource.volume = 0.0f;
            this.audioSource.pitch = 0.0f;
        }

        if (this.thrustEffects != null)
        {
            this.initialEffectEmmissions = new float[this.thrustEffects.Length];

            for (int i = 0; i < this.thrustEffects.Length; i++)
            {
                this.initialEffectEmmissions[i] = this.thrustEffects[i].emission.rateOverTime.constant;
                var emission = thrustEffects[i].emission;
                emission.rateOverTime = 0.0f;
            }
        }

        if (this.rWingFlap == null || this.lWingFlap == null || this.tailFlap == null)
        {
            Debug.Log("Airplane flap(s) not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAltitude();
    }

    public void Thrust(float power)
    {
        this.rigidBody.AddForce(this.transform.forward * power * this.thrustStrength * Time.deltaTime, ForceMode.Force);
        
        if (this.audioSource != null)
        {
            float newVolume = power * this.audioSourceInitialVolume;
            newVolume = Mathf.Lerp(this.audioSource.volume, newVolume, Time.deltaTime);

            float newPitch = power * this.audioSourceInitialPitch;
            newPitch = Mathf.Lerp(this.audioSource.pitch, newPitch, Time.deltaTime);

            //Clamp so engine never "shuts off"
            this.audioSource.volume = Mathf.Max(newVolume, 0.01f);
            this.audioSource.pitch = Mathf.Max(newPitch, 0.25f);
        }

        if (this.thrustEffects != null)
        {
            for (int i = 0; i < this.thrustEffects.Length; i++)
            {
                var emission = this.thrustEffects[i].emission;

                float newEmmissionRate = power * this.initialEffectEmmissions[i];
                newEmmissionRate = Mathf.Lerp(emission.rateOverTime.constant, newEmmissionRate, Time.deltaTime);
                emission.rateOverTime = newEmmissionRate;
            }
        }
    }

    public void Rotate(float pitch, float roll, float yaw)
    {
        //Adjust wing flap rotation (a lot of these little effects could be animations instead, too)
        Vector3 lWingFlapRotation = this.lWingFlap.transform.localRotation.eulerAngles;
        Vector3 rWingFlapRotation = this.rWingFlap.transform.localRotation.eulerAngles;
        Vector3 tailFlapRotation = this.tailFlap.transform.localRotation.eulerAngles;

        this.lWingFlap.transform.localRotation = Quaternion.Euler(-roll * 80.0f, lWingFlapRotation.y, lWingFlapRotation.z);
        this.rWingFlap.transform.localRotation = Quaternion.Euler(roll * 80.0f, rWingFlapRotation.y, rWingFlapRotation.z);
        this.tailFlap.transform.localRotation = Quaternion.Euler(tailFlapRotation.x, yaw * 80.0f, tailFlapRotation.z);

        //Adujst actual rotations for speed and time
        pitch *= this.rotationSpeed * Time.deltaTime;
        roll *= this.rotationSpeed * Time.deltaTime;
        yaw *= this.rotationSpeed * Time.deltaTime;

        this.transform.Rotate(pitch, yaw, roll);

    }

    private void UpdateAltitude()
    {
        if(Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hitInfo, MaxAltitude))
        {
            this.Altitude = hitInfo.distance;
        }
        else
        {
            this.Altitude = MaxAltitude;
        }
    }
}
