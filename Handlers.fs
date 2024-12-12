module Handlers

open System.Windows.Forms
open Types

type MyFormHandler() = 

    let mutable users: Types.User list = [
        {  ID = 1; Username = "John123" ; Password = "123";  Role = Types.UserRole.Admin}
        {  ID = 2; Username = "aly123" ;  Password = "123" ; Role = Types.UserRole.Student}
        {  ID = 3; Username = "Seif430" ; Password = "123" ; Role = Types.UserRole.Student}
        {  ID = 4; Username = "Saif437" ; Password = "123" ; Role = Types.UserRole.Student}
        {  ID = 5; Username = "Sherif555";Password = "123";  Role = Types.UserRole.Student}
        {  ID = 6; Username = "Salma147" ;Password = "123" ; Role = Types.UserRole.Student}
    ]




    let mutable students: Types.Student list =[
        {
            User = users.[1] ; 
            Grades = Map.empty<Types.Subject, int>
                        .Add(Types.Subject.Math, 60)
                        .Add(Types.Subject.Arabic, 85)
                        .Add(Types.Subject.English, 95)
                        .Add(Types.Subject.Science, 75);
            ClassId = 1

        }
        {
            User = users.[2] ; 
            Grades = Map.empty<Types.Subject, int>
                        .Add(Types.Subject.Math, 40)
                        .Add(Types.Subject.Arabic, 70)
                        .Add(Types.Subject.English, 88)
                        .Add(Types.Subject.Science, 65);

            ClassId = 1
        }
        {
            User = users.[3] ; 
            Grades =Map.empty<Types.Subject, int>
                        .Add(Types.Subject.Math, 50)
                        .Add(Types.Subject.Arabic, 78)
                        .Add(Types.Subject.English, 92)
                        .Add(Types.Subject.Science, 68);
            ClassId = 1

        }
        {
            User = users.[4] ; 
            Grades = Map.empty<Types.Subject, int>
                        .Add(Types.Subject.Math, 72)
                        .Add(Types.Subject.Arabic, 82)
                        .Add(Types.Subject.English, 89)
                        .Add(Types.Subject.Science, 74);

            ClassId = 2
        }
        {
            User = users.[5] ; 
            Grades = Map.empty<Types.Subject, int>
                        .Add(Types.Subject.Math, 60)
                        .Add(Types.Subject.Arabic, 50)
                        .Add(Types.Subject.English, 68)
                        .Add(Types.Subject.Science, 89)
            ClassId = 2
        }
    ] 
    

    let availableClasses = [1; 2; 3]

    member this.isAdmin (user: User) =
        match user.Role with
        | Admin -> true
        | Student -> false

    member this.Login(username, password) = 
             match List.tryFind (fun x -> x.Username = username) users with
                | Some user when user.Password = password -> Some user
                | _ -> None

    member this.GetStudent(id:int) = 
        match List.tryFind (fun x -> x.User.ID = id) students with
                | Some user -> Some user
                | _ -> None

    member this.CalcStudentAvg(student: Student) = 
        let average =
            student.Grades
            |> Map.toSeq           
            |> Seq.map snd
            |> Seq.averageBy float
        average

    member this.GetSubjectName(subject: Subject) =
        match subject with
        | Math -> "Math"
        | Science -> "Science"
        | English -> "English"
        | Arabic -> "Arabic"

    member this.ToString(var) = 
        string(var)