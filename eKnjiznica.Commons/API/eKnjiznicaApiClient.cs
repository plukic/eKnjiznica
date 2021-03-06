﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using eKnjiznica.Commons;
using eKnjiznica.Commons.ViewModels;
using eKnjiznica.Commons.ViewModels.Auctions;
using eKnjiznica.Commons.ViewModels.Books;
using eKnjiznica.Commons.ViewModels.Category;
using eKnjiznica.Commons.ViewModels.Clients;
using Newtonsoft.Json;

namespace eKnjiznica.Commons.API
{
    public class EKnjiznicaApiClient : IApiClient
    {
        private HttpClient httpClient;

        public EKnjiznicaApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        public Task<HttpResponseMessage> RateBook(int bookId, int rating)
        {
            return Post(rating,$"api/books/{bookId}/ratings");
        }

        public Task<HttpResponseMessage> GetRecommendedBooks()
        {
            return Get("api/books/recommended");
        }


        public Task<HttpResponseMessage> BuyBook(IList<BookOfferVM> books)
        {

            return Post(books, "api/clients/books/buy");
        }

        public Task<HttpResponseMessage> ResendBookToEmail(int bookId)
        {
            return Get($"api/clients/books/{bookId}/resend");
        }


        public Task<HttpResponseMessage> UpdateAuction(AuctionUpdateVM auctionUpdateVM, int id)
        {
            return Put(auctionUpdateVM, $"api/auctions/{id}");
        }

        public Task<HttpResponseMessage> CreateAuction(AuctionCreateVM auctionCreateVM)
        {
            return Post(auctionCreateVM, $"api/auctions");
        }


        public Task<HttpResponseMessage> GetAuctions(DateTime dateFrom, DateTime dateTo, bool includeInactive)
        {
            return Get($"api/auctions?dateFrom={dateFrom.ToString()}&dateTo={dateTo.ToString()}&inactive={includeInactive}");
        }
        public Task<HttpResponseMessage> GetAuctionDetail(int auctionId)
        {
            return Get($"api/auctions/{auctionId}");
        }

        public Task<HttpResponseMessage> GetTopSellingBooks()
        {
            return Get($"api/books/topselling");
        }


        public Task<HttpResponseMessage> GetPurchaces(string title, string author, string user)
        {
            //   [FromUri(Name = "title")] string title = null,
            //   [FromUri(Name = "author")] string author = null,
            //   [FromUri(Name = "user")] string user = null
            return Get($"api/clients/books/all?title={title}&author={author}&user={user}");
        }

        public Task<HttpResponseMessage> GetClientBooks(string clientId)
        {
            return Get($"api/clients/books/{clientId}");
        }
        public Task<HttpResponseMessage> GetClientBooks()
        {
            return Get($"api/clients/books/my");
        }


        public Task<HttpResponseMessage> GetMyTransactions()
        {
            return Get($"api/transactions/my");

        }

        public Task<HttpResponseMessage> GetTransaction(int transactionId)
        {
            return Get($"api/transactions/{transactionId}");

        }


        public Task<HttpResponseMessage> GetTransactions(string username, string adminUsername)
        {
            return Get($"api/transactions?clientUsername={username}&adminUsername={adminUsername}");
        }


        public Task<HttpResponseMessage> MakePayInRequest(decimal value, string id)
        {

            return Post(value, $"api/transactions/client/{id}");
        }


        public Task<HttpResponseMessage> UpdateClientAccount(ClientUpdateVM clientUpdateVM, string id)
        {
            return Put(clientUpdateVM, $"api/clients/{id}");

        }
        public Task<HttpResponseMessage> UpdateClientAccount(ClientUpdateVM clientUpdateVM)
        {
            return Put(clientUpdateVM, $"api/clients/profile");
        }

        public Task<HttpResponseMessage> CreateClientAccount(ClientAddVM clientAddVM)
        {
            return Post(clientAddVM, $"api/clients");

        }




        public Task<HttpResponseMessage> LoadClientAccounts(string username, bool includeInactive)
        {
            return Get($"api/clients?username={username}&includeInactive={includeInactive}");

        }



        public Task<HttpResponseMessage> GetBookOffersByCategory(int categoryId)
        {
            return Get($"api/books/category/{categoryId}");
        }

        public Task<HttpResponseMessage> GetBookOffers(string bookTitle, string authorName, bool includeInactive)
        {
            return Get($"api/books/admin/offers?title={bookTitle}&author={authorName}&includeInactive={includeInactive}");
        }


        public Task<HttpResponseMessage> UpdateExistingBookOffer(UpdateBookOfferVM bookOfferUpdate, int id)
        {
            return Put(bookOfferUpdate, $"api/books/admin/offers/{id}");
        }

        public Task<HttpResponseMessage> CreateBookOffer(CreateBookOfferVM bookOfferCreate)
        {
            return Post(bookOfferCreate, $"api/books/admin/offers");
        }

