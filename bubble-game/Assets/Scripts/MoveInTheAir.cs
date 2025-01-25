using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.Rendering;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class MoveInTheAir : MonoBehaviour
{
    [SerializeField] float customGravity = 2; // Accélération gravité personnalisée
    [SerializeField] float horizontalMovementSpeed = 5f;

    [SerializeField] float maxJumpForce = 30;

    [SerializeField] float maxChargeTimeJump;
    [SerializeField] float maxSpeedInTheAir;
    [SerializeField] float decelarationHorizontal;



    Vector2 jumpDirection;
    
    
    /*A foutre ailleurs*/
    [SerializeField] private string murTag;
    [SerializeField] private string savonTag;
    

    Rigidbody2D rb;


    /*Infos sur le savon*/
    GameObject blocSavonSurLequelOnEstAttache;
    bool accrochedAuSavon;


        /*Pour éviter de sortir du bloc de savon*/
        float minDistSavon ;
        float maxDistSavon ;


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
        /*On lit l'input*/
        inputHorizontalDirection = Input.GetAxis("Horizontal");
        inputVerticalDirection = Input.GetAxis("Vertical");


        /*************** SUR LE SOL************/
        if (accrochedAuSavon)
        {
            /*Sur un mur vertical*/
            if (movementDirectionOnGround == new Vector2(0,1))
            {

                /*On bloque si on est au bord*/
                if (transform.position.y > maxDistSavon)
                    inputVerticalDirection = Mathf.Min(inputVerticalDirection, 0);
                if (transform.position.y < minDistSavon)
                    inputVerticalDirection = Mathf.Max(inputVerticalDirection, 0);
                  

                rb.linearVelocity = new Vector2(0, inputVerticalDirection * horizontalMovementSpeed);
                
            }

            /*Sur un mur horizontal*/
            else if (movementDirectionOnGround == new Vector2(1, 0))
            {
                /*On bloque si on est au bord*/
                if (transform.position.x > maxDistSavon) 
                    inputHorizontalDirection = Mathf.Min(inputHorizontalDirection,0);
                if (transform.position.x < minDistSavon)
                    inputHorizontalDirection = Mathf.Max(inputHorizontalDirection, 0);

                rb.linearVelocity = new Vector2(inputHorizontalDirection * horizontalMovementSpeed, 0);
            }
            else
            {
                throw new System.Exception("Pas de sens de déplacement");
            }


            if (Input.GetKey(KeyCode.Space) && accrochedAuSavon)
            {
                _timeChargingJump += Time.deltaTime;
                Debug.Log("On charge le saut charge Time : " + (_timeChargingJump / maxChargeTimeJump) * 100 + " % ");
            }

            else
            {
                if (_timeChargingJump > 0 && accrochedAuSavon)
                {
                    _timeChargingJump = Mathf.Min(_timeChargingJump, maxChargeTimeJump);

                    /*On récup le pourcentage de temps appuyé*/
                    float timePressedPercent = _timeChargingJump / maxChargeTimeJump;
                    doAJump(this.jumpDirection, Mathf.Lerp(0, maxJumpForce, timePressedPercent));
                }
                _timeChargingJump = 0;
            }
        }
        /***************DANS LES AIRS************/
        else
        {
            rb.AddForceX(inputHorizontalDirection * horizontalMovementSpeed);

            /*Freinage pour revenir à 0*/
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, Vector2.zero, decelarationHorizontal * Time.fixedDeltaTime);
            
            rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocityX, -maxSpeedInTheAir, maxSpeedInTheAir), Mathf.Clamp(rb.linearVelocityY, -maxSpeedInTheAir, maxSpeedInTheAir));
            
            rb.linearVelocityY += customGravity * Time.fixedDeltaTime;

        }
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
        blocSavonSurLequelOnEstAttache = blocSavon.gameObject;

        accrochedAuSavon = true;
       
        this.jumpDirection = blocSavon.getJumpDirection();

        BoxCollider2D colliderDuSavon = blocSavonSurLequelOnEstAttache.GetComponent<BoxCollider2D>();
        Vector2[] anglesDuSavon = Utils.getAngles(colliderDuSavon);


        /*On récupère la taille max*/
        if (blocSavon.isWallHorizontal())
        {
            this.movementDirectionOnGround = new Vector2(1, 0);
            minDistSavon = anglesDuSavon[2].x;
            maxDistSavon = anglesDuSavon[3].x;
        }
        else
        {
            this.movementDirectionOnGround = new Vector2(0, 1);
            minDistSavon = anglesDuSavon[0].y;
            maxDistSavon = anglesDuSavon[2].y;
        }


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

            if(!accrochedAuSavon)
                stickTo(collision.gameObject.GetComponent<Soap>());
           
        }
    }


}