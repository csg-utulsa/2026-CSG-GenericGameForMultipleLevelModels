using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    //Health script is required for CombatTarget
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
            //sets the cursor to combat
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) { return false; }
            //if the calling controller (probably player) cant attack the gameObject (this object) then return false
            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
                //if the calling controler can attack this game object, return true
            }
            return true;
            //returns true even for hovering over a target
        }
    }
}

