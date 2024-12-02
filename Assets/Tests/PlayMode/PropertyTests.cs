using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PropertyTests
{
    [UnityTest]
    public IEnumerator Mortgage()
    {
        GameObject player = new GameObject();
        player.AddComponent<Player>();

        yield return null;
    }
}
