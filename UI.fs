module UI

open Types
open Handlers
open System.Windows.Forms
open System.Drawing
open OxyPlot
open OxyPlot.WindowsForms
open System


type StatisticsForm(handler: MyFormHandler, classId: int) as this  = 
    inherit Form(Text = "Class " + string classId + " Statistics", Width = 600, Height = 500
                    , StartPosition=FormStartPosition.WindowsDefaultLocation)
    
    let data = handler.GetClassStats(classId)
    let model = new PlotModel(Title = "Pass/Fail Statistics For Class " + string classId)

    // Create series for Pass and Fail counts
    let passSeries = new OxyPlot.Series.BarSeries(Title = "Pass", StrokeColor = OxyColors.Black, StrokeThickness = 1.0)
    let failSeries = new OxyPlot.Series.BarSeries(Title = "Fail", StrokeColor = OxyColors.Black, StrokeThickness = 1.0)

    do    
    // Set the location to the left of the parent form
       

        // Add data to the series
        data |> List.iter (fun stat -> 
            passSeries.Items.Add(OxyPlot.Series.BarItem(stat.PassCount))
            failSeries.Items.Add(OxyPlot.Series.BarItem(stat.FailCount))
        )


        // Set the categories on the X-axis
        model.Axes.Add(new OxyPlot.Axes.CategoryAxis(
                                Position = OxyPlot.Axes.AxisPosition.Left,
                                Key = "Class",
                                ItemsSource = (data |> List.map (fun s -> s.SubjectName))))

        // Add the series to the model
        model.Series.Add(passSeries)
        model.Series.Add(failSeries)

        // Create PlotView and add it to the form
        let plotView = new PlotView(Model = model)
        plotView.Dock <- DockStyle.Fill
        base.Controls.Add(plotView)

        this.KeyPreview <- true // work if focus on a txt field for examples
        this.KeyDown.Add(fun args ->
            if args.KeyCode = Keys.Escape then
                this.Close() // Close the form
        )
        


type SummaryForm(handler: MyFormHandler, classId:int) as this = 
    inherit Form(Text = "Class " + string classId + " Summary", Width = 500, Height = 450
                    , StartPosition=FormStartPosition.WindowsDefaultLocation) 

    let lowHighGrades = handler.GetSummaryInClass classId

    let header = new Label(Text = "Class " + string classId + " Summary:", Width = 500, Height = 30)

    do

        header.Font <- new Font(header.Font.FontFamily, 15.0f, FontStyle.Bold ||| FontStyle.Italic)
        header.Top <- 20
        header.TextAlign <- ContentAlignment.TopCenter
        this.Controls.Add(header)

        if (Seq.length lowHighGrades) = 0 then 
            let empty = new Label(Text = "EMPTY!" , Width=600, Height=200)
            empty.Font <- new Font(empty.Font.FontFamily, 30.0f, FontStyle.Bold)
            empty.Top <- (this.ClientSize.Height/2) - 40
            empty.Left <- (this.ClientSize.Width/2) - 70
            this.Controls.Add(empty)
        else ignore()

        lowHighGrades |> 
        Seq.iteri(fun i (subject, max, min, avg) -> 
            let headerLabelSubject = new Label(Text = string subject + ":" , Width=600, Height=30)
            headerLabelSubject.Font <- new Font(headerLabelSubject.Font.FontFamily, 12.0f, FontStyle.Bold)
            headerLabelSubject.Top <- header.Top + (i*80) + headerLabelSubject.Height
            headerLabelSubject.Left <- 20

            let maxGrade = new Label(Text = "Maximum Grade: " + string max + "/100", Width=100, Height=30)
            maxGrade.Font <- new Font(maxGrade.Font.FontFamily, 9.0f, FontStyle.Underline)
            maxGrade.Left <- 20
            maxGrade.Top <-  headerLabelSubject.Top + headerLabelSubject.Height


            let minGrade = new Label(Text = "Minimum Grade: " + string min + "/100", Width=100, Height=30)
            minGrade.Font <- new Font(minGrade.Font.FontFamily, 9.0f, FontStyle.Underline)
            minGrade.Left <- maxGrade.Left + maxGrade.Width + 50
            minGrade.Top <-  headerLabelSubject.Top + headerLabelSubject.Height

            let avgGrade = new Label(Text = "Grades Average: " + avg.ToString("F2") + "%", Width=100, Height=30)
            avgGrade.Font <- new Font(avgGrade.Font.FontFamily, 9.0f, FontStyle.Underline)
            avgGrade.Left <- minGrade.Left + minGrade.Width + 50
            avgGrade.Top <-  headerLabelSubject.Top + headerLabelSubject.Height

            this.Controls.Add(headerLabelSubject)
            this.Controls.Add(maxGrade)
            this.Controls.Add(minGrade)
            this.Controls.Add(avgGrade)
        )

        this.KeyPreview <- true // work if focus on a txt field for examples
        this.KeyDown.Add(fun args ->
            if args.KeyCode = Keys.Escape then
                this.Close() // Close the form
        )












