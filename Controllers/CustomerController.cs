using Microsoft.AspNetCore.Mvc;
using ModernStore.Data.Transaction;
using ModernStore.Domain.Commands;
using ModernStore.Domain.Handlers;
using ModernStore.Domain.Repositories;

namespace ModernStore.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICustomerRepository _repository;
        private readonly CustomerCommandHandler _handler;


        public CustomerController(IUnitOfWork uow, ICustomerRepository repository, CustomerCommandHandler handler) : base(uow)
        {
            _uow = uow;
            _repository = repository;
            _handler = handler;
        }

        [HttpGet]
        [Route("v1/customers")]
        public IActionResult Get()
        {
            return Ok(_repository.Get());
        }

        [HttpPost]
        [Route("v1/customers")]
        public IActionResult Post([FromBody]RegisterCustomerCommand command)
        {
            _handler.Handle(command);
            return Response(command, "Cliente cadastrado com sucesso!", "Falha ao registrar", _handler.Notifications);
        }
    }
}