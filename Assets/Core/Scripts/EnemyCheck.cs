using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    public GameObject enemy1, enemy2, enemy3;
    void Start()
    {
        
    }

    void Update()
    {
        //if at least one enemy isn't alive, then it's confirmed all enemies are defeated
        if (enemy1.activeSelf || enemy2.activeSelf || enemy3.activeSelf)
        {

        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
