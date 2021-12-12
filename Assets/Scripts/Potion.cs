using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().AddItem();
            Destroy(gameObject);
        }
    }
}
