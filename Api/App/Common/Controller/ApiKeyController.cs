using Api.App.Domain.Security.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Api.App.Common.Controller;

[ApiController]
[ApiKey]
public class ApiKeyController : ApiController
{
}