type StudentForm(handler: MyFormHandler, user: User) = 
    inherit Form(Text = "Welcome " + user.Username + "!", Width = 500, Height = 400
                            , StartPosition=FormStartPosition.CenterScreen)

    let student = handler.GetStudent(user.ID)

    let studentName = new Label(Text = "Student Name: " + user.Username, Top = 20, Width=500, Height=30)

    let avg = handler.CalcStudentAvg(student.Value)
    let percentage = new Label(Text = "Student Average: " + avg.ToString("F2") + " %", Width=500, Height=30)
    

    let classNumber = new Label(Text = "Class Number: " + string student.Value.ClassId, Width=500, Height=30)

    let GradesHead = new Label(Text = "Your Grades: " , Width=500, Height=30)

    let grades = Map.toList student.Value.Grades

    let button = new Button(Text = "Class Statistics", Width = 100, Height = 30)

    let button2 = new Button(Text = "Class Summary", Width = 100, Height = 30)



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




        button.Left <- ((base.ClientSize.Width - button.Width) / 2) - 90
        button.Top <- grade4.Height  + grade4.Top + 30

        // Add click event handler
        button.Click.Add(fun _ ->
            // Create and show the second form
            let secondForm = new StatisticsForm(handler, student.Value.ClassId)
            secondForm.ShowDialog() |> ignore  
        )

        base.Controls.Add(button)

        button2.Left <- ((base.ClientSize.Width - button.Width) / 2) + button.Width
        button2.Top <- button.Top
        button2.Click.Add(fun _ ->
            // Create and show the second form
            let secondForm = new SummaryForm(handler, student.Value.ClassId)
            secondForm.ShowDialog() |> ignore
        )

        base.Controls.Add(button2)




type SampleForm(title: string) =
    inherit Form()

    do
        base.Text <- title
        base.Size <- new Size(300, 200)
        base.StartPosition <- FormStartPosition.CenterScreen
        let label = new Label(Text = $"Welcome to {title}", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter)
        base.Controls.Add(label)


type ClassNumberQuestionForm(handler: MyFormHandler, formType: System.Type) as this =
    inherit Form(StartPosition=FormStartPosition.WindowsDefaultLocation) 
    
    do
        // Set the form properties
        base.Text <- "Enter Class ID"
        base.Size <- Size(300, 300)
        
        // Create the label
        let label = new Label(Text = "Enter the Class ID:", AutoSize = true)
        label.Location <- Point((base.ClientSize.Width - label.Width) / 2, 50)
        base.Controls.Add(label)

        // Create the text box
        let textBox = new TextBox()
        textBox.Width <- 200
        textBox.Location <- Point((base.ClientSize.Width - textBox.Width) / 2, label.Bottom + 10)
        base.Controls.Add(textBox)

        // Create the button
        let button = new Button(Text = "Submit", Width = 100)
        button.Location <- Point((base.ClientSize.Width - button.Width) / 2, textBox.Bottom + 20)
        button.Click.Add(fun _ ->
            try
                let classId = int(textBox.Text)
                if not(handler.IsClassAvailable classId) then (failwith "Class Not Found") else ignore()
                let objArr:Object array = [|handler ; classId;|]
                let form = Activator.CreateInstance(formType ,objArr)

                let method = formType.GetMethod("ShowDialog", Type.EmptyTypes);                

                this.Close()
                method.Invoke(form, null) |> ignore 
            
            with
                | (ex:exn) ->  MessageBox.Show($"Invalid Class Number") |> ignore
            
        )
        base.AcceptButton <- button
        base.Controls.Add(button)



