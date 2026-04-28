using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class DemonKillNode : ActionNode
{
    private NavMeshAgent agent;
    private Transform demon;
    private Transform player;
    private DemonAnimator demonAnimator;
    private const float ReloadDelay = 1.5f;

    private bool killing = false;
    private float timer = 0f;

    public DemonKillNode(NavMeshAgent agent, Transform demon, Transform player, DemonAnimator demonAnimator)
    {
        this.agent = agent;
        this.demon = demon;
        this.player = player;
        this.demonAnimator = demonAnimator;
    }

    public override NodeState Evaluate()
    {
        if (!killing)
        {
            Debug.Log("Kill triggered");
            agent.ResetPath();
            agent.velocity = Vector3.zero;

            Vector3 direction = (player.position - demon.position).normalized;
            demon.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

            demonAnimator.TriggerBite();
            killing = true;
            timer = 0f;
        }

        timer += Time.deltaTime;

        if (timer >= ReloadDelay)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        State = NodeState.Running;
        return State;
    }
}
