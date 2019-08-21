namespace ElmSpheres.Model

open System
open Aardvark.Base
open Aardvark.Base.Incremental
open ElmSpheres.Model

[<AutoOpen>]
module Mutable =

    
    
    type MModel(__initial : ElmSpheres.Model.Model) =
        inherit obj()
        let mutable __current : Aardvark.Base.Incremental.IModRef<ElmSpheres.Model.Model> = Aardvark.Base.Incremental.EqModRef<ElmSpheres.Model.Model>(__initial) :> Aardvark.Base.Incremental.IModRef<ElmSpheres.Model.Model>
        let _cameraState = Aardvark.UI.Primitives.Mutable.MCameraControllerState.Create(__initial.cameraState)
        let _spheres = MList.Create(__initial.spheres)
        let _selected = MOption.Create(__initial.selected)
        
        member x.cameraState = _cameraState
        member x.spheres = _spheres :> alist<_>
        member x.selected = _selected :> IMod<_>
        
        member x.Current = __current :> IMod<_>
        member x.Update(v : ElmSpheres.Model.Model) =
            if not (System.Object.ReferenceEquals(__current.Value, v)) then
                __current.Value <- v
                
                Aardvark.UI.Primitives.Mutable.MCameraControllerState.Update(_cameraState, v.cameraState)
                MList.Update(_spheres, v.spheres)
                MOption.Update(_selected, v.selected)
                
        
        static member Create(__initial : ElmSpheres.Model.Model) : MModel = MModel(__initial)
        static member Update(m : MModel, v : ElmSpheres.Model.Model) = m.Update(v)
        
        override x.ToString() = __current.Value.ToString()
        member x.AsString = sprintf "%A" __current.Value
        interface IUpdatable<ElmSpheres.Model.Model> with
            member x.Update v = x.Update v
    
    
    
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Model =
        [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
        module Lens =
            let cameraState =
                { new Lens<ElmSpheres.Model.Model, Aardvark.UI.Primitives.CameraControllerState>() with
                    override x.Get(r) = r.cameraState
                    override x.Set(r,v) = { r with cameraState = v }
                    override x.Update(r,f) = { r with cameraState = f r.cameraState }
                }
            let spheres =
                { new Lens<ElmSpheres.Model.Model, Aardvark.Base.plist<Aardvark.Base.V3d>>() with
                    override x.Get(r) = r.spheres
                    override x.Set(r,v) = { r with spheres = v }
                    override x.Update(r,f) = { r with spheres = f r.spheres }
                }
            let selected =
                { new Lens<ElmSpheres.Model.Model, Microsoft.FSharp.Core.Option<System.String>>() with
                    override x.Get(r) = r.selected
                    override x.Set(r,v) = { r with selected = v }
                    override x.Update(r,f) = { r with selected = f r.selected }
                }
