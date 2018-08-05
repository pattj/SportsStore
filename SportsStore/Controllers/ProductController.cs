using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;

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

        public ViewResult List() =>View(repository.Products);
        
    }
}