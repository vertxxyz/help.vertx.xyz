## Import settings
### Description
Some settings can delay the moment audio is played. If disabled this may cause frame hitches.  
These delays are one time only, only causing issues on the first play.

### Resolution
The per-clip **Load in Background** and **Preload Audio Data** settings have direct effects on each other. Please consider the table below:

| Load in background | Preload audio data | Outcome                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
|--------------------|--------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| 🟢 Enabled         | 🟢 Enabled         | When the scene is loaded, these clips begin loading but do not stall the main thread. They will continue to load in the background as the scene plays.<br/>If an unloaded sound is triggered, it will behave the same as if it had Preload disabled (see immediately below).                                                                                                                                                                                  |
| 🟢 Enabled         | 🔴 Disabled        | When the sound is triggered for the first time, it will load in the background and play as soon as it is ready. If the file is large, this will cause a noticeable delay between triggering and playing, but this is not an issue for subsequent plays of the sound.                                                                                                                                                                                          |
| 🔴 Disabled        | 🟢 Enabled         | The audio is loaded while the scene is loaded. The scene will not start until these clips are loaded into memory.                                                                                                                                                                                                                                                                                                                                             |
| 🔴 Disabled        | 🔴 Disabled        | When the sound is triggered for the first time, it uses the main thread to load itself into memory - if the file is large, this will cause a frame hitch, but this not an issue for subsequent plays of the sound.<br/>This is only recommended this for very small files, and even then, make sure to measure the impact this has on performance with the profiler, and consider whether a large number of these sounds might possibly be triggered at once. |

More information, and the source of this table can be found [here](https://www.gamedeveloper.com/audio/unity-audio-import-optimisation---getting-more-bam-for-your-ram#:~:text=Load%20in%20Background/Preload%20Audio%20Data).

---

[My audio is still delayed](Delayed%20Trigger.md)