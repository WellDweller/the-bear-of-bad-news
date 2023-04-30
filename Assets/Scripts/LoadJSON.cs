using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadJSON : MonoBehaviour
{
    public string filePath = "/Users/kevin/Development/BadNewsBear/tools/question-data.json";
    // public Dictionary<string, List<Encounter>> encounters;
    public Dictionary<string, List<EncounterStage>> encounters;
    // Start is called before the first frame update

    void Start()
    {
        string jsonString = File.ReadAllText(filePath);

        // encounters = JsonUtility.FromJson<Dictionary<string, List<Encounter>>>(jsonString);
        // Debug.Log("Loaded conversation");
        // Debug.Log(string.Join(", ", encounters.Keys));

        EncounterStage encounterStage1 = new EncounterStage();
        encounterStage1.question = "You asked to see me, doctor?";
        encounterStage1.responses = new Dictionary<string, List<string>>();
        encounterStage1.responses.Add("good", new List<string>{"Yes, thank you for coming.", "I am afraid ", "I have some bad news for you"});
        encounterStage1.responses.Add("med", new List<string>{"I did? Ah, yes.", "So, here's the thing,", "things aren't great"});
        encounterStage1.responses.Add("bad", new List<string>{"Sup.", "...", "shit's fucked"});

        Encounter manualEncounter = new Encounter();
        manualEncounter.name = "Dead Husband";
        manualEncounter.encounterStages = new List<EncounterStage>();
        manualEncounter.encounterStages.Add(encounterStage1);

        Debug.Log(manualEncounter.name);
        Debug.Log(string.Join(", ", manualEncounter.encounterStages[0].responses.Keys));

        encounters["Dead Husband"] = new List<EncounterStage>();
        encounters["Dead Husband"].Add(encounterStage1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class Encounter
{
    public string name;
    public List<EncounterStage> encounterStages;
}

[System.Serializable]
public class EncounterStage {
    public string question;
    public Dictionary<string, List<string>> responses;
}