﻿using UnityEngine;
using System.Collections;

public class GlobalValues
{
	public static string ShipTag = "Ship";
    public static string ModuleTag = "Module";
    public static string StructureTag = "Structure";    
    public static string DefensiveModuleTag = "Defensive";
    public static string ProjectileTag = "Projectile";

    public static string AvailablePosTag = "AvailablePos";

	public static string StarTag = "Star";
	public static string PlanetTag = "Planet";
	public static string MoonTag = "Moon";
	public static string AsteroidTag = "Asteroid";
	public static string DebrisTag = "Debris";

    //public static int StructureMass = 1;
    public static int CoreMass = 4;
    public static float MinCrashMagnitude = 3;
    public static float HitFeedbackDuration = 1;
    public static int CrashDamage = 1;
    public static int MissileDamage = 1;
    public static int LaserDamage = 1;
	public static float EffectiveZeroMass = 0.1f;
}
