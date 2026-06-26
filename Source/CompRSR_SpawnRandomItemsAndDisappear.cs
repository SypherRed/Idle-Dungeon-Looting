using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld;

public class CompRSR_SpawnRandomItemsAndDisappear : ThingComp
{
    private bool spawnQueued;

    private CompProperties_RSR_SpawnRandomItemsAndDisappear Props =>
        (CompProperties_RSR_SpawnRandomItemsAndDisappear)props;

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        base.PostSpawnSetup(respawningAfterLoad);

        if (!respawningAfterLoad)
        {
            spawnQueued = true;
        }
    }

    public override void CompTick()
    {
        base.CompTick();

        if (!spawnQueued)
        {
            return;
        }

        spawnQueued = false;
        SpawnLootAndDestroy();
    }

    private void SpawnLootAndDestroy()
    {
        if (parent.Map == null)
        {
            parent.Destroy();
            return;
        }

        if (Props.ItemSpawnList.NullOrEmpty())
        {
            Log.Warning($"[Idle Dungeon Looting] {parent.def.defName} has no loot entries.");
            parent.Destroy();
            return;
        }

        List<RSR_ItemSpawnList> validEntries = Props.ItemSpawnList
            .Where(entry => entry?.itemToSpawn != null && entry.itemSpawnWeight > 0)
            .ToList();

        if (validEntries.Count == 0)
        {
            Log.Warning($"[Idle Dungeon Looting] {parent.def.defName} has no valid loot entries.");
            parent.Destroy();
            return;
        }

        int dropCount = Rand.RangeInclusive(
            Props.amountOfDropsMin,
            Props.amountOfDropsMax < Props.amountOfDropsMin ? Props.amountOfDropsMin : Props.amountOfDropsMax);

        for (int i = 0; i < dropCount; i++)
        {
            RSR_ItemSpawnList entry = validEntries.RandomElementByWeight(x => x.itemSpawnWeight);
            int itemCount = Rand.RangeInclusive(
                entry.itemAmountMin,
                entry.itemAmountMax < entry.itemAmountMin ? entry.itemAmountMin : entry.itemAmountMax);

            for (int j = 0; j < itemCount; j++)
            {
                Thing thing = ThingMaker.MakeThing(entry.itemToSpawn);
                GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near);
            }
        }

        parent.Destroy();
    }
}
