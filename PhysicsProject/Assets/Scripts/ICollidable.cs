using UnityEngine;

public interface ICollidable
{
    
    bool CollidingWith(ICollidable c)
    {
        return false;
        // Write it to actually work for assignment
    }
}
