using UnityEngine;
using UnityEngine.AI;

namespace RPG.Helpers
{
    public static class NavMeshExtensions
    {
        public static float GetPathLength(this NavMeshPath path)
        {
            var length = 0f;

            if (path.corners.Length < 2)
            {
                return length;
            }
            
            for (var index = 0; index < path.corners.Length - 1; index++)
            {
                var corner = path.corners[index];
                
                var nextCorner = path.corners[index + 1];

                length += Vector3.Distance(nextCorner, corner);
            }

            return length;
        }
    }
}