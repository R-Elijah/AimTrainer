# README for AimTrainer

    Student: Elijah Reddy
    Date: 2023/05/05

Summary:
    This submission contains a Unity project named AimTrainer which serves as a
    game to help with aim practice for First Person Shooters. The game consists
    of a firing range with targets that react to shots from a the player when
    mouse 1 is clicked. There is a hit tracker as well as an accuracy tracker.
    The default game state is endless mode when a target recedes when hit and
    returns to its original position after a second has passed, but the player
    can enter a practice state by hitting the "p" key. One in the practice state
    the targets are receded and a random target is available to fire at roughly
    every second over the course of a minute. The target is only available for
    a second and the aim is to score as much as possible within the time limit.
    Upon 60 seconds passing a results screen is displayed and the player can
    return to the range by pressing Enter.

Outside Resources Used:
    https://assetstore.unity.com/packages/3d/props/guns/stylized-m4-assault-rifle-with-scope-complete-kit-with-gunshot-v-178197
    Provides the gun model, sounds and particle effects.

Scenes:
    Title -- Has the title text and text telling the player to press enter.
             Switches to Main when enter is hit.
    
    Main -- the actual firing range. The firing range is built using a cube
            made in ProBuilder with targets on one wall using cylinders made
            using ProBuilder. The player is implemented using the FPSController
            from class and a gun model is provided in the view model. There is
            text displays for the practice mode instructions as well as hit and
            accuracy counters. Pressing P will start the practice mode.

    Results -- A results screen for the practice mode which shows your hits,
               accuracy and a prompt to hit enter to return to the range.

Scene Flow:

                                                      Enter
                                       ┌────────────────────────────────────┐
                                       │                                    │
                                       │                                    │
                                       │                                    │
 ┌─────────────┐            ┌──────────▼─────────┐      ┌───────────────┐   │
 │             │   Enter    │                    │ P    │               │   │
 │ Title Scene ├────────────► Firing Range Scene ├──────► Results Scene ├───┘
 │             │            │       (Main)       │      │               │
 └─────────────┘            └────────────────────┘      └───────────────┘


Scripts:
    LoadSceneOnInput.cs -- Taken from Dread50 in class. Simply switches the scene
                           to the Main scene (Firing Range) when Enter is hit in
                           either the Title scene or Results scene. The script is
                           attached to EnterText in the Title and ContText in
                           the Results.

    PlayerStats.cs -- A simple static class holding two public variables to
                      allow tracking hits and shots between scenes for the
                      result display.

    ResultStatDisplay.cs -- Grabs the values stored in PlayerStats to display
                            the number of hits and accuracy on the results
                            screen in HitsText and AccText.
                            Attached to TitleText.

    Weapon.cs -- Implements the majority of the logic for the shooting, targets,
                 range and the practice mode. Shooting is implemented using
                 RayCasts like in the Portal50 demo. RayCasts are implemented
                 such that the code inside is only run if the layer that's being
                 hit is a Target layer (the targets). The targets are all simply
                 cyliders with a Target layer and Target tag for ease of access.
                 When the player hits Mouse1 the muzzle flash particle animation
                 and shot sound are player and a RayCast is shot out from the
                 centre of the screen where the crosshair is.
                 Upon a RayCast hitting a target, the hit tracker is incremented
                 and a coroutine is started in which the target is moved outside
                 the range and then back in after a second. The shot counter is
                 incremented regardless of a hit or not so accuracy can be
                 calculated and displayed. When the player hits P the practice
                 mode is started by calling the PracticeMode function which
                 gets an array of all the targets via tag, resets all the stats
                 and then retracts all the targets outside the range.
                 The practice coroutine is then called which uses a while loop
                 to send random targets into the range roughly every second for
                 a total of 60 seconds and then displays the results in the
                 Results scene.
