using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEncounter
{
    public string Name { get; }

    public List<IEncounterRound> Rounds { get; }
}

public interface IEncounterRound
{
    public string Question { get; }

    public IEncounterResponses  Responses{ get; }
}

public interface IEncounterResponses
{
    public List<string> Good { get; }
    public List<string> Med { get; }
    public List<string> Bad { get; }
}


[CreateAssetMenu(fileName = "Data", menuName = "Conversation/Dialog")]
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
public class EncounterData: IEncounter
{
    public string Name => name;

    public List<IEncounterRound> Rounds {
        get
        {
            List<IEncounterRound> res = new();
            res.AddRange(encounterRounds);
            return res;
        }
    }

    public string name;
    public List<EncounterRoundData> encounterRounds;
}

[Serializable]
public class EncounterRoundData: IEncounterRound {
    public string Question => question;

    public IEncounterResponses Responses => responses;

    public string question;
    public EncounterRoundResponses responses;
}

[Serializable]
public class EncounterRoundResponses: IEncounterResponses {
    public List<string> Good => good;
    public List<string> Med => med;
    public List<string> Bad => bad;

    public List<string> good;
    public List<string> med;
    public List<string> bad;
}