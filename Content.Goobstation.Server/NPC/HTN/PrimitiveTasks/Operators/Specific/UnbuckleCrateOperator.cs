using System.Threading;
using System.Threading.Tasks;
using Content.Goobstation.Shared.MULE.Components;
using Content.Server.Buckle.Systems;
using Content.Server.NPC;
using Content.Server.NPC.HTN.PrimitiveTasks;

namespace Content.Goobstation.Server.NPC.HTN.PrimitiveTasks.Operators.Specific;

public sealed partial class UnbuckleCrateOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entMan = default!;
    private BuckleSystem _buckle = default!;

    public override void Initialize(IEntitySystemManager sysMan)
    {
        base.Initialize(sysMan);

        _buckle = sysMan.GetEntitySystem<BuckleSystem>();
    }

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        if (!_entMan.TryGetComponent<MuleComponent>(owner, out var mule))
            return (false, null);

        if (mule.CurrentObject is not {} crate)
            return (false, null);

        if (!_buckle.TryUnbuckle(crate, owner))
            return (false, null);

        return (true, new Dictionary<string, object>() {});
    }
}
