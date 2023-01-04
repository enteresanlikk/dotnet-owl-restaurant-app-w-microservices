using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OwlRestaurant.Services.ProductAPI.Abstractions.Repositories;
using OwlRestaurant.Services.ProductAPI.DBContexts;
using OwlRestaurant.Services.ProductAPI.DTOs;
using OwlRestaurant.Services.ProductAPI.Models;

namespace OwlRestaurant.Services.ProductAPI.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDTO> CreateUpdateProduct(ProductDTO product)
    {
        try
        {
            var _product = _mapper.Map<Product>(product);

            if (_product.Id == Guid.Empty)
            {
                await _context.Products.AddAsync(_product);
            }
            else
            {
                _context.Products.Update(_product);
            }
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDTO>(_product);
        }
        catch (Exception)
        {
        }
        return null;
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        try
        {
            ProductDTO findedProduct = await GetProductById(productId);
            
            var _product = _mapper.Map<Product>(findedProduct);

            _context.Products.Remove(_product);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            
        }
        return false;
    }

    public async Task<ProductDTO> GetProductById(Guid productId)
    {
        Product product = await _context.Products.AsNoTracking().Where(p => p.Id == productId).FirstOrDefaultAsync();

        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<IEnumerable<ProductDTO>> GetProducts()
    {
        List<Product> products = await _context.Products.AsNoTracking().ToListAsync();

        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }
}
