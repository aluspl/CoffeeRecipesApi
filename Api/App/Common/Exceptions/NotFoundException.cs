﻿namespace Api.App.Common.Exceptions;

public class NotFoundException(string message) : Exception(message);