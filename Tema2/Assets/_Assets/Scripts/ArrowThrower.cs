using UnityEngine;
using UnityEngine.XR;

public class ArrowThrower : MonoBehaviour
{
    private GameObject Arrow;
    
    private float startTime, endTime, swipeDistance, swipeTime, maxThrowTime;

    private Vector2 startPos;
    private Vector2 endPos;
    
    public float MinSwipeDistance = 0;
    private float ArrowVelocity = 0;
    private float ArrowSpeed = 0;
    private float MaxArrowSpeed = 1;
    private Vector3 angle;
    
    private bool thrown;
    private Vector3 newPosition;
    
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupArrow();
    }

    private void SetupArrow()
    {
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Player");

        foreach (var arrow in arrows)
        {
            if(arrow == null)
                continue;
            else
            {
                Arrow = arrow;
                break;
            }
        }
        
        rb = Arrow.GetComponent<Rigidbody>();
        ResetArrow();
    }

    private void ResetArrow()
    {
        angle = Vector3.zero;
        endPos = Vector2.zero;
        startPos = Vector2.zero;
        ArrowSpeed = 0;
        startTime = 0;
        endTime = 0;
        swipeDistance = 0;
        swipeTime = 0;
        thrown = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        Arrow.transform.position = transform.position;
    }
    
    private void Update()
    {
        if (thrown)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Raycast not necessary for VR controllers, use input directly
            startTime = Time.time;
        }
        else if (Input.GetKeyUp(KeyCode.G))
        {
            endTime = Time.time;
            swipeDistance = Vector3.Distance(newPosition, transform.position);
            swipeTime = endTime - startTime;
            
            if (swipeTime < 1f && swipeDistance > 0.3f) // Adjust these thresholds as needed
            {
                //throw arrow
                CalSpeed();
                CalAngle();
                rb.AddForce(new Vector3((angle.x * ArrowSpeed), (angle.y * ArrowSpeed / 3), (angle.z * ArrowSpeed) * 2));
                rb.useGravity = true;
                rb.isKinematic = false;
                thrown = true;
                rb.mass = 2f;
                Invoke("ResetArrow", 2f);
            }
            else
                ResetArrow();
        }
    }
    
    private void CalAngle()
    {
        angle = Camera.main.ScreenToWorldPoint(new Vector3(endPos.x, endPos.y + 50f, (Camera.main.nearClipPlane + 5)));
    }
    
    void CalSpeed()
    {
        if(swipeTime > 0)
            ArrowVelocity = swipeDistance / (swipeDistance - swipeTime);
        
        ArrowSpeed = ArrowVelocity * 40;
        
        if(ArrowSpeed <= MaxArrowSpeed)
        {
            ArrowSpeed = MaxArrowSpeed;
        }
        swipeTime = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "0")
        { 
            GameManager.instance.UpdateScore(0);
        }
        
        else if(other.gameObject.tag == "5")
        { 
            GameManager.instance.UpdateScore(5);
        }
        
        else if(other.gameObject.tag == "15")
        { 
            GameManager.instance.UpdateScore(15);
        }
        
        else if(other.gameObject.tag == "30")
        { 
            GameManager.instance.UpdateScore(30);
        }
        
        else if(other.gameObject.tag == "50")
        { 
            GameManager.instance.UpdateScore(50);
        }
    }
}