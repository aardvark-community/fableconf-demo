namespace ElmSpheres

open System
open Aardvark.Base
open Aardvark.Base.Incremental
open Aardvark.UI
open Aardvark.UI.Primitives
open Aardvark.Base.Rendering
open ElmSpheres.Model

type Message =
    | CameraMessage of FreeFlyController.Message

module App =
    
    let initial = { cameraState = FreeFlyController.initial }

    let update (m : Model) (msg : Message) =
        match msg with
        | CameraMessage msg ->
            { m with cameraState = FreeFlyController.update m.cameraState msg }

    let view (m : MModel) =

        let frustum = 
            Frustum.perspective 60.0 0.1 100.0 1.0 
                |> Mod.constant

        let sg =
            Sg.sphere 5 (Mod.constant C4b.Green) (Mod.constant 2.0)
            |> Sg.fillMode (Mod.constant FillMode.Line)
            |> Sg.shader {
                do! DefaultSurfaces.trafo
                do! DefaultSurfaces.simpleLighting
            }

        let att =
            [
                style "position: fixed; left: 0; top: 0; width: 100%; height: 100%"
            ]

        body [] [
            FreeFlyController.controlledControl m.cameraState CameraMessage frustum (AttributeMap.ofList att) sg
            Incremental.div ([style "position: fixed; right: 20px; top: 20px; backgroundcolor:white"] |> AttributeMap.ofList) AList.empty
        ]

    let app =
        {
            initial = initial
            update = update
            view = view
            threads = Model.Lens.cameraState.Get >> FreeFlyController.threads >> ThreadPool.map CameraMessage
            unpersist = Unpersist.instance
        }