using System;

public enum PlayerStateType
{
    Idle,
    Move,
    Cover
}

[Flags]
public enum Layers
{
    Coverable = 1 << 6,
}

public enum WeaponType
{
    Melee,
    Handgun,
    Shotgun,
    SubmachineGun
}
