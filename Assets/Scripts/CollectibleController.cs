using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private void Update()
    {
        RotateCollectibles();
    }

    private void RotateCollectibles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).rotation *= Quaternion.Euler(1, 1, 1);
        }
    }
}
