public class BehaviourTask
{
    public ExecutionState executionState;
    public BehaviourSequence sequence;
    public Character character;

    public virtual void Init(Character character)
    {
        this.character = character;
        executionState = ExecutionState.NONE;
    }

    public virtual void StartTask()
    {
        executionState = ExecutionState.ACTIVE;
    }

    public virtual void UpdateTask()
    {
        // Do stuff...
    }

    public virtual void StopTask()
    {
        executionState = ExecutionState.COMPLETED;
    }
}