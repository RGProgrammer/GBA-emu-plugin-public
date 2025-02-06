

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor.AssetImporters;
using UnityEditor;

[ScriptedImporter(1, new[] { "gba", "bin" })]
public class GameBoyRomImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var data  = System.IO.File.ReadAllBytes(ctx.assetPath);
        AssetGameBoy asset = ScriptableObject.CreateInstance<AssetGameBoy>();
        asset.setAssetPathAndData(ctx.assetPath, data); 
        ctx.AddObjectToAsset("GameBoyRomAsset", asset);
        ctx.SetMainObject(asset);
        EditorUtility.SetDirty(asset);
    }
}
#endif