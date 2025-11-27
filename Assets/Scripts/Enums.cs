using System;
using Unity.Behavior;

public enum PlayerStateType
{
    Idle,
    Move,
    Cover
}

[Flags]
public enum Layers
{
    PlayerCoverable = 1 << 6,
    HitCollider = 1 << 7,
    EnemyCoverable = 1 << 8
}

public enum WeaponType
{
    Melee,
    Handgun,
    Shotgun,
    SubmachineGun
}

public enum HitBoxType
{
    Head,
    Body,
    Leg,
    Player
}

[BlackboardEnum]
public enum EnemyStateType
{
    Idle,
    BattleIdle,
    Cover,
    Chase,
    Dead
}

public enum AnimationActionType
{
    Idle,
    BattleIdle,
    Aim,
    Attack,
    Cover,
    Die
}