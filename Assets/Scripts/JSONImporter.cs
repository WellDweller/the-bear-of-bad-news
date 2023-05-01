using System.Collections.Generic;
using UnityEngine;
using System.IO;


[UnityEditor.AssetImporters.ScriptedImporter(1, "dialog")]
public class JSONImporter : UnityEditor.AssetImporters.ScriptedImporter
{
    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
    {
        Debug.Log(ctx);

        var data = JsonUtility.FromJson<DialogData>(File.ReadAllText(ctx.assetPath));
        
        List<Encounter> encounters = new();

        foreach (var encounterData in data.encounters)
        {
            var encounterAsset = ScriptableObject.CreateInstance<Encounter>();
            encounterAsset.SetData(encounterData);
            encounterAsset.name = encounterAsset.Name;
            ctx.AddObjectToAsset(encounterAsset.Name, encounterAsset);
        }

        var dialogAsset = ScriptableObject.CreateInstance<Dialog>();
        dialogAsset.SetData(data);
        ctx.AddObjectToAsset("dialog", dialogAsset);
        ctx.SetMainObject(dialogAsset);
    }
}
