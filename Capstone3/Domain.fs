module Domain

open System

type Account = { Customer: string; Balance : Decimal; AcctNo: int }
type Transaction = { Action: char; TimeStamp: DateTime; Success: bool; Amount: decimal } 