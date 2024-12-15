open System
open System.Windows.Forms
open System.IO
open System.Text.Json


let options = JsonSerializerOptions()
options.Converters.Add(Types.RoleJsonConverter())
options.Converters.Add(Types.GradesMapJsonConverter())
options.WriteIndented <- true


let userDBfilePath = "UserDb.json"
let studentDBfilePath = "StudentDB.json"

let userDb = JsonSerializer.Deserialize<Types.User list>(File.ReadAllTextAsync(userDBfilePath).Result, options)

let studentDb = JsonSerializer.Deserialize<Types.Student list>(File.ReadAllTextAsync(studentDBfilePath).Result, options)

let handler = Handlers.MyFormHandler(userDb, studentDb)

[<STAThread>]
let main() =
    let form = new UI.LoginForm(handler)
    try
        form.FormClosed.Add (fun event ->    
            try
                let usersDB = JsonSerializer.Serialize(handler.GetUsers(), options)
                let userDBfilePath = "UserDb.json"

                let studentDB = JsonSerializer.Serialize(handler.GetStudents(), options)
                let studentDBfilePath = "StudentDB.json"

                File.WriteAllText(userDBfilePath, usersDB)
                File.WriteAllText(studentDBfilePath, studentDB)
            with
                | ex -> printfn "exception occured:s %s" ex.Message
        )
        Application.Run(form)
    with
        | :? InvalidOperationException -> printfn "Invalid Operation"
        | (ex:exn) -> printfn "exception occured:s %s" ex.Message
 
 
main()
