using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.Rendering;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class Bulle : MonoBehaviour
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
    public GameObject blocSavonSurLequelOnEstAttache;
    bool accrochedAuSavon;


        /*Pour éviter de sortir du bloc de savon*/
        float minDistSavon ;
        float maxDistSavon ;


    float _timeChargingJump = 0f;

    Vector2 movementDirectionOnGround;

    float inputVerticalDirection;  
    float inputHorizontalDirection;

    AngleSavon currentZoneAngle;
    Animator myAnimator;
    Animator barreChargementAnimator;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        myAnimator = this.GetComponent<Animator>();
        barreChargementAnimator = transform.Find("BarreChargement").GetComponent<Animator>();
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
            if (currentZoneAngle != null && this.oppositeDirectionPressed() )
            {
                currentZoneAngle.changerDeBord(this);
            }
            /*Sur un mur vertical*/
            if (movementDirectionOnGround == new Vector2(0,1))
            {

                /*On bloque si on est au bord*/
                if (transform.position.y > maxDistSavon)
                    inputVerticalDirection = Mathf.Min(inputVerticalDirection, 0);
                if (transform.position.y < minDistSavon)
                    inputVerticalDirection = Mathf.Max(inputVerticalDirection, 0);
                  

                rb.linearVelocity = new Vector2(-jumpDirection.x * 8 , inputVerticalDirection * horizontalMovementSpeed);
            }

            /*Sur un mur horizontal*/
            else if (movementDirectionOnGround == new Vector2(1, 0))
            {
              
                /*On bloque si on est au bord*/
                if (transform.position.x > maxDistSavon) 
                    inputHorizontalDirection = Mathf.Min(inputHorizontalDirection,0);
                if (transform.position.x < minDistSavon)
                    inputHorizontalDirection = Mathf.Max(inputHorizontalDirection, 0);

                rb.linearVelocity = new Vector2(inputHorizontalDirection * horizontalMovementSpeed, -jumpDirection.y * 8);
            }
            else
            {
                throw new System.Exception("Pas de sens de déplacement");
            }


            if (Input.GetKey(KeyCode.Space) && accrochedAuSavon)
            {
                if (_timeChargingJump == 0)
                {
                    barreChargementAnimator.SetTrigger("startChargement");
                    myAnimator.SetTrigger("ChargeJump");
                }
                    
                _timeChargingJump += Time.deltaTime;
                Debug.Log("On charge le saut charge Time : " + (_timeChargingJump / maxChargeTimeJump) * 100 + " % ");
            }

            else
            {
                if (_timeChargingJump > 0 && accrochedAuSavon)
                {

                    Debug.Log(" RELEASE ");
                    
                    barreChargementAnimator.SetTrigger("releaseButton");
                    myAnimator.SetTrigger("Jump");

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

            /* Custom Gravity*/
            rb.linearVelocityY += customGravity * Time.fixedDeltaTime;

        }
    }


    void doAJump(Vector2 jumpDirection, float jumpForce)
    {
        if (jumpDirection == Vector2.zero)
            throw new System.Exception("Ah ben non on peut pas sauter à 0 "); 
        unstick();
        myAnimator.SetTrigger("Jump");
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        Debug.Log("On saute avec une force de " + jumpForce);
    }


    public void stickTo(Soap blocSavon)
    {
        
        Debug.Log("On s'accroche au bloc : " + blocSavon.gameObject+ "\n \tJump direction : " + blocSavon.getJumpDirection() + "\n\t Horizontal ? " + blocSavon.isWallHorizontal());

        blocSavonSurLequelOnEstAttache = blocSavon.gameObject;
        accrochedAuSavon = true;
        this.jumpDirection = blocSavon.getJumpDirection();
        this.rotate(this.jumpDirection);
        myAnimator.SetBool("Soap", true);
            


        /*On récupère la taille max*/
        
        if (blocSavon.isWallHorizontal())
        {
            this.movementDirectionOnGround = new Vector2(1, 0);
            minDistSavon = blocSavon.getCoin(Utils.CoinRectangle.BAS_GAUCHE).x;
            maxDistSavon = blocSavon.getCoin(Utils.CoinRectangle.BAS_DROITE).x;
        }
        else
        {
            this.movementDirectionOnGround = new Vector2(0, 1);
            minDistSavon = blocSavon.getCoin(Utils.CoinRectangle.BAS_GAUCHE).y;
            maxDistSavon = blocSavon.getCoin(Utils.CoinRectangle.HAUT_GAUCHE).y;
        }

        rb.AddForce(-jumpDirection * 8 );

        /*On s'arrête d'un coup A retirer ? *//*
        rb.linearVelocity = Vector2.zero;*/
    }


    void unstick()
    {
        myAnimator.SetBool("Soap", false);
        accrochedAuSavon = false;
    }



    void pop()
    {
        GameManager.Instance.LooseGame();
        Destroy(this.gameObject);
        return;
    }

    
    /*Retourne vrai si on est sur un mur et qu'on appuie sur une autre*/
    bool oppositeDirectionPressed()
    {
        if (movementDirectionOnGround == Vector2.up)
        {
            return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow);
        }

        if (movementDirectionOnGround == Vector2.right)
        {
            return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow);
        }
        return false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(murTag) && !accrochedAuSavon)
        {
            pop();
            return;
        }
        else if (collision.gameObject.CompareTag(savonTag))
        {
            if (!accrochedAuSavon)
                /* testInfoCollider(collision);*/
                stickTo(collision.gameObject.GetComponent<Soap>());
            
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("angle"))
        {
            Debug.Log("On rentre dans un angle");
            currentZoneAngle = collision.gameObject.GetComponent<AngleSavon>();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("angle"))
        {
            Debug.Log("On sort d'un angle");
            currentZoneAngle = null;
        }
    }


    

    void rotate(Vector2 normalVector)
    {
        float newAngle =  Utils.calculateRealAngle(Vector2.up, normalVector);
        transform.rotation = Quaternion.Euler(0, 0, newAngle); 
    }

    void testInfoCollider(Collision2D collision)
    {
        Collider2D hitCollider = collision.collider;

        if (hitCollider is PolygonCollider2D polygonCollider)
        {
            Debug.Log("Nombre de points dans le collider: " + polygonCollider.points.Length);
            // Affiche les points du collider
            foreach (Vector2 point in polygonCollider.points)
            {
                Debug.Log("Point: " + point);
            }
        }
    }

}