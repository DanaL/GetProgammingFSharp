module Domain

open System

type Operation = Withdraw | Deposit
type Account = { Customer: string; Balance : Decimal; AcctNo: int }
type Transaction = { Action: Operation; TimeStamp: DateTime; Success: bool; Amount: decimal } 