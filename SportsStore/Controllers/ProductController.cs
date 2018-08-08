using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;


        /*When MVC needs to create a ProductController class to handle an 
        HTTP request, it will inspect the constructor to see that it requires
        an object that implements the IProductRepository interface. 
        MVC will then check startup.cs to see what implementation class would be used
        An object of that specified class would be created everytime for use with 
        the constructor. 
        
        This allows the controller constructor to access the data repository
        thru the IProductRepository interface without having any need to know which 
        implementation class has been configured.

        */

        public ProductController (IProductRepository repo)
        {
            repository = repo; 
        }

        public ViewResult List(int productPage = 1)
         => View(new ProductsListViewModel
         {
          /*Get product objects, order them by primary key (ProductID),
            Skip over products that occured before the start of the current page,
            and takenumber of products specified by the PageSize field.

             Example: productPage = 1;
             Skip ((1 - 1) * 4 ) objects
             Take 4 product objects afterwards.


           */
          Products = repository.Products
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                     .Take(PageSize),
          PagingInfo = new PagingInfo
             {
                 CurrentPage = productPage,
                 ItemsPerPage = PageSize,
                 TotalItems = repository.Products.Count()
             }
         });

    }
}