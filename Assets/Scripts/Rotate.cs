using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public bool canRotate;
    public float Speed;
    public bool X_Rotate;
    public bool Y_Rotate;
    public bool CanControl;

    

    void Update()
    {
        if(CanControl)
        {

        if(Input.GetMouseButton(0))
        {

        if (canRotate)
        {
            if (X_Rotate)
            {
                transform.Rotate(90 * Time.deltaTime * Speed,0.0f, 0.0f);
            }
            else if(Y_Rotate)
            {
                transform.Rotate(0.0f, -90 * Time.deltaTime * Speed, 0.0f);
            }
            else
            {
                transform.Rotate(0.0f,0.0f, -90 * Time.deltaTime * Speed);
            }
        }


            }
       if(Input.GetMouseButtonUp(0))
            {
                GameManager.Instance.canProgress = false;
            }
        }
        else
        {
            if (canRotate)
            {
                if (X_Rotate)
                {
                    transform.Rotate(90 * Time.deltaTime * Speed, 0.0f, 0.0f);
                }
                else if (Y_Rotate)
                {
                    transform.Rotate(0.0f, -90 * Time.deltaTime * Speed, 0.0f);
                }
                else
                {
                    transform.Rotate(0.0f, 0.0f, -90 * Time.deltaTime * Speed);
                }
            }
        }
    }
}
