open System
open System.IO

let folder = "/Users/dana/Documents/"

let files = Directory.GetFiles(folder)
                |> Seq.map (fun f -> FileInfo(f))

let length inputs =
    Seq.fold (fun count _ -> count + 1) 0 inputs

let count = length files
printfn "# of files in folder %s: %d" folder count

let biggestFile inputs =
    Seq.fold (fun (max_name, max_size) (curr:FileInfo) ->
              if curr.Length > max_size then curr.Name, curr.Length
              else max_name, max_size)
        ("", 0)
        inputs
        
let name, size = biggestFile files
Console.WriteLine($"%s{name} is the largest file at %d{size} bytes")

// A type aliases!
type Rule = string -> bool * string // function signature that takes in string
                                    // and returns a bool and a string

let rules : Rule list =
    [ fun text -> (text.Split ' ').Length = 3, "Must be three words"
      fun text -> (text.Length <= 30, "Max length is 30 characters")
      fun text -> text
                  |> Seq.filter Char.IsLetter
                  |> Seq.forall Char.IsUpper, "All letters must be uppercase"
      fun text -> text
                  |> Seq.filter Char.IsDigit
                  |> Seq.length = 0, "No digits allowed" ]
                  
let buildValidator (rules : Rule list) =
    rules
    |> List.reduce(fun firstRule secondRule ->
                   fun word ->
                       let passed, error = firstRule word
                       if passed then
                           let passed, error = secondRule word
                           if passed then true, "" else false, error
                       else false, error)

let validate = buildValidator rules
let word = "HELLO FROM1 F#"
Console.WriteLine(validate word)
