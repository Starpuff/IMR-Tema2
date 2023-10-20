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
    
    private float smooth = 0.7f;
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupArrow();
    }

    private void SetupArrow()
    {
        GameObject arrow = GameObject.FindGameObjectsWithTag("Player")[0];
        Arrow = arrow;
        
        rb = arrow.GetComponent<Rigidbody>();
        rb.mass = 1.2f;
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
            
            if (swipeTime < 0.5f && swipeDistance > 0.3f) // Adjust these thresholds as needed
            {
                //throw arrow
                CalSpeed();
                CalAngle();
                rb.AddForce(new Vector3((angle.x * ArrowSpeed), (angle.y * ArrowSpeed / 3), (angle.z * ArrowSpeed) * 2));
                rb.useGravity = true;
                rb.isKinematic = false;
                thrown = true;
                Invoke("ResetArrow", 4f);
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
}
