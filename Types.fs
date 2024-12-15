module Types

type UserRole =
        | Admin
        | Student

// Define the User record
type User = {
    ID: int
    Username: string
    Password: string
    Role: UserRole
}


type Subject = 
        | Math 
        | Science
        | English
        | Arabic       


// Define the Student record with ID, name, and grades
type Student = {
    User: User
    ClassId: int
    Grades: Map<Subject, int>
}

type SubjectStats = {
    SubjectName: Subject
    PassCount: int
    FailCount: int
}