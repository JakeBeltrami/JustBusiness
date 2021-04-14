using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAltPosition : Toggleable
{
    public Toggleable altToggle;

    public override void Toggle()
    {
        altToggle.Toggle();
    }
}
