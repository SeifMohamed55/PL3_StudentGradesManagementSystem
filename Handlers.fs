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
            Grades = Map.empty<Types.ClassTypes, int>
                        .Add(Types.ClassTypes.Math, 60)
                        .Add(Types.ClassTypes.Arabic, 85)
                        .Add(Types.ClassTypes.English, 95)
                        .Add(Types.ClassTypes.Science, 75);
            ClassId = 1

        }
        {
            User = users.[2] ; 
            Grades = Map.empty<Types.ClassTypes, int>
                        .Add(Types.ClassTypes.Math, 40)
                        .Add(Types.ClassTypes.Arabic, 70)
                        .Add(Types.ClassTypes.English, 88)
                        .Add(Types.ClassTypes.Science, 65);

            ClassId = 1
        }
        {
            User = users.[3] ; 
            Grades =Map.empty<Types.ClassTypes, int>
                        .Add(Types.ClassTypes.Math, 50)
                        .Add(Types.ClassTypes.Arabic, 78)
                        .Add(Types.ClassTypes.English, 92)
                        .Add(Types.ClassTypes.Science, 68);
            ClassId = 1

        }
        {
            User = users.[4] ; 
            Grades = Map.empty<Types.ClassTypes, int>
                        .Add(Types.ClassTypes.Math, 72)
                        .Add(Types.ClassTypes.Arabic, 82)
                        .Add(Types.ClassTypes.English, 89)
                        .Add(Types.ClassTypes.Science, 74);

            ClassId = 2
        }
        {
            User = users.[5] ; 
            Grades = Map.empty<Types.ClassTypes, int>
                        .Add(Types.ClassTypes.Math, 60)
                        .Add(Types.ClassTypes.Arabic, 50)
                        .Add(Types.ClassTypes.English, 68)
                        .Add(Types.ClassTypes.Science, 89)
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

