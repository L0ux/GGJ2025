using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.Rendering;
using UnityEditor.Experimental.GraphView;

public class MoveInTheAir : MonoBehaviour
{
    [SerializeField] float customGravity = 2; // Accélération gravité personnalisée
    [SerializeField] float horizontalMovementSpeed = 5f;

    [SerializeField] float maxJumpForce = 30;

    [SerializeField] float maxChargeTimeJump;
    [SerializeField] float decelarationHorizontal;
    Vector2 jumpDirection;
    
    
    /*A foutre ailleurs*/
    [SerializeField]private string murTag;
    [SerializeField]private string savonTag;


    Rigidbody2D rb;
    bool accrochedAuSavon;


    float _timeChargingJump = 0f;

    Vector2 movementDirectionOnGround;

    float inputVerticalDirection;  
    float inputHorizontalDirection;


    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }
   
    // Update is called once per frame
    void Update()
    {
        /***************MOUVEMENT HORIZONTAL************/

        /*On lit l'input*/
        inputHorizontalDirection = Input.GetAxis("Horizontal");
        inputVerticalDirection = Input.GetAxis("Vertical");


        /*Mouvement sur le sol*/
        if (accrochedAuSavon)
        {
            /*Sur un mur vertical*/
            if (movementDirectionOnGround == new Vector2(0,1))
            {
                Debug.Log("Vertical");
                rb.linearVelocity = new Vector2(0, inputVerticalDirection * horizontalMovementSpeed) ;
            }

            /*Sur un mur horizontal*/
            else if (movementDirectionOnGround == new Vector2(1, 0))
            {
                Debug.Log("Horizontal");
                rb.linearVelocity = new Vector2(inputHorizontalDirection * horizontalMovementSpeed, 0);
            }
            else
            {
                throw new System.Exception("Pas de sens de déplacement");
            }

        }
        /*Mouvement dans les airs*/
        else
        {
            rb.AddForceX(inputHorizontalDirection * horizontalMovementSpeed);
        }





        /*Freinage*/
        /* ?? peut être mettre le freinage uniquement si on n'appuie pas sur la touche ?? */
        rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, 0, decelarationHorizontal * Time.fixedDeltaTime);

        /*?? Ajouter une vitesse max ??*/


        /***************GESTION DU SAUT************/

        if(Input.GetKey(KeyCode.Space) && accrochedAuSavon)
        {
            _timeChargingJump += Time.deltaTime;
            Debug.Log("On charge le saut charge Time : " + _timeChargingJump);
        }
        else 
        {
            if (_timeChargingJump > 0 && accrochedAuSavon)
            {
                _timeChargingJump = Mathf.Max(_timeChargingJump, maxChargeTimeJump);
                
                /*On récup le pourcentage de temps appuyé,
                 * 0 si 0secondes 
                 * 1 si appuyé maxChargeTimeJump*/
                float timePressedPercent =  _timeChargingJump/maxChargeTimeJump;

                doAJump(this.jumpDirection, Mathf.Lerp(0,maxJumpForce, timePressedPercent));
            }
            _timeChargingJump = 0;
        }


    }

    void FixedUpdate()
    {
        if(!accrochedAuSavon)
            rb.linearVelocityY += customGravity * Time.fixedDeltaTime;
    }


    void doAJump(Vector2 jumpDirection, float jumpForce)
    {
        if (jumpDirection == Vector2.zero)
            throw new System.Exception("Ah ben non on peut pas sauter à 0 "); 
        unstick();
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        Debug.Log("On saute avec une force de " + jumpForce);
    }


    void stickTo(Soap blocSavon)
    {
        Debug.Log("On s'accroche au bloc : " + blocSavon.gameObject+ "\n \tJump direction : " + blocSavon.getJumpDirection() + "\n\t Horizontal ? " + blocSavon.isWallHorizontal());
        accrochedAuSavon = true;
       
        this.jumpDirection = blocSavon.getJumpDirection();
        
        if(blocSavon.isWallHorizontal())
            this.movementDirectionOnGround = new Vector2(1, 0);
        else
            this.movementDirectionOnGround = new Vector2(0, 1);



        /*On s'arrête d'un coup A retirer ? */
        rb.linearVelocity = Vector2.zero;
    }


    void unstick()
    {
        accrochedAuSavon = false;
    }



    void pop()
    {
        Destroy(this.gameObject);
        return;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag(murTag))
        {
            pop();
            return;
        }

        else if (collision.gameObject.CompareTag(savonTag))
        {
            if(! accrochedAuSavon)
                stickTo(collision.gameObject.GetComponent<Soap>());
        }
    }


}