
# VR Visual Field Testing System - README

**I have deleted:**
* `src/IntegratedVFTnR/Assets/ViveSr` 
* `src/IntegratedVFTnR/Assets/ViveSR_Experience` 
  
**as they are too massive to upload.**
**Full src folder can be found here: https://github.com/Caleb-Mok/CSFYP**

## System Overview

This project is a virtual reality-based visual field testing platform designed using Unity and the HTC Vive Pro Eye. It incorporates real-time eye tracking, stimulus presentation, gaze-based feedback, and automatic data logging and reporting.



## System Requirements

### General Hardware:
* VR-capable PC (with compatible GPU)
* HTC Vive Pro Eye headset
* Vive Tracked Controllers
* SteamVR 2.0 Base Stations
* Steam and SteamVR properly installed and configured
* Windows 10 (developed on)


### Unity Requirements:
* Unity Editor version: `2019.4.35f1` *(exact version required to prevent plugin conflicts)*
* SteamVR Plugin: `2.7.3`
* SRanipal SDK: `1.3.6.8` *(deprecated - ensure you have local admin access)*
* Vive SRWorks SDK: `0.9.8.4` *(deprecated - ensure you have local admin access)*



### Python (for data processing and report generation):
Ensure the following packages are installed and imported:
```python
json, matplotlib, numpy, copy, pprint, os, sys, datetime, scipy.stats, io, PIL, reportlab
```

From `reportlab.platypus`:
* `Table`, `TableStyle`, `Paragraph`, `Spacer`, `Image`, `ListFlowable`, `ListItem`, `SimpleDocTemplate`

From `reportlab.lib`:
* `getSampleStyleSheet`, `colors`


## Important Project Structures

* `src/IntegratedVFTnR`: Main Unity project directory  
* `Assets/Saved Data/run/[timestamp]`: Stores raw, processed, and report data for each test  
* `Assets/Python Script`: Python scripts for analysis 


## Running in Unity Editor

**The project can only be ran in Unity Editor**

1. **Open the Project:**
   * Open Unity Hub
   * Open Project
   * Navigate to `src/IntegratedVFTnR`  

2. **Before Running:**
   * Ensure SteamVR is running  
   * Accept SRanipal admin permission popup (required once per session) 
   * Connect and power on the Vive headset and controllers  

3. **Run the Project:**
   * Navigate to `Assets/Scenes/Core Menu` to enter the Main Menu  
   * In Unity Editor, press play
   * Use the VR controller trigger to interact with the UI  
   * Select and enter a test scene  
   * Press Spacebar to start the test (after a 3-second countdown)  
   * Press Spacebar again to register stimulus detection  


## Test Flow

* Each test lasts approximately 3-4 minutes
* System auto-generates and stores:
   * Raw data  
   * Final PDF report
* Raw data and reports are saved in:  
  `Assets/Saved Data/run/[timestamp]/`

### Note:
All directories for storing run data are automatically created by the system  
Admin rights may be required for some plugins (especially SRanipal)  
Ensure controller(s) are active and tracked before testing begins  
