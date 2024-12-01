open System
open System.Windows.Forms

// Create the main application
let createForm() =
    let form = new Form()
    form.Text <- "My First F# WinForms App"
    form.ClientSize <- System.Drawing.Size(300, 200)

    let label = new Label()
    label.Text <- "Hello from F#"
    label.Location <- System.Drawing.Point(100, 80)
    form.Controls.Add(label)
    form

let main() =
    let form = createForm()
    try
        Application.Run(form)
    with
        | :? InvalidOperationException -> printfn "Invalid Operation"
        | (ex:exn) -> printfn "exception occured:s %s" ex.Message
 
 
main()
