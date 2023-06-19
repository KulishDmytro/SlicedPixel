# Realtime VFX - Sliced Pixel

![PixelSlicer_BaseCharacter_x2](https://github.com/KulishDmytro/SlicedPixel/assets/49024192/3616842b-4188-448b-b0bf-1ffae2be740c)
![PixelSlicer_BaseCharacter](https://github.com/KulishDmytro/SlicedPixel/assets/49024192/50bf756e-fdd8-4876-843d-be4128cb1025)
![PixelSlicer_MV](https://github.com/KulishDmytro/SlicedPixel/assets/49024192/51f40169-3c1f-4301-b96f-d16e624aa0a6)
## Sliced Pixel VFX based on Compute shader and custom motion vector buffer which is used in regular shader for vertex offset.
![Overview_Mesh_02](https://github.com/KulishDmytro/SlicedPixel/assets/49024192/ee34ac23-a7be-40b3-8ae4-85b7a0c3fc45)
![Overview_Mesh_01](https://github.com/KulishDmytro/SlicedPixel/assets/49024192/0d59182e-dff2-4874-990c-081c780c367a)
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
