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
