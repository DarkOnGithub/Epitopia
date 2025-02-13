using UnityEngine;
using UnityHFSM;
using World;

namespace Entities
{
    public class BaseEntity : AbstractEntity
    {
        public BaseEntity(AbstractWorld worldIn) : base(worldIn, "Slime")
        {
            StateMachine.AddState("Idle", new State(
                onEnter: state =>
                {
                    Debug.Log("Idling");
                },
                onLogic: state =>
                {
                    
                }
                ));
            StateMachine.SetStartState("Idle");
        }
    }
}