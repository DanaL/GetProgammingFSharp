module Operations

open System

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
    
let writeTransactionConsole transaction acct =
    let t = if transaction.Action = 'w' then "withdrawal"
            else "deposit"
            
    let s = $"Account %s{acct.Customer}: %s{t} of $%0.00M{transaction.Amount} approved(%b{transaction.Success})"
    Console.WriteLine(s)
    
    if transaction.Success then
        let s = $"Current balance: $%0.00M{acct.Balance}"
        Console.WriteLine(s)
    
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
        
    
   
    
