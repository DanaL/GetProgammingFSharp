open System

type Customer =  { Name:string; SafetyScore:int; YearPassed: int option }

let customers = [ { Name = "Dana"; SafetyScore = 46; YearPassed = Some 2012 };
                  { Name = "Kylie"; SafetyScore = 0; YearPassed = None } ]

let calculatePremium customer =
    match customer.YearPassed with
    | Some _ -> $"Premium for %s{customer.Name}: %d{customer.SafetyScore * 10}"
    | None -> $"Unable to offer insurance to %s{customer.Name}."

customers |> List.map calculatePremium |> List.map Console.WriteLine

let tryLoadCustomer id =
    if id >= 2 && id <= 7 then Some $"Customer %d{id}"
    else None

[0 .. 10]
    |> List.choose tryLoadCustomer
    |> List.map Console.WriteLine

// Modified version of exercise from Lesson 21 using 
// Option type instead of my Result discriminated union
// Return value of None indicates string passed all
// the rules
type Rule = string -> string option

let rule0 (text:string) =
    if (text.Split ' ').Length = 3 then None
    else Some "Must be three words"

let rule1 text =
    let b = text |> Seq.filter Char.IsLetter
                 |> Seq.forall Char.IsUpper

    if b then None
    else Some "All letters must be uppercase"

let rule2 (text:string) =
    if text.Length <= 30 then None
    else Some "Max length is 30 characters"

let rule3 (text:String) =
    let len = text |> Seq.filter Char.IsDigit
                   |> Seq.length
    if len = 0 then None
    else Some "No digits allowed"

let rules : Rule list = [ rule0; rule1; rule2; rule3 ]
                  
let buildValidator (rules : Rule list) =
    rules
    |> List.reduce(fun firstRule secondRule ->
                   fun word ->
                       let result = firstRule word
                       match result with
                       | None -> secondRule word
                       | Some err -> Some err)
                   
let validate = buildValidator rules
let result = validate "HELLO FROM F#!!"

let msg = match result with
          | None -> "The string is fine."
          | Some err -> err
    
Console.WriteLine(msg)

