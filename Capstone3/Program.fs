open System

open Domain
open Operations

let acct = { Customer = "Dana"; Balance = 0.0M; AcctNo = 5555 }

let processCmdConsole = processCommand writeTransactionConsole

let account =
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
    |> Seq.fold processCmdConsole acct
    
Console.WriteLine("")
Console.WriteLine(account)