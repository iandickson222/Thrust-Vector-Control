using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainEngine : MonoBehaviour
{
    public float mainEmpty = 78000.0f;
    public float mainFull = 110000.0f;
    public float thrust = 5225000.0f;
    public float burnTime = 480.0f;
    public float launchTime = 124.0f;
    public ParticleSystem fire;
    public ParticleSystem smoke;

    private float t = 0.0f;
    private float discharge_rate = 0.0f;
    private bool launch = false;
    private bool landerDeployed = false;
    private bool fuelDischarged = false;   
    private Animator animator;
    private Rigidbody rb;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.mass = mainFull;
        discharge_rate = (mainFull - mainEmpty) / burnTime;
        fire.Stop();
        smoke.Stop();
    }

    void FixedUpdate()
    {
        if (rb.mass >= mainEmpty && t > launchTime)
        {
            rb.AddForce(transform.forward * thrust * Time.fixedDeltaTime * 50.0f);
            rb.mass -= Time.fixedDeltaTime * discharge_rate;
        }
        else if (!fuelDischarged && t > launchTime)
        {
            fire.Stop();
            smoke.Stop();
            fuelDischarged = !fuelDischarged;
        }
        else if(t >= launchTime)
        {
            if (transform.position.y < 100.0f && !landerDeployed)
            {
                animator.SetTrigger("land");
                landerDeployed = true;
                fire.Play();
                smoke.Play();
            }
            else
            {
                if (rb.velocity.y < 10.0 && transform.position.y < 100.0f)
                {                
                    rb.AddForce(transform.forward * 9.81f * rb.mass * Time.fixedDeltaTime * 10.0f * rb.velocity.magnitude);
                    rb.AddForceAtPosition((new Vector3(transform.forward.x, 0.0f, transform.forward.z)) * thrust * Time.fixedDeltaTime * 10.0f, transform.position);
                }
            }
        }
    }

    private void Update()
    {
        if (launch)
        {
            text.text = Mathf.Round(rb.velocity.magnitude).ToString() + " KPH";
            t += Time.deltaTime;
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                fire.Play();
                smoke.Play();
                launch = true;
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Terrain" && landerDeployed)
        {
            text.text = "0 KPH";
            fire.Stop();
            smoke.Stop();
            Destroy(this);
        }
    }
}
