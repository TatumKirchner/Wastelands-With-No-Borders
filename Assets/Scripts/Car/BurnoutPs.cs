using UnityEngine;

public class BurnoutPs : MonoBehaviour
{
    private ParticleSystem tirePs;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CarController CarController;
    [SerializeField] private float slipLimit;

    // Start is called before the first frame update
    void Start()
    {
        tirePs = GetComponent<ParticleSystem>();
        tirePs.Stop();
    }

    private void FixedUpdate()
    {
        PlayParticleSystem();
    }

    void PlayParticleSystem()
    {
        CarController.m_WheelColliders[3].GetGroundHit(out WheelHit wheelHit);

        //If the wheel is slipping more than the limit start the particle system
        if (wheelHit.forwardSlip >= slipLimit)
        {
            tirePs.Play();
        }
        else
        {
            tirePs.Stop();
        }
    }
}
