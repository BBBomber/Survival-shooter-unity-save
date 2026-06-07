# Survival-shooter-unity  - Original Repo
Survival shooter tutorial from https://unity3d.com/ru/learn/tutorials/projects/survival-shooter-tutorial

Unity version 6000.3.10f1

# Upgraded with a Save Load System

# Overview 
- Saves and restores game state (score, player health and position, camera, spawner timers, and live enemies) plus a separate audio config file (toggle, music, effects).
- Save from the pause menu.
- Start game from the Main Menu and click on Continue to restore save. It is atm deleted if you press new game or make a new save.

Game save: savegame.json in the persistent data path.
Audio config: config.json in the persistent data path.

Both are written atomically with a .bak fallback, so an interrupted write
cannot corrupt the live file.



