using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* A custom named property attribute.
**/

public class ReadOnlyAttribute : PropertyAttribute
{
    public bool OnlyShowInPlayMode = false;
}
