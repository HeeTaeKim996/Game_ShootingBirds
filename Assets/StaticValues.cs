using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticValues
{
    public static PhysicMaterial nonFrictionMaterial = new PhysicMaterial() { dynamicFriction = 0, staticFriction = 0, bounciness = 0 , frictionCombine = PhysicMaterialCombine.Maximum};
}
