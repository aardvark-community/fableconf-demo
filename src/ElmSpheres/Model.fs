namespace ElmSpheres.Model

open System
open Aardvark.Base
open Aardvark.Base.Incremental
open Aardvark.UI.Primitives

[<DomainType>]
type Model =
    {
        cameraState     : CameraControllerState     
        spheres         : plist<V3d>

        [<NonIncremental>]
        past            : option<Model>
        [<NonIncremental>]
        future          : option<Model>
    }