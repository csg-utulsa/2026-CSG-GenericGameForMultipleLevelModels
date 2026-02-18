using RPG.Attributes;
using RPG.Control;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;
        //a refrence to what weapon is on the ground
        [SerializeField] float respawnTime = 5f;
        //time the object will be hidden before becomeing active again
        [SerializeField] float healthToRestore = 0;
        //temporary value for health restoration for testing.

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);
                //pick up this object, passing who is picking it up
            }
        }

        private void Pickup(GameObject subject)
        {
            if (weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);
                //get the players passed fighter component and run EquipWeapon with the passed weapon type 
            }
            
            if (healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
                //get the players passed health component and run Heal with the passed healthToRestore
            }
            StartCoroutine(HideForSeconds(respawnTime));
            //starts the respawn timer and functions
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            //disables or enables the collider
            foreach (Transform child in transform)
                //gets all the children
            {
                child.gameObject.SetActive(shouldShow);
                //toggles all the children
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
                //pick up this object, passing who is picking it up
            }
            return true;
            //says that the weapon is being picked up, dont walk or enter combat
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
