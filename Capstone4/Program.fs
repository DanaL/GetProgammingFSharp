open System
open System.IO

open Operations

let processCmdConsole = processCommand writeTransactionConsole
let processCmdFile = processCommand writeTransactionFile

let account =
    Console.Write("What is your name? ")
    let name = Console.ReadLine()
    
    let acct = if File.Exists($"%s{name}.txt") then accountFile name
                 else accountConsole name
    
    let cmds = seq {
        while true do
        Console.Write("(d)eposit (w)ithdraw or e(x)it: ")
        yield Console.ReadKey().KeyChar
        Console.WriteLine("")
    }

    cmds
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (isStopCommand >> not)
    |> Seq.map getAmountConsole
    |> Seq.fold processCmdFile acct
    
Console.WriteLine("")
Console.WriteLine(account)