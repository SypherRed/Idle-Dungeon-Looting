using System.Collections.Generic;
using Verse;

namespace RimWorld;

public class CompProperties_RSR_SpawnRandomItemsAndDisappear : CompProperties
{
    public int amountOfDropsMin = 5;
    public int amountOfDropsMax = 5;
    public List<RSR_ItemSpawnList> ItemSpawnList = new();

    public CompProperties_RSR_SpawnRandomItemsAndDisappear()
    {
        compClass = typeof(CompRSR_SpawnRandomItemsAndDisappear);
    }
}
