open System
open System.IO

type FolderSummary = { Name : string; Count : int; Total : int64; Avg : int64;
                       Exts : string array }
                       
type FileDetails = { Name : string; Size : int64; Ext : string;
                     ParentDir : string }

let fileDeets (fi:FileInfo) =
    { Name = fi.Name; Size = fi.Length; Ext = fi.Extension;
                     ParentDir = fi.Directory.Name }

let summarize (name, folder) =
    let total = folder |> Array.sumBy (fun f -> f.Size)
    let length = folder.Length
    let avg = total / int64(length)

    { Name = name; Count = length; Total = total; Avg = avg;
      Exts = folder |> Array.map (fun f -> f.Ext) |> Array.distinct }

let folder = "/Users/dana/Development/"
let files = DirectoryInfo(folder).GetDirectories()
            |> Array.map(fun di -> di.GetFiles())
            |> Array.concat
            |> Array.map fileDeets
            |> Array.groupBy(fun d -> d.ParentDir)
            |> Array.map summarize
            |> Array.sortByDescending (fun s -> s.Total)

Console.WriteLine($"%A{files}")



