using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Conversation/Encounter")]
public class Encounter : ScriptableObject, IEncounter
{
    public string Name => encounterName;

    public List<IEncounterRound> Rounds
    {
        get
        {
            List<IEncounterRound> res = new();
            res.AddRange(encounterRounds);
            return res;
        }
    }

    [SerializeField] string encounterName;
    [SerializeField] List<EncounterRoundData> encounterRounds;

    public void SetData(EncounterData data)
    {
        encounterName = data.name;
        encounterRounds = data.encounterRounds;
    }
}
