using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class MoveInTheAir : MonoBehaviour
{
    [SerializeField] float customGravity = 2; // Accélération gravité personnalisée
    [SerializeField] float horizontalMovementSpeed = 50f;
    Vector2 movementDirection;
    Rigidbody2D rb;
    float horizontalMoveDirection;
    bool accrochedAuSavon;


    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        stick();


    }
   
    // Update is called once per frame
    void Update()
    {
        horizontalMoveDirection = Input.GetAxis("Horizontal");

        rb.linearVelocityX = horizontalMoveDirection * horizontalMovementSpeed * Time.fixedDeltaTime;
        
        Debug.Log("Test " + movementDirection.x * horizontalMovementSpeed);
    }

    void FixedUpdate()
    {
        if(!accrochedAuSavon)
            rb.linearVelocityY += customGravity * Time.fixedDeltaTime;
    }

    public void OnJump()
    {
        unstick();
    }


    void stick()
    {
        accrochedAuSavon = true;

    }


    void unstick()
    {
        accrochedAuSavon = false;

    }
}