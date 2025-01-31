using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace UnityEngine.Tilemaps
{

    [Serializable]
    public class TileSavon : TileBase
    {
        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            Debug.Log("Refresh !! ");

        }
    }


#if UNITY_EDITOR
    /// <summary>
    /// Custom Editor for a PipelineExampleTile. This is shown in the Inspector window when a PipelineExampleTile asset is selected.
    /// </summary>
    [CustomEditor(typeof(TileSavon))]
    public class PipelineExampleTileEditor : Editor
    {
        private TileSavon tile { get { return (target as TileSavon); } }


        public void OnEnable()
        {
           
        }

      

        /// <summary>
        /// The following is a helper that adds a menu item to create a PipelineExampleTile Asset in the project.
        /// </summary>
        [MenuItem("Assets/Create/TileSavon")]
        public static void CreatePipelineExampleTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Pipeline Example Tile", "New Pipeline Example Tile", "Asset", "Save Pipeline Example Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TileSavon>(), path);
        }
    }
#endif
}

