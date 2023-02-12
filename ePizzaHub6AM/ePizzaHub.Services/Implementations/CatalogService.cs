﻿using ePizzaHub.Entities;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;

namespace ePizzaHub.Services.Implementations
{
    public class CatalogService : ICatalogService
    {
        IRepository<Item> _itemRepo;
        IRepository<Category> _categoryRepo;
        IRepository<ItemType> _itemTypeRepo;
        public CatalogService(IRepository<Item> itemRepo,
        IRepository<Category> categoryRepo,
        IRepository<ItemType> itemTypeRepo)
        {
            _itemRepo = itemRepo;
            _categoryRepo = categoryRepo;
            _itemTypeRepo = itemTypeRepo;
        }
        public IEnumerable<Item> GetItems()
        {
            return _itemRepo.GetAll().OrderBy(item => item.CategoryId).ThenBy(item => item.ItemTypeId);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _categoryRepo.GetAll();
        }

        public IEnumerable<ItemType> GetItemTypes()
        {
            return _itemTypeRepo.GetAll();
        }

        public void AddItem(Item item)
        {
            _itemRepo.Add(item);
            _itemRepo.SaveChanges();
        }

        public Item GetItem(int id)
        {
            return _itemRepo.Find(id);
        }

        public void UpdateItem(Item item)
        {
            _itemRepo.Update(item);
            _itemRepo.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            _itemRepo.Delete(id);
            _itemRepo.SaveChanges();
        }
    }
}
