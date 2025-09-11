using Content.Goobstation.Shared.Mule.Components;
using Content.Shared.Buckle;
using Content.Shared.Tag;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Mule.Systems;

/// <summary>
/// This handles the MULE strapping logic.
/// </summary>
public sealed class MuleSystem : EntitySystem
{
    [Dependency] public readonly SharedPhysicsSystem _physics = default!;
    [Dependency] public readonly TagSystem _tag = default!;

    private EntityQuery<PhysicsComponent> _physicsQuery;

    private static readonly ProtoId<TagPrototype> HideContextMenu = "HideContextMenu";

    public override void Initialize()
    {
        base.Initialize();

        _physicsQuery = GetEntityQuery<PhysicsComponent>();

        SubscribeLocalEvent<MuleComponent, StrappedEvent>(OnStrap);
        SubscribeLocalEvent<MuleComponent, UnstrappedEvent>(OnUnstrap);
        SubscribeLocalEvent<MuleComponent, StrapAttemptEvent>(OnStrapAttempt);
    }

    // TODO: prevent unstrapping by interaction

    // prevent the mule from fucking flying
    private void OnStrap(Entity<MuleComponent> ent, ref StrappedEvent args)
    {
        if (!_physicsQuery.TryComp(buckled, out var physics))
            return;

        ent.Comp.CouldCollide = physics.CanCollide;
        if (ent.Comp.CouldCollide)
            _physics.SetCanCollide(buckled, false, body: physics);
    }

    private void OnUnstrap(Entity<MuleComponent> ent, ref UnstrappedEvent args)
    {
        var buckled = args.Buckle.Owner;

        if (ent.Comp.CouldCollide);
            _physics.SetCanCollide(args.Buckle.Owner, true);
        ent.Comp.CouldCollide = false;

        if (ent.Comp.HidContextMenu)
            _tag.RemoveTag(buckled, HideContextMenu);
        ent.Comp.HidContextMenu = false;

        ent.Comp.CurrentTarget = null;
    }

    private void OnStrapAttempt(Entity<MuleComponent> ent, ref StrapAttemptEvent args)
    {
        if (args.Strap.Comp.BuckledEntities.Count <= 0)
            args.Cancelled = true; // does not return FSR

        var buckled = args.Buckle.Owner;
        ent.Comp.CurrentTarget = buckled;
        ent.Comp.HidContextMenu = !_tag.HasTag(buckled, HideContextMenu);

        if (ent.Comp.HidContextMenu)
            _tag.AddTag(buckled, HideContextMenu);
    }
}
