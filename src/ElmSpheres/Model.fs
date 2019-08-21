namespace ElmSpheres.Model

open System
open Aardvark.Base
open Aardvark.Base.Incremental
open Aardvark.UI.Primitives

type Primitive =
    | Box
    | Sphere


[<DomainType>]
type Model =
    {
        cameraState     : CameraControllerState
        spheres         : plist<V3d>
        selected        : option<string>
    }