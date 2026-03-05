using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public virtual bool IsActionAvailable()
    {
        return true;
    }

    public virtual void CancelAction()
    {
        Debug.Log($"Action : {this} is cancelled");  
    }
}