type AdminForm(handler: MyFormHandler, user: User) = 
     inherit Form(Text = "Adminstration", Width = 500, Height = 400
                            , StartPosition=FormStartPosition.CenterScreen)

     let usernameLabel = new Label(Text = "Welcome Admin "  + user.Username + "!", Top = 20, Width=500, Height=30)
     
     let buttonWidth = 150
     let leftMargin, rightMargin = 30, 300
     let verticalSpacing = 20
     let buttonTitles = [ "Add Student"; "Add Admin"; "Edit User"; "View Statistics"; "Remove User";  "View Summary"]



     do
        usernameLabel.Font <- new Font(usernameLabel.Font.FontFamily, 14.0f, FontStyle.Bold)
        usernameLabel.TextAlign <- ContentAlignment.TopCenter
        base.Controls.Add(usernameLabel)

        let mutable btnList = []
        buttonTitles
        |> List.iteri (fun i title ->
            let button = new Button(Text = title, Width = buttonWidth, Height = 30)
            let isLeft = i % 2 = 0
            button.Left <- if isLeft then leftMargin else rightMargin
            button.Top <- usernameLabel.Top + 80 + (i / 2) * (button.Height + verticalSpacing)

            let btnCases = match title with 
                            | "Add Student" -> button.Click.Add(fun _ ->
                                                        let form = new SampleForm(title)
                                                        form.ShowDialog() |> ignore
                                                   )
                            | "Add Admin" ->button.Click.Add(fun _ ->
                                                        let form = new SampleForm(title)
                                                        form.ShowDialog() |> ignore
                                                  )
                            | "Edit User" -> button.Click.Add(fun _ ->
                                                            let form = new SampleForm(title)
                                                            form.ShowDialog() |> ignore
                                                   )
                            | "View Statistics" ->button.Click.Add(fun _ ->
                                                        let form = new ClassNumberQuestionForm(handler, typeof<StatisticsForm>)
                                                        form.ShowDialog() |> ignore
                                                  )
                            | "Remove User" -> button.Click.Add(fun _ ->
                                                        let form = new SampleForm(title)
                                                        form.ShowDialog() |> ignore
                                                  )
                            | "View Summary" -> button.Click.Add(fun _ ->
                                                        let form = new ClassNumberQuestionForm(handler, typeof<SummaryForm>)
                                                        form.ShowDialog() |> ignore
                                                  )

                            | _ -> ignore()


            // Add button to the main form
            btnList <- (button :> Control) :: btnList 
        )
        base.Controls.AddRange((Array.ofList btnList))




type LoginForm(handler: MyFormHandler) as this = 
    inherit Form(Text = "Login", Width = 400, Height = 300, StartPosition=FormStartPosition.CenterScreen)

    // Fields to hold user inputs
    let usernameLabel = new Label(Text = "Username:", AutoSize=true)
    let usernameTextBox = new TextBox()

    let passwordLabel = new Label(Text = "Password:", AutoSize=true)
    let passwordTextBox = new TextBox(PasswordChar = '*')

    let loginButton = new Button(Text = "Login", Width = 100)

    // Constructor to initialize the form
    do
        

        usernameLabel.Location <- Point(80, 30)
        usernameTextBox.Location <- Point(150, 30)
        usernameTextBox.Width <- 120



        passwordLabel.Location <- Point(80, 70)
        passwordTextBox.Location <- Point(150, 70)
        passwordTextBox.Width <- 120

        loginButton.Width <- 80
        loginButton.Location <- Point( 150 , 70 + 50)

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
                                            | Admin ->  let form = new AdminForm(handler, user)
                                                        try
                                                            this.Hide() 
                                                            form.ShowDialog() |> ignore 
                                                            this.Show()
                                                        with
                                                            | :? System.InvalidOperationException -> printfn "Invalid Operation"
                                                            | (ex:exn) -> printfn "exception occured:s %s" ex.Message
                                                       
                                            | Student-> let form = new StudentForm(handler, user)
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
