# Mobile Game Development

## Overview

### Introduction

Welcome to the official documentation for my mobile game project uploaded for BCU's CMP6187 Mobile Game Development module. This documentation will give a comprehensive guide through all relevant features pertaining to CMP6187's mark scheme as well as other features specific to the gameplay of this project. It will also give a brief overview of folder structure as well as showing development screenshots and design concepts. Finally this documentation will also link any relevant tutorials or assets used in the creation of this project.

For a more in depth explanation purely on the features relevant to CMP6187's assignment brief please refer to the video supplied in the .zip upload to Moodle.

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

The folder structure for the CMP6187 .zip file uploaded to moodle should be as follows:

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

### Credits
