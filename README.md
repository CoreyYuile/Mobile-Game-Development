# Mobile Game Development

## Overview

### Introduction

Welcome to the official documentation for my mobile game project uploaded for BCU's CMP6187 Mobile Game Development module. This documentation will give a comprehensive guide through all relevant features pertaining to CMP6187's mark scheme as well as other features specific to the gameplay of this project. It will also give a brief overview of folder structure as well as showing development screenshots and design concepts. Finally this documentation will also link any relevant tutorials or assets used in the creation of this project.


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

A downloadable .apk of this project is available over on Itch.io, accessible through this link - [LINK HERE]

If choosing the .apk from the CMP6187 .zip you must transfer it over to an android device.

From there open the .apk and begin allowing the download of the game. Once complete you should get an app on your device named "3D Mobile Game". Tap on this app to begin playing the game.

The project was also uploaded onto the Google Play Store in a .aab format, however is unavailable for download due to the 12 playtester restriction. However hopefully proof of it's upload was included in the associated project video, where it shows that the game downloads and runs from the Play Store! Further proof of it's upload can be seen in these screenshots below on Google Developer Console:

ADD PHOTOS HERE

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

#### Accelerometer

The accelerometer of a mobile device are utilised for two different features within this game:
* AutoHarvest - Shake to automatically harvest all plots that have fully grown crops
* AutoPlant - Shake to automatically plant a seed in all empty plots. The type of crop is determined by what crop is currently selected on the UI.

Toggling between these features is controlled by tapping anywhere on this part of the screen:

The accelerometer is supported within Unity Remote.

### Frameworks

This section explains the code and logic behind features that would be considered as a usage of "frameworks" in CMP6187's Assignment Brief.

#### Unity Ads

##### Rewarded Ads

##### Banner Ads

#### Leaderboard Manager

#### APIs

##### Locational Requests

A locational request to an API is called in order to be able to call openweather with the correct info in order to get accurate data of the weather of the user's rough geographical location.

##### Weather Request

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

### Saving / Loading

### FarmPlot & States

#### Crop Handling

### Mobile Controls

## Development Screenshots & Concepts

### Concept Drawings

### Screenshots

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
