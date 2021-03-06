﻿using eShop_Mvc.SharedKernel.Enums;
using System;
using System.Collections.Generic;

namespace eShop_Mvc.Models.AccountViewModels
{
    public class AppUserViewModel
    {
        public Guid? Id { get; set; }
        public string FullName { get; set; }
        public string BirthDay { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public string Gender { get; set; }
        public Status Status { get; set; } = Status.Active;
        public DateTime DateCreated { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}