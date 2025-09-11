using System.Threading;
using System.Threading.Tasks;
using Content.Goobstation.Shared.Mule.Components;
using Content.Server.NPC;
using Content.Server.NPC.HTN.PrimitiveTasks;
using Content.Server.NPC.Pathfinding;

namespace Content.Goobstation.Server.NPC.HTN.PrimitiveTasks.Operators.Specific;

public sealed partial class SetDropoffAsOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entMan = default!;
    private EntityLookupSystem _lookup = default!;
    private PathfindingSystem _pathfinding = default!;

    /// <summary>
    /// Target entity to travel too in the blackboard
    /// </summary>
    [DataField(required: true)]
    public string TargetKey = string.Empty;

    /// <summary>
    /// Target entitycoordinates to move to in the blackboard
    /// </summary>
    [DataField(required: true)]
    public string TargetMoveKey = string.Empty;

    [DataField]
    public string RangeKey = String.Empty;

    public override void Initialize(IEntitySystemManager sysMan)
    {
        base.Initialize(sysManager);

        _lookup = sysMan.GetEntitySystem<EntityLookupSystem>();
        _pathfinding = sysMan.GetEntitySystem<PathfindingSystem>();
    }

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        if (!_entMan.TryGetComponent<MuleComponent>(owner, out var mule))
            return (false, null);

        if (mule.CurrentTarget is not {} target)
            return (false, null);

        return (true, new Dictionary<string, object>()
        {
            {TargetKey, target},
            {TargetMoveKey, _entMan.GetComponent<TransformComponent>(target).Coordinates},
        });
    }
}
