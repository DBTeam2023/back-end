﻿namespace Order.exception
{
    public class NotFoundException : MyException
    {
        public NotFoundException(string message) : base(message) { }
    }
}
