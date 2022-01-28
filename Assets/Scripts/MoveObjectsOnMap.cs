using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectsOnMap : MonoBehaviour
{
    bool isGameStarted = false;
    public float speed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startGame(bool par) {
        isGameStarted = par;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameStarted)   transform.Translate(-1*speed*Vector3.up * Time.deltaTime);
            
    }
}
