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
    public class ProvidersService : IService<ProviderDTO>
    {
        private UnitOfWork _unitOfWork;
        private ProductMapper _productMapper;
        private ProviderMapper _providerMapper;
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
        public ProviderDTO Create(ProviderDTO dto)
        {
            if (dto == null) throw new ArgumentNullException();
            else if (UnitOfWork.ProviderRepository.Get(p => p.Name == dto.Name).FirstOrDefault() != null)
            {
                throw new ArgumentException("Entity already exist");
            }
            else
            {
                var entity  = ProviderMapper.Map(dto);
                UnitOfWork.ProviderRepository.Insert(entity);
                UnitOfWork.Save();
                var created = UnitOfWork.ProviderRepository.Get((c => c.Name == dto.Name)).FirstOrDefault();
                return ProviderMapper.Map(created);
            }
        }

        public void Delete(ProviderDTO dto)
        {
            if (dto == null) throw new ArgumentNullException();
            else if (UnitOfWork.ProviderRepository.Get((p => p.Name == dto.Name)) == null)
            {
                throw new ArgumentException("Entity doesn't exist");
            }
            else
            {
                var entity = UnitOfWork.ProviderRepository.Get((p => p.Name == dto.Name)).FirstOrDefault();
                UnitOfWork.ProviderRepository.Delete(entity);
                UnitOfWork.Save();
            }
        }

        public IEnumerable<ProviderDTO> GetAll()
        {
            var providers = UnitOfWork.ProviderRepository.Get();
            return ProviderMapper.Map(providers);
        }

        public ProviderDTO GetById(int? id)
        {
            if (id == null) throw new ArgumentNullException();
            else
            {
                var provider = UnitOfWork.ProviderRepository.GetById(id);
                return ProviderMapper.Map(provider);
            }
        }

        public void Update(ProviderDTO dto)
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

        public IEnumerable<ProductDTO> GetProductsByProvider(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else if (UnitOfWork.ProviderRepository.GetById(id) == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                var productCollection = UnitOfWork.ProductRepository.Get((p => p.ProviderId == id));
                return ProductMapper.Map(productCollection);
            }
            
        }

        public IEnumerable<ProviderDTO> GetProvidersWithFilter(IEnumerable<PropertyFilterDTO> propertyFilters)
        {
            if (propertyFilters == null) throw new ArgumentNullException();
            else
            {
                Filter<Provider> filter = new Filter<Provider>();
                foreach (PropertyFilterDTO propertyFilter in propertyFilters)
                {
                    if (propertyFilter.PropertyId == null) throw new ArgumentNullException();
                    else if (propertyFilter.PropertyId.CompareTo("Id") != 0 && propertyFilter.PropertyId.CompareTo("Name") != 0)
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
                var providers = UnitOfWork.ProviderRepository.Get(filter);
                return ProviderMapper.Map(providers);
            }
        }
    }
}
