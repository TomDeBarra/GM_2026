using UnityEngine;
using UnityEngine.UIElements;

public interface ICollidable
{
    bool collidingWith(ICollidable c);
    void resolve(ICollidable c);
}
