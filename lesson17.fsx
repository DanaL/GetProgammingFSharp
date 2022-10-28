open System
open System.Collections.Generic
open System.IO

let dateToAge (date:DateTime) =
    (DateTime.Now - date).Days
    
// Look-up directory info for root/C:\ and convert to Map of Folder name -> Age in days
let folder = @"C:\"
let folders = Directory.EnumerateDirectories(folder)
            |> Seq.map DirectoryInfo
            |> Seq.map (fun di -> di.Name, di.CreationTime |> dateToAge)
            |> Map.ofSeq

let yearOld, younger = folders |> Map.partition( fun _ age -> age > 365)

let format (kvp:KeyValuePair<string,int>) =
    $"   Folder %s{kvp.Key} (%d{kvp.Value} days old)"

let writeOut (dirs: seq<KeyValuePair<string,int>>) =
    dirs |> Seq.sortByDescending(fun kvp -> kvp.Value)
         |> Seq.map format
         |> Seq.iter Console.WriteLine

Console.WriteLine("Folders more than 1 year old:")
yearOld |> writeOut

Console.WriteLine("Folders 1 year old or younger:")
younger |> writeOut
