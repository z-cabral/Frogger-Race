using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogBehavior : MonoBehaviour
{
    public float _logSpeed = 4f;

    [SerializeField] int boundOffset = 10;
    [SerializeField] bool moveLeft = true;

    void Update()
    {
        if (GameBehavior.Instance.CurrentState == State.Play)
        {
            LogMove();
        }
    }

    void LogMove()
    {
        //If the log is moving left, then move the log left.
        //If the log reaches the left edge of the play boundary,
        //Teleport the log to the other side of the map to create a "looping" effect.
        //If the log isn't moving left, then replace "left" with "right"
        //in the above explaination.
        if (moveLeft == true)
        {
            transform.position += Vector3.left * Time.deltaTime * _logSpeed;

            if (transform.position.x <= -GameBehavior.Instance.xBound)
            {
                transform.position = new Vector3(
                    GameBehavior.Instance.xBound-boundOffset,
                    transform.position.y,
                    transform.position.z);
            }
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * _logSpeed;
            
            if (transform.position.x >= GameBehavior.Instance.xBound)
            {
                transform.position = new Vector3(
                    -GameBehavior.Instance.xBound+boundOffset,
                    transform.position.y,
                    transform.position.z);
            }
        }
    }
}
