module Operations

open System
open System.IO

open Domain

let tryGetAmount cmd =
    Console.Write("\nEnter amount: ")
    let amount = Console.ReadLine() |> Decimal.TryParse
    match amount with
    | true, amount -> Some(cmd, amount)
    | false, _ -> None

let writeBalance acct =
    let s = $"Current balance: $%0.00M{acct.Balance}"
    Console.WriteLine(s)

let operationDescription operation =
    match operation with
    | Withdraw -> "w"
    | Deposit -> "d"

let writeTransactionConsole transaction acct =            
    let s = $"Account %s{acct.Customer}: %s{operationDescription transaction.Action} of $%0.00M{transaction.Amount} approved(%b{transaction.Success})"
    Console.WriteLine(s)
    
    if transaction.Success then
        writeBalance acct

let writeTransactionFile transaction acct =
    let msg = $"%d{acct.AcctNo}***%s{operationDescription transaction.Action}***%0.00M{transaction.Amount}***%s{transaction.TimeStamp.Ticks.ToString()}\n"
    File.AppendAllText($"%s{acct.Customer}.txt", msg)
    
let processCommand writer (acct:Account) (operation, amount) =
    let newBalance = match operation with
                     | Withdraw -> acct.Balance - amount
                     | Deposit -> acct.Balance + amount

    if newBalance < 0.0M then
        let transaction = { Action= operation; TimeStamp= DateTime.Now; Success=false; Amount= amount}
        writer transaction acct
        acct
    else
        let updated = { acct with Balance = newBalance }
        let transaction = { Action = operation; TimeStamp= DateTime.Now; Success=true; Amount= amount}
        writer transaction updated
        updated

let accountConsole name =
    { Customer = name; Balance = 0.0M; AcctNo = Random().Next(1000, 10_000) }

let deserialize (txt:String) =
    let pieces = txt.Split("***")

    if pieces[1][0] = 'w' then Withdraw, Decimal.Parse(pieces[2])
    else Deposit, Decimal.Parse(pieces[2])
    
let nullWriter _ _ =
    ()

let processDeserialize = processCommand nullWriter

let accountFile name =
    let lines = File.ReadAllLines($"%s{name}.txt")
    let acctNo = lines[0].Substring(0, 4)
    let acct = { Customer = name; Balance = 0.0M; AcctNo = Convert.ToInt32(acctNo) }
    
    let account =
        lines |> Array.map deserialize
              |> Array.fold processDeserialize acct
              
    writeBalance account
    
    account

