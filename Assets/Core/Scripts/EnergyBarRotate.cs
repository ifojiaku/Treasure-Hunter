using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBarRotate : MonoBehaviour
{
    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
    }
}
