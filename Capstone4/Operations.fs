module Operations

open System
open System.IO

open Domain

let isValidCommand c =
    if c = 'w' then true
    elif c = 'd' then true
    elif c = 'x' then true
    else false
    
let isStopCommand c = c = 'x'

let getAmountConsole c =
    Console.Write("\nEnter amount: ")
    let s = Console.ReadLine()
    let amt = Decimal.Parse(s)
    (c, amt)

let writeBalance acct =
    let s = $"Current balance: $%0.00M{acct.Balance}"
    Console.WriteLine(s)
        
let writeTransactionConsole transaction acct =
    let t = if transaction.Action = 'w' then "withdrawal"
            else "deposit"
            
    let s = $"Account %s{acct.Customer}: %s{t} of $%0.00M{transaction.Amount} approved(%b{transaction.Success})"
    Console.WriteLine(s)
    
    if transaction.Success then
        writeBalance acct

let writeTransactionFile transaction acct =
    let msg = $"%d{acct.AcctNo}***%c{transaction.Action}***%0.00M{transaction.Amount}***%s{transaction.TimeStamp.Ticks.ToString()}\n"
    File.AppendAllText($"%s{acct.Customer}.txt", msg)
    
let processCommand writer (acct:Account) (cmd, amount) =
    let newBalance = if cmd = 'w' then acct.Balance - amount 
                     else acct.Balance + amount         
    if newBalance < 0.0M then
        let transaction = { Action= cmd; TimeStamp= DateTime.Now; Success=false; Amount= amount}
        writer transaction acct
        acct
    else
        let updated = { acct with Balance = newBalance }
        let transaction = { Action= cmd; TimeStamp= DateTime.Now; Success=true; Amount= amount}
        writer transaction updated
        updated

let accountConsole name =
    { Customer = name; Balance = 0.0M; AcctNo = Random().Next(1000, 10_000) }

let deserialize (txt:String) =
    let pieces = txt.Split("***")
    pieces[1][0], Decimal.Parse(pieces[2])

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

