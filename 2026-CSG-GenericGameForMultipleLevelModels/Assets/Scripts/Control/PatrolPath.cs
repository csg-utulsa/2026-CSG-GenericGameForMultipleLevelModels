using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f;
        //sets a constant size of the waypoint gizmo for viewing in the editor
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            //for each child component
            {
                int j = GetNextIndex(i);
                //gets the value of the next index
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                //draw a sphere at the child component
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }

        }

        public int GetNextIndex(int i)
        {
            return (i+1) % transform.childCount;
            //devide i+1 by the number of children, return the remaining. (modulo) giving us the next index, preventing indexOutOfRange error
        }
        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
            //returns the location of the child waypoint at index i
        }
    }
}
