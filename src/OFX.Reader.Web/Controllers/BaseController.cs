using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace OFX.Reader.Web.Controllers {

    public abstract class BaseController : Controller {

        private IMediator _mediator;

        protected IMediator Mediator => this._mediator ?? (this._mediator = this.HttpContext.RequestServices.GetService<IMediator>());

    }

}