open ElmSpheres

open Aardium
open Aardvark.Service
open Aardvark.UI
open Suave
open Suave.WebPart
open Aardvark.Rendering.Vulkan
open Aardvark.Base
open System

open Aardvark.Base.Incremental
type Changes = unit



type Object = {  name : string; pos : V3d;  }

type Scene = 
    {
        cameraPos : V3d
        objects : list<Object>
    }

type MScene =
    {
        mcameraPos : IModRef<V3d>
        mobjects   : list<Object>
    }


let oldScene : Scene  =
    {
        cameraPos = V3d(10,10,10)
        objects = [ { name = "Chair"; pos = V3d(0,0,0) }]
    }

let newScene : Scene  =
    {
        cameraPos = V3d(10,10,10)
        objects = [  { name = "Chair"; pos = V3d(0,10,0)}
                     { name = "harry"; pos = V3d(10,10,10) } 
                  ]
    }

let computeChanges (old : Scene) (newScene : Scene) : Changes = 
    failwith "todo smart implementation"



let applyChanges (old : Scene) (newScene : Scene) (state : MScene) : unit = 
    transact (fun _ -> 
        if old.cameraPos <> newScene.cameraPos then
            state.mcameraPos.Value <- newScene.cameraPos
        // same, for set but more complicated..
    )

// old = new -> nothing to do
// finegrained changes fed into incremental system



[<EntryPoint>]
let main args =
    Ag.initialize()
    Aardvark.Init()
    Aardium.init()

    let app = new HeadlessVulkanApplication(true)

    WebPart.startServer 4321 [
        MutableApp.toWebPart' app.Runtime false (App.start App.app)
    ] |> ignore
    
    Aardium.run {
        title "Aardvark rocks \\o/"
        width 1024
        height 768
        url "http://localhost:4321/"
    }

    0
