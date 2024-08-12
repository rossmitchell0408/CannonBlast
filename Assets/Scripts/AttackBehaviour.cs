using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField]
    Sprite sprite;
    [SerializeField]
    float damage;
    [SerializeField]
    float knockback;
    PlayerController player;
    [SerializeField]
    float attackTimer = 0.5f;
    [SerializeField]
    AnimationClip attackAnimation;

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
    }

    public float GetAttackTime()
    {
        return attackAnimation.length/*attackTimer*/;
    }    

    public void Attack()
    {
        gameObject.SetActive(true);
        StartCoroutine(AttackTime());
    }

    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(attackAnimation.length/*attackTimer*/);
        gameObject.SetActive(false);
    }

    void HitEnemy(PlayerController target)
    {
        target.GetHit(player.transform.position, knockback, damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject != player.gameObject)
        {
            HitEnemy(collision.GetComponent<PlayerController>());
        }
    }
}
