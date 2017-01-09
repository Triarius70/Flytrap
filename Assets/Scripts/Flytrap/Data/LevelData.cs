using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum LevelRequirement
{
    All,
    Specific
}

public enum LevelType
{
    Basic,
    Random,
    FreeRandom
}

[Serializable]
public class LevelData : ScriptableObject {
    public LevelRequirement Requirement;
    public LevelType Type;
    [TextArea(3, 10)]
    public string Briefing = "Briefing text";
    public List<Transform> AvailableCreatures;
    public int NumOfRandomCreatures;
    public int NumberOfCreaturesPerType;
    public int LevelNum = 0;

}
