# Build using ARFoundation Helper

AR Foundation is free and powerful. However, its editor user experience is not that good as other commercial SDKs like Vuforia oe EasyAR. (especially for Image Tracking) So I develop this tool to improve the editor experience.

## Main Features

### WYSIWYG experience with *NY Image Tracker

* Simply add *NY Image Tracker* component on empty object, then you can setup the target image and size.
* Children objects will move with the tracker
* If you don't want to group objects under any tracker, but still want them to move with tracker, you can set with the referenceGrouper attribute.

### On / Off tracking event handle 

### Manage Image Tracker Library through a single button

* After finish all *NYImageTracker* setup, press the Update Library button on *NYImageTrackerManager*, and the Image Library will be generated and auto assign to AR Foundation in runtime.
