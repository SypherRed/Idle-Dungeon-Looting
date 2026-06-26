using System;
using Verse;

namespace RimWorld;

public class CompRSR_SpawnRandomItemsAndDisappear : ThingComp
{
    private CompProperties_RSR_SpawnRandomItemsAndDisappear Props =>
        (CompProperties_RSR_SpawnRandomItemsAndDisappear)props;

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        base.PostSpawnSetup(respawningAfterLoad);

        if (respawningAfterLoad || Props.ItemSpawnList.NullOrEmpty())
        {
            return;
        }

        int dropCount = Rand.RangeInclusive(Props.amountOfDropsMin, Props.amountOfDropsMax);
        for (int i = 0; i < dropCount; i++)
        {
            RSR_ItemSpawnList entry = Props.ItemSpawnList.RandomElementByWeight(x => x.itemSpawnWeight);
            int itemCount = Rand.RangeInclusive(entry.itemAmountMin, entry.itemAmountMax);

            for (int j = 0; j < itemCount; j++)
            {
                Thing thing = ThingMaker.MakeThing(entry.itemToSpawn);
                GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near);
            }
        }

        parent.Destroy();
    }
}
