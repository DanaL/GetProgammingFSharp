open System

type Disk =
     | HardDisk of RPM:int * Platters: int
     | SolidState
     | MMC of NumberOfPins:int

let seek disk =
    match disk with
    | HardDisk _ -> "Seeking loudly at a reasonable speed!"
    | MMC _ -> "Seeking quietly but slowly"
    | SolidState -> "Already found it!"

let describe disk =
    match disk with
    | SolidState -> "I am a newfangled SSD."
    | MMC 1 -> "I have only one pin."
    | MMC pins when pins <= 5 -> "I have a few pins."
    | MMC pins -> $"I am an MMC with %d{pins} pins."
    | HardDisk(5400, _) -> "I am a slow hard disk."
    | HardDisk(_, 7) -> "I have seven spindles!"
    | HardDisk _ -> "I'm a hard disk."
    


let mySsd = SolidState
Console.WriteLine(describe mySsd)

let mmc = MMC 5
Console.WriteLine(describe mmc)
let mmc2 = MMC 1
Console.WriteLine(describe mmc2)
let mmc3 = MMC 13
Console.WriteLine(describe mmc3)

let hd = HardDisk(RPM=7200, Platters=4)
Console.WriteLine(describe hd)
let hd2 = HardDisk(RPM=5400, Platters=5)
Console.WriteLine(describe hd2)
let hd3 = HardDisk(RPM=7200, Platters=7)
Console.WriteLine(describe hd3)
Console.WriteLine("\n")

// Modified version of exercise from Lesson 18 that for result
// returns a Pass/Fail discriminated union
type Result =
    | Pass
    | Fail of Err:string
    
type Rule = string -> Result 

let rule0 (text:string) =
    if (text.Split ' ').Length = 3 then Pass
    else Fail "Must be three words"

let rule1 text =
    let b = text |> Seq.filter Char.IsLetter
                 |> Seq.forall Char.IsUpper

    if b then Pass
    else Fail "All letters must be uppercase"

let rule2 (text:string) =
    if text.Length <= 30 then Pass
    else Fail "Max length is 30 characters"

let rule3 (text:String) =
    let len = text |> Seq.filter Char.IsDigit
                   |> Seq.length
    if len = 0 then Pass
    else Fail "No digits allowed"

let rules : Rule list = [ rule0; rule1; rule2; rule3 ]
                  
let buildValidator (rules : Rule list) =
    rules
    |> List.reduce(fun firstRule secondRule ->
                   fun word ->
                       let result = firstRule word
                       match result with
                       | Pass -> secondRule word
                       | Fail _ -> result)
                   
let validate = buildValidator rules
let word = "HELLOO FROM F#!!"
Console.WriteLine(validate word)



