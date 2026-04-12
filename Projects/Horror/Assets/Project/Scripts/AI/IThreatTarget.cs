using UnityEngine;

public interface IThreatTarget 
{
    Vector3 targetPosition { get;  }
    MovementState currentMovementState { get; }

}
