module UI

open System.Windows.Forms
open System.Drawing
open Handlers
open Types



type StatisticsForm(handler: MyFormHandler, id: int) = 
    inherit Form(Text = "Class " + string id + " Statistics", Width = 500, Height = 400)




type StudentForm(handler: MyFormHandler, user: User) = 
    inherit Form(Text = "Welcome " + user.Username + "!", Width = 500, Height = 400)

    let student = handler.GetStudent(user.ID)

    let studentName = new Label(Text = "Student Name: " + user.Username, Top = 20, Width=500, Height=30)

    let avg = handler.CalcStudentAvg(student.Value)
    let percentage = new Label(Text = "Student Average: " + avg.ToString("F2") + " %", Width=500, Height=30)
    

    let classNumber = new Label(Text = "Class Number: " + string student.Value.ClassId, Width=500, Height=30)

    let GradesHead = new Label(Text = "Your Grades: " , Width=500, Height=30)

    let grades = Map.toList student.Value.Grades

    let button = new Button(Text = "Class Statistics", Width = 100, Height = 30)



    // Name : centered
    // avg for student: center
    // padding then (subject: grade) 2 2
    do
        studentName.Font <- new Font(studentName.Font.FontFamily, 14.0f, FontStyle.Bold)
        studentName.TextAlign <- ContentAlignment.TopCenter
        studentName.Left <- (base.ClientSize.Width - studentName.Width) / 2
        base.Controls.Add(studentName)

        percentage.Font <- new Font(studentName.Font.FontFamily, 12.0f, FontStyle.Bold)
        percentage.TextAlign <- ContentAlignment.TopCenter
        percentage.Left <- (base.ClientSize.Width - studentName.Width) / 2
        percentage.Top <- studentName.Top + studentName.Height + 10
        base.Controls.Add(percentage)
        
        classNumber.Font <- new Font(studentName.Font.FontFamily, 10.0f, FontStyle.Bold)
        classNumber.Left <- 20
        classNumber.Top <- percentage.Top + percentage.Height + 10
        base.Controls.Add(classNumber)


        GradesHead.Font <- new Font(GradesHead.Font.FontFamily, 14.0f, FontStyle.Bold)
        GradesHead.Left <- 20
        GradesHead.Top <- classNumber.Top + classNumber.Height + 10

        base.Controls.Add(GradesHead)

        let grade1 = string(fst grades.[0]) + ": " + string(snd grades.[0]) + "/" + string(handler.FinalGrade)
        let grade2 = string(fst grades.[1]) + ": " + string(snd grades.[1]) + "/" + string(handler.FinalGrade)
        let grade3 = string(fst grades.[2]) + ": " + string(snd grades.[2]) + "/" + string(handler.FinalGrade)
        let grade4 = string(fst grades.[3]) + ": " + string(snd grades.[3]) + "/" + string(handler.FinalGrade)

        let grade1 = new Label(Text = grade1, Width = 200, Height = 30)
        let grade2 = new Label(Text = grade2, Width = 200, Height = 30)
        let grade3 = new Label(Text = grade3, Width = 200, Height = 30)
        let grade4 = new Label(Text = grade4, Width = 200, Height = 30)

        grade1.Left <- 20 
        grade1.Top <- GradesHead.Top + GradesHead.Height + 10 
        base.Controls.Add(grade1)

        grade2.Left <- base.ClientSize.Width - grade1.Width + 100
        grade2.Top <- GradesHead.Top + GradesHead.Height + 10 
        base.Controls.Add(grade2)

        grade3.Left <- 20 
        grade3.Top <- grade1.Top + grade1.Height + 10 
        base.Controls.Add(grade3)

        grade4.Left <- base.ClientSize.Width - grade3.Width + 100
        grade4.Top <- grade2.Top + grade2.Height + 10 
        base.Controls.Add(grade4)



        // Create a button

        // Center the button
        button.Left <- (base.ClientSize.Width - button.Width) / 2
        button.Top <- grade4.Height  + grade4.Top

        // Add click event handler
        button.Click.Add(fun _ ->
            // Create and show the second form
            let secondForm = new StatisticsForm(handler, 1)
            secondForm.ShowDialog() |> ignore  
        )

        base.Controls.Add(button)








type AdminForm(handler: MyFormHandler, user: User) = 
     inherit Form(Text = "AdminLogin", Width = 500, Height = 400)

     let usernameLabel = new Label(Text = "Hello Admin", Top = 20, Left=20, Width=80)
     do
        base.Controls.Add(usernameLabel)


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

