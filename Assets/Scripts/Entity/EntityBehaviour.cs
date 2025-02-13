using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class EntityBehaviour : MonoBehaviour
    {
        public AbstractEntity Entity;

        public void Init(AbstractEntity entity)
        {
            Entity = entity;            
        }

        public void FixedUpdate()
        {
            
        }

        [Header("Movement Settings")]
        public float moveSpeed = 3f;      // Speed for normal moves
        public float jumpDuration = 0.5f; // Duration of jump animation

        [Header("Jump Settings")]
        // Animation curve for jump height (e.g., peaks at 0.5 with value 1 for a nice parabolic arc)
        public AnimationCurve jumpCurve = AnimationCurve.EaseInOut(0, 0, 0.5f, 1);

        // Queue to store waypoints (in world space) computed by your PathFinder
        private Queue<Vector2> waypoints = new Queue<Vector2>();
        private bool isMoving = false;

        /// <summary>
        /// Set the path for this entity. Converts grid positions (Vector2Int) to world coordinates.
        /// Adjust conversion if your grid-to-world mapping is different.
        /// </summary>
        public void SetPath(List<Vector2Int> path)
        {
            waypoints.Clear();

            foreach (var gridPoint in path)
            {
                Vector2 worldPoint = new Vector2(gridPoint.x, gridPoint.y);
                waypoints.Enqueue(worldPoint);
            }

            if (!isMoving)
                StartCoroutine(MoveAlongPath());
        }


        private IEnumerator MoveAlongPath()
        {
            isMoving = true;
            // Continue until there are no more waypoints
            while (waypoints.Count > 0)
            {
                Vector2 target = waypoints.Dequeue();
                Vector2 startPos = transform.position;
                Vector2 direction = target - startPos;

                bool isJump = (direction.y > 0);

                if (isJump)
                    yield return StartCoroutine(JumpToTarget(startPos, target));
                else
                {
                    while (Vector2.Distance(transform.position, target) > 0.1f)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                        yield return null;
                    }
                    transform.position = target;
                }
            }

            isMoving = false;
            yield return null;
        }

        private IEnumerator JumpToTarget(Vector2 start, Vector2 target)
        {
            float elapsed = 0f;
            while (elapsed < jumpDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / jumpDuration);

                // Lerp position horizontally
                Vector2 horizontalPos = Vector2.Lerp(start, target, t);
                // Determine jump offset from jumpCurve. Adjust multiplier if higher jumps are needed.
                float jumpOffset = jumpCurve.Evaluate(t);
                // Apply jump offset to the vertical position.
                Vector2 newPos = new Vector2(horizontalPos.x, horizontalPos.y + jumpOffset);
                transform.position = newPos;
                yield return null;
            }
            transform.position = target;
        }
    }
}