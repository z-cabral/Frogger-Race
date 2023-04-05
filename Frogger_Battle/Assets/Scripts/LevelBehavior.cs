using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehavior : MonoBehaviour
{
    //needs to keep track of camPosY
    //when camPosY plus thisPosY  is >= thisPosY + instantiateThreshold,
    //instantiate the level again at thisTransformPos (thisPosY + targetPosY),
    //when camPosY plus thisPosY  is >= thisPosY + destroyThreshold,
    //Destroy this gameObject.

    float _camPosY, _thisPosY;

    [SerializeField] GameObject level;

    [SerializeField] bool isDuplicated=false;

    [SerializeField] int
        instantiateThreshold=10,
        destroyThreshold=30,
        _targetPosY=40,
        _gridOffset; 

    public void Awake()
    {
        _camPosY = Camera.main.gameObject.transform.position.y;
        _thisPosY = this.transform.position.y + _gridOffset;
    }

    public void Update()
    {
        _camPosY = Camera.main.gameObject.transform.position.y;


            if (_camPosY >= _thisPosY + instantiateThreshold)
            {
                if (isDuplicated == false)
                {
                    Instantiate(level,
                        new Vector3(0, _thisPosY + _targetPosY, 0),
                        Quaternion.identity);

                    isDuplicated = true;

                }
            }


        if (_camPosY >= _thisPosY + destroyThreshold)
        {
            Destroy(this.gameObject);
        }
    }
}
