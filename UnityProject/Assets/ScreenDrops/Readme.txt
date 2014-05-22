
ScreenDrops Readme
*******************

   ScreenDrops is a screen glare effect designed for use in both indoor and outdoor 
   maps. You can customize the glare materials and add as many layers of textures 
   as you want. It is aspect ratio independent and you can combine it with light 
   shafts for an even better looking effect!


Installation
*************

 1) Extract the ScreenDrops Unity package into your project.

 2) Drag the ScreenDrops or ScreenDropsComplex prefab inside the 
   Main Camera. 

 5) That's it! Look at some lights and enjoy the glare!


Customisation
**************

  To add new layers, simply duplicate an existing one inside the 
 ScreenDrops prefab. Create new materials and assign different shaders and 
 textures until it looks right.

  Materials options:
   - Alpha: is the maximum opacity value for the layer. Value should be between 
    0 and 1.
   - Influence: higher means that the effect will be shown when looking at the 
    light at a smaller angle. Value should be between 1 and infinity.
   - Bumpiness: increases/decreases the bumpiness of the normal maps. Value 
    should be between 0 and infinity.


FAQ / Troubleshooting
**********************
 1) I can't see the glare effect.

   Try the following:
   - Make sure there is at least one light in the scene.
   - Check that the ScreenDrops script is assigned to the ScreenDrops gameobject.
   - Make sure that the nearClipPlane of your Camera is lower than 1.0.

2) The glare effect is visible when not facing the light.

   If you have more than 2-4 lights in your scene, you need to increase the Pixel 
  Light Count for your project:
   - Go to Edit > Project Settings > Quality. Change the Pixel Light Count for 
  each of the quality levels accordingly.

3) The skybox hides the effect.

   To solve this you need to render the skybox with a different camera.
   - Set the clear flags of your Main Camera to Don't Clear. 
   - Set the clear flags of the Skybox camera to Skybox (default).
   - Change the Culling Mask of the Skybox camera to Nothing.
   - Change the Depth value of the Skybox camera to a value lower than Main 
   Camera's depth.
   - Drag and drop the Skybox Camera into your Main Camera to set it as its 
   child.


Questions? 
***********

   Email me at screendrops@gmail.com

**********************************************************************************
                                    CHANGELOG                                    
**********************************************************************************

 V1.2.2
   - Updated FAQ with skybox info.

 V1.2
   - No longer using a custom layer.
   - Faster setup. Drag the prefab into the camera, and it's done!
   - Added support for Forward Rendering shadows.
   - Using quads instead of planes (Unity 4.2).
   - Updated FAQ.

 V1.1
   - Fixed a bug where the ScreenDrops script would not assign the layer to the 
     camera culling mask.
   - Added Pixel Lights to FAQ.

 V1.0
   - Initial version.