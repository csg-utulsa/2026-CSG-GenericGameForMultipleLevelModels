using RPG.Attributes;
using RPG.Movement;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
    //Namespaces prevent overlapping of class names, need to add in using statements to refrence namespaced classes
{

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Mover PlayerMover;
        //set the component that we are using as the player navmesh
        [SerializeField] Health health;
        //cache refrence to the health component
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        //max distance the navmesh will project to find the closest point on a mesh if clicked off a nav mesh
        [SerializeField] float raycastRadius = 1f;
        //radius for the raycast on interactable enemy targeting


        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            //enum for the type of cursor
            public Texture2D texture;
            //Texture for the cursor
            public Vector2 hotspot;
            //point on the cursor that interacts with the screen
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        private void Start()
        {
            health = GetComponent<Health>();
        }
        private void Update()
        {
            if (InteractWithUI()) { return; }
            //if interacting with UI, ignore
            if (health.IsDead()) 
            {
                SetCursor(CursorType.None);
                //sets the cursor to none
                return;
                //dont do any other actions
            }
            //if the player is dead, do nothing

            if (InteractWithComponent()) { return; }
            //if it can interact with a component, return.
            if (InteractWithMovement()) { return; }
            //if InteractWithMovement is true, allow movement, if not, it will continue and say there is no action the player can take
            SetCursor(CursorType.None);
            //set the kind of cursor that appears on screen to no action

        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            //everything the ray hits into an sorted array
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                //creates an array based on every raycastable game object
                foreach(IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        //sets the cursor type to the racastable decided cursor type
                        return true;
                    }
                }
            }
            return false;
            //there were no Raycastable objects hit
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            //puts everything the sphere cast hits into an array
            float[] distances = new float[hits.Length];
            //creates an array for the hit item distances thats the same length as the hits array
            for( int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
                //sets the value of distance[i] to the distance between the hit and its distance from the raycast
            }
            Array.Sort(distances, hits);
            //rearranges the second array based on the first array
            return hits;
            
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            //refers to UI Game objects, not world game objects. Returns if over UI or not.
            {
                SetCursor(CursorType.UI);
                //sets the cursor to the UI cursor
                return true;
            }
            return false;
        }


        private bool InteractWithMovement()
        {
            
            Vector3 target;
            //the vector3 of where we clicked
            bool hasHit = RaycastNavmesh(out target);
            //checks if the location is on the navmesh, and returns the target if true

            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;
                //if the player can move to the target

                if (Input.GetMouseButton(0))
                //GetMouseButton is true while the button is held,GetMouseButtonDown is true when pressed, must be pressed again to trigger again
                {
                    PlayerMover.StartMoveAction(target, 1f);
                    //passes the point where the ray hits an object as a vector3, as well as player full speed
                }
                SetCursor(CursorType.Movement);
                //set the kind of cursor that appears on screen to movement
                return true;
                //if the ray finds an object the player can interact with, return true
            }
            return false;
            //if the ray does not find an object the player can interact with, return false
        }

        private bool RaycastNavmesh(out Vector3 target)
        {
            target = new Vector3();
            //default vector3 val for if hasHit is false
            RaycastHit hit;
            //variable for where the Raycast has hit
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            //passing in ray and hit, retrieveing out hit and storing information on where the raycast has hit into the hit var.
            //RaycastHit passes out a bool.

            if(!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            //tells us if we sucessfully found a position closest to our navmesh within our max projection distance
            if(!hasCastToNavMesh) return false;
            //failed to find spot within projection distance on nav mesh
            target = navMeshHit.position;
            //gives position on the nav mesh that we cast to
            return true;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            //gets the current cursor mapping
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
            //sets the cursor to the texture, location, and mode
        }

        private CursorMapping GetCursorMapping(CursorType type) 
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
            //if the mapping does not match they type, return the default mapping
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
            //returns the ray created from the mouse
        }
    }
}
