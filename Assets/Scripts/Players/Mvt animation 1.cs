using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Mvtanimation1", menuName = "Scriptable Objects/Mvtanimation1")]
public class Mvtanimation1 : ScriptableObject
{
    [field: SerializeField] public AnimationClip Left{ get; private set; } 
    
    [field: SerializeField] public AnimationClip Right{ get; private set; } 
    
    [field: SerializeField] public AnimationClip Jump{ get; private set; }

    public AnimationClip GetFacingClip(Vector2 FacinDirection)
    {
        Vector2 closestDirection = GetFacingDirection(FacinDirection);

        if (closestDirection == Vector2.left)
        {
            return Left;
        }
        else if (closestDirection == Vector2.right)
        {
            return Right;
        }
        else if (closestDirection == Vector2.up)
        {
            return Jump;
        }
        else
        {
            throw new ArgumentException($"Direction {closestDirection} is not valid");
        }
    }

    public Vector2 GetFacingDirection(Vector2 inputDirection)
    {
        Vector2 normalizedDirection = inputDirection.normalized;
        Vector2 closestDirection = Vector2.zero;
        float closestDistance = 0f;
        bool firstSet = false;
        Vector2[] directionToCheck = new Vector2[4] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };


        for (int i = 0; i < 4; i++)
        {
            if (!firstSet)
            {
                closestDirection = directionToCheck[i];
                closestDistance = Vector2.Distance(inputDirection, directionToCheck[i] );
                firstSet = true;
            }
            else
            {
                float nextDistance = Vector2.Distance(inputDirection, directionToCheck[i]);
                if (nextDistance < closestDistance)
                {
                    closestDistance = nextDistance;
                    closestDirection = directionToCheck[i];
                }
            }
        }
        
        return closestDirection;
    }
}
