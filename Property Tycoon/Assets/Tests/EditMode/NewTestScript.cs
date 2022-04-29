using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    [Test]
    public void TestMortgage()
    {
        Property property = new Property();
        property.SetMortgaged(true);
        Assert.AreEqual(true, property.IsMortgaged());
    }
}
