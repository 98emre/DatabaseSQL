using Crud.DAL;
using Crud.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Controllers
{
	public class CustomerController : Controller
	{
		private readonly Customer_DAL _dal;

        public CustomerController(Customer_DAL dal)
        {
            _dal = dal;
        }


		[HttpGet]
        public IActionResult Index()
		{
			List<Customer> customers = new List<Customer>();
			
			try
			{
				customers = _dal.GetAll();
			}

			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
			}


			return View(customers);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Customer customer)
		{
			try
			{
				_dal.Add(customer);
				TempData["message"] = "Customer added successfully.";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
				return View(customer);
			}
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			Customer customer = null;
			try
			{
				customer = _dal.GetCustomerById(id);
				if (customer == null)
				{
					return NotFound();
				}
				return View(customer);
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
				return RedirectToAction("Index");
			}
		}

		[HttpPost]
		public IActionResult Edit(Customer customer)
		{
			try
			{
				_dal.Update(customer);
				TempData["message"] = "Customer updated successfully.";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
				return View(customer);
			}
		}

        [HttpGet]
        public IActionResult DeleteConfirmation(int id)
        {
            try
            {
                var customer = _dal.GetCustomerById(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _dal.Delete(id);
                TempData["message"] = "Customer deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }

}
