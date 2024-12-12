module UI

open System.Windows.Forms
open System.Drawing
open Handlers
open Types



type AdminForm(handler: MyFormHandler, user: User) = 
     inherit Form(Text = "AdminLogin", Width = 500, Height = 400)

     let usernameLabel = new Label(Text = "Hello Admin", Top = 20, Left=20, Width=80)
     do
        base.Controls.Add(usernameLabel)



type StudentForm(handler: MyFormHandler, user: User) = 
    inherit Form(Text = "StudentLogin", Width = 500, Height = 400)

    let studentName = new Label(Text = "Student Name: " + user.Username, Top = 20, Width=500, Height=50)
    let percentage = new Label(Text = "Student Name: " + user.Username, Top = 20, Width=500, Height=50)
    
    



    // Name : centered
    // avg for student: center
    // padding then (subject: grade) 2 2
    do
        studentName.Font <- new Font(studentName.Font.FontFamily, 12.0f, FontStyle.Bold)
        studentName.TextAlign <- ContentAlignment.TopCenter
        studentName.Left <- (base.ClientSize.Width - studentName.Width) / 2
        base.Controls.Add(studentName)

        studentName.Font <- new Font(studentName.Font.FontFamily, 12.0f, FontStyle.Bold)
        studentName.TextAlign <- ContentAlignment.TopCenter
        studentName.Left <- (base.ClientSize.Width - studentName.Width) / 2
        base.Controls.Add(studentName)



type LoginForm(handler: MyFormHandler) as this = 
    inherit Form(Text = "Login", Width = 500, Height = 400)

    // Fields to hold user inputs
    let usernameLabel = new Label(Text = "Username:", Top = 20, Left=20, Width=80)
    let usernameTextBox = new TextBox(Top = 20, Left = 110, Width = 150)

    let passwordLabel = new Label(Text = "Password:", Top = 60, Left = 20, Width = 80)
    let passwordTextBox = new TextBox(Top = 60, Left = 110, Width = 150, PasswordChar = '*')

    let loginButton = new Button(Text = "Login", Top = 100, Left = 110, Width = 80)

    // Constructor to initialize the form
    do
        base.Controls.Add(usernameLabel)
       
        base.Controls.Add(usernameTextBox)

        base.Controls.Add(passwordLabel)

        base.Controls.Add(passwordTextBox)

        base.Controls.Add(loginButton)

        // Button Click Event Handler
        loginButton.Click.Add(fun _ -> 
            let username = usernameTextBox.Text
            let password = passwordTextBox.Text
            match handler.Login(username, password) with
                        | Some user -> match user.Role with
                                            | Admin ->  let form = new AdminForm(this.FormHandlers, user)
                                                        try
                                                            this.Hide() 
                                                            form.ShowDialog() |> ignore 
                                                            this.Show()
                                                        with
                                                            | :? System.InvalidOperationException -> printfn "Invalid Operation"
                                                            | (ex:exn) -> printfn "exception occured:s %s" ex.Message
                                                       
                                            | Student-> let form = new StudentForm(this.FormHandlers, user)
                                                        try
                                                            this.Hide() 
                                                            form.ShowDialog() |> ignore 
                                                            this.Show()
                                                        with
                                                            | :? System.InvalidOperationException -> printfn "Invalid Operation"
                                                            | (ex:exn) -> printfn "exception occured:s %s" ex.Message
                                            
                        | _ ->  MessageBox.Show($"Invalid Username or Password!") |> ignore

        )

        base.AcceptButton <- loginButton

    // Public methods to access the input values
    member this.Username = usernameTextBox.Text
    member this.Password = passwordTextBox.Text
    member this.FormHandlers = handler

