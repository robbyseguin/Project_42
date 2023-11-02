using System;

namespace Entities.Parts
{
    [Flags] public enum PartIdentifier
    {
        // Movement
        MOVEMENT_PART_DEFAULT = 1 << 0,
        MOVEMENT_PART_SPRINT = 1 << 1,
        MOVEMENT_PART_WARP = 1 << 2,
        // MOVEMENT_PART_03 = 1 << 3,
        // MOVEMENT_PART_04 = 1 << 4,
        // MOVEMENT_PART_05 = 1 << 5,
        
        // Cockpit
        COCKPIT_PART_DEFAULT = 1 << 6,
        COCKPIT_PART_ABSORBTION = 1 << 7,
        // COCKPIT_PART_02 = 1 << 8,
        // COCKPIT_PART_03 = 1 << 9,
        // COCKPIT_PART_04 = 1 << 10,
        // COCKPIT_PART_05 = 1 << 11,
        
        // Head
        HEAD_PART_DEFAULT = 1 << 12,
        HEAD_PART_SWITCH = 1 << 13,
        HEAD_PART_WARP = 1 << 14,
        HEAD_PART_SHIELD = 1 << 15,
        // HEAD_PART_04 = 1 << 16,
        // HEAD_PART_05 = 1 << 17,
        
        // Heavy weapon
        HEAVY_WEAPON_PART_DEFAULT = 1 << 18,
        HEAVY_WEAPON_PART_GRENADINE_LAUNCHER = 1 << 19,
        HEAVY_WEAPON_PART_PLASMA_THROWER = 1 << 20,
        // HEAVY_WEAPON_PART_03 = 1 << 21,
        // HEAVY_WEAPON_PART_04 = 1 << 22,
        // HEAVY_WEAPON_PART_05 = 1 << 23,
        
        // Light weapon
        LIGHT_WEAPON_PART_DEFAULT = 1 << 24,
        LIGHT_WEAPON_PART_LASER_ASSAULT_RIFLE = 1 << 25,
        LIGHT_WEAPON_PART_SNIPER_RIFLE = 1 << 26,
        // LIGHT_WEAPON_PART_03 = 1 << 27,
        // LIGHT_WEAPON_PART_04 = 1 << 28,
        // LIGHT_WEAPON_PART_05 = 1 << 29,
        
        // Extra
        // EXTRA_PART_01 = 1 << 30,
        // EXTRA_PART_02 = 1 << 31,
        
        // Group
        GROUP_MOVEMENT_PART = MOVEMENT_PART_DEFAULT | MOVEMENT_PART_SPRINT | MOVEMENT_PART_WARP,
        GROUP_COCKPIT_PART = COCKPIT_PART_DEFAULT | COCKPIT_PART_ABSORBTION,
        GROUP_HEAD_PART = HEAD_PART_DEFAULT | HEAD_PART_SWITCH | HEAD_PART_WARP | HEAD_PART_SHIELD,
        GROUP_HEAVY_WEAPON_PART = HEAVY_WEAPON_PART_DEFAULT | HEAVY_WEAPON_PART_GRENADINE_LAUNCHER | HEAVY_WEAPON_PART_PLASMA_THROWER,
        GROUP_LIGHT_WEAPON_PART = LIGHT_WEAPON_PART_DEFAULT | LIGHT_WEAPON_PART_LASER_ASSAULT_RIFLE | LIGHT_WEAPON_PART_SNIPER_RIFLE,
        GROUP_DEFAULT = MOVEMENT_PART_DEFAULT | COCKPIT_PART_DEFAULT | HEAD_PART_DEFAULT | HEAVY_WEAPON_PART_DEFAULT | LIGHT_WEAPON_PART_DEFAULT,

        // Pool
        POOL_PLAYER = GROUP_MOVEMENT_PART | GROUP_COCKPIT_PART | HEAD_PART_SWITCH | GROUP_HEAVY_WEAPON_PART | LIGHT_WEAPON_PART_LASER_ASSAULT_RIFLE,
        POOL_ENEMY = GROUP_MOVEMENT_PART | GROUP_COCKPIT_PART | GROUP_HEAD_PART | GROUP_HEAVY_WEAPON_PART | GROUP_LIGHT_WEAPON_PART,
        POOL_LOOT = ~(MOVEMENT_PART_DEFAULT | HEAD_PART_DEFAULT | HEAVY_WEAPON_PART_DEFAULT | LIGHT_WEAPON_PART_DEFAULT),
        POOL_DUMMY = ~(MOVEMENT_PART_DEFAULT | HEAD_PART_DEFAULT | LIGHT_WEAPON_PART_DEFAULT)
    }
}