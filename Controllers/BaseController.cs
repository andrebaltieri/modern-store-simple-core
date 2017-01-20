using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ModernStore.Data.Transaction;
using ModernStore.Validation;

namespace ModernStore.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUnitOfWork _uow;
        public BaseController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IActionResult Response(object result, string success, string error, Dictionary<string, string> notifications)
        {
            if (notifications.Count > 0)
            {
                return BadRequest(new { success = false, message = error, data = result, errors = notifications });
            }
            else
            {
                try
                {
                    _uow.Commit();
                    return Ok(new { success = true, message = success, data = result, errors = "" });
                }
                catch
                {
                    return BadRequest(new { success = false, message = "Ocorreu um erro no servidor" });
                }
            }
        }
    }
}