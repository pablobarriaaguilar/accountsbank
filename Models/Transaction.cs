#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace bankaccounts.Models;

public class Transaction{
[Key]
public int TransactionId { get; set; }
[Range(1,int.MaxValue)]
public decimal Amount { get; set; }

public DateTime CreatedAt {get;set;} = DateTime.Now;   
public DateTime UpdatedAt {get;set;} = DateTime.Now;

public int UserId { get; set; }
public User? Owner { get; set; }

}


