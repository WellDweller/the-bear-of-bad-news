using UnityEngine;
using UnityEditor;

[UnityEditor.AssetImporters.ScriptedImporter(1, "dialog")]
public class JSONImporter : UnityEditor.AssetImporters.ScriptedImporter
{
    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
    {
        Debug.Log(ctx);
    }
}