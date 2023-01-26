using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public float primaryDamage;
    public float knockbackForce = 500f;
    public Collider2D primaryCollider;
    public GameObject player;
    public bool faceRight;
    public bool faceLeft;
    public bool faceUp;
    public bool faceDown;
    PlayerController playerController;
    Vector3 scaleRight;
    Vector3 posRight;
    Vector3 scaleUp = new Vector3(0.32f, 0.64f, 1f);
    Vector3 posUp = new Vector3(0.22f, 0.72f, 0);
    Vector3 posDown = new Vector3(-0.16f, -1f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        posRight = gameObject.transform.localPosition;
        scaleRight = gameObject.transform.localScale;
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void hitboxShape()
    {
        if(playerController.currentDirection == PlayerController.CurrentDirection.UP)
        {
            if(!faceUp)
            {
                faceUp = true;
                faceDown = false;
                faceLeft = false;
                faceRight = false;
                gameObject.transform.localScale = scaleUp;
                gameObject.transform.localPosition = posUp;
            }
        }
        else if(playerController.currentDirection == PlayerController.CurrentDirection.DOWN)
        {
            if(!faceDown)
            {
                faceUp = false;
                faceDown = true;
                faceLeft = false;
                faceRight = false;
                gameObject.transform.localScale = scaleUp;
                gameObject.transform.localPosition = posDown;
            }
        }
        else if(playerController.currentDirection == PlayerController.CurrentDirection.LEFT)
        {
            if(!faceLeft)
            {
                faceUp = false;
                faceDown = false;
                faceLeft = true;
                faceRight = false;
                gameObject.transform.localScale = scaleRight;
                gameObject.transform.localPosition = new Vector3(-posRight.x, posRight.y, 0f);
            }
        }
        else if(playerController.currentDirection == PlayerController.CurrentDirection.RIGHT)
        {
            if(!faceRight)
            {
                faceUp = false;
                faceDown = false;
                faceLeft = false;
                faceRight = true;
                gameObject.transform.localScale = scaleRight;
                gameObject.transform.localPosition = posRight;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();

        if(damageableObject != null)
        {
            // Calculate direction between character and enemy
            Vector3 parentPosition = transform.parent.position;

            Vector2 direction = (Vector2) (collider.gameObject.transform.position - parentPosition).normalized;
            Vector2 knockback = direction * knockbackForce;
            damageableObject.OnHit(primaryDamage, knockback);
        }
        else
        {
            Debug.LogWarning("Collider does not implement IDamageable");
        }
    }
}
