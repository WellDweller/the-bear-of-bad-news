# Minigames

Scenes in this directory should be set up to be loaded as a minigame.
In general, that means:

- Include the Prefabs/SceneInitialization script
- Include exactly one `Minigame` component (or a script that inherits from it).
- Your game should call `Minigame.CompleteGame` to end the game
- Your game should have **3 stages**.  Each stage should select one string that, together, form a sentence for the bear to say.
  After each stage, your controller should call its `CompleteStage` method with a `MinigameResult` with values for _just that stage_.


## The MinigameResultDisplay

There's a prefab for the word cloud display at the top of the minigame that handles

- sending the selected sentence parts to the correct TextBalloon instance
- updating the balloon colors based on the result
- playing the correct sound effect

If you want to tweak any of these things, edit the prefab and it'll apply to all minigames

## Creating a new minigame

Clone the `Template` scene in this directory.  You can replace the `SlideBar` script in there with
a script for your minigame, which should inherit from `Minigame`.  There's a couple events you'll need
to wire together to ensure that everything works

1. The `MinigameResultDisplay` component (on the object of the same name inside `WorldCanvas`) must have its
   `OnComplete` event wired up to call your Minigame's `CompleteGame`.  This makes sure that the game doesn't
   try to end before the last word bubble finishes typing.
2. Your Minigame's `OnComplete` event should be wired up to call the `SlideInOut.SlideOut()` method of **both**
   the `GameGroup` and `MinigameResultDisplay` objects in the scene.  This kicks off the exit animation after
   the game is complete.
3. Your Minigame's `OnMinigameStageResult` event should be wired up to call the
   `MinigameResultDisplay.HandleMinigameResult()` method.  This makes sure that the word bubbles actually get the
   results from your game when it calls the `CompleteStage` method.