using BLL.DTO;
using BLL.Interfaces;
using BLL.Mappers;
using DAL.Models;
using DAL.UnitOfWork;
using ExpressionBuilder.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services
{
    public class ProductsService : IService<ProductDTO>
    {
        private UnitOfWork _unitOfWork;
        private ProductMapper _productMapper;
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

        public ProductDTO Create(ProductDTO dto)
        {
            if (dto == null) throw new ArgumentNullException();
            else if (UnitOfWork.ProductRepository.Get(p => p.Name == dto.Name).FirstOrDefault() != null)
            {
                throw new ArgumentException("Entity already exist");
            }
            else
            {
                var entity = ProductMapper.Map(dto);
                UnitOfWork.ProductRepository.Insert(entity);
                UnitOfWork.Save();
                var created = UnitOfWork.ProductRepository.Get((c => c.Name == dto.Name)).FirstOrDefault();
                return ProductMapper.Map(created);
            }
        }

        public void Delete(ProductDTO dto)
        {
            if (dto == null) throw new ArgumentNullException();
            else if (UnitOfWork.ProductRepository.Get((p => p.Name == dto.Name)) == null)
            {
                throw new ArgumentException("Entity doesn't exist");
            }
            else
            {
                var entity = UnitOfWork.ProductRepository.Get((p => p.Name == dto.Name)).FirstOrDefault();
                UnitOfWork.ProductRepository.Delete(entity);
                UnitOfWork.Save();
            }
        }

        public IEnumerable<ProductDTO> GetAll()
        {
            var products = UnitOfWork.ProductRepository.Get();
            return ProductMapper.Map(products);
        }

        public ProductDTO GetById(int? id)
        {
            if (id == null) throw new ArgumentNullException();
            else
            {
                var product = UnitOfWork.ProductRepository.GetById(id);
                return ProductMapper.Map(product);
            }
        }

        public void Update(ProductDTO dto)
        {
            if (dto == null) throw new ArgumentNullException();
            else if (UnitOfWork.ProviderRepository.Get((p => p.Id == dto.Id)) == null)
            {
                throw new ArgumentException("Entity doesn't exist");
            }
            else
            {
                var entity = UnitOfWork.ProviderRepository.GetById(dto.Id);
                UnitOfWork.ProviderRepository.Update(entity);
                UnitOfWork.Save();
            }
        }

        public IEnumerable<ProductDTO> GetProductsWithFilter(IEnumerable<PropertyFilterDTO> propertyFilters)
        {
            if (propertyFilters == null) throw new ArgumentNullException();
            else
            {
                Filter<Product> filter = new Filter<Product>();
                foreach (PropertyFilterDTO propertyFilter in propertyFilters)
                {
                    if (propertyFilter.PropertyId == null) throw new ArgumentNullException();
                    else if (propertyFilter.PropertyId.CompareTo("Id") != 0 && propertyFilter.PropertyId.CompareTo("Name")!=0 &&
                        propertyFilter.PropertyId.CompareTo("Price")!=0 || propertyFilter.PropertyId.CompareTo("CategoryId") !=0
                        && propertyFilter.PropertyId.CompareTo("ProviderId")!=0)
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
                var products = UnitOfWork.ProductRepository.Get(filter);
                return ProductMapper.Map(products);
            }
        }
    }
}
