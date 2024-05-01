using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    [SerializeField] private Transform Gun;
    [SerializeField] private Transform AlsoGun;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, out hitInfo))
        {
            if(hitInfo.collider != null)
            {
                Vector3 direction = hitInfo.point - Gun.position;
                Vector3 alsoDirection = hitInfo.point - AlsoGun.position;

                Gun.rotation = Quaternion.LookRotation(direction);
                AlsoGun.rotation = Quaternion.LookRotation(alsoDirection);
                Debug.Log("Yep!");



            }
        }
    }
}
