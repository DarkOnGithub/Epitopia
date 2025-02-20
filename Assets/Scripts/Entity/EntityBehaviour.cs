using System;
using System.Collections.Generic;
using Entities.Entity;
using Unity.Netcode;
using UnityEngine;

namespace Entities
{
    public class EntityBehaviour : NetworkBehaviour
    {
        
        [SerializeField] public Vector2Int targetPosition;
        private Queue<Vector2Int> _path;
        private AbstractEntity _entity;
        private PathFinder _pathFinder;
        private int _tickCounter = 0;
        
        private void Awake()
        {
            
        }

        public void Init(AbstractEntity entity)
        {
            Debug.Log("zaeeazeazeza");
            _entity = entity;
            _pathFinder = new PathFinder(entity.WorldIn);
        }
        
        public void SetTarget(Vector2Int target)
        {
            targetPosition = target;
        }
        
        
        public void Update()
        {
            if (_path == null || _path.Count == 0)
            {
                if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
                {
                    _path = new Queue<Vector2Int>(_pathFinder.FindPath(transform.position, targetPosition));
                }
                else
                    return;
            }

            _tickCounter++;
            if(_tickCounter % 100 == 0)
            { 
                _path = new Queue<Vector2Int>(_pathFinder.FindPath(transform.position, targetPosition));
                Debug.Log(_path.Count);
            }
            var target = _path.Peek();
            if(Vector2.Distance(transform.position, target) < 0.1f)
                _path.Dequeue();
        }


        private void OnDrawGizmos()
        {
            if (_path != null && _path.Count > 0)
            {
                                
                Vector3 previousPoint = transform.position;
                Debug.Log(previousPoint);
                foreach (Vector2Int point in _path.ToArray())
                {
                    Vector3 nextPoint = new Vector3(point.x, point.y, transform.position.z);
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(previousPoint, nextPoint);
                    Debug.Log("a");
                    Gizmos.DrawSphere(nextPoint, 0.1f);
                    previousPoint = nextPoint;
                }
            }
        }
    }
}