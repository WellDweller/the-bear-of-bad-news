using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Custom/Dialog")]
public class Dialog : ScriptableObject
{
    public List<Encounter> Encounters { get; private set; }

    [SerializeField] DialogData internalData;

    void OnEnable()
    {
        Debug.Log("Is it awakening?");
        GenerateExternalData();
    }

    public void SetData(DialogData data)
    {
        internalData = data;
    }

    void GenerateExternalData()
    {
        if (internalData == null)
            return;

        if (Encounters == null)
            Encounters = new();
        else
            Encounters.Clear();

        foreach (var encounterData in internalData.encounters)
        {
            Encounter encounter = new() { name = encounterData.name, encounterRounds = new() };

            foreach (var roundData in encounterData.encounterRounds)
            {
                ResponseOptions options = new();
                EncounterRound round = new() { question = roundData.question, responses = options };

                // each set of options for good/med/bad in the JSON are in their own objects, but it'll
                // be more convenient to have a single object with all of them, so this consolidates them
                foreach (var responseOption in roundData.responses)
                {
                    if (responseOption.good != null)
                        options.good = responseOption.good;
                    if (responseOption.med != null)
                        options.med = responseOption.med;
                    if (responseOption.bad != null)
                        options.bad = responseOption.bad;
                }

                encounter.encounterRounds.Add(round);
            }

            Encounters.Add(encounter);
        }
    }
}

[Serializable]
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

    public List<ResponseOptions> responses;
}

[Serializable]
public class ResponseOptions
{
    public List<string> good;
    public List<string> med;
    public List<string> bad;
}


public class Encounter
{
    public string name;

    public List<EncounterRound> encounterRounds;
}

public class EncounterRound
{
    public string question;

    public ResponseOptions responses;
}

// Generated stuff
// [System.Serializable]
// public class DialogueOption {
//     public string name;
//     public EncounterRound[] encounterRounds;

//     [System.Serializable]
//     public class EncounterRound {
//         public string question;
//         public Response[] responses;

//         [System.Serializable]
//         public class Response {
//             public string[] good;
//             public string[] med;
//             public string[] bad;
//         }
//     }
// }

// string json = File.ReadAllText("/Users/kevin/Development/BadNewsBear/tools/test.json");
// DialogueOption dialogueOption = JsonUtility.FromJson<DialogueOption>(json);

// Debug.Log(dialogueOption);