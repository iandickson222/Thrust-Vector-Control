using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Booster : MonoBehaviour
{
    public float boosterEmpty = 68000.0f;
    public float boosterFull = 571000.0f;
    public float thrust = 12500000.0f;
    public float burnTime = 124.0f;
    public ParticleSystem fire;
    public ParticleSystem smoke;

    private float discharge_rate = 0.0f;
    private Rigidbody rb;
    private FixedJoint fixedJoint;
    private Animator animator;
    private Vector3 direction;
    private float disconnectTime = 0.5f;
    private bool landerDeployed = false;
    private bool launch = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fixedJoint = GetComponent<FixedJoint>();
        animator = GetComponent<Animator>();
        rb.mass = boosterFull;
        discharge_rate = (boosterFull - boosterEmpty) / burnTime;
        fire.Stop();
        smoke.Stop();
    }

    private void Update()
    {
        if (!launch && Input.GetKeyDown(KeyCode.Space))
        {
            fire.Play();
            smoke.Play();
            launch = true;          
        }
    }

    void FixedUpdate()
    {
        if(launch)
        {
            if (fixedJoint != null)
            {
                if (rb.mass >= boosterEmpty)
                {
                    rb.AddForce(transform.forward * thrust * Time.fixedDeltaTime * 50.0f);
                    rb.mass -= Time.fixedDeltaTime * discharge_rate;
                }
                else
                {
                    if (gameObject.name == "Right Booster")
                    {
                        direction = -transform.right;
                    }
                    else if (gameObject.name == "Left Booster")
                    {
                        direction = transform.right;
                    }

                    Destroy(fixedJoint);
                }
            }
            else if (disconnectTime > 0.0f)
            {
                rb.AddForce(direction * thrust * Time.fixedDeltaTime);
                disconnectTime -= Time.fixedDeltaTime;
                fire.Stop();
                smoke.Stop();
            }
            else
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Terrain" && landerDeployed)
        {
            fire.Stop();
            smoke.Stop();
            Destroy(this);
        }
    }
}
