using System;
using System.Collections.Generic;
using BLL.Mappers;
using BLL.DTO;
using BLL.Interfaces;
using DAL.UnitOfWork;
using System.Linq;
using DAL.Models;
using System.Linq.Expressions;
using ExpressionBuilder.Generics;

namespace BLL.Services
{
    public class CategoriesService : IService<CategoryDTO>
    {
        private CategoryMapper _categoryMapper;
        private ProductMapper _productMapper;
        private ProviderMapper _providerMapper;
        private UnitOfWork _unitOfWork;

        private CategoryMapper CategoryMapper 
        {
            get
            {
                if (_categoryMapper == null)
                {
                    _categoryMapper = new CategoryMapper();
                }
                return _categoryMapper;
            }
        }

        private ProductMapper ProductMapper
        {
            get
            {
                if (_productMapper == null)
                {
                    _productMapper = new ProductMapper();
                }
                return _productMapper;
            }
        }

        private ProviderMapper ProviderMapper
        {
            get
            {
                if (_providerMapper == null)
                {
                    _providerMapper = new ProviderMapper();
                }
                return _providerMapper;
            }
        }
        private UnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                {
                    _unitOfWork = new UnitOfWork();
                }
                return _unitOfWork;
            }
        }
        public CategoryDTO Create(CategoryDTO dto)
        {
            if (dto == null) throw new ArgumentNullException();
            else if (UnitOfWork.CategoryRepository.Get((c => c.Name == dto.Name)).FirstOrDefault() != null)
            {
                throw new ArgumentException("Entity already exist");
            }
            else
            {
                var entity = CategoryMapper.Map(dto);
                UnitOfWork.CategoryRepository.Insert(entity);
                UnitOfWork.Save();
                var created = UnitOfWork.CategoryRepository.Get((c => c.Name == dto.Name)).FirstOrDefault();
                return CategoryMapper.Map(created);
            }
        }

        public void Delete(CategoryDTO dto)
        {
            if (dto == null) throw new ArgumentNullException();
            else if (UnitOfWork.CategoryRepository.Get((c => c.Name == dto.Name)) == null)
            {
                throw new ArgumentException("Entity doesn't exist");
            }
            else
            {
                var entity = UnitOfWork.CategoryRepository.Get((c => c.Name == dto.Name)).FirstOrDefault();
                UnitOfWork.CategoryRepository.Delete(entity);
                UnitOfWork.Save();
            }
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            var categories = UnitOfWork.CategoryRepository.Get();
            return CategoryMapper.Map(categories);
        }

        public CategoryDTO GetById(int? id)
        {
            if (id == null) throw new ArgumentNullException();
            else
            {
                var category = UnitOfWork.CategoryRepository.GetById(id);
                return CategoryMapper.Map(category);
            }
        }

        public void Update(CategoryDTO dto)
        {
            if (dto == null) throw new ArgumentNullException();
            else if (UnitOfWork.CategoryRepository.Get((c => c.Id == dto.Id)) == null)
            {
                throw new ArgumentException("Entity doesn't exist");
            }
            else
            {
                var entity = UnitOfWork.CategoryRepository.GetById(dto.Id);
                UnitOfWork.CategoryRepository.Update(entity);
                UnitOfWork.Save();
            }
        }
        public IEnumerable<ProductDTO> GetProductsByCategory(int? categoryId)
        {
            if (categoryId == null) throw new ArgumentNullException();
            if (UnitOfWork.CategoryRepository.GetById(categoryId) == null) throw new ArgumentException();
            else
            {
                var productCollection = UnitOfWork.ProductRepository.Get((p => p.CategoryId == categoryId));
                return ProductMapper.Map(productCollection);
            }
        }
        public IEnumerable<ProviderDTO> GetProvidersByCategory(int? categoryId)
        {
            var products = GetProductsByCategory(categoryId);
            if (categoryId == null)
            {
                throw new ArgumentNullException();
            }
            if (products == null)
            {
                throw new ArgumentException();
            }
            else
            {
                List<Provider> providers = new List<Provider>();
                foreach (ProductDTO product in products)
                {
                    var provider = UnitOfWork.ProviderRepository.Get((pr => pr.Name == product.ProviderName)).FirstOrDefault();
                    if (provider != null && !providers.Contains(provider))
                    {
                        providers.Add(provider);
                    }
                }
                return ProviderMapper.Map(providers);
            }
        }

        public IEnumerable<CategoryDTO> GetCategoriesWithFilter(IEnumerable<PropertyFilterDTO> propertyFilters)
        {
            if (propertyFilters == null) throw new ArgumentNullException();
            else
            {
                Filter<Category> filter = new Filter<Category>();
                foreach (PropertyFilterDTO propertyFilter in propertyFilters)
                {
                    if (propertyFilter.PropertyId == null) throw new ArgumentNullException();
                    else if (propertyFilter.PropertyId.CompareTo("Id") !=0 && propertyFilter.PropertyId.CompareTo("Name") != 0)
                        throw new ArgumentException();
                    else
                    {
                        var type = (new Category()).GetType().GetProperty(propertyFilter.PropertyId).PropertyType;
                        if (propertyFilter.Value != null && propertyFilter.Value2 != null)
                        {
                            var value1 = Convert.ChangeType(propertyFilter.Value, type);
                            var value2 = Convert.ChangeType(propertyFilter.Value2, type);
                            filter.By(propertyFilter.PropertyId, propertyFilter.Operation, value1,
                                value2, propertyFilter.Connector);
                        }
                        else if (propertyFilter.Value != null)
                        {
                            var value1 = Convert.ChangeType(propertyFilter.Value, type);
                            filter.By(propertyFilter.PropertyId, propertyFilter.Operation, value1, null, propertyFilter.Connector);
                        }
                        else
                        {
                            filter.By(propertyFilter.PropertyId, propertyFilter.Operation, propertyFilter.Connector);
                        }
                    }
                }
                var categories = UnitOfWork.CategoryRepository.Get(filter);
                return CategoryMapper.Map(categories);
            }
        }
    }
}
