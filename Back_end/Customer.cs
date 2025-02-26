﻿namespace WebShopSem4
{
    public class Customer : User
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public Role Role { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
    }
}
