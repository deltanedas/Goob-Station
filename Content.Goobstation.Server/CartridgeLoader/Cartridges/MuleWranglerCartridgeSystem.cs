using Content.Goobstation.Shared.CartridgeLoader.Cartridges;
using Content.Goobstation.Shared.Mule.Components;
using Content.Server.CartridgeLoader;
using Content.Server.NPC;
using Content.Server.NPC.HTN;
using Content.Server.NPC.Systems;
using Content.Shared.CartridgeLoader;
using Content.Shared.NPC.Systems;

namespace Content.Goobstation.Server.CartridgeLoader.Cartridges;

public sealed class MuleWranglerCartridgeSystem : SharedMuleWranglerCartridgeSystem
{
    [Dependency] private readonly CartridgeLoaderSystem _loader = default!;
    [Dependency] private readonly HTNSystem _htn = default!;
    [Dependency] private readonly MuleSystem _mule = default!;
    [Dependency] private readonly NPCSystem _npc = default!;

    private List<NetEntity> _mules = new();
    private List<NetEntity> _beacons = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MuleWranglerCartridgeComponent, CartridgeUiReadyEvent>(OnUiReady);
        SubscribeLocalEvent<MuleWranglerCartridgeComponent, CartridgeMessageEvent>(OnMessage);
    }

    private void OnMessage(Entity<MuleWranglerCartridgeComponent> ent, ref CartridgeMessageEvent args)
    {
        if (args is MuleWranglerTransportMessage)
        {
            if (GetMule(ent) is not {} mule) return;

            _mule.SetOrder(mule, MuleOrderType.Transport);
            _mule.SetTarget(mule, ent.Comp.SelectedBeacon);
            UpdateMuleBlackboard(mule, MuleOrderType.Transport);
        }
        else if (args is MuleWranglerReturnMessage)
        {
            if (GetMule(ent) is not {} mule) return;

            UpdateMuleBlackboard(mule, MuleOrderType.Return);
        }
        else if (args is MuleWranglerSetMuleMessage msg)
        {
            // TODO: verify that it's a mule on the same map
            ent.Comp.SelectedMule = msg.Mule;
            Dirty(ent);
        }
        else if (args is MuleWranglerSetBeaconMessage msg)
        {
            // TODO: verify that it's a beacon on the same map
            ent.Comp.SelectedBeacon = msg.Beacon;
            Dirty(ent);
        }


        UpdateUiState(ent, GetEntity(args.LoaderUid));
    }

    private void OnUiReady(Entity<MuleWranglerCartridgeComponent> ent, ref CartridgeUiReadyEvent args)
    {
        UpdateUiState(ent, args.Loader);
    }

    private Entity<MuleComponent>? GetMule(Entity<MuleWranglerCartridgeComponent> ent)
    {
        return GetEntity(ent.Comp.SelectedMule) is {} uid && TryComp<MuleComponent>(uid, out var comp))
            ? (uid, comp)
            : null;
    }

    public void UpdateMuleBlackboard(Entity<MuleComponent> ent MuleOrderType orderType)
    {
        _mule.SetOrder(ent, orderType);

        if (!TryComp<HTNComponent>(ent, out var htn))
            return;

        if (htn.Plan != null)
            _htn.ShutdownPlan(htn);

        _npc.SetBlackboard(ent.Owner, NPCBlackboard.CurrentOrders, orderType);
        _htn.Replan(htn);
    }

    private void UpdateUiState(Entity<MuleWranglerCartridgeComponent> ent, EntityUid loaderUid)
    {
        var map = Transform(loaderUid).MapUid;

        _mules.Clear();
        var query = EntityQueryEnumerator<MuleComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out _, out var xform))
        {
            if (xform.MapUid == map)
                _mules.Add(GetNetEntity(uid));
        }

        _beacons.Clear();
        var beaconQuery = EntityQueryEnumerator<MuleDropOffComponent, TransformComponent>();
        while (beaconQuery.MoveNext(out var uid, out _, out var xform))
        {
            if (xform.MapUid == map)
                _beacons.Add(GetNetEntity(uid));
        }

        var state = new MuleWranglerUiState(_mules, _beacons);
        _loader.UpdateCartridgeUiState(loaderUid, state);
    }
}
