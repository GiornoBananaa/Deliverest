using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Level
{

    public float stone_min_delay, stone_max_delay;
    public float avalanche_min_delay, avalanche_max_delay;
    public float target_height, time_for_level;
    public float stormMinDelay, stormMaxDelay;
    public float stormMinDuration, stormMaxDuration;
    public float appearSpeed;
    public float stormSpeed;
    public float signTime;
    public int levelWinSceneID;
    public bool snow_storm;
    public bool wind_sound;
    public Sprite backGround;
    public AudioClip music_on_level;
}
