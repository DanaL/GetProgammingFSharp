open System
open System.IO

open Domain
open Operations

type Command = AccountCommand of Operation | Exit
let tryParseCommand c =
    match c with
    | 'w' -> Some (AccountCommand Withdraw)
    | 'd' -> Some (AccountCommand Deposit)
    | 'x' | 'q' -> Some Exit
    | _ -> None

let isExitCommand cmd = 
    match cmd with
    | Command.Exit -> true
    | _ -> false

let toOperation cmd =
    match cmd with
    | Command.Exit -> None
    | AccountCommand cmd -> Some cmd

let fetchCommand () = 
    Console.Write("(d)eposit (w)ithdraw or e(x)it: ")
    let output = Console.ReadKey().KeyChar
    Console.WriteLine()
    output

let processCmdConsole = processCommand writeTransactionConsole
let processCmdFile = processCommand writeTransactionFile

let account =
    Console.Write("What is your name? ")
    let name = Console.ReadLine()    
    let acct = if File.Exists($"%s{name}.txt") then accountFile name
                    else accountConsole name

    let commands =
        Seq.initInfinite(fun _ ->
            Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
            let output = Console.ReadKey().KeyChar
            Console.WriteLine()
            output)

    commands 
    |> Seq.choose tryParseCommand
    |> Seq.takeWhile (isExitCommand >> not)
    |> Seq.choose toOperation 
    |> Seq.choose tryGetAmount
    |> Seq.fold processCmdConsole acct

Console.WriteLine(account)
  