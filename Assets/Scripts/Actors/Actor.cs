using UnityEngine;

public abstract class Actor: MonoBehaviour
{
    public virtual void TurnRight()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }
    public virtual void TurnLeft()
    {
        transform.eulerAngles = new Vector3(180, 0, 180);
    }
    public virtual bool IsTurnToTheLeft()
    {
        return transform.eulerAngles.y == 180;
    }

    public virtual void TurnTo(Vector3 target)
    {
        if (target.x > transform.position.x)
        {
            TurnRight();
        }
        else
        {
            TurnLeft();
        }
    }
}

