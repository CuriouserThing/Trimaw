using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Trimaw.Core.SnackSystem;
using Trimaw.Core.Utils;

namespace Trimaw.Core.GodotControls;

public class MorselStockWrapper : NodeWrapper<Control>
{
    // Technical minimum is slots+1; bare minimum is 2*slots
    private const int PoolSize = 3 * SnackConstants.PrepSlots;
    public static readonly SpireField<NCreature, MorselStockWrapper> NCreatureTable = new(() => null);

    private readonly PooledSlot[] _pool = new PooledSlot[PoolSize];
    private readonly PooledSlot?[] _stock = new PooledSlot?[SnackConstants.PrepSlots];
    private readonly Vector2[] _stockSlotPositions = new Vector2[SnackConstants.PrepSlots];

    private readonly Vector2[]
        _stockSlotScales = new Vector2[SnackConstants.PrepSlots]; // probably all the same but meh

    private int _timestamp;

    public MorselStockWrapper(Control node) : base(node)
    {
        node.Position = new Vector2(0, -350);

        // Use included slots as a reference for positioning,
        // then let the wrapper clean them like the new slots we instantiate.
        for (var i = 0; i < SnackConstants.PrepSlots; i += 1)
        {
            var slot = Node.GetNode<Node2D>($"Slot{i}");
            _stockSlotPositions[i] = slot.Position;
            _stockSlotScales[i] = slot.Scale;
            _pool[i] = new PooledSlot(i, new MorselSlotWrapper(slot), 0, null);
        }

        var scene = GD.Load<PackedScene>(Pathfinder.Scene("morsel_slot"));
        for (var i = SnackConstants.PrepSlots; i < PoolSize; i += 1)
        {
            var slot = scene.Instantiate<Node2D>();
            node.AddChild(slot);
            _pool[i] = new PooledSlot(i, new MorselSlotWrapper(slot), 0, null);
        }
    }

    public void AddMorsel(Morsel morsel)
    {
        for (var i = 0; i < SnackConstants.PrepSlots; i += 1)
        {
            if (_stock[i] is not null) continue;
            PullFromPoolToStock(i, morsel);
            return;
        }

        MainFile.Logger.Error($"No open UI slot for the {morsel} morsel just prepped. It won't appear visually.");
    }

    /// <summary>
    ///     Reclaim the oldest slot in the pool that isn't an active morsel in the stock.
    ///     Ideally, the pool is large enough that this slot has already disappeared as a result of its animation,
    ///     or at least is obscured by other slots.
    /// </summary>
    private void PullFromPoolToStock(int stockIdx, Morsel morsel)
    {
        var oldestValue = int.MaxValue;
        var oldestIdx = 0;
        for (var i = 0; i < PoolSize; i += 1)
        {
            var timestamp = _pool[i].Timestamp;
            if (timestamp < oldestValue && _pool[i].StockIdx is null)
            {
                oldestValue = timestamp;
                oldestIdx = i;
            }
        }

        var slot = RecycleSlot(oldestIdx, stockIdx);
        slot.IsActiveWithinStock = true;
        slot.SetMorsel(morsel, _stockSlotPositions[stockIdx], _stockSlotScales[stockIdx]);
    }

    public void PopSnack(SnackResult snackResult)
    {
        var tail = 0;
        for (var i = 0; i < _stock.Length; i += 1)
        {
            if (_stock[i] is not { } stockSlot) continue;
            var poolIdx = stockSlot.PoolIdx;
            if (snackResult.MorselIsUsedInSnack(i))
            {
                // Remove slot from the stock, visually popping it up 
                var slot = RecycleSlot(poolIdx, null);
                slot.IsActiveWithinStock = false;
                var node = slot.Node;

                var tween = node.CreateTween();
                tween
                    .TweenProperty(node, "position", new Vector2(0, -70), 1.0)
                    .AsRelative()
                    .SetEase(Tween.EaseType.Out)
                    .SetTrans(Tween.TransitionType.Back);
                tween
                    .TweenProperty(node, "modulate", Colors.Transparent, 0.5)
                    .SetDelay(0.5)
                    .SetEase(Tween.EaseType.Out)
                    .SetTrans(Tween.TransitionType.Cubic);
            }
            else
            {
                // Shift the slot as far left in the stock as it can now go
                var slot = RecycleSlot(poolIdx, tail);
                var node = slot.Node;

                var tween = node.CreateTween();
                tween
                    .TweenProperty(node, "position", _stockSlotPositions[tail], 0.75)
                    .SetEase(Tween.EaseType.InOut)
                    .SetTrans(Tween.TransitionType.Cubic);

                tail += 1;
            }
        }
    }

    private MorselSlotWrapper RecycleSlot(int poolIdx, int? stockIdx)
    {
        var slot = _pool[poolIdx];
        if (slot.StockIdx is { } oldStockIdx) _stock[oldStockIdx] = null;

        _timestamp += 1;
        slot.Timestamp = _timestamp;
        slot.StockIdx = stockIdx;

        if (stockIdx is { } newStockIdx) _stock[newStockIdx] = slot;
        return slot.Slot;
    }

    private class PooledSlot(int poolIdx, MorselSlotWrapper slot, int timestamp, int? stockIdx)
    {
        public int PoolIdx { get; } = poolIdx;
        public MorselSlotWrapper Slot { get; } = slot;
        public int Timestamp { get; set; } = timestamp;
        public int? StockIdx { get; set; } = stockIdx;
    }
}