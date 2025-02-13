using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

namespace Entities.States
{
    public class IdleState : StateBase
    {
        private Vector2 _range;
        private List<Vector2Int> _path = new();
        private AbstractEntity _entity;
        
        public IdleState(AbstractEntity entity, Vector2 range, bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
            _range = range;
        }

        public override void OnEnter()
        {
            Debug.Log("Idle");
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if (_path.Count == 0)
            {
                var destination = new Vector2(Random.Range(-_range.x, _range.x), Random.Range(-_range.y, _range.y));
                _path = _entity.PathFinder.FindPath(_entity.Position, destination + _entity.Position);
            }
        }
    }
}