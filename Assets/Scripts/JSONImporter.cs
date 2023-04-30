using System.Collections.Generic;
using UnityEngine;
using System.IO;


[UnityEditor.AssetImporters.ScriptedImporter(1, "dialog")]
public class JSONImporter : UnityEditor.AssetImporters.ScriptedImporter
{
    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
    {
        Debug.Log(ctx);

        var data = JsonUtility.FromJson<Dictionary<string, List<Encounter>>>(File.ReadAllText(ctx.assetPath));

        var dialogAsset = ScriptableObject.CreateInstance<Dialog>();
        dialogAsset.SetData(data);

        ctx.AddObjectToAsset("new dialog", dialogAsset);
        ctx.SetMainObject(dialogAsset);
    }
}