        public Task<HttpResponseMessage> GetBookFile(int id)
        {
            return Get($"api/books/{id}/files");
        }
        public Task<HttpResponseMessage> GetBooks(string bookTitle, string authorName,bool includeInactive,int categoryId)
        {
            return
                Get($"api/books?title={bookTitle}&author={authorName}&includeInactive={includeInactive}&categoryId={categoryId}");
        }


        public Task<HttpResponseMessage> UploadBookPicture(byte[] uploadImage, string imageName, int id)
        {
            return PostMultiPart($"api/books/{id}/picture", uploadImage, imageName);

        }


        public Task<HttpResponseMessage> UploadFile(Stream fileStream, string fileName, int id)
        {
            return PostMultiPart($"api/books/{id}/files", fileStream, fileName);
        }


        public Task<HttpResponseMessage> GetCategories(string categoryNameFilter, bool includeInactiveCategories)
        {
            return Get($"api/categories?nameFilter={categoryNameFilter}&includeInactive={includeInactiveCategories}");
        }


        public Task<HttpResponseMessage> CreateAdminAccount(AdminAddVM adminAdd)
        {
            return Post(adminAdd, "api/admin");
        }

        public Task<HttpResponseMessage> LoadAminAccounts(string usernameFilter)
        {
            if (string.IsNullOrEmpty(usernameFilter))
                return Get("api/admin");
            return Get($"api/admin?username={usernameFilter}");
        }

        public Task<HttpResponseMessage> LoginUser(LoginVM loginVM, string clientId)
        {
            var dict = new Dictionary<string, string>
            {
                { "username", loginVM.Username },
                { "password", loginVM.Password },
                { "grant_type", "password" },
                { "client_id", clientId}
            };
            var req = new HttpRequestMessage(HttpMethod.Post, "token") { Content = new FormUrlEncodedContent(dict) };
            return httpClient.SendAsync(req);
        }
        public Task<HttpResponseMessage> RefreshToken(string refreshToken)
        {
            var dict = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken}
            };
            var req = new HttpRequestMessage(HttpMethod.Post, "token") { Content = new FormUrlEncodedContent(dict) };
            return httpClient.SendAsync(req);
        }

        public Task<HttpResponseMessage> UpdateAdminAccount(AdminUpdateVM adminUpdateVM)
        {
            return Patch(adminUpdateVM, "api/admin");
        }

        public Task<HttpResponseMessage> GetAuditLogs()
        {
            return Get("api/admin/logs");
        }
        public Task<HttpResponseMessage> CreateCategory(CategoryAddVM model)
        {
            return Post(model, "api/categories");
        }
        public Task<HttpResponseMessage> UpdateCategory(CategoryUpdateVm categoryUpdateVm, int id)
        {
            return Put(categoryUpdateVm, $"api/categories/{id}");

        }


        public Task<HttpResponseMessage> CreateBook(CreateBookVM createBookVM)
        {


            return Post(createBookVM, "api/books");
        }

        public Task<HttpResponseMessage> UpdateBook(UpdateBookVM updateBook, int id)
        {
            return Post(updateBook, $"api/books/{id}");
        }



        public Task<HttpResponseMessage> GetClientAccount()
        {
            return Get("api/clients/profile");
        }

        public Task<HttpResponseMessage> GetActiveAuctions()
        {
            return Get("api/auctions/active");

        }



        public Task<HttpResponseMessage> MakeBid(decimal amount, int auctionId)
        {
            return Post(amount, $"api/auction/{auctionId}/bids");

        }








        #region Private

        public Task<HttpResponseMessage> Patch<T>(T value, string requestUri)
        {
            var content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = content };

            return httpClient.SendAsync(request);
        }

        private Task<HttpResponseMessage> Get(string path)
        {
            return httpClient.GetAsync(path);
        }

        private Task<HttpResponseMessage> Post<T>(T body, string path)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("POST"), path) { Content = content };
            return httpClient.SendAsync(request);
        }

        private Task<HttpResponseMessage> Put<T>(T body, string path)
        {
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PUT"), path) { Content = content };
            return httpClient.SendAsync(request);
        }



        private Task<HttpResponseMessage> PostMultiPart(string path, Stream stream, string fileName)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "book_file",
                FileName = fileName
            };
            form.Add(content);
            return httpClient.PostAsync(path, form);
        }

        private Task<HttpResponseMessage> PostMultiPart(string path, byte[] uploadImage, string imageName)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new ByteArrayContent(uploadImage);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "image_file",
                FileName = imageName
            };
            form.Add(content);
            return httpClient.PostAsync(path, form);
        }

        public void AppendToken(AuthenticationResponseVM newAuth)
        {
            if (string.IsNullOrEmpty(newAuth.AccessToken) || string.IsNullOrEmpty(newAuth.TokenType))
                return;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(newAuth.TokenType, newAuth.AccessToken);
        }

     









        #endregion

    }
}
