using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    LevelManager lManager;
    Button btn;

    void Start()
    {
        lManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => { lManager.OpenLevelMenu(int.Parse(btn.transform.GetChild(0).GetComponent<Text>().text)); });
    }

    void Update()
    {
        
    }
}
