using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public Text text1;
    public Text text2;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void basildiginda() {
        text1.text = text2.text;
    }
}
