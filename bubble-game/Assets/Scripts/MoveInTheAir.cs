using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class MoveInTheAir : MonoBehaviour
{

    private InputAction moveAction;
    [SerializeField] InputActionReference movementAction;
    [SerializeField] float movementSpeed = 5f;
    Vector2 movementDirection;
    Rigidbody2D rb;
    float horizontalMoveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }


    

    // Update is called once per frame
    void Update()
    {

  
        horizontalMoveDirection = Input.GetAxis("Horizontal");
        rb.linearVelocityX = horizontalMoveDirection * movementSpeed * Time.fixedDeltaTime;

        Debug.Log("Test " + movementDirection.x * movementSpeed);
    }

    public void OnMove(InputValue input)
    {
        /*  movementDirection = context.ReadValue<Vector2>(); // Cela renverra un Vector2 comme -1, 0, 1 */
        Debug.Log(movementAction.action.activeValueType);
        Debug.Log(movementAction.action.activeControl);

        Debug.Log(input);


    }
}