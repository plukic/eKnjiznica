﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eKnjiznica.Commons.ViewModels.Category;
using eKnjiznica.CORE.Repository;
using eKnjiznica.DAL.EF;

namespace eKnjiznica.DAL.Repository
{
    public class CategoriesRepo : ICategoriesRepo
    {
        private EKnjiznicaDB context;

        public CategoriesRepo(EKnjiznicaDB context)
        {
            this.context = context;
        }

        public void CreateCategory(CategoryAddVM model, string userId)
        {
            context.Categories.Add(new Model.Category
            {
                CategoryName = model.CategoryName,
                IsActive = true,
                UserId = userId
            });
            context.SaveChanges();
        }

        public IList<CategoryVM> GetCategories(string nameFilter,bool includeInactive)
        {
            return context.Categories
                .Where(x=>string.IsNullOrEmpty(nameFilter) || x.CategoryName.Contains(nameFilter))
                .Where(x => includeInactive || x.IsActive)
                .Select(x => new CategoryVM
            {
                Id = x.Id,
                CategoryName = x.CategoryName,
                IsActive = x.IsActive,
                NumberOfBooks = x.Books.Where(y=>y.IsActive).Count()
            }).ToList();
        }

        public Dictionary<int, int> GetCategoriesAndBookNumber(string userId)
        {
            var userBooks = context
                .UserBooks
                .Include(x=>x.BookOffer)
                .Include(x=>x.BookOffer.Book)
                .Include(x=>x.BookOffer.Book.Categories)
                .Where(x => x.UserId == userId)
                .ToList();

            Dictionary<int, int> result = new Dictionary<int, int>();
            foreach (var item in userBooks)
            {
                foreach (var cat in item.BookOffer.Book.Categories)
                {
                    if (!cat.IsActive)
                        continue;
                    if (result.ContainsKey(cat.CategoryId))
                    {
                        result[cat.CategoryId] += 1;
                    }
                    else
                    {
                        result.Add(cat.CategoryId, 1);
                    }
                }
              
            }

            return result;
        }

        public CategoryVM GetCategory(int id)
        {
            return context.Categories.Where(x => x.Id == id)

              .Select(x => new CategoryVM
              {
                  Id = x.Id,
                  CategoryName = x.CategoryName,
                  IsActive = x.IsActive,
                  NumberOfBooks = x.Books.Count()
              }).FirstOrDefault();
        }

        public CategoryVM GetCategoryByName(string categoryName)
        {
            return context.Categories.Where(x => x.CategoryName.Equals(categoryName))
             .Select(x => new CategoryVM
             {
                 Id = x.Id,
                 CategoryName = x.CategoryName,
                 IsActive = x.IsActive,
                 NumberOfBooks = x.Books.Count()
             }).FirstOrDefault();
        }

        public IList<CategoryVM> GetCategoryTopSellingCategories(int take=4)
        {

            Dictionary<int, int> catNumberDict = new Dictionary<int, int>();

            var selledBooks = context.UserBooks.Include(x => x.BookOffer.Book.Categories);

            foreach (var book in selledBooks)
            {
                foreach (var cat in book.BookOffer.Book.Categories)
                {
                    if (catNumberDict.ContainsKey(cat.CategoryId))
                    {
                        catNumberDict[cat.CategoryId] += 1;
                    }
                    else
                    {
                        catNumberDict.Add(cat.CategoryId, 1);
                    }
                }
            }

            var bookCategories = catNumberDict.OrderByDescending(x => x.Value).Select(x => x.Key).Take(take);
            var result = context.Categories.Where(x => bookCategories.Any(y => y == x.Id))
                .Include(x=>x.Books)
                .Select(x => new CategoryVM
                {
                    CategoryName = x.CategoryName,
                    Id = x.Id,
                    IsActive = x.IsActive,
                    NumberOfBooks = x.Books.Count()
                }).ToList();

            return result;

        }

        public void UpdateCategory(CategoryVM category)
        {
            var result = context.Categories.Where(x => x.Id == category.Id).First();
            result.CategoryName = category.CategoryName;
            result.IsActive= category.IsActive;
            context.SaveChanges();
        }

    }
}
