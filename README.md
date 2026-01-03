# Mobile Game Development

## Overview

### Introduction

### List of Features

## Folder Structure

## Feature Explanation

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
