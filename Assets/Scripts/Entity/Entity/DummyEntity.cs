using Entities.States;
using Unity.VisualScripting;
using UnityEngine;
using World;

namespace Entities.Entity
{
    public class DummyEntity : AbstractEntity
    {
        
        public DummyEntity(AbstractWorld worldIn) : base(worldIn, "Dummy")
        {
            StateMachine.AddState("Idle", new IdleState(this, 5, false));
            StateMachine.SetStartState("Idle");
        }
    }
}