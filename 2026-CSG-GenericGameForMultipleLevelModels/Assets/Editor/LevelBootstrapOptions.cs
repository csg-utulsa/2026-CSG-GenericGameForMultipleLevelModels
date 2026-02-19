using RPG.Control;
using UnityEditor;
using UnityEngine;

public class LevelBootstrapOptions : EditorWindow
{
    [SerializeField] private GameObject core;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject weaponPickup;
    [SerializeField] private GameObject healthPickup;
    [SerializeField] private GameObject meleeEnemy;
    [SerializeField] private GameObject rangedEnemy;
    [SerializeField] private GameObject patrolPath;

    private GameObject assetSpawnPoint;

    private int numberOfPortalsToSpawn;
    private int numberOfWeaponPickups;
    private int numberOfHealthPickups;
    private int numberOfStaticRangedEnemies;
    private int numberOfStaticMeleeEnemies;
    private int numberOfPatrolRangedEnemies;
    private int numberOfPatrolMeleeEnemies;

    private bool shouldSpawnCore;
    private bool shouldSpawnPortals;
    private bool shouldSpawnPickups;
    private bool shouldSpawnWeaponPickups;
    private bool shouldSpawnHealthPickups;
    private bool shouldSpawnEnemies;
    private bool shouldSpawnStaticEnemies;
    private bool shouldSpawnPatrolEnemies;

    [MenuItem("Window/QuickStart/Level Bootstrap Options")]
    public static void ShowWindow()
    {
        // Open the window, or create a new one if it doesn't exist
        GetWindow<LevelBootstrapOptions>("Level Bootstrapper");
    }

    void OnDestroy()
    {
        DestroySpawnPoint();
    }

    void OnGUI()
    {
        EditorGUIUtility.labelWidth = 200f;

        // 1. Give the user an oportunity to move a spawn point
        DrawSpawnPointUI();

        GUILayout.Space(25);
        DrawCoreUI();

        DrawPortalsUI();

        DrawPickupUI();

        DrawEnemyUI();

        if (GUILayout.Button("Create Selected Choices"))
        {
            CreateSelectedChoices();
        }
    }

    private void CreateSelectedChoices()
    {
        if (!DoesSceneContainSpawnPoint()) { CreateSpawnPoint(); }

        if (shouldSpawnCore)
        {
            Instantiate(core, assetSpawnPoint.transform.position, assetSpawnPoint.transform.rotation);
        }
        
        if (shouldSpawnPortals)
        {
            for(int i = 0; i < numberOfPortalsToSpawn; i++)
            {
                var newPortal = Instantiate(portal, assetSpawnPoint.transform.position, assetSpawnPoint.transform.rotation);
                newPortal.name = $"Portal{i}";
            }
        }

        if(shouldSpawnPickups)
        {
            if(shouldSpawnWeaponPickups)
            {
                for (int i = 0; i < numberOfWeaponPickups; i++)
                {
                    var newWeaponPickup = Instantiate(weaponPickup, assetSpawnPoint.transform.position, assetSpawnPoint.transform.rotation);
                    newWeaponPickup.name = $"WeaponPickup{i}";
                }
            }
            if(shouldSpawnHealthPickups)
            {
                for (int i = 0; i < numberOfHealthPickups; i++)
                {
                    var newHealthPickup = Instantiate(healthPickup, assetSpawnPoint.transform.position, assetSpawnPoint.transform.rotation);
                    newHealthPickup.name = $"HealthPickup{i}";
                }
            }
        }

        if(shouldSpawnEnemies)
        {
            if (shouldSpawnStaticEnemies)
            {
                for (int i = 0; i < numberOfStaticRangedEnemies; i++)
                {
                    var newStaticRangedEnemy = Instantiate(rangedEnemy, assetSpawnPoint.transform.position, assetSpawnPoint.transform.rotation);
                    newStaticRangedEnemy.name = $"StaticRangedEnemy{i}";
                }
                for (int i = 0; i < numberOfStaticMeleeEnemies; i++)
                {
                    var newStaticMeleeEnemy = Instantiate(meleeEnemy, assetSpawnPoint.transform.position, assetSpawnPoint.transform.rotation);
                    newStaticMeleeEnemy.name = $"StaticMeleeEnemy{i}";
                }
            }
            if(shouldSpawnPatrolEnemies)
            {
                for (int i = 0; i < numberOfPatrolRangedEnemies; i++)
                {
                    var newPatrollingRangedEnemy = Instantiate(rangedEnemy, assetSpawnPoint.transform.position, assetSpawnPoint.transform.rotation);
                    newPatrollingRangedEnemy.name = $"PatrollingRangedEnemy{i}";
                    CreatePatrolPathForEnemy(newPatrollingRangedEnemy);

                }
                for (int i = 0; i < numberOfPatrolMeleeEnemies; i++)
                {
                    var newPatrollingMeleeEnemy = Instantiate(meleeEnemy, assetSpawnPoint.transform.position, assetSpawnPoint.transform.rotation);
                    newPatrollingMeleeEnemy.name = $"PatrollingMeleeEnemy{i}";
                    CreatePatrolPathForEnemy(newPatrollingMeleeEnemy);
                }
            }
        }


    }

