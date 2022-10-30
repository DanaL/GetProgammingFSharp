module Operations

open System

open Domain

let isValidCommand c =
    if c = 'w' then true
    elif c = 'd' then true
    elif c = 'x' then true
    else false
    
let isStopCommand c = c = 'x'

let getAmount c =
    if c = 'd' then ('d', 50M)
    elif c = 'w' then ('w', 25M)
    else ('x', 0M)

let getAmountConsole c =
    Console.Write("\nEnter amount: ")
    let s = Console.ReadLine()
    let amt = Decimal.Parse(s)
    (c, amt)

let writeTransaction name c amount valid =
    let t = if c = 'w' then "withdrawal"
            else "deposit"
            
    let s = $"Account %s{name}: %s{t} of $%0.00M{amount} approved(%b{valid})"
    Console.WriteLine(s)
    
let writeBalance acct =
    let s = $"Current balance: $%0.00M{acct.Balance}"
    Console.WriteLine(s)
    
let processCommand (acct:Account) (cmd, amount) =
    let newBalance = if cmd = 'w' then acct.Balance - amount 
                     else acct.Balance + amount 
    
    if newBalance < 0.0M then
        writeTransaction acct.Customer cmd amount false
        acct
    else
        writeTransaction acct.Customer cmd amount true
        let updated = { acct with Balance = newBalance }
        writeBalance updated
        updated
        
    
   
    
