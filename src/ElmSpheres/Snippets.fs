| Undo ->
  match m.past with
  | Some p -> { p with future = Some m }
  | None -> m
| Redo ->
  match m.future with
  | Some f -> { f with past = Some m }
  | None -> m


|> Sg.withEvents [
    Sg.onClick(fun p -> AddSphere p)
]

(
    alist {
        for s in m.spheres do
            yield 
                div [clazz "item"] [
                    icon
                    div[clazz "content"] [
                        div[clazz "header"; ][text "Sphere"]
                        div[clazz "description"][text (s.ToString("0.000"))]
                    ]
                ]
    }
)

div [clazz "inverted ui buttons"][
    button [clazz "ui button"; onClick (fun _ -> Undo)][text "Undo"]
    button [clazz "ui button"; onClick (fun _ -> Redo)][text "Redo"]
]