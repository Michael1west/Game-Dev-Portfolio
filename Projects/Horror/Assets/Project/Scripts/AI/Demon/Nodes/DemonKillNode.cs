using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class DemonKillNode : ActionNode
{
    private const float ReloadDelay = 1.5f;

    private NavMeshAgent agent;
    private Transform demon;
    private Transform player;
    private DemonAnimator demonAnimator;
    private DemonBrain brain;

    private bool killing = false;
    private float timer = 0f;

    public DemonKillNode(NavMeshAgent agent, Transform demon, Transform player, DemonAnimator demonAnimator, DemonBrain brain)
    {
        this.agent = agent;
        this.demon = demon;
        this.player = player;
        this.demonAnimator = demonAnimator;
        this.brain = brain;
    }

    public override NodeState Evaluate()
    {
        // Setup on first frame — commit to the kill
        if (!killing)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            Vector3 direction = (player.position - demon.position).normalized;
            demon.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            demonAnimator.TriggerBite();
            brain.SetState(DemonState.Killing);
            killing = true;
            timer = 0f;
        }

        // Wall-clock countdown to reload
        timer += Time.deltaTime;
        if (timer >= ReloadDelay)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        State = NodeState.Running;
        return State;
    }
}