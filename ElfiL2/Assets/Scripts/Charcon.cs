using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charcon : MonoBehaviour
{
    // Start is called before the first frame update

    Transform tr;
    Rigidbody rb;
    Vector3 groundPosition, groundNormal, movement, gravity;
    bool grounded = false, onStep = false;
    [SerializeField] float maxSpeed, acc, gravCoef;
    float speed;
    [SerializeField]ParticleSystem pS;

    void Start()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
        groundPosition = tr.position;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        groundDetection();
        movementSet();
        if (!grounded)
        {
            gravity -= tr.up * gravCoef;
        }
        else
        {
            gravity = Vector3.zero;
        }
        rb.velocity = (movement + gravity) * Time.fixedDeltaTime;
    }


    void movementSet()
    {
        movement = Input.GetAxisRaw("Horizontal") * Camera.main.transform.right + Input.GetAxisRaw("Vertical") * Camera.main.transform.forward;
        movement = Vector3.ProjectOnPlane(movement, groundNormal).normalized;
        if(Vector3.Angle(movement, tr.up)<45 && tr.position.y - groundPosition.y >.5f)
        {
            speed = 0;
        }
        else
        {
            movement *= speedCalculation();
        }
    }

    float speedCalculation()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            if(speed>0)
            {
                speed -= acc ;
            }
            else
            {
                speed = 0;
            }
        }
        else
        {
            if(!onStep)
            {
                StartCoroutine(step());
            }
            if(speed<maxSpeed)
            {
                speed += acc ;
            }
            else
            {
                speed = maxSpeed;
            }
        }
        return speed;
    }

    void groundDetection()
    {
        Ray ray = new Ray(tr.position+ (tr.up *0.5f), -tr.up);
        RaycastHit hit;
        if (Physics.SphereCast(ray, .25f, out hit, 1))
        {
            groundPosition = hit.point;
            groundNormal = hit.normal;
            grounded = true;
        }
        else
        {
            grounded = false;
            groundNormal = tr.up;
        }
    }
    IEnumerator step()
    {
        onStep = true;
        pS.Play();
        yield return new WaitForSeconds(.5f);
        onStep = false;
    }
}
