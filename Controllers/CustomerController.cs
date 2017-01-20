using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernStore.Data.Transaction;
using ModernStore.Domain.Commands;
using ModernStore.Domain.Handlers;
using ModernStore.Domain.Repositories;

namespace ModernStore.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerRepository _repository;
        private readonly CustomerCommandHandler _handler;


        public CustomerController(IUnitOfWork uow, ICustomerRepository repository, CustomerCommandHandler handler) : base(uow)
        {
            _repository = repository;
            _handler = handler;
        }

        [HttpGet]
        [Route("v1/customers")]
        [Authorize(Policy = "User")]
        public IActionResult Get()
        {
            return Ok(_repository.Get());
        }

        [HttpPost]
        [Route("v1/customers")]
        [Authorize(Policy = "Admin")]
        public IActionResult Post([FromBody]RegisterCustomerCommand command)
        {
            _handler.Handle(command);
            return Response(command, "Cliente cadastrado com sucesso!", "Falha ao registrar", _handler.Notifications);
        }
    }
}