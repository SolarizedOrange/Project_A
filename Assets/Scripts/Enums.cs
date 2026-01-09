using System;
using Unity.Behavior;

[Flags]
public enum Layers
{
    Default = 1,
    TransparentFX = 1 << 1,
    IgnoreRaycast = 1 << 2,
    Water = 1 << 3,
    UI = 1 << 4,
    PlayerCoverable = 1 << 6,
    HitCollider = 1 << 7,
    EnemyCoverable = 1 << 8,
    Player = 1 << 9,
    RagdollCollider = 1 << 10,
}

public enum WeaponType
{
    None,
    Melee,
    Handgun,
    Shotgun,
    SubmachineGun,
    Length
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

[BlackboardEnum]
public enum EnemyActionType
{
    None,
    Attack,
    Reload,
    Hit,
    MeleeTargeted
}

[BlackboardEnum]
public enum EnemyAnimationActionType
{
    Idle,
    BattleIdle,
    Aim,
    Attack,
    Cover,
    Reload
}

// StatType
public enum CharacterStatType
{
    HP, MoveSpeed, Precision
}

public enum WeaponStatType
{
	Accuracy, AttackRate, Capacity, Damage, ShotCount, AttackRange, Recoil, ReloadTime
}

// Perk
public enum PerkGroup
{
	Group1,Group2,Group3,Group4
}

public enum EndureNoteType
{
    Tap, Hold
}

public enum DecalType
{
    BulletHole,
    BloodOnWall,
    Blood
}