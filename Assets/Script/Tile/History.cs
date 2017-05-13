using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum HistoryType
{
    AllocateTile,
};
public class History {
    public HistoryType Type;
}

public class ChangedResourceHistory: History
{
    public ChangedResourceHistory( )
    {
        TargetRenderers = new List<SpriteRenderer>();
        PrevSprites = new List<Sprite>();
        ChangedSprites = new List<Sprite>();
    }

    public List<SpriteRenderer> TargetRenderers { get; set; }

    public List<Sprite> PrevSprites    { get; set; }
    public List<Sprite> ChangedSprites { get; set; }

    public void Restore()
    {
        for (int n = 0; n < TargetRenderers.Count; ++n)
            TargetRenderers[n].sprite = PrevSprites[n];

        Swap();
    }
    private void Swap()
    {
        for (int n = 0; n < TargetRenderers.Count; ++n)
        {
            Sprite temp = PrevSprites[n];
            PrevSprites[n]= ChangedSprites[n];
            ChangedSprites[n] = temp;
        }
    }
}
