using UnityEngine;
using UnityEngine.UIElements;

public interface ICollidable
{
    bool collidingWith(ICollidable c);
    void resolve(ICollidable c, ref Vector3 newPosition, ref Vector3 newVelocity);
}
