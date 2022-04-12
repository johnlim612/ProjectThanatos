# ProjectThanatos

## Controls
Only keyboard and mouse compatible as of this moment:
| Key on Keyboard | Control |
| ------------------ | ------ | 
| W A S D | Forward, Left, Back, Right |
| Mouse | Look Around / Rotate Camera
| Space | Interact
| Shift | Sprint
| Q | Open and Close Tablet *(Use the left mouse button to press on the tabs in the tablet - **Quest, Map, Diary**)*

## Instructions on how to obtain the unity project files
1. Go to https://github.com/johnlim612/ProjectThanatos 
2. Navigate to the develop branch
3. Clone off of the develop branch to obtain unity project files.

4. **Be sure to use Unity Version 2017.1.17f1** when attempting to run the unity project.

## List of Outstanding Bugs
- Pressing Space repeatedly and extremely quickly OR holding it may bug out the dialogue system.
- Quest system is not fully integrated in that players are not forced to complete dialogue objectives in order to advance to the next day
- The Settings button on the Main Menu doesn't have a connected UI. When you press the button, there is a close animation that never reopens, so the user must Alt-F4 to quit the game and relaunch.
- Finding the dead body isn't completely integrated into the dialogue. The player won't fully know where they are searching for the dead body.
- In the EOD diary script where the player chooses who they are suspicious of, the script still has placeholder text that says 
"[SUSPECT]". This needs to be updated with the chosen NPC in the future.
- Calendar next to the player's bed only decrements in the IntroCutscene. The method is never called to update it in the GameScene.
- Text shown on tablet will become illegible if opened too quickly
- Multiple interactables can be interacted with at the same time and will bug out the system.

## Unique Project Features
- Tablet with a map projecting the player's position consistently
- Dialogue system featuring unique changes throughout the entire gameplay
- Introduction Cutscene and Ending Cutscene