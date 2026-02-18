using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistantObjectPrefab;
        //A refrence to the persistant object prefab in the editor

        static bool hasSpawned = false;
        //static variables live and die with the application, not with the class (global variable)

        private void Awake () 
        {
            if(hasSpawned) {return;}
            //if the object has already spawned, do nothing
            SpawnPersistantObjects();
            //Spawn in the persistant objects
            hasSpawned = true;
            //set that they have already spawned
        }

        private void SpawnPersistantObjects()
        {
            GameObject persistantObject = Instantiate(persistantObjectPrefab);
            //creates the persistant objects prefab
            DontDestroyOnLoad(persistantObject);
            //dont destroy the persistant objects on load
        }
    }
}
