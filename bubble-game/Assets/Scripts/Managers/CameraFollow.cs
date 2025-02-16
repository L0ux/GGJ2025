using UnityEngine;
using GameUtils;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assigne ici ton joueur dans l'inspecteur
    public float INIT_POS_Y;
    private float MAX_POS_Y = 100;
    
    private float limitPos;

    private Bulle playerController;
    public float fixedX; // Position X fixe
    public float fixedZ; // Position Z fixe (classique pour une caméra 2D)

    public  float INIT_SMOOTH_SPEED = 0.5f; // Contrôle la vitesse de transition de la caméra
    private  float MAX_SMOOTH_SPEED = 5.0f;
    private bool isAtLowerCameraY = true;
    private bool isAtHigherCameraY = false;

    public float increaseSmoothSpeed;
    private float smoothSpeed;
    public float lerpDuration = 1f; // Durée de la transition fluide en secondes
    private float lerpTime = 0f; // Compteur pour le temps de transition

    private Direction oldDirection = Direction.NEUTRAL; // Ancienne direction du joueur

    void Start()
    {
        limitPos = INIT_POS_Y - getDeltaHeightForMiddle();
        playerController = player.GetComponent<Bulle>();
        smoothSpeed = INIT_SMOOTH_SPEED;
        Vector3 targetPosition = new Vector3(fixedX, INIT_POS_Y, fixedZ);
        transform.position = targetPosition;
    }

    void UpdateLimitPos(Direction direction)
    {
        switch(direction){
            case Direction.MOVING_UP:
                limitPos = Camera.main.transform.position.y - getDeltaHeightForMiddle();
                break;
            case Direction.MOVING_DOWN:
                limitPos = Camera.main.transform.position.y + getDeltaHeightForMiddle();
                break;
            case Direction.NEUTRAL:
                limitPos = player.position.y;
                break;
            default:
                limitPos = player.position.y;
                break;
        }
    }

    float getDeltaHeightForMiddle(){
        return (Camera.main.orthographicSize / 6);
    }

    float getNewYPosCamera(Direction direction){
        if(player.position.y < (INIT_POS_Y + MAX_POS_Y)/2){
            switch(direction){
                case Direction.MOVING_UP:
                    return Mathf.Max(player.position.y + getDeltaHeightForMiddle(), INIT_POS_Y);
                case Direction.MOVING_DOWN:
                    return Mathf.Max(player.position.y - getDeltaHeightForMiddle(), INIT_POS_Y);
                case Direction.NEUTRAL:
                    return Mathf.Max(player.position.y, INIT_POS_Y);
                default:
                    return Mathf.Max(player.position.y, INIT_POS_Y);
            }
        } else {
            switch(direction){
                case Direction.MOVING_UP:
                    return player.position.y + getDeltaHeightForMiddle();
                case Direction.MOVING_DOWN:
                    return player.position.y - getDeltaHeightForMiddle();
                case Direction.NEUTRAL:
                    return player.position.y;
                default:
                    return player.position.y;
            }
        }
    }

    bool isCameraLockedBot(Direction direction){
        switch(direction){
            case Direction.MOVING_UP:
                return player.position.y < INIT_POS_Y - getDeltaHeightForMiddle();
            case Direction.MOVING_DOWN:
                return player.position.y > INIT_POS_Y + getDeltaHeightForMiddle();
            case Direction.NEUTRAL:
                return player.position.y < INIT_POS_Y;
            default:
                return player.position.y < INIT_POS_Y;
        }
    }

    bool isCameraLockedTop(Direction direction){
        switch(direction){
            case Direction.MOVING_UP:
                return player.position.y > Camera.main.transform.position.y + getDeltaHeightForMiddle();
            case Direction.MOVING_DOWN:
                return player.position.y < Camera.main.transform.position.y - getDeltaHeightForMiddle();
            case Direction.NEUTRAL:
                return player.position.y > Camera.main.transform.position.y;
            default:
                return player.position.y > Camera.main.transform.position.y;
        }
    }

    private void moveCamera(){
        if (player != null)
        {
            float targetYPos = getNewYPosCamera(playerController.GetDirection());
            Vector3 targetPosition = new Vector3(fixedX, targetYPos, fixedZ);

            if (oldDirection != playerController.GetDirection())
            {
                lerpTime = lerpDuration; 
                smoothSpeed = INIT_SMOOTH_SPEED;
            }

            if (lerpTime > 0f)
            {
                smoothSpeed = Mathf.Min(MAX_SMOOTH_SPEED, smoothSpeed * increaseSmoothSpeed);
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
                lerpTime -= Time.deltaTime;
            }
            else
            {
                transform.position = targetPosition;
                smoothSpeed = INIT_SMOOTH_SPEED;
            }
            oldDirection = playerController.GetDirection();
            UpdateLimitPos(playerController.GetDirection());
        } else {
            Vector3 targetPosition = new Vector3(fixedX, INIT_POS_Y, fixedZ);
            transform.position = targetPosition;
        }
    }

    void Update()
    {
        moveCamera();
    }
}
