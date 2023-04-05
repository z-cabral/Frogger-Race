using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehavior : MonoBehaviour
{
    [SerializeField] float speed = 4f;
    [SerializeField] int boundOffset = 10;
    [SerializeField] bool moveLeft = true;

    void Update()
    {
        if (GameBehavior.Instance.CurrentState == State.Play)
        {
            CarMove();
        }
    }

    void CarMove()
    {
        if (moveLeft == true)
        {
            transform.position += Vector3.left * Time.deltaTime * speed;
            transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);

            if (transform.position.x <= -GameBehavior.Instance.xBound)
            {
                transform.position = new Vector3(
                    GameBehavior.Instance.xBound+boundOffset,
                    transform.position.y,
                    transform.position.z);
            }
        }
        else
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
            transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);

            if (transform.position.x >= GameBehavior.Instance.xBound)
            {
                transform.position = new Vector3(
                    -GameBehavior.Instance.xBound-boundOffset,
                    transform.position.y,
                    transform.position.z);
            }
        }
    }
}