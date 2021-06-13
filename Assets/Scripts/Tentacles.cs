using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacles : MonoBehaviour
{
    public void makeZombie()
    {
        StartCoroutine(GrowTentacles());
    }

    private IEnumerator GrowTentacles()
    {
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.15f);
            transform.localScale += Vector3.one * 0.01f;
        }
    }
}
