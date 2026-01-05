# Mobile Game Development

## Overview

### Introduction

Welcome to the official documentation for my mobile game project uploaded for BCU's CMP6187 Mobile Game Development module. This documentation will give a comprehensive guide through all relevant features pertaining to CMP6187's mark scheme as well as other features specific to the gameplay of this project. It will also give a brief overview of folder structure as well as showing development screenshots and design concepts. Finally this documentation will also link any relevant tutorials or assets used in the creation of this project.

The game developed is a idle / offline farming game made within Unity 6000.0.58f2. The genre is hyper-casual and theme was low-poly modern farms.

There is also a video included in the .zip folder uploaded to Moodle that will explain specifically Frameworks and Mobile Features.

If you have any inquiries please contact me!

Corey.Yuile@mail.bcu.ac.uk

### List of Features

The following is a quick breakdown of features that count towards CMP6187's assignment brief markscheme in one way or another and section links to in-depth explanations of each feature:

* [Uploaded to Itch.io and Google Play](https://github.com/CoreyYuile/Mobile-Game-Development/edit/main/README.md#available-storefronts--installation-guide)
* [Haptics / Vibrations](https://github.com/CoreyYuile/Mobile-Game-Development/edit/main/README.md#haptics--vibrations)
* [Accelerometer](https://github.com/CoreyYuile/Mobile-Game-Development/edit/main/README.md#accelerometer)
* [Ads](https://github.com/CoreyYuile/Mobile-Game-Development/edit/main/README.md#accelerometer)
* [Leaderboard](https://github.com/CoreyYuile/Mobile-Game-Development/edit/main/README.md#leaderboard-manager)
* [APIs](https://github.com/CoreyYuile/Mobile-Game-Development/edit/main/README.md#leaderboard-manager)
* [CineMachine](https://github.com/CoreyYuile/Mobile-Game-Development/edit/main/README.md#leaderboard-manager)

### Folder Structure

The folder structure for this repository is as follows:

+ 3D Mobile Game - This is the main project files for Unity. Most things in here will not be useful aside from the /Assets/ folder.
  + /Assets
    + /Assets - Contains all assets ranging from Prefabs, Materials, Textures, Models, etc.
    + /Packages - Includes most third-party packages / frameworks imported into the project
    + /Scenes - Contains the Unity scenes used within the final product
    + /Scripts - Stores all the relevant scripts used within the project
      + /Ads - All scripts used to initialise ads as well as define individual ad types
      + /Crops - Stores all of the scriptable objects created for individual crop types as well as the script used to instantiate all cropdata objects
      + /Leaderboard - External leaderboard scripts required for the framework to run
      + /Managers - Holds all of the scripts deemed as "Managers" in the project, ranging from UI to Save/Load to Tapping
    + /Settings - URP assets for graphics
+ Screenshots - all development screenshots and images used within this doc

The folder structure for the CMP6187 .zip file uploaded to moodle should be as follows:

+ Bin - Supplied .apk file to play on an android device
+ Code - Project files
  + 3D Mobile Game - This is the main project files for Unity. Most things in here will not be useful aside from the /Assets/ folder.
    + /Assets
      + /Assets - Contains all assets ranging from Prefabs, Materials, Textures, Models, etc.
      + /Packages - Includes most third-party packages / frameworks imported into the project
      + /Scenes - Contains the Unity scenes used within the final product
      + /Scripts - Stores all the relevant scripts used within the project
        + /Ads - All scripts used to initialise ads as well as define individual ad types
        + /Crops - Stores all of the scriptable objects created for individual crop types as well as the script used to instantiate all cropdata objects
        + /Leaderboard - External leaderboard scripts required for the framework to run
        + /Managers - Holds all of the scripts deemed as "Managers" in the project, ranging from UI to Save/Load to Tapping
      + /Settings - URP assets for graphics
+ Screenshots - Screenshot evidence of the game being uploaded to storefronts
+ Video - Video explaining all features relevant to CMP6187's markscheme
+ README - .txt file that relays important info and links to this page for official documentation and further reading

### Available Storefronts / Installation Guide

You must have an android device in order to run the supplied builds of this game. Supplied builds can either be found over on my Itch.io page or in the CMP6187 .zip file provided in the module's respective Moodle page.

A downloadable .apk of this project is available over on Itch.io, accessible through this link - https://shlumptee.itch.io/idle-farmer

<img width="1803" height="923" alt="Screenshot 2026-01-05 123433" src="https://github.com/user-attachments/assets/127ab6fa-45de-43b0-8951-f2f3c60d246c" />

If choosing the .apk from the CMP6187 .zip you must transfer it over to an android device.

From there open the .apk and begin allowing the download of the game. Once complete you should get an app on your device named "3D Mobile Game". Tap on this app to begin playing the game.

The project was also uploaded onto the Google Play Store in a .aab format, however is unavailable for download due to the 12 playtester restriction. However hopefully proof of it's upload was included in the associated project video, where it shows that the game downloads and runs from the Play Store! Further proof of it's upload can be seen in these screenshots below on Google Developer Console:

<img width="1577" height="831" alt="Screenshot 2026-01-05 003544" src="https://github.com/user-attachments/assets/7b395d04-124c-4587-9488-a7940b9f0559" />

![IMG_8533](https://github.com/user-attachments/assets/c0595d33-57f6-46e4-a777-418be7947024)

![IMG_8534](https://github.com/user-attachments/assets/f32e0b2c-228e-4279-b238-21a08d6d327b)

## Feature Explanation

This section is dedicated to documenting and explaining all the features this mobile game has included. In general most scripts that I write include comprehensive code comments that may further explain what a specific section of code is meant to do, but this section will document the overview of each feature and the finished version.

### Mobile-Only Features

The following section is dedicated to explaining the code and logic behind features that would be classed as "mobile features" in CMP6187's Assignment Brief.

#### Haptics / Vibrations

Haptics / vibrations are performed through a seperate framework called CandyCoded. CandyCoded allows for 3 different levels of rumble intensity which have been utilised for the different states of crop growth, which is listed below:
* Light Vibration - Called on planting a seed in a plot (may be too light of a vibration to feel on certain devices)
* Medium Vibration - Called once a seed has grown into a plant and the call is made to swap from the seed prefab to plant prefab
* Heavy Vibration - Called when harvesting a plot, right after destroying the plant prefab

There is also a DefaultVibration function that utilises Unity's in-built haptics system, however this function goes unused and was previously used as a placeholder for where CC vibrations should be played (as Unity Vibration calls a debug log to confirm that it works)

Below is the script which holds all of the vibration functions:

<img width="362" height="509" alt="Haptic Funcs" src="https://github.com/user-attachments/assets/0a109545-8d07-4f03-aa4b-4e527894c9db" />

#### Accelerometer

The accelerometer of a mobile device are utilised for two different features within this game:
* AutoHarvest - Shake to automatically harvest all plots that have fully grown crops
* AutoPlant - Shake to automatically plant a seed in all empty plots. The type of crop is determined by what crop is currently selected on the UI.

Usage of it can be found in the TapManager.cs script, where the magnitude of the Vector3 value is used to be compared against a threshold value. This threshold value is put in place so that the game doens't consider any slight movement to be an indication of shaking. From there the script will call one of two functions depending on what the player has selected the toggle to be set to.

The functions essentially loop through every farm plot in the scene and either call functions to plant whatever is the currently selected crop or harvest all available crops.

Toggling between these features is controlled by tapping anywhere on the toggle below the weather info panel in the main UI.

The accelerometer is supported within Unity Remote.

<img width="883" height="470" alt="Accelerometer Func" src="https://github.com/user-attachments/assets/f534d66f-aa4f-47d7-89e1-eb2d86489427" />

### Frameworks

This section explains the code and logic behind features that would be considered as a usage of "frameworks" in CMP6187's Assignment Brief.

#### Unity Ads

Unity ads is a framework / package that allows for the displaying of ads to be made through Unity's services. Within this project Advertisement Legacy 4.4.2 is used.

Originally the most up-to-date version of Unity ads was used, however there seemed to be a commonly cited bug with that version where banner ads were more likely to never get loaded. For this reason Advertisement Legacy was downgraded.

##### Rewarded Ads

Rewarded ads are used in this game to give the player 100 money after watching an ad. To see this in action, tap anywhere on the plus button next to the money indicator.

##### Banner Ads

Banner ads are shown in the game whenever the player decides to open the leaderboard while in the farm scene. This does not happen for the leaderboard seen in the main menu as we do not yet have ads initialised within that scene.

#### Leaderboard Manager

The Leaderboard Manager is a plugin / package / framework for Unity created by Danial Jumagaliyev. The download page for this is available at: [https://danqzq.itch.io/leaderboard-creator]

Much of the leaderboard code within this project was taken from my previous project Gravimatic (available at [https://shlumptee.itch.io/gravimatic]), which was code derived and modified from Danial's official documentation, online tutorials utilising this framework, and some code from demos.

#### APIs

APIs are used to collect weather data from the player's general location. This data is then used to affect gameplay in ways such as increasing / decreasing growth time per crop or giving out bonus money for harvesting while a certain weather type is active.

Any personal information gathered from these APIs are temporary and never saved for any additional usage. The data collected is only used for determining the player's realtime weather to make gameplay adjustments accordingly.

##### Locational Requests

A locational request to two APIs are called in order to be able to call openweather with the correct info in order to get accurate data of the weather of the user's rough geographical location.

The first API used is [https://www.ipify.org/], which is used to get the player's current IP address temporarily

<img width="768" height="405" alt="IP API Call" src="https://github.com/user-attachments/assets/c0b23e87-595e-477a-bcf9-c202f6cdcd31" />


<img width="1310" height="582" alt="LatLon API Call" src="https://github.com/user-attachments/assets/f71afab3-4f36-482d-a3d7-4a5d1b0ec21a" />

##### Weather Request



<img width="1386" height="762" alt="Weather API Call" src="https://github.com/user-attachments/assets/63cc0cc3-25cc-4f92-a782-4a0f0713f578" />

#### CineMachine

##### Position Composer

Position Composer allows for the camera to smoothly drag around, resulting in a much more professional-quality camera compared to Unity's in-built camera system.

Hard limits are set on the composer in order to make sure the target position doesn't stray too far off-camera, leading to the virtual camera taking too long to catch up. However in most situations the target never hits these hard-limits on mobile devices as it would require a super fast swipe which doesn't usually happen in normal gameplay.

Other features of the composer were considered, such as the lookahead feature, however they resulted in camera actions that felt too janky or offputting - either feeling like the camera was overshooting way too much or just wasn't pointing towards the direction the player was aiming to go towards.

##### Confiner

Confiner is used to make sure that the camera doesn't go out of bounds, and instead stays within level geometry.

#### CandyCoded

Please Refer to the Vibration & Haptics section above.

## Gameplay Explanation

### Scenes

Idle Farmer consists of 2 scenes. A main menu and then the main game scene (referred to as "Farm" in the project files).

The main menu holds the credits section as well as quick access to the [Leaderboard](https://github.com/CoreyYuile/Mobile-Game-Development/edit/main/README.md#leaderboard-manager) to change username etc.

The farm scene is where main gameplay takes place.

<img width="169" height="113" alt="Scenes folder loc" src="https://github.com/user-attachments/assets/80a39af6-83a9-4790-bc75-6482ab0ed558" />

### Saving / Loading

### FarmPlot & States

#### Grid Generation

#### Plot Unlocking

#### Plot States

#### Crop Selection and Handling

### Mobile Controls

#### Camera Panning

Panning the camera is handled in the CameraPan.cs script. This script determines when the player is swiping to pan the camera or pinch to zoom in / out.

##### Panning

If there is one finger on the screen, get the distance between it's starting position and current position. Then calculate the movement direction the player is swiping in.

This is a bit more complex than if the camera was top-down since Idle Farmer's camera is angled slightly, and therefore camforward and camright are used to make sure that the camera still pans across the horizontal plane appropriately.

However since the CineMachine Camera is focusing on a target to move around, we apply the translation to the target object instead of the Camera. This target also needs rough boundaries to make sure that it doesn't go too far outside the game scene and leave our camera behind, so the position of the target is clamped to similar level boundaries as the camera.

<img width="1529" height="545" alt="CameraPan Pan Dectection" src="https://github.com/user-attachments/assets/f83daae8-5ba4-432d-a697-4305822763db" />

##### Zooming

If two fingers are detected on the screen, get the difference in distance between the starting position and current position for BOTH fingers. Then get the vector magnitudes of where they started and where they are now. If there is a discernable difference between the magnitude of both vectors, that indicates that the player is wanting to zoom in / out. Call the zoom function with whatever value this distance ends up being. The zoom function adjusts the CineMachine Camera's FOV slider. 

<img width="744" height="340" alt="CameraPan Zoom Detection" src="https://github.com/user-attachments/assets/10b84c73-815f-4ade-a30c-a2319e1e7ea6" />


#### Tapping

Tapping on the screen is handled in the TapManager.cs script.

This will first check to make sure that the player is not just tapping on the screen to pan the camera. If they are not doing this, that means that the player is indicating they are tapping on something. In this game, tapping is only used for UI selections (which this script will ignore a tap made if it is over a UI item) or for interacting with one of the many farm plots.

Try selecting whatever plot the player's tap ended up being over. To do this we cast a ray forward from the camera. If we hit an object with the FarmPlot component, then we know that we have found a farm plot. Call the FarmPlot's HandleTap() function to deal with whatever is needed.

<img width="817" height="769" alt="TapManager Tap Dectection" src="https://github.com/user-attachments/assets/6bab52a9-4e18-4fdb-8b9b-856979a39b8f" />
<img width="644" height="252" alt="TapManager SelectPlot" src="https://github.com/user-attachments/assets/e21fed16-8ad4-4dec-b427-114a3a39d76e" />

### "AutoHarvest" / "AutoPlant"

These terms refer to the gameplay addition of shaking the mobile device to either plant or harvest. In hindsight there is definitely a better and more concise name for this, however at the time this is what I called then in development.

Both of these features use the accelerometer, which is better explained in [this section](https://github.com/CoreyYuile/Mobile-Game-Development/edit/main/README.md#accelerometer).

#### AutoHarvest

In order for this feature to be chosen out of the two, make sure that the AutoHarvest toggle is set to "true" (indicated by the tick in the circle).

The associated function for this is AutoHarvest(), located within TapManager.cs, and will iterate through every plot in the scene and check if their current state is set to ReadyToHarvest. If this is the case, call to that plot's FarmPlot script and execute the HarvestCrop() function. For more information on this function please refer to the [Plot States section](https://github.com/CoreyYuile/Mobile-Game-Development/edit/main/README.md#plot-states).

<img width="696" height="278" alt="AutoHarvest Func" src="https://github.com/user-attachments/assets/d91a7fe8-1ff0-410b-953c-b7345d87d41e" />

#### AutoPlant

In order for this feature to be chosen out of the two, make sure that the AutoHarvest toggle is set to "false" (indicated by no tick being visible in the circle).

The associated function for this is AutoPlant(), located within TapManager.cs, and will iterate through every plot found in the scene and check if the have been unlocked AND their state is currently set to empty. If both of these conditions are met, call the plot's FarmPlot script and execute the PlantSeed() function. For more information on this function please refer to the [Plot States section](https://github.com/CoreyYuile/Mobile-Game-Development/edit/main/README.md#plot-states).

<img width="720" height="280" alt="AutoPlant Func" src="https://github.com/user-attachments/assets/0cbdb63a-a617-4b34-96a3-b08431274edb" />

## Development Screenshots & Concepts

The following are a selection of screenshots and concept drawings taken during development. For more screenshots please view the /Screenshots/ folder of this repository.

### Concept Drawings

![IMG_8535](https://github.com/user-attachments/assets/7adcf888-40dd-410b-9121-d86e29986cc8)

![IMG_8536](https://github.com/user-attachments/assets/64b88bc5-b4ea-49ee-9e41-7bcb5f94ddc7)

![IMG_8537](https://github.com/user-attachments/assets/499aa5e0-5bf7-4f2d-953d-ac969936a363)

![IMG_8376](https://github.com/user-attachments/assets/29a0fe89-eec5-4fa6-8e69-d9c360910507)

![IMG_8375](https://github.com/user-attachments/assets/ffa12605-acf6-41ab-8260-0164cb29a56d)

<img width="1343" height="719" alt="Screenshot 2026-01-05 200302" src="https://github.com/user-attachments/assets/f07f152b-26e1-484f-b1c5-5d86960bf3ce" />

<img width="1281" height="701" alt="Untitled" src="https://github.com/user-attachments/assets/8ad66805-cdca-4531-8f69-bb9384beafa5" />

### Screenshots

<img width="1233" height="859" alt="Screenshot 2025-10-18 161810" src="https://github.com/user-attachments/assets/47aea58e-c39f-4b5b-b36f-b07aaa7fcf4e" />

<img width="888" height="863" alt="Screenshot 2025-10-18 161825" src="https://github.com/user-attachments/assets/a5ad39bc-7d96-4cd9-bb77-6121475c4f02" />

<img width="776" height="865" alt="Screenshot 2025-10-18 161911" src="https://github.com/user-attachments/assets/4269747b-f613-45e5-9729-c6ec6e133e56" />

<img width="752" height="858" alt="Screenshot 2025-10-19 155226" src="https://github.com/user-attachments/assets/f2d1a031-901f-48da-8b2d-a33ca6a347f2" />

<img width="1567" height="863" alt="Screenshot 2025-10-19 172657" src="https://github.com/user-attachments/assets/247e93dd-f87e-4090-b88d-4cb2771c5236" />

<img width="1566" height="856" alt="Screenshot 2025-12-17 231226" src="https://github.com/user-attachments/assets/e687fd8b-03bb-4912-a1b9-af37c6d5cab4" />

<img width="483" height="860" alt="Screenshot 2025-12-23 204229" src="https://github.com/user-attachments/assets/f2ca68c3-ab65-4d4d-ad16-1b6c3e2f698d" />

<img width="477" height="856" alt="Screenshot 2025-12-24 011207" src="https://github.com/user-attachments/assets/42b2fc54-b513-4e52-875c-bbec58215550" />

## References & Credits

Various assets and tutorials were used and adapted in the creation of this game. Any resources used will be listed within this section.

### References

* Scriptable Objects
  * https://youtu.be/7jxS8HIny3Q?si=GLCORJSPPeP3tTsj
  * https://youtu.be/7jxS8HIny3Q?si=zg3SiVK8MpiEA2Ki
  * https://youtu.be/dIAAi54Ty58?si=Yvgvx-4kNz9V_P8R
* Accelerometer
  * https://youtu.be/XZWNXsjIvrE?si=Td2UqD7l8Jb34slr
* Camera Panning & Zooming
  * https://youtu.be/K_aAnBn5khA?si=bXjfsDndzLOd5wU8
  * https://youtu.be/4_HUlAFlxwU?si=zA6KikWkpUPUirYv
* Leaderboard Manager
  * Code derived from a previous game project available at: https://shlumptee.itch.io/gravimatic
* Saving / Loading
  * https://youtu.be/6uMFEM-napE?si=aIN2AUGL3sBdMFsT
  * https://youtu.be/aUi9aijvpgs?si=fpx-ftzX0WNnWQ2e
* Unity Ads
  * https://youtu.be/seTvVkaU2dk?si=h5mEIusPm4SJp9ZC
* API Requests
  * https://youtu.be/2vjpwNFU5To?si=cZu6mcsaXmkxyB1i
  * https://youtu.be/PnCtUemkqZs?si=hYMk8ds0ZP7v-QBV

### Credits

All credits are listed in-game, but links will also be provided here.

* Leaderboard Manager
  * https://danqzq.itch.io/leaderboard-creator
* Low Poly Simple Nature Pack
  * https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-simple-nature-pack-162153
* Farming Crops Low Poly Models
  * https://craftpix.net/freebies/free-farming-crops-3d-low-poly-models/?num=1&count=35&sq=farm%20crops&pos=3
* Farm Ranch Low Poly Pack
  * https://assetstore.unity.com/packages/3d/props/pandazole-farm-ranch-low-poly-pack-206756
* 2D Casual Game UI
  * https://assetstore.unity.com/packages/2d/gui/2d-casual-game-ui-hd-259245
* Low Poly Bird
  * https://assetstore.unity.com/packages/3d/characters/animals/birds/bird-330238
