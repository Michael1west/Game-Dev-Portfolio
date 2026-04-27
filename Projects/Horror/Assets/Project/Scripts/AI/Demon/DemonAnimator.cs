using UnityEngine;
using UnityEngine.AI;

public class DemonAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float maxSpeed = 5f;

    private void Update()
    {
        float targetSpeed = agent.velocity.magnitude / maxSpeed;
        float currentSpeed = animator.GetFloat("Speed");
        float smoothedSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 10f);
        animator.SetFloat("Speed", smoothedSpeed);
    }
}