using UnityEngine;

[CreateAssetMenu(fileName = "testanimationslime", menuName = "Scriptable Objects/testanimationslime")]
public class testanimationslime : ScriptableObject
{
    [field: SerializeField] public AnimationClip Left{ get; private set; } 
    
    [field: SerializeField] public AnimationClip Right{ get; private set; } 
    
    [field: SerializeField] public AnimationClip Jump{ get; private set; } 
    
    [field: SerializeField] public AnimationClip Hurt{ get; private set; } 
    
    [field: SerializeField] public AnimationClip Death{ get; private set; } 
    
    [field: SerializeField] public AnimationClip Attacks{ get; private set; } 
}
