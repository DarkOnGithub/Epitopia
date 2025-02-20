using System.Collections.Generic;
using Entities.Entity;
using UnityEngine;
using UnityHFSM;

namespace Entities.States
{
    public class IdleState : StateBase
    {
        private int _range;
        private readonly AbstractEntity _entity;
        private bool t = false;
        public IdleState(AbstractEntity entity, int xRange, bool needsExitTime, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
            _range = xRange;
            _entity = entity;
        }

        public override void OnEnter()
        {
            Debug.Log($"{_entity.Name} is now idling");
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if (t) return;
            t = true;
            var distanceFactor = Random.Range(-_range, _range);
            var target = _entity.Position + new Vector2(distanceFactor, 0);
            _entity.MoveTo(Vector2Int.RoundToInt(target));
                
        }
    }
}