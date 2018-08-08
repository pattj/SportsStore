using System;
namespace SportsStore.Models.ViewModels
{
 /*Purpose of this class:
 * 
 * View model and Helper class for a tag helper (used for generating HTML markup for the links required).
 * Used specifically to help pass data between a controller and a view for the paginate feature. 
 * 
 */


    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages =>
        (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}