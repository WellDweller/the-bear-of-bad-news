using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Custom/Dialog")]
public class Dialog : ScriptableObject
{
    [SerializeField] public List<EncounterData> encounters;

    public void SetData(DialogData data)
    {
        encounters = data.encounters;
    }
}

public class DialogData
{
    public List<EncounterData> encounters;
}

[Serializable]
public class EncounterData
{
    public string name;

    public List<EncounterRoundData> encounterRounds;
}

[Serializable]
public class EncounterRoundData {
    public string question;
    public EncounterRoundResponses responses;
}

[Serializable]
public class EncounterRoundResponses {
    public List<string> good;
    public List<string> med;
    public List<string> bad;
}