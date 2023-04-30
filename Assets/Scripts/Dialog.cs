using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Custom/Dialog")]
public class Dialog : ScriptableObject
{
    public Dictionary<string, List<Encounter>> Data { get; private set; }

    public void SetData(Dictionary<string, List<Encounter>> data)
    {
        Data = data;
    }
}

[System.Serializable]
public class Encounter
{
    public string name;
    public List<EncounterRound> encounterRounds;
}

[System.Serializable]
public class EncounterRound {
    public string question;

    public Dictionary<string, List<string>> responses;
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