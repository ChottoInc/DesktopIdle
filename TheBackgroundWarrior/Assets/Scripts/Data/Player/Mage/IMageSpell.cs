using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMageSpell
{
    public void SetPositions(Vector2 startPos, Vector2 target);
    public void Perform();
}
