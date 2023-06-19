# Realtime VFX - Sliced Pixel

![PixelSlicer_BaseCharacter_x2](https://github.com/KulishDmytro/SlicedPixel/assets/49024192/8a25abbb-bb21-4043-a783-9efacea1c56f)
![PixelSlicer_BaseCharacter](https://github.com/KulishDmytro/SlicedPixel/assets/49024192/20dc7bbf-6c4f-4b14-8c79-7f10f0ba8e5b)
![PixelSlicer_MV](https://github.com/KulishDmytro/SlicedPixel/assets/49024192/dc282cef-b0be-4b75-88d5-36d93b812de8)
## Sliced Pixel VFX based on Compute shader and custom motion vector buffer which is used in regular shader for vertex offset.
![Overview_Mesh_02](https://github.com/KulishDmytro/SlicedPixel/assets/49024192/1aaf0ca0-ef20-45a9-aa4a-fb2cb79e95d8)
![Overview_Mesh_01](https://github.com/KulishDmytro/SlicedPixel/assets/49024192/9d1bd76f-bd32-4965-9157-081e63499e95)
## Code-generated mesh with custom data, based on Texture.

## Usage

* Add the `SlicedPixel` directory to your Unity project's `Assets` directory.
* Create empty GameObject, assign `PixelMeshGenerator` component 
* Assing your texture into `Texture` field, choose folder, type name and click on `Generate` bool. 
* Create material from `SlicedPixel_Shader` and set Texture to it
* Create empty GameObject which will represent `ControlPoint` which will slice your object
* Create GameObject with `MeshFilter` and `MeshRenderer`
* Assign your custom mesh and material to it
* Add `PassBufferIntoRegularShader` and setup properties on it.
* Hit play!

## License
All code in this repository ([SlicedPixel](https://github.com/KulishDmytro/SlicedPixel)) is made freely available under the MIT license. This essentially means you're free to use it however you like as long as you provide attribution.
