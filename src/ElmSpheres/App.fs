namespace ElmSpheres

open System
open Aardvark.Base
open Aardvark.Base.Rendering
open Aardvark.Base.Incremental

open Aardvark.SceneGraph

open Aardvark.UI
open Aardvark.UI.Primitives
open ElmSpheres.Model

type Message =
    | CameraMessage of FreeFlyController.Message
    | AddSphere     of V3d
    | Redo 
    | Undo

module App =
    
    let initial = 
        { 
            cameraState = FreeFlyController.initial; 
            spheres = PList.empty 
            past = None
            future = None

        }

    let update (m : Model) (msg : Message) =
        match msg with
        | CameraMessage msg ->
            { m with cameraState = FreeFlyController.update m.cameraState msg }
        | AddSphere p ->
           { m with spheres = m.spheres |> PList.prepend p; past = Some m }
        | Undo ->
          match m.past with
          | Some p -> { p with future = Some m }
          | None -> m
        | Redo ->
          match m.future with
          | Some f -> { f with past = Some m }
          | None -> m
          

    let view (m : MModel) =

        let frustum = 
            Frustum.perspective 60.0 0.1 100.0 1.0 
                |> Mod.constant

        let sg =
            Sg.sphere 5 (Mod.constant C4b.Green) (Mod.constant 2.0)
            |> Sg.fillMode (Mod.constant FillMode.Line)
            |> Sg.requirePicking
            |> Sg.noEvents
            |> Sg.shader {
                do! DefaultSurfaces.trafo
                do! DefaultSurfaces.simpleLighting
            }
            |> Sg.withEvents [
                Sg.onClick(fun p -> AddSphere p)
            ]

        let littleSpheres =
            m.spheres
            |> AList.toASet
            |> ASet.map(fun x ->
                Sg.sphere 5 (Mod.constant C4b.Red) (Mod.constant 0.15)
                |> Sg.fillMode (Mod.constant FillMode.Fill)
                |> Sg.requirePicking
                |> Sg.noEvents
                |> Sg.shader {
                    do! DefaultSurfaces.trafo
                    do! DefaultSurfaces.simpleLighting
                }
                |> Sg.trafo (x |> Trafo3d.Translation |> Mod.constant)
            )
            |> Sg.set

        //let color sphereId = 
        //    m.selected 
        //    |> Mod.map(fun x ->
        //        match x with
        //        | Some id -> if sphereId = id then C4b.Blue else C4b.Red
        //        | None -> C4b.Red)


        let icon = i [clazz "circle white middle aligned icon"][]
        let sphereList =
            Incremental.div
                ([clazz "ui divided list inverted"] |> AttributeMap.ofList)
                (
                    alist {
                        for s in m.spheres do
                            yield div [clazz "item"] [
                                icon
                                div[clazz "content"] [
                                    div[clazz "header"; ][text "Sphere"]
                                    div[clazz "description"][text (s.ToString("0.000"))]
                                ]
                            ]
                    }
                )

        let att = [ style "position: fixed; left: 0; top: 0; width: 100%; height: 100%" ]
        require (Html.semui) (
            body [clazz "ui"] [
                FreeFlyController.controlledControl m.cameraState CameraMessage frustum (AttributeMap.ofList att) ([sg; littleSpheres] |> Sg.ofList)               
                div [clazz "ui inverted segment"; style "position: fixed; right: 20px; top: 20px;"] [
                    div [clazz "inverted ui buttons"][
                        button [clazz "ui button"; onClick (fun _ -> Undo)][text "Undo"]
                        button [clazz "ui button"; onClick (fun _ -> Redo)][text "Redo"]
                    ]
                    sphereList
                ]
            ]
        )

    let app =
        {
            initial = initial
            update = update
            view = view
            threads = Model.Lens.cameraState.Get >> FreeFlyController.threads >> ThreadPool.map CameraMessage
            unpersist = Unpersist.instance
        }