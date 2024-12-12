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


type ClassTypes = 
        | Math
        | Science
        | English
        | Arabic       


// Define the Student record with ID, name, and grades
type Student = {
    User: User
    ClassId: int
    Grades: Map<ClassTypes, int>
}

