module Domain

open System

type Account = { Customer: string; Balance : Decimal; AcctNo: int }
type TransactionType = Withdrawal | Deposit
type Transaction = { Action: TransactionType; Amount: Decimal } 