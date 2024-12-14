module Handlers

open System.Windows.Forms
open Types

type MyFormHandler() = 

    let mutable users: Types.User list = [
        {  ID = 1; Username = "abdo123" ; Password = "123";  Role = Types.UserRole.Admin}
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

    let NextAvailableId() = List.tryLast (List.map (fun x-> x.ID) users)

    member this.UserNameExist (username:string) : bool = List.contains username (List.map<User,string> (fun x-> x.Username) users)

    member this.FinalGrade = 100

    member this.IsClassAvailable (classId: int) = 
        List.contains classId availableClasses


    member this.isAdmin (user: User) =
        match user.Role with
        | Admin -> true
        | Student -> false

    member this.Login(username, password) = 
             match List.tryFind<User> (fun x -> x.Username = username) users with
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



    member this.GetStudentsInClass(classId: int) = 
         List.filter (fun (x:Student) -> x.ClassId = classId) students

    member this.GetStudent() = 
         students

    member this.GetStudentsPassMap(students: Student list) =
         let listFinal = []
         let mutable passMap = Map.ofList [(Subject.Arabic, 0) ; (Subject.Math, 0); (Subject.English, 0); (Subject.Science, 0)]
         List.iter<Student> (fun student -> 
            (Map.toSeq student.Grades) |>
            Seq.iter (fun grade ->
                        match grade with
                        | (Arabic, value)  -> if value >= 50 then  passMap <- passMap.Add(Arabic, (snd (passMap.TryGetValue(Arabic)) + 1))
                        | (Math, value)    -> if value >= 50 then  passMap <- passMap.Add(Math, (snd (passMap.TryGetValue(Math)) + 1))
                        | (English, value) -> if value >= 50 then  passMap <- passMap.Add(English, (snd (passMap.TryGetValue(English)) + 1))
                        | (Science, value) -> if value >= 50 then  passMap <- passMap.Add(Science, (snd (passMap.TryGetValue(Science)) + 1))
                      )
            ) students
         passMap

    member this.CreateSubjectStats(passMap : Map<Subject,int>, totalClassStudents: int) = 
        let mutable list = []
        let seqPass = Map.toSeq passMap
        seqPass |> Seq.iter(fun subjectPass ->
            let subjStat =  { 
                PassCount = (snd subjectPass);
                FailCount = totalClassStudents - (snd subjectPass) ;
                SubjectName = fst subjectPass
            }
            list <- subjStat :: list
        )
        list

    member this.GetClassStats(classId:int) = 
        let studentsInClass = this.GetStudentsInClass(classId)
        let studentsPassMap = this.GetStudentsPassMap(studentsInClass)
        this.CreateSubjectStats(studentsPassMap, studentsInClass.Length)

    member this.GetSummaryInClass (classId:int) = 
            let students = this.GetStudentsInClass classId
            let mutable gradesSeq = Seq.empty<(Subject*int)>
            let mutable max_low_sub = Map.empty<Subject,(int*int)> 
            students |>
            List.iter<Student> (fun student -> 
               let studentGrades = (Map.toSeq student.Grades)  
               gradesSeq <- Seq.append gradesSeq studentGrades 
            )
            let subjectStats = 
                gradesSeq
                |> Seq.groupBy fst  // Group by Subject
                |> Seq.map (fun (subject, values) -> 
                    let grades = values |> Seq.map snd |> Seq.toList // Get all the grades for the subject
                    let maxGrade = grades |> List.max // Get max grade for this subject
                    let minGrade = grades |> List.min // Get min grade for this subject
                    let avg = grades |> List.map (fun x -> float x) |> List.average 
                    (subject, maxGrade, minGrade, avg)) // Return a tuple with Subject, Max, and Min grades
            subjectStats

    
    member this.IsNullOrEmptyString(str:string) : bool = match str with
                                                            | "" -> true
                                                            | null -> true
                                                            | _ -> false

    member this.CreateAdmin (username:string) (password:string) : bool = 
        if this.UserNameExist username || this.IsNullOrEmptyString username || this.IsNullOrEmptyString password  then false 
        else 
            let newAdminId = NextAvailableId()
            match newAdminId with
                | None -> false
                | Some id -> 
                            let newAdmin = {ID = id + 1 ; Username = username ; Password = password ; Role = Admin }
                            users <-  users @ [newAdmin]
                            true

    member this.CreateStudent(studentDTO: StudentFormDTO) = 
        let newId  = NextAvailableId()
        match newId with
                | None -> false
                | Some id -> 
                            let user = {ID = id + 1 ; Username = studentDTO.Username; Password= studentDTO.Password; Role = Student} 
                            users <- users @ [user]
                            let student = {User = user ; ClassId = studentDTO.ClassId; Grades=studentDTO.Grades}
                            students <- students @ [student]
                            true
        
        
                            
                        
            
(*type User = {
    ID: int
    Username: string
    Password: string
    Role: UserRole
}
*)