    private void CreatePatrolPathForEnemy(GameObject newPatrollingEnemy)
    {
        if (newPatrollingEnemy.TryGetComponent(out AIController aiController))
        {
            var newPatrolPath = Instantiate(patrolPath, assetSpawnPoint.transform.position, assetSpawnPoint.transform.rotation);
            if (newPatrolPath.TryGetComponent(out PatrolPath patrolPathComponent))
            {
                aiController.SetPatrolPath(patrolPathComponent);
                newPatrolPath.name = $"{newPatrollingEnemy.name}'s patrolPath";
            }
            else
            {
                DestroyImmediate(newPatrolPath);
            }
        }
    }

    private void DrawEnemyUI()
    {
        shouldSpawnEnemies = EditorGUILayout.Toggle("Spawn Enemies", shouldSpawnEnemies);
        if (shouldSpawnEnemies)
        {
            EditorGUI.indentLevel++;

            shouldSpawnStaticEnemies = EditorGUILayout.Toggle("Spawn Static Enemies", shouldSpawnStaticEnemies);
            if (shouldSpawnStaticEnemies)
            {
                EditorGUI.indentLevel++;
                numberOfStaticRangedEnemies = EditorGUILayout.IntField("Number of Ranged Enemies:", numberOfStaticRangedEnemies);
                numberOfStaticMeleeEnemies = EditorGUILayout.IntField("Number of Melee Enemies:", numberOfStaticMeleeEnemies);
                EditorGUI.indentLevel--;
            }

            shouldSpawnPatrolEnemies = EditorGUILayout.Toggle("Spawn Patrolling Enemies", shouldSpawnPatrolEnemies);
            if (shouldSpawnPatrolEnemies)
            {
                EditorGUI.indentLevel++;
                numberOfPatrolRangedEnemies = EditorGUILayout.IntField("Number of Ranged Enemies:", numberOfPatrolRangedEnemies);
                numberOfPatrolMeleeEnemies = EditorGUILayout.IntField("Number of Melee Enemies:", numberOfPatrolMeleeEnemies);
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }
    }

    private void DrawPickupUI()
    {
        shouldSpawnPickups = EditorGUILayout.Toggle("Spawn Pickups", shouldSpawnPickups);
        if (shouldSpawnPickups)
        {
            EditorGUI.indentLevel++;

            shouldSpawnWeaponPickups = EditorGUILayout.Toggle("Spawn Pickups", shouldSpawnWeaponPickups);
            if (shouldSpawnWeaponPickups)
            {
                EditorGUI.indentLevel++;
                numberOfWeaponPickups = EditorGUILayout.IntField("Number of Weapons:", numberOfWeaponPickups);
                EditorGUI.indentLevel--;
            }

            shouldSpawnHealthPickups = EditorGUILayout.Toggle("Spawn Pickups", shouldSpawnHealthPickups);
            if (shouldSpawnHealthPickups)
            {
                EditorGUI.indentLevel++;
                numberOfHealthPickups = EditorGUILayout.IntField("Number of Health Pickups:", numberOfHealthPickups);
                EditorGUI.indentLevel--;
            }
            
            EditorGUI.indentLevel--;
        }
    }

    private void DrawPortalsUI()
    {
        shouldSpawnPortals = EditorGUILayout.Toggle("Create Level Portals", shouldSpawnPortals);
        if (shouldSpawnPortals)
        {
            EditorGUI.indentLevel++;
            numberOfPortalsToSpawn = EditorGUILayout.IntField("Number of Portals:", numberOfPortalsToSpawn);
            EditorGUI.indentLevel--;

        }
    }

    private void DrawCoreUI()
    {
        shouldSpawnCore = EditorGUILayout.Toggle("Create Core Assets", shouldSpawnCore);
    }

    private void DrawSpawnPointUI()
    {
        GUILayout.Label("Spawnpoint Is where assets will be created", EditorStyles.boldLabel);
        GUILayout.Label("If one doesnt exist, assets will spawn at 0,0,0", EditorStyles.boldLabel);
        if (GUILayout.Button("Create Asset Spawn Point"))
        {
            Instantiate(core, assetSpawnPoint.transform.position, assetSpawnPoint.transform.rotation);
            DestroyImmediate(assetSpawnPoint.gameObject);
        }
    }

    private bool DoesSceneContainSpawnPoint()
    {
        assetSpawnPoint = GameObject.Find("LevelBootstrapAssetSpawnPoint");
        return (assetSpawnPoint != null);
    }

    private void CreateSpawnPoint()
    {
        if (!DoesSceneContainSpawnPoint()) { assetSpawnPoint = new GameObject("LevelBootstrapAssetSpawnPoint"); }
    }

    private void DestroySpawnPoint()
    {
        if (DoesSceneContainSpawnPoint()) { DestroyImmediate(assetSpawnPoint.gameObject); }
        
    }
}
