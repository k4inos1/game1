using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker
{
    [MenuItem("Tools/Bake NavMesh (Auto)")]
    private static void BakeNavMesh()
    {
        // Try to call NavMeshSurface.BuildNavMesh via reflection if the NavMeshComponents package is present
        var surfaceType = System.Type.GetType("UnityEngine.AI.NavMeshSurface, Unity.AI.Navigation" )
                          ?? System.Type.GetType("NavMeshSurface, Assembly-CSharp");

        if (surfaceType != null)
        {
            var surfaces = Object.FindObjectsOfType(surfaceType);
            if (surfaces != null && surfaces.Length > 0)
            {
                foreach (var s in surfaces)
                {
                    var mi = surfaceType.GetMethod("BuildNavMesh");
                    if (mi != null) mi.Invoke(s, null);
                }
                Debug.Log($"Built {surfaces.Length} NavMeshSurface(s) via reflection");
                return;
            }
        }

        // fallback: use built-in NavMeshBuilder to build for all active objects in the scene
        var settings = NavMesh.GetSettingsByIndex(0);
        var sources = new System.Collections.Generic.List<NavMeshBuildSource>();

        // collect sources from active renderers/terrains
        var go = GameObject.FindObjectsOfType<GameObject>();
        foreach (var g in go)
        {
            if (g.activeInHierarchy)
            {
                var mr = g.GetComponent<MeshRenderer>();
                var mf = g.GetComponent<MeshFilter>();
                var tr = g.GetComponent<Terrain>();
                if (mr != null && mf != null && mf.sharedMesh != null)
                {
                    var src = new NavMeshBuildSource
                    {
                        shape = NavMeshBuildSourceShape.Mesh,
                        sourceObject = mf.sharedMesh,
                        transform = g.transform.localToWorldMatrix,
                        area = 0
                    };
                    sources.Add(src);
                }
                else if (tr != null && tr.terrainData != null)
                {
                    var src = new NavMeshBuildSource
                    {
                        shape = NavMeshBuildSourceShape.Terrain,
                        sourceObject = tr.terrainData,
                        transform = g.transform.localToWorldMatrix,
                        area = 0
                    };
                    sources.Add(src);
                }
            }
        }

        if (sources.Count == 0)
        {
            Debug.LogWarning("No NavMesh sources found in the scene. Make sure your walkable geometry has MeshRenderer/MeshFilter or Terrain components.");
            return;
        }

        var bounds = new Bounds(Vector3.zero, Vector3.one * 500f);
        var data = NavMeshBuilder.BuildNavMeshData(NavMesh.GetSettingsByIndex(0), sources, bounds, Vector3.zero, Quaternion.identity);
        if (data != null)
        {
            NavMesh.AddNavMeshData(data);
            Debug.Log($"Built NavMesh data with {sources.Count} source(s)");
        }
        else
        {
            Debug.LogWarning("NavMeshBuilder returned no data");
        }
    }
}
