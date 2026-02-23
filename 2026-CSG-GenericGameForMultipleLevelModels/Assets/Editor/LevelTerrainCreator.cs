using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;

public class LevelTerrainCreator : EditorWindow
{
    private Object selectedGameObjectAsset;
    private Object selectedTerrainAsset;
    private string assetPath;
    private bool shouldCreateBootstrapSpawnpoint;

    // Add a menu item to the Window menu to open the window
    [MenuItem("Window/QuickStart/Level Terrain Creator")]
    public static void ShowWindow()
    {
        // Open the window, or create a new one if it doesn't exist
        GetWindow<LevelTerrainCreator>("Level Terrain Creator");
    }

    void OnGUI()
    {
        EditorGUIUtility.labelWidth = 200f;
        GUILayout.Label("Select an asset from the Project window:", EditorStyles.boldLabel);

        // 1. Use an ObjectField to drag and drop assets/select assets
        selectedGameObjectAsset = EditorGUILayout.ObjectField("Level Object", selectedGameObjectAsset, typeof(GameObject), false);

        GUILayout.Label("OR", EditorStyles.boldLabel);

        selectedTerrainAsset = EditorGUILayout.ObjectField("Level TerrainData", selectedTerrainAsset, typeof(TerrainData), false);

        if (selectedGameObjectAsset != null && selectedTerrainAsset == null)
        {
            assetPath = AssetDatabase.GetAssetPath(selectedGameObjectAsset);
            EditorGUILayout.TextField("Asset Path", assetPath);
            GUILayout.Space(25);
        }
        else if (selectedGameObjectAsset == null && selectedTerrainAsset != null)
        {
            assetPath = AssetDatabase.GetAssetPath(selectedTerrainAsset);
            EditorGUILayout.TextField("Asset Path", assetPath);
            GUILayout.Space(5);
        }
        else if (selectedGameObjectAsset != null && selectedTerrainAsset != null)
        {
            EditorGUILayout.TextField("Cannot Load Both Object and TerrainData! Pick one.");
            return;
        }
        else
        {
            EditorGUILayout.LabelField("Asset Path", "None selected");
            return;
        }

        //Ping the Asset Button
        if (GUILayout.Button("Ping Object in Project"))
        {
            if (selectedGameObjectAsset != null)
            {
                EditorGUIUtility.PingObject(selectedGameObjectAsset);
            }
            else if (selectedTerrainAsset != null)
            {
                EditorGUIUtility.PingObject(selectedTerrainAsset);
            }           
        }

        //Create Toggle on if the spawnpoint is created or not to provide easier bootstrapping
        shouldCreateBootstrapSpawnpoint = EditorGUILayout.Toggle("Create Bootsrap Asset Spawnpoint", shouldCreateBootstrapSpawnpoint);
        GUILayout.Space(25);

        // 2. Button to get the currently selected asset in the Project Browser
        if (GUILayout.Button("Import Into Scene"))
        {

            if (selectedGameObjectAsset != null)
            {
                var assetToLoad = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (assetToLoad != null)
                {
                    // Instantiate the loaded GameObject
                    GameObject newGameObject = Instantiate(assetToLoad, Vector3.zero, Quaternion.identity);
                    CreateNavmesh(newGameObject);
                }
                else
                {
                    Debug.LogError("Object asset not found at path: " + assetToLoad);
                }
            }
            else if (selectedTerrainAsset != null)
            {
                var assetToLoad = AssetDatabase.LoadAssetAtPath<TerrainData>(assetPath);
                if (assetToLoad != null)
                {
                    //Create a terrain object
                    GameObject newTerrainGameObject = Terrain.CreateTerrainGameObject(assetToLoad);
                    newTerrainGameObject.transform.position = new Vector3(0, 0, 0);
                    newTerrainGameObject.transform.rotation = new Quaternion();
                    CreateNavmesh(newTerrainGameObject);
                }
                else
                {
                    Debug.LogError("TerrainData asset not found at path: " + assetToLoad);
                }
            }
        }
    }

    private void CreateNavmesh(GameObject _createdLevelObject)
    {
        if(_createdLevelObject.TryGetComponent(out NavMeshSurface navMesh))
        {
            navMesh.BuildNavMesh();
            Debug.Log("Navmesh Updated on terrain");
        }
        else
        {
            var newNavMesh = _createdLevelObject.AddComponent<NavMeshSurface>();
            newNavMesh.BuildNavMesh();
            Debug.Log("Navmesh created on terrain");
        }
        if (shouldCreateBootstrapSpawnpoint)
        {
            new GameObject("LevelBootstrapAssetSpawnPoint");
        }       
        this.Close();
    }
}

