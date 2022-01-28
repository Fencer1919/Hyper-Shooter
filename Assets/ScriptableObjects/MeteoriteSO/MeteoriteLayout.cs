using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Layout",menuName="New Layout")]
public class MeteoriteLayout : ScriptableObject
{
    public float difficulty;
    public float yHeight;
    public List<Vector3> objectPositionList;
    
}
