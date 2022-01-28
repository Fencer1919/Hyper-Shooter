using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Task",menuName="New Task")]
public class Tasks : ScriptableObject
{
    [SerializeField]
    public taskType taskType;

    public int[] possibleTaskNumbers;

    public int[] possibleTaskRewards;
}

public enum taskType {
        BreakMeteorite,
        Hits,
        EarnGold,
    }
