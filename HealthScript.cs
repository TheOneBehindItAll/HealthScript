using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HealthScript : MonoBehaviour
{
    //Used for calculating the damage that the player takes and how much health the enemys have
    public int dificultyMode;
    //Used for calculating the damage the player does (Default is 1)
    public int playerDamage = 1;
    //Curent objects health
    public int hp;
    //curent objects maximum health
    public int maxHP;
    //The curent objects animator component
    public Animator animator;
    //How long the object can invincibilty for after getting hit
    public float waitSeconds;
    //Defines weather the object is the player or an enemy
    public bool enemy;
    //The objects sprite renderer component
    public SpriteRenderer sp;
    //The tag of the object that this object takes damage from
    public string tagName = "Sword";
    //Defines if the curent object can take damage
    public bool hitCoolDown;
    //objects position (To stop moving during Iframes;
    Vector2 position;

    void Start()
    {
        hitCoolDown = true;
        if(dificultyMode == 0)
            dificultyMode = 1;
        if(enemy)
            maxHP = maxHP * dificultyMode;
        hp = maxHP;
    }
    private void Update()
    {
        //locks the objects position durning Iframes
        if (!hitCoolDown)
            transform.position = position;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Calls TakeDamage
        if (other.CompareTag(tagName))
        {
            if (hitCoolDown)
            {
                StartCoroutine(hitCoolDownCo());
                TakeDamage(1);
            }
        }
    }
    public void TakeDamage(int amount)
    {
        //This applys damage to the curent objects health
        if (sp)
            StartCoroutine(changeColor());
        if (!enemy)
            hp -= amount * dificultyMode;
        else
            hp -= amount * playerDamage;
        if (hp <= 0)
            Die();
    }
    IEnumerator changeColor()
    {
        //causes the objects color to change to red for a moment on hit
        if (animator)
            animator.enabled = false;
            Color32 colr = new Color32(255, 255, 255, 255);
            sp.color = new Color32(255, 0, 0, 255);
            yield return new WaitForSeconds(0.07f);
            sp.color = colr;
        if (animator)
            animator.enabled = true;
    }
    IEnumerator hitCoolDownCo()
    {
        //How long till the object can be hit again
        hitCoolDown = false;
        position = new Vector2(transform.position.x, transform.position.y);
        yield return new WaitForSeconds(waitSeconds);
        hitCoolDown = true;
    }
   
    void Die()
    {
        //Sets the object to become inactive after heath drops to zero
        if (enemy)
            gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(false);
            PlayerDie();
        }
    }
    public void PlayerDie()
    {
        //Player Death Code Here
    }
}
