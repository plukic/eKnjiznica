﻿using eKnjiznica.Commons.ViewModels.Books;
using eKnjiznica.Commons.ViewModels.ClientBook;
using eKnjiznica.CORE.Repository;
using eKnjiznica.DAL.EF;
using eKnjiznica.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKnjiznica.DAL.Repository
{
    public class ClientBooksRepo : IClientBooksRepo
    {
        private EKnjiznicaDB context;

        public ClientBooksRepo(EKnjiznicaDB context)
        {
            this.context = context;
        }

        public void AddBooksToUser(string userId, IList<BookOfferVM> books)
        {
            var amount = books.Sum(x => x.Price);
            var accountBalance = context.UserFinancialAccounts.First(x => x.UserFinancialAccountId == userId);

            var transaction = new Transaction
            {
                Amount = amount,
                DateUtc = DateTime.UtcNow,
                PreviousAccountBalance = accountBalance.Balance,
                NewAccountBalance = accountBalance.Balance - amount,
                TransactionType = Commons.ViewModels.TransactionType.BUY,
                UserFinancialAccountId = accountBalance.UserFinancialAccountId
            };
            foreach (var item in books)
            {
                context.UserBooks.Add(new UserBook
                {
                    BookOfferId = item.Id,
                    DateOfPurchase = DateTime.UtcNow,
                    Transaction = transaction,
                    UserId = userId
                });
            }
            accountBalance.Balance -= amount;
            context.SaveChanges();
        }

        public List<ClientBookVM> GetClientBooks(string userId)
        {
            return context.UserBooks
                .Where(x => x.UserId == userId)
                 .Include(x => x.BookOffer.Book)
                .Include(x => x.BookOffer)
                .Include(x => x.User)
                 .Select(ClientVmMapper)
                 .ToList();
        }


        public List<ClientBookVM> GetClientBooks(string title, string author, string user)
        {
            return context.UserBooks
                .Include(x => x.BookOffer.Book)
                .Include(x => x.BookOffer)
                .Include(x => x.User)
                .Where(x => string.IsNullOrEmpty(title) || x.BookOffer.Book.Title.Contains(title))
                .Where(x => string.IsNullOrEmpty(author) || x.BookOffer.Book.Autor.Contains(author))
                .Where(x => string.IsNullOrEmpty(user) || x.User.UserName.Contains(user))
                .Select(ClientVmMapper).ToList();
        }
        private ClientBookVM ClientVmMapper(UserBook x)
        {
            return new ClientBookVM
            {
                AuthorName = x.BookOffer.Book.Autor,
                BookTitle = x.BookOffer.Book.Title,
                Date = x.DateOfPurchase,
                Id = x.Id,
                Price = x.BookOffer.Price,
                TransactionId = x.TransactionId,
                ClientName = x.User.UserName
            };
        }
    }
}
