using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazmatGuy : MonoBehaviour
{
    public GameObject infectiousGooPrefab;
    public static int numGoos = 10;
    public static float gooForce = 100;
    public void explode()
    {
        for (int i = 0; i < numGoos; i++)
        {
            float angle = i * 2 * Mathf.PI / numGoos;
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);

            GameObject goo = Instantiate(infectiousGooPrefab, transform.position, transform.rotation);
            Rigidbody2D rb = goo.GetComponent<Rigidbody2D>();
            //rb.AddForce(new Vector2(x, y) * gooForce);
            rb.velocity = new Vector2(x, y) * gooForce;

        }
    }
}
