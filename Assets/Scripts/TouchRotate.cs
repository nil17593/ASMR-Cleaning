using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    public float rotatespeed = 10f;
    private float _startingPosition;
    public bool CanRotate;

    void Update()
    {

        if (Input.GetKey(KeyCode.D))

            transform.Rotate(Vector3.back, rotatespeed * Time.deltaTime);


        if (Input.GetKey(KeyCode.A))

            transform.Rotate(Vector3.back, -rotatespeed * Time.deltaTime);



        //if (Input.touchCount > 0 && CanRotate)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    switch (touch.phase)
        //    {
        //        case TouchPhase.Began:
        //            _startingPosition = touch.position.x;
        //            break;
        //        case TouchPhase.Moved:
        //            if (_startingPosition > touch.position.x)
        //            {
        //                transform.Rotate(Vector3.up, rotatespeed * Time.deltaTime);
        //            }
        //            else if (_startingPosition < touch.position.x)
        //            {
        //                transform.Rotate(Vector3.up, -rotatespeed * Time.deltaTime);
        //            }
        //            break;
        //        case TouchPhase.Ended:
        //            Debug.Log("Touch Phase Ended.");
        //            break;
        //        case TouchPhase.Stationary:
        //            _startingPosition = touch.position.x;
        //            break;
        //    }
        //}
    }
}