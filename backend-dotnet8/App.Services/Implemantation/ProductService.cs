using App.DataAccessLayer.EntityModel.SQL.Model;
using App.Infrastructure;
using App.Services.Dto.General;
using App.Services.Dto.Product;
using App.Services.Helper;
using App.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace App.Services.Implemantation;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto> CreateAsync(ProductDto model)
    {
        ResponseDto resp = new ResponseDto();
        try
        {
            bool isAlready = await _unitOfWork.GenericRepository<Product>().ExistsAsync(x => x.Name.Equals(model.Name));
            if (isAlready)
            {
                resp.IsSucceed = false;
                resp.StatusCode = StatusCodes.Status400BadRequest;
                resp.Message = $"Product Name:{model.Name} is already Exit";
                return resp;
            }
            Product data = ModelConverter.ConvertTo<ProductDto, Product>(model);
            await _unitOfWork.GenericRepository<Product>().AddAsync(data);

        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.StatusCode = StatusCodes.Status417ExpectationFailed;
            resp.Message = ex.Message;

        }

        return resp;
    }

    public async Task<ResponseDto> DeleteAsync(int id)
    {
        ResponseDto resp = new ResponseDto();
        try
        {
            var Result = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Id.Equals(id));
            if (Result is null)
            {
                resp.IsSucceed = false;
                resp.Message = "Product is not Found";
                resp.StatusCode = StatusCodes.Status406NotAcceptable;
                return resp;
            }
            Result.UpdatedAt = DateTime.UtcNow;
            Result.IsDeleted = true;
            await _unitOfWork.GenericRepository<Product>().UpdateAsync(Result);
            await _unitOfWork.SaveAsync();

        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.StatusCode = StatusCodes.Status406NotAcceptable;
            resp.Message = ex.Message;

        }

        return resp;
    }
    public async Task<ResponseDto> RecycleItemAsync(string name)
    {
        ResponseDto resp = new ResponseDto();
        try
        {
            var Result = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Name.Equals(name) && x.IsDeleted == true);
            if (Result is null)
            {
                resp.IsSucceed = false;
                resp.Message = "Product is not Found";
                resp.StatusCode = StatusCodes.Status406NotAcceptable;
                return resp;
            }
            Result.UpdatedAt = DateTime.UtcNow;
            Result.IsDeleted = false;
            await _unitOfWork.GenericRepository<Product>().UpdateAsync(Result);
            await _unitOfWork.SaveAsync();

        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.StatusCode = StatusCodes.Status406NotAcceptable;
            resp.Message = ex.Message;

        }

        return resp;
    }
    public async Task<ResponseDto> ActiveAsync(int id, bool Active)
    {
        ResponseDto resp = new ResponseDto();
        try
        {
            var Result = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Id.Equals(id));
            if (Result is null)
            {
                resp.IsSucceed = false;
                resp.Message = "Product is not Found";
                resp.StatusCode = StatusCodes.Status406NotAcceptable;
                return resp;
            }
            Result.UpdatedAt = DateTime.UtcNow;
            Result.IsActive = Active;

            await _unitOfWork.GenericRepository<Product>().UpdateAsync(Result);
            await _unitOfWork.SaveAsync();
            resp.Message = Active ? "Product is Active Succssfully" : "Product is InActive Succssfully";
        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.StatusCode = StatusCodes.Status406NotAcceptable;
            resp.Message = ex.Message;

        }

        return resp;
    }

    public async Task<ResponseDto> GetAllAsync()
    {
        ResponseDto resp = new ResponseDto();
        try
        {
            var Data = await _unitOfWork.GenericRepository<Product>().GetAllAsync(filter: x => x.IsDeleted == false && x.IsActive == true, orderby: a => a.OrderByDescending(x => x.CreatedAt));
            resp.Result = ModelConverter.ConvertTo<Product, ProductDto>(Data);

        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.StatusCode = StatusCodes.Status406NotAcceptable;
            resp.Message = ex.Message;
        }

        return resp;
    }

    public async Task<ResponseDto> GetByIdAsync(int id)
    {
        ResponseDto resp = new ResponseDto();
        try
        {
            var Result = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Id.Equals(id));
            if (Result is null)
            {
                resp.IsSucceed = false;
                resp.Message = "Product is not Found";
                resp.StatusCode = StatusCodes.Status406NotAcceptable;
                return resp;
            }
            resp.Result = ModelConverter.ConvertTo<Product, ProductDto>(Result);

        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.StatusCode = StatusCodes.Status406NotAcceptable;
            resp.Message = ex.Message;
        }

        return resp;
    }

    public async Task<ResponseDto> UpdateAsync(int id, ProductDto model)
    {
        ResponseDto resp = new ResponseDto();
        try
        {
            var Result = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Id.Equals(id));
            if (Result is null)
            {
                resp.IsSucceed = false;
                resp.Message = "Product is not Found";
                resp.StatusCode = StatusCodes.Status406NotAcceptable;
                return resp;
            }
            Result.Name = model.Name;
            Result.UpdatedAt = DateTime.Now;
            //so on property you call
            await _unitOfWork.GenericRepository<Product>().UpdateAsync(Result);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.StatusCode = StatusCodes.Status406NotAcceptable;
            resp.Message = ex.Message;
        }

        return resp;
    }


}
