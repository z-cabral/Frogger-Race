using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    //There should be the ability to have
    //multiple camera objects in the scene.

    //Each camera should reference a frog player object.
    [SerializeField] Frog frog1,frog2;
    [SerializeField] int _camOffset = 3;

    Transform
        _frog1Pos,
        _frog2Pos,
        _camStartTransform,
        _camPos;

    int camImpulse=1;

    private void Awake()
    {
        _frog1Pos = frog1.GetComponent<Transform>();
        _frog2Pos = frog2.GetComponent<Transform>();
        _camStartTransform = this.GetComponent<Transform>();
        _camPos = this.GetComponent<Transform>();
    }

    //The frog should be able to move around a 3x3 square within the camera
    //before the camera updates it's position to follow the player.
    private void LateUpdate()
    {
        _frog1Pos = frog1.GetComponent<Transform>();
        _frog2Pos = frog2.GetComponent<Transform>();

        if (GameBehavior.Instance.CurrentState == State.Title)
        {
            this.transform.position = _camStartTransform.position;
        }

        if (_frog1Pos.position.y > _camPos.position.y + _camOffset || _frog2Pos.position.y > _camPos.position.y + _camOffset)
        {
            _camPos.position = new Vector3(
                _camPos.position.x,
                _camPos.position.y + camImpulse,
                _camPos.position.z);
        }
    }
}
