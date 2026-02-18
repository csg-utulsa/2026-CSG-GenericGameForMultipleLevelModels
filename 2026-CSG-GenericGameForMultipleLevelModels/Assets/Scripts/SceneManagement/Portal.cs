using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,B,C,D,E,F,G,H
        }

        [SerializeField] int sceneToLoad;
        //set the int for the scene to load from the build index
        [SerializeField] Transform spawnPoint;
        //set the transform component of the spawn point gameobject
        [SerializeField] DestinationIdentifier destination;
        //set the destination target
        [SerializeField] float fadeOutTime = 2f;
        //set the time for the screen fade out
        [SerializeField] float fadeWaitTime = 0.5f;
        //set the time to wait before fading the screen back in
        [SerializeField] float fadeInTime = 1f;
        //set the time for the screen fade in


        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
                //if the player collides with the trigger
            {
                StartCoroutine(Transition());
                //start the Transition() coroutine
            }
        }
        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            //dont destroy the portal game object on load

            Fader fader = GameObject.FindObjectOfType<Fader>();
            //finds the fader GameObject
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            //gets a refrence to the players player controller
            playerController.enabled = false;
            //disables player control

            yield return fader.FadeOut(fadeOutTime);
            //fade the screen out

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            //loads next scene based off of load index value inputted, returns async operation when the scene has finished loading,
            //calling the coroutine again.
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            //gets a refrence to the new scene's player, player controller
            newPlayerController.enabled = false;
            //disables player control on the new scene's player

            Portal otherPortal = GetOtherPortal();
            //get a local refrence to the destination portal
            UpdatePlayer(otherPortal);
            //set the player location and rotation to that of the portals spawn point

            yield return new WaitForSeconds(fadeWaitTime);
            //pause to let all the cameras and player objects find their correct posititon
            fader.FadeIn(fadeInTime);
            //fade the screen back in, immedeately continue on, dont wait for fader to fade all the way in

            newPlayerController.enabled = true;
            //returns control to the player once all transitions are done
            Destroy(gameObject);
            //destroys portal when its no longer needed, preventing unwanted overlap
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            //create a refrence to the player game object
            player.GetComponent<NavMeshAgent>().enabled = false;
            //reference to the player nav mesh agent
            player.transform.position = otherPortal.spawnPoint.position;
            //set the player transform to the transform of the spawnpoint
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            //set the player rotation to the rotation of the spawnpoint
            player.GetComponent<NavMeshAgent>().enabled = true;
            //reenable the player nav mesh agent
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
                //Finds all the objects of type portal and turns them into a list.
            {
                if (portal == this) { continue; }
                //if the portal is the current portal, continue to the next portal in the list
                if (portal.destination != destination) { continue; }
                //if the destination portal is the current portal, continue to the next portal in the list
                return portal;
                //return the portal
            }
            return null;
            //if no portals, retun null
        }
    }
}
