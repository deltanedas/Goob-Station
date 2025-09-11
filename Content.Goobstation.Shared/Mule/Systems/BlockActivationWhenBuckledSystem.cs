using Content.Goobstation.Shared.Mule.Components;
using Content.Shared.Interaction;

namespace Content.Goobstation.Shared.Mule.Systems;

/// <summary>
/// Cancels activate attempts when this entity is buckled to another entity.
/// This is so you cant just press E on a crate on a mule to open it like a shitter.
/// </summary>
public sealed class BlockActivateWhenBuckledSystem : EntitySystem
{
    [Dependency] private readonly SharedBuckleSystem _buckle = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BlockActivateWhenBuckledComponent, ActivateAttemptEvent>(OnActivateAttempt);
    }

    private void OnActivateAttempt(Entity<BlockActivateWhenBuckledComponent> ent, ref ActivateAttemptEvent args)
    {
        if (_buckle.IsBuckled(ent.Owner))
            args.Cancel();
    }
}
