open System
open System.Windows.Forms

// Create the main application

let handler = Handlers.MyFormHandler()

let main() =
    let form = new UI.LoginForm(handler)
    try
        Application.Run(form)
    with
        | :? InvalidOperationException -> printfn "Invalid Operation"
        | (ex:exn) -> printfn "exception occured:s %s" ex.Message
 
 
main()
 