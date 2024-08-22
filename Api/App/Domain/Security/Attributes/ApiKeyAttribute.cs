using Api.App.Common.Controller;
using Microsoft.AspNetCore.Mvc;

namespace Api.App.Domain.Security.Attributes;

public class ApiKeyAttribute() : ServiceFilterAttribute(typeof(ApiKeyAuthFilter));