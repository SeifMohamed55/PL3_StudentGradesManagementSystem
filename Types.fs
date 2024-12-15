module Types

open System.Text.Json.Serialization
open System.Text.Json



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


type RoleJsonConverter() =
    inherit JsonConverter<UserRole>()
    
    override this.Read(reader: byref<Utf8JsonReader>, typeToConvert: System.Type, options: JsonSerializerOptions) =
        match reader.GetString() with
        | "Admin"  ->     UserRole.Admin
        | "Student"    -> UserRole.Student
        | _ -> raise (JsonException("Invalid Subject value"))

    override this.Write(writer: Utf8JsonWriter, value: UserRole, options: JsonSerializerOptions) =
        match value with
        | UserRole.Admin -> writer.WriteStringValue("Admin")
        | UserRole.Student -> writer.WriteStringValue("Student")





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


type GradesMapJsonConverter() =
    inherit JsonConverter<Map<Subject, int>>()
    
    override this.Read(reader: byref<Utf8JsonReader>, typeToConvert: System.Type, options: JsonSerializerOptions) =
        if reader.TokenType <> JsonTokenType.StartObject then
            raise (JsonException("Expected StartObject"))
        
        let mutable map = Map.empty
        
        while reader.Read() && reader.TokenType <> JsonTokenType.EndObject do
            if reader.TokenType = JsonTokenType.PropertyName then
                let subjectStr = reader.GetString() // subject name
                reader.Read() |> ignore 
                let value = reader.GetInt32() // grade value
                
                let subject = 
                    match subjectStr with
                    | "Arabic" ->  Subject.Arabic
                    | "Math" ->    Subject.Math
                    | "Science" -> Subject.Science
                    | "English" -> Subject.English
                    | _ -> raise (JsonException("Invalid Subject key"))
                
                map <- map.Add(subject, value)
        
        map

    override this.Write(writer: Utf8JsonWriter, value: Map<Subject, int>, options: JsonSerializerOptions) =
        writer.WriteStartObject()
        value |> Map.iter (fun subject grade ->
            let subjectStr = 
                match subject with
                | Subject.Arabic -> "Arabic"
                | Subject.Math -> "Math"
                | Subject.Science -> "Science"
                | Subject.English -> "English"
            writer.WriteNumber(subjectStr, grade)
        )
        writer.WriteEndObject()




type SubjectStats = {
    SubjectName: Subject
    PassCount: int
    FailCount: int
}


// in admin list students table
type StudentFormDTO = {
    Username: string
    Password: string
    ClassId: int
    Grades: Map<Subject, int>
}

