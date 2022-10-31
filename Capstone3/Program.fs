open System

open Domain
open Operations

let processCmdConsole = processCommand writeTransactionConsole
let processCmdFile = processCommand writeTransactionFile

let account =
    Console.Write("What is your name? ")
    let name = Console.ReadLine()
    let acct = { Customer = name; Balance = 0.0M; AcctNo = Random().Next(1000, 10_000) }
    
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