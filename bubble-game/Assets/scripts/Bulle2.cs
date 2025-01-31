using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class Bulle2 : MonoBehaviour
{




    /*Variable à ajuster*/
    [SerializeField] float flottementVersLeHaut = 2; // Accélération gravité Vers le haut


    [SerializeField] float moveSpeedOnTheGRound = 8f;
    [SerializeField] float moveSpeedOnTheAir;
    [SerializeField] float decelarationInTheAir;


    [SerializeField] float maxJumpForce = 30;
    [SerializeField] float maxChargeTimeJump;


    /*[SerializeField] float maxSpeedInTheAir;*/
    

    /*Affecté dans le start*/
    Rigidbody2D rb;
    Animator myAnimator;
    Animator barreChargementAnimator;


    Soap2 blocSavonSurLequelOnEstAttache;

    Vector2 directionSaut;

    bool inputAccepted = true;

    float _timeChargingJump = 0;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        myAnimator = this.GetComponent<Animator>();
        barreChargementAnimator = transform.Find("BarreChargement").GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float moveInputHorizontal = 0;
        float moveInputVerical = 0;

        if (inputAccepted)
        {
            moveInputHorizontal = Input.GetAxisRaw("Horizontal");  // "Horizontal" : touches directionnelles (A, D ou flèches)
            moveInputVerical = Input.GetAxisRaw("Vertical");  // "Horizontal" : touches directionnelles (A, D ou flèches)
        }

        /*************DANS LES AIRS************************/
        if (!isAccrocheAuSavon())
        {
            /* On pousse vers le haut*/
            rb.linearVelocityY += flottementVersLeHaut * Time.fixedDeltaTime;


            // Déplacement horizontal, conserve la vitesse verticale
            rb.linearVelocityX += moveInputHorizontal * moveSpeedOnTheAir *Time.fixedDeltaTime; 


            /*ON ralentit la vitesse*/
            rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, 0, decelarationInTheAir * Time.fixedDeltaTime);
            

        }

        /*************SUR LE SOL*******************************/
        else
        {
            /*Si on en train de charger un saut */
            if (Input.GetKey(KeyCode.Space))
                chargerSaut();
            else
            {
                /*On saute si un saut a été chargé*/
                if (_timeChargingJump > 0)
                    sauter();
                else
                {
                    /*Sinon on gère le déplacement*/
                    /*Sur un mur horizontal*/
                    if (this.directionSaut.x == 0)
                        rb.linearVelocityX = moveInputHorizontal * moveSpeedOnTheGRound; // Déplacement horizontal, conserve la vitesse verticale

                    /*Sur un mur vertical*/
                    if (this.directionSaut.y == 0)
                        rb.linearVelocityY = moveInputVerical * moveSpeedOnTheGRound; // Déplacement horizontal, conserve la vitesse verticale

                    /*On se colle contre le bord de savon*/
                    rb.linearVelocity += -this.directionSaut *1;
                }

                _timeChargingJump = 0;
            }

        }
    }

    bool isAccrocheAuSavon()
    {
        return blocSavonSurLequelOnEstAttache != null;
    }



    void chargerSaut()
    {
        if (_timeChargingJump == 0)
        {
            barreChargementAnimator.SetTrigger("startChargement");
            myAnimator.SetTrigger("ChargeJump");
            rb.linearVelocity = Vector2.zero;
        }

        _timeChargingJump += Time.deltaTime;
    }

    void sauter()
    {
        barreChargementAnimator.SetTrigger("releaseButton");

        _timeChargingJump = Mathf.Min(_timeChargingJump, maxChargeTimeJump);


        /*On récup le pourcentage de temps appuyé*/
        float pourcentagePuissanceSaut = _timeChargingJump / maxChargeTimeJump;
        if (directionSaut == Vector2.zero)
            throw new System.Exception("Ah ben non on peut pas sauter à 0 ");

        myAnimator.SetTrigger("Jump");
        myAnimator.SetBool("Soap", false);

        rb.AddForce(directionSaut * (pourcentagePuissanceSaut * maxJumpForce), ForceMode2D.Impulse);
        StartCoroutine(blocInput());
        Debug.Log("On saute avec une force de " + (pourcentagePuissanceSaut * maxJumpForce));
    }


    IEnumerator blocInput()
    {
        inputAccepted = false;
        yield return new WaitForSeconds(0.05f);
        inputAccepted = true;

    }

    private void stickTo(Soap2 blocSavon)
    {
        Debug.Log("On s'accroche au bloc " + blocSavon);
        this.directionSaut =blocSavon.getJumpDirection();
        blocSavonSurLequelOnEstAttache = blocSavon;

    }
    private void unStick()
    {
        Debug.Log("On se détache du bloc " + blocSavonSurLequelOnEstAttache);
        this.directionSaut = Vector2.zero;
        blocSavonSurLequelOnEstAttache = null;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bordCollant")
        {
            Soap2 hehe = collision.gameObject.GetComponent<Soap2>();
            blocSavonSurLequelOnEstAttache = hehe;
            this.directionSaut = hehe.getJumpDirection();
            this.directionSaut = hehe.getJumpDirection();
        }
    }



    /*  private void OnTriggerEnter2D(Collider2D collision)
      {
          if(collision.gameObject.tag == "bordCollant")
          {
              stickTo(collision.gameObject.GetComponent<Soap2>());
          }
      }
  */



    /*Vérif si il n'y a pas d'autres collisions*/
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bordCollant")
        {
            if (collision.gameObject.GetComponent<Soap2>() == blocSavonSurLequelOnEstAttache)
            {
                unStick();
            }
        }

    }


}
