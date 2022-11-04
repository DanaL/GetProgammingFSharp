open System

type StudentNumber = StudentNumber of string
type Email = Email of string
type Grade = Grade of string
type SchoolDetails =
    | Elementary of string
    | HighSchool of string
    
type Student = { StudentNumber : StudentNumber
                 Email : Email
                 Grade : Grade
                 School : SchoolDetails
                 AltSchool : SchoolDetails option }
type EarlyYearsStudent = EarlyYearsStudent of Student

let createStudent studentNumber email grade school alt =
    { StudentNumber = studentNumber
      Email = email
      Grade = grade
      School = school 
      AltSchool = alt }

let isEarlyYears grade =
    let (Grade grade) = grade
    match grade with
        | "KG" -> true
        | "01" -> true
        | "02" -> true
        | _ -> false
        
let earlyYears (student:Student) =
    let main =
        match student.School with
        | Elementary _ when isEarlyYears student.Grade -> true  
        | _ -> false

    if main then Some(student)
    else
        match student.AltSchool with
            | Some alt -> match alt with
                          | Elementary _ -> Some(student)
                          | _ -> None
            | _ -> None
        
    //    let (SchoolDetails alt) = student.AltSchool
    //    None
        (*
        match student.AltSchool with
        | Some Elementary when isEarlyYears student.Grade -> Some(student)
        | _ -> None*)
    
let alt = Some (HighSchool "atc")
let student = createStudent (StudentNumber "001234")
                            (Email "stu@lrsd.net")
                            (Grade "01")
                            (Elementary "shamrock")
                            alt

let alt2 = Some (Elementary "archwood")                                                        
let student2 = createStudent (StudentNumber "002222")
                             (Email "foo@lrsd.net")
                             (Grade "11")
                             (HighSchool "cjs")
                             alt2
//Console.WriteLine(student)
//Console.WriteLine(student2)
Console.WriteLine(earlyYears student2)
//Console.WriteLine(earlyYears student)

      
